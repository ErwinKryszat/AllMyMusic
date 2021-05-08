using System;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AllMyMusic.ViewModel
{
    public class MessageBoxViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private Boolean _messageBoxVisible;
        private Boolean _ok_ButtonVisible = true;
        private Boolean _cancel_ButtonVisible = true;
        private String _titleText;
        private String _captionText;
        private String _messageText;
        private String _exceptionText;
        private BitmapImage _severityImage;
        private MessageBoxImage _severityType;
        private DialogResult _dialogAnswer;
        private Action _okAction;
        private RelayCommand<object> _okCommand;
        private RelayCommand<object> _cancelCommand;
        #endregion // Fields

        #region Presentation Properties
        public DialogResult DialogAnswer
        {
            get { return _dialogAnswer; }
            set
            {
                if (value == _dialogAnswer) 
                    return;

                _dialogAnswer = value;
                RaisePropertyChanged("DialogAnswer");
            }
        }
        public String ExceptionText
        {
            get { return _exceptionText; }
            set
            {
                if (value == _exceptionText)
                    return;

                _exceptionText = value;
                RaisePropertyChanged("ExceptionText");
            }
        }
        public String TitleText
        {
            get { return _titleText; }
            set
            {
                _titleText = value;
                RaisePropertyChanged("TitleText");
            }
        }
        public String CaptionText
        {
            get { return _captionText; }
            set
            {
                _captionText = value;
                RaisePropertyChanged("CaptionText");
            }
        }
        public String MessageText
        {
            get { return _messageText; }
            set
            {
                if (value == _messageText) 
                    return;

                _messageText = value;
                RaisePropertyChanged("MessageText");
            }
        }
        public Boolean MessageBoxVisible
        {
            get { return _messageBoxVisible; }
            set
            {
                if (value == _messageBoxVisible) 
                    return;

                _messageBoxVisible = value;

                if (_messageBoxVisible == false)
                {
                    OnVisibilityOff();
                }

                RaisePropertyChanged("MessageBoxVisible");
            }
        }
        public BitmapImage SeverityImage
        {
            get { return _severityImage; }
            set
            {
                _severityImage = value;

                RaisePropertyChanged("SeverityImage");
            }
        }
        public Boolean OK_ButtonVisible
        {
            get { return _ok_ButtonVisible; }
            set
            {
                if (value == _ok_ButtonVisible)
                    return;

                _ok_ButtonVisible = value;

                RaisePropertyChanged("OK_ButtonVisible");
            }
        }
        public Boolean Cancel_ButtonVisible
        {
            get { return _cancel_ButtonVisible; }
            set
            {
                if (value == _cancel_ButtonVisible)
                    return;

                _cancel_ButtonVisible = value;

                RaisePropertyChanged("Cancel_ButtonVisible");
            }
        }
        public Action OkAction
        {
            get { return _okAction; }
            set
            {
                if (value == _okAction)
                    return;

                _okAction = value;
                RaisePropertyChanged("OkAction");
            }
        }
      
        #endregion // Presentation Properties

        #region Commands
        public ICommand OkCommand
        {
            get
            {
                if (null == _okCommand)
                    _okCommand = new RelayCommand<object>(ExecuteOkCommand, CanOkCommand);

                return _okCommand;
            }
        }
        private void ExecuteOkCommand(object notUsed)
        {
            if (_okAction != null)
            {
                _okAction.Invoke();
            }
            DialogAnswer = DialogResult.OK;
            MessageBoxVisible = false;
        }
        private bool CanOkCommand(object notUsed)
        {
            return true;
        }

        public ICommand CancelCommand
        {
            get
            {
                if (null == _cancelCommand)
                    _cancelCommand = new RelayCommand<object>(ExecuteCancelCommand, CanCancelCommand);

                return _cancelCommand;
            }
        }
        private void ExecuteCancelCommand(object notUsed)
        {
            DialogAnswer = DialogResult.Cancel;
            MessageBoxVisible = false;
        }
        private bool CanCancelCommand(object notUsed)
        {
            return true;
        }    
        #endregion // Commands

        #region Constructor
        public MessageBoxViewModel()
        {
            Localize();
        }
        public MessageBoxViewModel(String message)
        {
            CaptionText = "Please excuse me for this error in AllMyMusic";
            MessageText = message;
            _severityType = MessageBoxImage.Error;
            _exceptionText = String.Empty;
            LoadSeverityImage();

            Localize();
        }

        public MessageBoxViewModel(String message, Exception exception)
        {
            CaptionText = "Please excuse me for this error in AllMyMusic";
            MessageText = message;
            _severityType = MessageBoxImage.Error;
            _exceptionText = exception.ToString();
            LoadSeverityImage();

            Localize();
        }
        public MessageBoxViewModel(String message, AggregateException ae)
        {
            CaptionText = "Please excuse me for this error in AllMyMusic";
            MessageText = message;
            _severityType = MessageBoxImage.Error;

            StringBuilder sbExceptionText = new StringBuilder();
            sbExceptionText.Append(message);
            sbExceptionText.Append(Environment.NewLine + Environment.NewLine);

            foreach (var e in ae.InnerExceptions)
            {
                sbExceptionText.Append(e.ToString());
                sbExceptionText.Append(Environment.NewLine + Environment.NewLine);
            }

            _exceptionText = sbExceptionText.ToString();
            LoadSeverityImage();

            Localize();
        }
        public MessageBoxViewModel(String caption, String message, Exception exception, MessageBoxImage severityType)
        {
            CaptionText = caption;
            MessageText = message;
            _severityType = severityType;

            LoadSeverityImage();

            Localize();
        }
        public MessageBoxViewModel(MessageBoxButtons buttons)
        {
            if (buttons == MessageBoxButtons.OK_Cancel)
            {
                _ok_ButtonVisible = true;
                _cancel_ButtonVisible = true;
            }
            if (buttons == MessageBoxButtons.OK_only)
            {
                _ok_ButtonVisible = true;
                _cancel_ButtonVisible = false;
            }

            Localize();
        }
        #endregion  // Constructor


        private void OnVisibilityOff()
        {
            _captionText = String.Empty;
            _messageText = String.Empty;
            _exceptionText = String.Empty;
            _okAction = null;
        }

        private void LoadSeverityImage()
        {
            switch (_severityType)
            {
                case MessageBoxImage.Error:
                    SeverityImage = new BitmapImage(new Uri(Global.Images + "error.png", UriKind.Relative));
                    break;
                case MessageBoxImage.Question:
                    SeverityImage = new BitmapImage(new Uri(Global.Images + "help.png", UriKind.Relative));
                    break;
                case MessageBoxImage.Warning:
                    SeverityImage = new BitmapImage(new Uri(Global.Images + "warning.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
        }
        #region Events

        #endregion // Events

        #region Localization
        private String _okButtonText;
        private String _cancelButtonText;

        public String OkButtonText
        {
            get { return _okButtonText; }
            set
            {
                if ((value == _okButtonText) || (value == null))
                    return;

                _okButtonText = value;
                RaisePropertyChanged("OkButtonText");
            }
        }
        public String CancelButtonText
        {
            get { return _cancelButtonText; }
            set
            {
                if ((value == _cancelButtonText) || (value == null))
                    return;

                _cancelButtonText = value;
                RaisePropertyChanged("CancelButtonText");
            }
        }

        public void Localize()
        {
            TitleText = "AllMyMusic Message";
            OkButtonText = AmmLocalization.GetLocalizedString("Common_OK");
            CancelButtonText = AmmLocalization.GetLocalizedString("Common_Cancel");
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

        ~MessageBoxViewModel()
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
