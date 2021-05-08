using System;
using System.Text;


namespace AllMyMusic.ViewModel
{
    public class ProgressDataViewModel : ViewModelBase
    {
        private Int32 _progressValue;
        private Int32 _progressMaximum;
        private String _currentFolder = String.Empty;
        private String _fileCount = String.Empty;
        private String _folderCount = String.Empty;
        private String _timeElapsed = String.Empty;
        private String _timeRemaining = String.Empty;
        private String _actionName = String.Empty;

        private Boolean _fileWriteAccessDenied;
        

        #region Properties
        public String ActionName
        {
            get { return _actionName; }
            set
            {
                if (value == _actionName)
                    return;

                _actionName = value;

                RaisePropertyChanged("ActionName");
            }
        }

        public Boolean FileWriteAccessDenied
        {
            get { return _fileWriteAccessDenied; }
            set
            {
                if (value == _fileWriteAccessDenied)
                    return;

                _fileWriteAccessDenied = value;

                RaisePropertyChanged("FileWriteAccessDenied");
            }
        }
        public Int32 ProgressValue
        {
            get { return _progressValue; }
            set
            {
                if (value == _progressValue)
                    return;

                _progressValue = value;

                RaisePropertyChanged("ProgressValue");
            }
        }

        public Int32 ProgressMaximum
        {
            get { return _progressMaximum; }
            set
            {
                if (value == _progressMaximum)
                    return;

                _progressMaximum = value;

                RaisePropertyChanged("ProgressMaximum");
            }
        }

        public String CurrentFolder
        {
            get { return _currentFolder; }
            set
            {
                if (value == _currentFolder)
                    return;

                _currentFolder = value;

                RaisePropertyChanged("CurrentFolder");
            }
        }

        public String FileCount
        {
            get { return _fileCount; }
            set
            {
                if (value == _fileCount)
                    return;

                _fileCount = value;

                RaisePropertyChanged("FileCount");
            }
        }

        public String FolderCount
        {
            get { return _folderCount; }
            set
            {
                if (value == _folderCount)
                    return;

                _folderCount = value;

                RaisePropertyChanged("FolderCount");
            }
        }

        public String TimeElapsed
        {
            get { return _timeElapsed; }
            set
            {
                if (value == _timeElapsed)
                    return;

                _timeElapsed = value;

                RaisePropertyChanged("TimeElapsed");
            }
        }
        public String TimeRemaining
        {
            get { return _timeRemaining; }
            set
            {
                if (value == _timeRemaining)
                    return;

                _timeRemaining = value;

                RaisePropertyChanged("TimeRemaining");
            }
        }
        #endregion

        public ProgressDataViewModel()
        {

        }
    }
}
