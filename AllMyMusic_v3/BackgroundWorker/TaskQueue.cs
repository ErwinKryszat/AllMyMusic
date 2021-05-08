using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AllMyMusic.ViewModel;

namespace AllMyMusic
{
    public delegate void ReportProgress_Delegate(object[] progressParams);
    public delegate void ReportProgress_Callback(ProgressDataViewModel progressData);
    public delegate void WorkDone_Callback();


    public class TaskQueue :IDisposable
    {
        #region Fields
        private static Queue<TaskQueueItem> _queue = null;
        private TaskQueueItem _backgroundJob;
        private static bool _busy = false;
        private static object lockObject = new object();
        

        private Int32 _totalTasks;
        private Int32 _completedTasks;
        #endregion



        public Int32 TotalTasks
        {
            get { return _totalTasks; }
            set { _totalTasks = value; }
        }

        public Int32 CompletedTasks
        {
            get { return _completedTasks; }
            set { _completedTasks = value; }
        }

        public TaskQueue()
        {
            _queue = new Queue<TaskQueueItem>();
            _totalTasks = 0;
            _completedTasks = 0;
        }

        public void CancelAll()
        {
            if (_backgroundJob.CTS != null)
            {
                _backgroundJob.CTS.Cancel();
            }

            lock (lockObject)
            {
                _queue.Clear();
            }
        }

        public void CancelCurrent()
        {
            if (_backgroundJob.CTS != null)
            {
                _backgroundJob.CTS.Cancel();
            }
        }

        public void Enqueue(TaskQueueItem backgroundJob)
        {
            lock (lockObject)
            {
                _totalTasks++;

                if (_busy)
                    _queue.Enqueue(backgroundJob);
                else
                {
                    _busy = true;
                    _backgroundJob = backgroundJob;
                    _backgroundJob.WorkDoneCallback = BackgroundJobDone;

                    Task.Run(() => _backgroundJob.BackgroundQueueJob.DoWork(_backgroundJob), _backgroundJob.CTS.Token);

                }
                EventArgs args = new EventArgs();
                OnJobAdded(this, args);
            }
        }

        public delegate void TaskDelegate();

        private void BackgroundJobDone()
        {
            _completedTasks++;
            EventArgs args = new EventArgs();
            OnJobCompleted(this, args);

            NextTask();
        }

        private void NextTask()
        {
            lock (lockObject)
            {
                if (_queue.Count > 0)
                {
                    _backgroundJob = _queue.Dequeue();
                    _backgroundJob.WorkDoneCallback = BackgroundJobDone;

                    Task.Run(() => _backgroundJob.BackgroundQueueJob.DoWork(_backgroundJob), _backgroundJob.CTS.Token);

                }
                else
                { 
                    _busy = false;
                    EventArgs args = new EventArgs();
                    OnAllJobsCompleted(this, args);
                }   
            }
        }

        #region Events
        public delegate void JobAddedEventHandler(object sender, EventArgs e);
        public event JobAddedEventHandler JobAdded;
        protected virtual void OnJobAdded(object sender, EventArgs e)
        {
            if (this.JobAdded != null)
            {
                this.JobAdded(this, e);
            }
        }

        public delegate void JobCompletedEventHandler(object sender, EventArgs e);
        public event JobCompletedEventHandler JobCompleted;
        protected virtual void OnJobCompleted(object sender, EventArgs e)
        {
            if (this.JobCompleted != null)
            {
                this.JobCompleted(this, e);
            }
        }

        public delegate void AllJobsCompletedEventHandler(object sender, EventArgs e);
        public event AllJobsCompletedEventHandler AllJobsCompleted;
        protected virtual void OnAllJobsCompleted(object sender, EventArgs e)
        {
            _totalTasks = 0;
            _completedTasks = 0;

            if (this.AllJobsCompleted != null)
            {
                this.AllJobsCompleted(this, e);
            }
        }

        #endregion // Events


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

        ~TaskQueue()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {

        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
