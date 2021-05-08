using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AllMyMusic
{
    public class TaskQueueItem
    {
        private ReportProgress_Callback _progressCallback;
        private WorkDone_Callback _workDoneCallback;
        private object _userData = null;
        private CancellationTokenSource _cts;
        private IBackgroundQueueJob _backgroundQueueJob;

        public TaskQueueItem(IBackgroundQueueJob backgroundQueueJob, object userData, ReportProgress_Callback progressCallback, CancellationTokenSource cts)
        {
            _backgroundQueueJob = backgroundQueueJob;
            _userData = userData;
            _progressCallback = progressCallback;
            _cts = cts;
        }

        public IBackgroundQueueJob BackgroundQueueJob
        {
            get { return _backgroundQueueJob; }
            set { _backgroundQueueJob = value; }
        }

        public ReportProgress_Callback ProgressCallback
        {
            get { return _progressCallback; }
            set { _progressCallback = value; }
        }

        public WorkDone_Callback WorkDoneCallback
        {
            get { return _workDoneCallback; }
            set { _workDoneCallback = value; }
        }

        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        public CancellationTokenSource CTS
        {
            get { return _cts; }
            set { _cts = value; }
        }
    }
}
