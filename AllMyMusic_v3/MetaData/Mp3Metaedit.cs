using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using NAudio.Wave;
using Metadata.ID3;
using AllMyMusic_v3;



namespace Metadata.Mp3
{
    /// <summary>
    /// This class is used to:
    /// Read the meta information saved in an MP3 file
    /// </summary>
    public class Mp3Metaedit : IDisposable
    {
        #region variables
        private int bitrate;
        private BitrateType bitrateType = BitrateType.Unknown;
        private Boolean fullyDownloaded = false;
        private Boolean tocCreated = false;
        private int sampleRate;
        private int totalSamples;
        private String channelMode = String.Empty;
        private String _fileName = String.Empty;
        private long fileSize = 0;
        private SongItem _song;

        private ID3Option id3Option;
        private long dataStartPosition = 0;
        private Int32 id3V2TagSize = 0;
        private long mp3DataLength = 0;

        private TagReader tagReader;
        private Id3TagCollection tagCollection;
        private Mp3Frame mp3Frame;
        private LameHeader lameHeader;
        private XingHeader xingHeader;

        
        private Stream mp3Stream;
        //private int frameLengthInBytes;
        #endregion

        #region InformationProperties
        
        /// <summary>
        /// The rate of bits per second according to the compressed MP3 file
        /// </summary>
        public Int32 BitRate
        {
            get { return bitrate; }
        }

        /// <summary>
        /// Varible or Constant bitrate
        /// </summary>
        public BitrateType BitrateType
        {
            get { return bitrateType; }
            set { bitrateType = value; }
        }

       
        public Boolean FullyDownloaded
        {
            get { return fullyDownloaded; }
            set { fullyDownloaded = value; }
        }

        /// <summary>
        /// The table of contense for the MP3 file is created
        /// </summary>
        public Boolean TocCreated
        {
            get { return tocCreated; }
            set { tocCreated = value; }
        }


        /// <summary>
        /// The rate of bits per second according to the uncompressed MP3 file
        /// </summary>
        public Int32 SampleRate
        {
            get { return sampleRate; }
        }

        /// <summary>
        /// Number of samples in this MP3 file
        /// </summary>
        public Int32 TotalSamples
        {
            get { return totalSamples; }
        }

        /// <summary>
        /// The full path of this file
        /// </summary>
        public String FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// The song information
        /// </summary>
        public SongItem Song
        {
            get
            {
                return _song;
            }
        }
        #endregion

        #region Options

        public ID3Option ID3Option
        {
            get { return id3Option; }
            set { id3Option = value; }
        }

        #endregion

        #region DataProperties
        
        /// <summary>
        /// The address of the first byte of an MP3 music frame
        /// </summary>
        public long DataStartPosition
        {
            get { return dataStartPosition; }
        }

        /// <summary>
        /// The length of the ID3V2 frame in bytes
        /// </summary>
        public Int32 ID3V2TagSize
        {
            get { return id3V2TagSize; }
        }

        /// <summary>
        /// The length of the music data in bytes
        /// </summary>
        public long MP3DataLength
        {
            get { return mp3DataLength; }
        }
        #endregion

        /// <summary>
        /// Constructor for the MP3 file information object
        /// </summary>
        /// <param name="fileName"></param>
        public Mp3Metaedit(String fileName)
        {
            this._fileName = fileName;

            if (this.id3Option == null)
            {
                this.id3Option = new ID3Option();
                this.id3Option.AddTLENtag = true;
                this.id3Option.DeleteUnsupportedTags = true;
                this.id3Option.RemoveXingFrames = true;
                this.id3Option.WriteID3V1Tags = false;
            }

            this.mp3Stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.fileSize = this.mp3Stream.Length;
            if (this.mp3Stream.Length > 50000)
            {
                ReadFile();    
            }   
        }

        /// <summary>
        /// Constructor for the MP3 file information object
        /// </summary>
        /// <param name="fileName"></param>
        public Mp3Metaedit(String fileName, ID3Option ID3Option)
        {
            this._fileName = fileName;
            this.id3Option = ID3Option;

            this.mp3Stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            ReadFile();
        }

