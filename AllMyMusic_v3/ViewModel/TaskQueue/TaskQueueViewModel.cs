using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;


namespace AllMyMusic_v3.ViewModel
{
    public class TaskQueueViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ProgressDataViewModel _progressData;
        private TaskQueue _taskQueue;
        private RelayCommand<object> _cancelAllTasksCommand;
        private RelayCommand<object> _cancelOneTasksCommand;
        #endregion // Fields

        #region Presentation Properties
        public ProgressDataViewModel ProgressData
        {
            get { return _progressData; }
            set
            {
                _progressData = value;

                RaisePropertyChanged("ProgressData");
            }
        }
        public TaskQueue TaskQueue
        {
            get { return _taskQueue; }
            set
            {
                _taskQueue = value;

                RaisePropertyChanged("TaskQueue");
            }
        }
        #endregion

        #region Commands
        public ICommand CancelAllCommand
        {
            get
            {
                if (null == _cancelAllTasksCommand)
                    _cancelAllTasksCommand = new RelayCommand<object>(ExecuteCancelAllCommand, CanCancelAllCommand);

                return _cancelAllTasksCommand;
            }
        }
        private void ExecuteCancelAllCommand(object _notUdes)
        {
            _taskQueue.CancelAll();
        }
        private bool CanCancelAllCommand(object _notUdes)
        {
            return (_taskQueue.TotalTasks > 0);
        }

         public ICommand CancelOneCommand
        {
            get
            {
                if (null == _cancelOneTasksCommand)
                    _cancelOneTasksCommand = new RelayCommand<object>(ExecuteCancelOneCommand, CanCancelOneCommand);

                return _cancelOneTasksCommand;
            }
        }
        private void ExecuteCancelOneCommand(object _notUdes)
        {
            _taskQueue.CancelCurrent();
        }
        private bool CanCancelOneCommand(object _notUdes)
        {
            return (_taskQueue.TotalTasks > 0);
        }
        #endregion

        #region Localization

        #endregion

        #region Constructor
        public TaskQueueViewModel(TaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
            _taskQueue.JobAdded += new AllMyMusic_v3.TaskQueue.JobAddedEventHandler(TaskQueue_JobCompleted);
            _taskQueue.JobCompleted += new TaskQueue.JobCompletedEventHandler(TaskQueue_JobCompleted);
            //_taskQueue.AllJobsCompleted += new TaskQueue.AllJobsCompletedEventHandler(TaskQueue_JobCompleted);
        }

        private void TaskQueue_JobCompleted(object sender, EventArgs e)
        {
            RaisePropertyChanged("TaskQueue");
        }
        #endregion

        #region Events
        //public delegate void CancelRequestEventHandler(object sender, EventArgs e);
        //public event CancelRequestEventHandler CancelRequest;
        //protected virtual void OnCancelRequest(object sender, EventArgs e)
        //{
        //    if (this.CancelRequest != null)
        //    {
        //        this.CancelRequest(this, e);
        //    }
        //}

        #endregion // Events

        #region Private Helpers
       
        //private void ViewVAChanged()
        //{

        //}

        #endregion // Private Helpers

        #region Localization
        public void Localize()
        {

        }
        #endregion // Localization

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

        ~TaskQueueViewModel()
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
