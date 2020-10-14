using System;
using System.Windows;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

using System.IO;
using System.Threading;


namespace AllMyMusic_v3
{
    #region Enums
    public enum Statuses { None, Initializing, Ready, Loading, Playing, Stopped, Pausing, Terminating, Terminated, Error };

    #endregion

    public class AmmAudioPlayback : IDisposable
    {
        #region Fields
        private IWavePlayer _playbackDevice;
        private WaveStream _fileStream;
        private AmmAudioFileReader _inputStream;
        private VolumeSampleProvider _volumeProvider;
        private float _volume;
        private Boolean _playbackStopped;
        
        private string _filename;
        private Thread _audioProcessingThread;
        private readonly object InitializedLock = new object();
        #endregion


        #region Events
        public event EventHandler<FftEventArgs> FftCalculated;
        protected virtual void OnFftCalculated(FftEventArgs e)
        {
            EventHandler<FftEventArgs> handler = FftCalculated;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;
        protected virtual void OnMaximumCalculated(MaxSampleEventArgs e)
        {
            EventHandler<MaxSampleEventArgs> handler = MaximumCalculated;
            if (handler != null) handler(this, e);
        }
        #endregion

        #region Properties
        public Boolean PlaybackStopped
        {
            get { return _playbackStopped; }
            set
            {
                if (value == _playbackStopped)
                    return;

                _playbackStopped = value;
            }
        }
       
        public float Volume
        {
            get { return _volume; }
            set
            {
                if (value == _volume) 
                    return;

                if (_volumeProvider != null)
                {
                    _volumeProvider.Volume = value;
                }

                _volume = value;
            }
        }
        public String CurrentTime
        {
            get
            {
                if (_inputStream != null)
                {
                    return FormatTimeSpan(_inputStream.CurrentTime);
                }
                return "00:00";
            }
        }
        public long StreamPosition
        {
            get
            {
                if (_inputStream != null)
                {
                    return _inputStream.Position;
                }
                return 0;
            }
            set
            {
                if (_inputStream != null)
                {
                    _inputStream.Position = value;
                }
            }
        }
        public long StreamLength
        {
            get
            {
                if (_inputStream != null)
                {
                    return _inputStream.Length;
                }
                return 0;
            }
        }
        private static string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
            {
                return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.TotalMinutes, ts.Seconds);
            }
            else
            {
                return string.Format("{0:D2}:{1:D2}", (int)ts.TotalMinutes, ts.Seconds);
            }
            
        }
        #endregion


        public void Load(string fileName)
        {
            _filename = fileName;

            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFile(_filename);
        }

        public void Load_Thread(string fileName)
        {
            _filename = fileName;


            // Create the Audio Processing Worker (Thread)
            _audioProcessingThread = new Thread(new ThreadStart(audioProcessingWorker_DoWork));
            _audioProcessingThread.Name = "AudioProcessingThread";
            _audioProcessingThread.IsBackground = true;
            _audioProcessingThread.Priority = ThreadPriority.Highest;
            // Important: MTA is needed for WMFSDK to function properly (for WMA support)
            // All WMA (COM) related actions MUST be done within the Thread's MTA otherwise there is a COM exception
            _audioProcessingThread.SetApartmentState(ApartmentState.MTA);

            // Allow initialization to start >>Inside<< the thread, the thread will stop and wait for a pulse
            _audioProcessingThread.Start();

            // Wait for thread for finish initialization
            lock (InitializedLock)
            {
                Monitor.Wait(InitializedLock);
            }
        }

        private void audioProcessingWorker_DoWork()
        {
            try
            {
                // Initialize audio playback
                try
                {
                    Stop();
                    CloseFile();
                    EnsureDeviceCreated();
                    OpenFile(_filename);
                }
                finally
                {
                    // Pulse the initialized lock to release the client (UI) that is waiting for initialization to finish
                    lock (InitializedLock)
                    {
                        Monitor.Pulse(InitializedLock);
                    }
                }

                Play();

                try
                {
                    // ==============================================
                    // ====  Perform the actual audio processing ====
                    //ProcessAudio();
                    // ==============================================
                }
                finally
                {
                    // Dispose of NAudio in context of thread (for WMF it must be disposed in the same thread)
                    //TerminateNAudio();
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Exception in audioProcessingWorker_DoWork, Filename: " + _filename;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
       
        private void CloseFile()
        {
            if (_fileStream != null)
            {
                _fileStream.Dispose();
                _fileStream = null;
            }
        }

        private void OpenFile(string fileName)
        {
            try
            {
                //_playbackStopped = false;
                _inputStream = new AmmAudioFileReader(fileName);
                _fileStream = _inputStream;

                var aggregator = new SampleAggregator(_inputStream);
                aggregator.NotificationCount = _fileStream.WaveFormat.SampleRate / 100;
                aggregator.PerformFFT = true;
                aggregator.FftCalculated += (s, a) => OnFftCalculated(a);
                aggregator.MaximumCalculated += (s, a) => OnMaximumCalculated(a);


                _volumeProvider = new VolumeSampleProvider(aggregator);
                _volumeProvider.Volume = _volume;
                _playbackDevice.Init(_volumeProvider);

                _playbackDevice.PlaybackStopped += new EventHandler<StoppedEventArgs>(_playbackDevice_PlaybackStopped);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Problem opening file");
                CloseFile();
            }
        }

        private void _playbackDevice_PlaybackStopped(object source, StoppedEventArgs e)
        {
            _playbackStopped = true;
        }

        private void EnsureDeviceCreated()
        {
            if (_playbackDevice != null)
            {
                _playbackDevice.Dispose();
            }
            
            CreateDevice();
        }

        private void CreateDevice()
        {
            _playbackDevice = new WaveOut {DesiredLatency = 200};
        }

        public void Play()
        {
            if (_playbackDevice != null && _fileStream != null && _playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                _playbackDevice.Play();
            }
            _playbackStopped = false;
        }

        public void Pause()
        {
            if (_playbackDevice != null) 
            {
                if (_playbackDevice.PlaybackState == PlaybackState.Playing)
                {
                    _playbackDevice.Pause();
                }
                else
                {
                    _playbackDevice.Play();
                }
            }
        }

        public void Stop()
        {
            if (_playbackDevice != null)
            {
                _playbackDevice.Stop();
            }
            if (_inputStream != null)
            {
                _inputStream.Dispose();
                _inputStream = null;
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();
        }

        ~AmmAudioPlayback()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            Stop();
            CloseFile();
            if (_playbackDevice != null)
            {
                _playbackDevice.Dispose();
                _playbackDevice = null;
            }

            if (_inputStream != null)
            {
                _inputStream.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion


    }
}