        public SongItem ReadMetadata()
        {
            return _song;
        }

        public void UpdateMetadata(ChangedPropertiesList changedProperties)
        {
            if (tagCollection == null)
            {
                tagCollection = new Id3TagCollection();
            }
            tagCollection.UpdateTags(changedProperties);
            

            if (id3Option.RemoveXingFrames == false)
            {
                dataStartPosition = ID3V2TagSize;
            }
            TagWriter.WriteTags(_fileName, tagCollection, (int)dataStartPosition);

            if (id3Option.WriteID3V1Tags == true)
            {
                ID3V1Tag.Save(_fileName, this._song);
            }
        }

        public void UpdateMetadata(SongItem song)
        {
            if (tagCollection == null)
            {
                tagCollection = new Id3TagCollection();
            }
            tagCollection.UpdateTags(song);


            if (id3Option.RemoveXingFrames == false)
            {
                dataStartPosition = ID3V2TagSize;
            }
            TagWriter.WriteTags(_fileName, tagCollection, (int)dataStartPosition);

            if (id3Option.WriteID3V1Tags == true)
            {
                ID3V1Tag.Save(_fileName, this._song);
            }
        }

        public void SaveMetadata()
        {
            if (tagCollection != null)
            {
                if (id3Option.RemoveXingFrames == false)
                {
                    dataStartPosition = ID3V2TagSize;
                }
                TagWriter.WriteTags(_fileName, tagCollection, (int)dataStartPosition);

                if (id3Option.WriteID3V1Tags == true)
                {
                    ID3V1Tag.Save(_fileName, this._song);
                }
            }
        }


        /// <summary>
        /// Replace the image in an APIC tag that matches the picType
        /// </summary>
        /// <param name="coverImage"></param>
        /// <param name="picType"></param>
        public void UpdatePicture(Image coverImage, PictureType picType)
        {
            tagCollection.UpdatePicture(coverImage, picType);
        }

        /// <summary>
        /// Delete all APIC tags
        /// </summary>
        public Int32 DeletePictureTags()
        {
            Int32 countDone = 0;
            Id3TagCollection newTagCollection = new Id3TagCollection();
            if (tagCollection != null)
            {
                foreach (Id3Tag tag in tagCollection)
                {
                    if (tag.TagType == TagType.APIC)
                    {
                        countDone++;
                    }
                    else
                    {
                        newTagCollection.Add(tag);
                    }
                }
            }
           
            tagCollection = newTagCollection;
            return countDone;
        }

        /// <summary>
        /// Get the image for the specified PictureType if it exists in the TagCollection
        /// </summary>
        /// <param name="picType"></param>
        /// <returns></returns>
        public Image GetAttachedPicture(PictureType pictureType)
        {
            return tagCollection.GetAttachedPicture(pictureType);
        }

