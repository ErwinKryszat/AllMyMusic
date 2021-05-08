using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AllMyMusic.ViewModel;

namespace AllMyMusic
{
    public static class ShowError
    {
        public static void ShowAndLog(Exception Err, String errorMessage, Int32 eventId)
        {
            EventLogging.Write.Error(Err, errorMessage, eventId);
            MessageBoxViewModel vmMessage = new MessageBoxViewModel(errorMessage, Err);
            frmMessage frmMsg = new frmMessage();
            frmMsg.DataContext = vmMessage;
            frmMsg.ShowDialog();
        }

        public static void ShowAndLog(AggregateException ae, String errorMessage, Int32 eventId)
        {
            EventLogging.Write.Error(ae, errorMessage, eventId);
            MessageBoxViewModel vmMessage = new MessageBoxViewModel(errorMessage, ae);
            frmMessage frmMsg = new frmMessage();
            frmMsg.DataContext = vmMessage;
            frmMsg.ShowDialog();
        }

        public static void ShowAndLog(String errorMessage, Int32 eventId)
        {
            EventLogging.Write.Error(errorMessage, eventId);
            MessageBoxViewModel vmMessage = new MessageBoxViewModel(errorMessage);
            frmMessage frmMsg = new frmMessage();
            frmMsg.DataContext = vmMessage;
            frmMsg.ShowDialog();
        }

        public static void Show(String message)
        {
            MessageBoxViewModel vmMessage = new MessageBoxViewModel(message);
            frmMessage frmMsg = new frmMessage();
            frmMsg.DataContext = vmMessage;
            frmMsg.ShowDialog();
        }
    }
}