        /// <summary>
        /// Read all META-Information of an MP3 file
        /// </summary>
        private void ReadFile()
        {
            try
            {
                try
                {
                    // Read teh ID3V2 frame
                    tagReader = new TagReader(mp3Stream);
                    dataStartPosition = tagReader.DataLength + 10;
                    _song = tagReader.GetSong(_fileName);
                    tagCollection = tagReader.BuildTagCollection(id3Option.DeleteUnsupportedTags);

                    if (_song == null)
                    {
                        // Read the ID3V1 frame at the end of the file
                        _song = ID3V1Tag.ReadTag(mp3Stream, _fileName);
                        if (_song != null)
                        {
                            mp3DataLength -= 128;

                            if (tagCollection == null)
                            {
                                tagCollection = new Id3TagCollection();
                            }
                            tagCollection.UpdateTags(_song);
                        }
                    }

                    if (_song == null)
                    {
                        _song = PathTagger.GuessTagsFromFileName(_fileName);
                    }
                }
                catch
                {
                    tagCollection = new Id3TagCollection();
                }
               
                mp3Stream.Position = dataStartPosition;
                id3V2TagSize = (int)dataStartPosition;

                dataStartPosition = (int)GetDataStart();
                mp3DataLength = mp3Stream.Length - dataStartPosition;

                mp3Frame = Mp3Frame.LoadFromStream(mp3Stream);
                if (mp3Frame != null)
                {
                    sampleRate = mp3Frame.SampleRate;
                    bitrate = mp3Frame.BitRate;
                    channelMode = mp3Frame.ChannelMode.ToString();
                    dataStartPosition = ProcessVBRheader();

                    bitrateType = TestBitrateType(50);
                    totalSamples = GetSampleCount();
                    DefineSongInformation();
                }
                else
                {
                    //CoreLibrary.InformationMessage("Not an MP3 file: " + fileName);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error updating Band, Album and Song for filename: " + _fileName;           
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            finally
            {
                mp3Stream.Close();
                mp3Stream = null;
            }
        }

        /// <summary>
        /// Determine at what byte position the music data starts
        /// </summary>
        /// <returns></returns>
        private long GetDataStart()
        {
            // Start of music data is only identified by two bytes
            // To make sure not some trash bytes where found we read two frames.

            Int32 startPosition = (Int32)mp3Stream.Position;
            long startOfMusicData = startPosition;

            // Woraround Erwin to detect start of music data
            Mp3Frame firstFrame = null;
            Mp3Frame currentFrame = null;
            Int32 goodFrames = 0;
            Int32 badFrames = 0;
            long streamPosition1 = mp3Stream.Position;
            long streamPosition2 = 0;
            long firstGoodFramePosition = 0;
            for (int i = 0; i < 1000; i++)
            {
                currentFrame = Mp3Frame.LoadFromStream(mp3Stream);
                streamPosition2 = mp3Stream.Position;

                if (currentFrame == null)
                {
                    Trace.WriteLine(_fileName);
                }

                if ((currentFrame != null) && (currentFrame.FrameLength == (streamPosition2 - streamPosition1)))
                {
                    goodFrames++;
                    if (firstGoodFramePosition == 0)
                    {
                        firstGoodFramePosition = streamPosition1;
                        firstFrame = currentFrame;
                    }
                    if (goodFrames == 10)
                    {
                        break;
                    }
                }
                else
                {
                    badFrames++;
                    goodFrames = 0;
                    firstGoodFramePosition = 0;
                }
                streamPosition1 = streamPosition2;
            }

            if (firstGoodFramePosition == 0)
            {
                firstGoodFramePosition = startOfMusicData;
            }

            startOfMusicData = firstGoodFramePosition;
            mp3Stream.Position = firstGoodFramePosition;

            return startOfMusicData;
        }
       
        /// <summary>
        /// Process the Xing frame which is usually found in VBR files
        /// </summary>
        /// <returns></returns>
        private Int32 ProcessVBRheader()
        {
            Int32 starPosition = (Int32)mp3Stream.Position;
            do
            {
                xingHeader = XingHeader.LoadXingHeader(mp3Frame);
                if (xingHeader != null)
                {
                    starPosition = (Int32)mp3Stream.Position;
                    mp3Frame = Mp3Frame.LoadFromStream(mp3Stream);
                }

            } while (xingHeader != null);

            do
            {
                lameHeader = LameHeader.LoadLameHeader(mp3Frame);
                if (lameHeader != null)
                {
                    starPosition = (Int32)mp3Stream.Position;
                    mp3Frame = Mp3Frame.LoadFromStream(mp3Stream);
                }

            } while ((lameHeader != null) && (mp3Frame != null));

            return starPosition;
        }

       

        /// <summary>
        /// Update the song object with some extra informations
        /// </summary>
        private void DefineSongInformation()
        {
            double seconds = (double)this.totalSamples / sampleRate;
            TimeSpan totalTime = TimeSpan.FromSeconds(seconds);

            if (bitrateType == BitrateType.VBR)
            {
                bitrate = (Int32)(mp3DataLength * 8 / seconds);
            }

            if (_song != null)
            {
                _song.BitrateType = bitrateType;
                _song.Bitrate = bitrate;
                _song.SampleRate = sampleRate;
                _song.ChannelMode = channelMode;
                _song.FileSize = fileSize;
                _song.Seconds = (Int32)seconds;
            }
        }

        /// <summary>
        /// Determine if this is an CBR or VBR file
        /// </summary>
        /// <param name="framesToCheck"></param>
        /// <returns></returns>
        private BitrateType TestBitrateType(Int32 framesToCheck)
        {
            BitrateType bt = BitrateType.CBR;
            long currentPosition = mp3Stream.Position;
            int lastBitRate = 0;
            Mp3Frame frame = null;
            do
            {
                frame = Mp3Frame.LoadFromStream(mp3Stream);
                if (frame != null)
                {
                    if ((lastBitRate > 0) && (lastBitRate != frame.BitRate))
                    {
                        bt = BitrateType.VBR;
                        framesToCheck = 0;
                    }
                    lastBitRate = frame.BitRate;
                }
                framesToCheck--;

            } while ((frame != null) && (framesToCheck > 0));
            mp3Stream.Position = currentPosition;
            return bt;
        }

        /// <summary>
        /// Count the number of samples in this MP3 file
        /// </summary>
        /// <param name="bitrateType"></param>
        /// <returns></returns>
        private Int32 GetSampleCount()
        {
            Int32 sampleCount = 0;

            if (bitrateType == BitrateType.CBR)
            {
                // duration = Byte count * 8 bit / bitrate (Bit/Second)
                Int32 seconds = (Int32)(mp3DataLength * (8 / (float)bitrate));
                sampleCount = seconds * sampleRate;
            }
            else 
            {
                if (tagReader != null)
                {
                    String ms = tagReader.GetStringValue(TagType.TLEN);
                    if (String.IsNullOrEmpty(ms) == false)
                    {
                        double milliseconds = Convert.ToDouble(ms);
                        Int32 seconds = (Int32)(milliseconds / 1000);
                        sampleCount = seconds * sampleRate;
                    }
                }

                if (sampleCount == 0)
                {
                    MemoryStream memStream = CreateMemoryStream(mp3Stream);
                    sampleCount = GetSampleCountFromStream(memStream);
                }
            }

            return sampleCount;
        }

        private Int32 GetSampleCountFromStream(MemoryStream memStream)
        {
            Int32 sampleCount = 0;
            try
            {
                // Read all frames of this file to determine the number of samples
                
                Int32 lastBitRate = 0;
                Mp3Frame frame = null;
                do
                {
                    try
                    {
                        frame = Mp3Frame.LoadFromStream(memStream);
                        if (frame != null)
                        {
                            sampleCount += frame.SampleCount;
                            lastBitRate = frame.BitRate;
                        }
                    }
                    catch
                    {
                        // ignore errors
                        frame = null;
                    }

                } while ((frame != null) && ((memStream.Position - 500) < memStream.Length));
            }
            catch (Exception Err)
            {
                String errorMessage = "Filename: " + _fileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            finally
            {
                memStream.Close();
            }

            return sampleCount;
        }
        private static MemoryStream CreateMemoryStream(Stream stream)
        {
            long currentPosition = stream.Position;
            Byte[] memBuffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(memBuffer, 0, (Int32)stream.Length);
            stream.Position = currentPosition;

            MemoryStream memStream = new MemoryStream(memBuffer);
            return memStream;
        }

        /// <summary>
        ///  Wether or not the TLEN tag exists already
        /// </summary>
        /// <returns></returns>
        public Boolean TLEN_Exists()
        {
            if (tagReader != null)
            {
                return tagReader.TLEN_Exists;
            }
            return false;
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

        ~Mp3Metaedit()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (tagReader != null)
            {
                tagReader.Dispose();
            }
            if (mp3Stream != null)
            {
                mp3Stream.Dispose();
            }


            
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
