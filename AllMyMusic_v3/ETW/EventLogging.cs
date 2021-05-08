using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace AllMyMusic
{

    internal sealed class EventLogging
    {
        public static readonly EventLogging Write = new EventLogging();

        private static bool IsEnabled = true;

        internal const string sSource = "AllMyMusic";
        internal const string sLog = "Application";
              
  
        public EventLogging()
        {
            try
            {
                if (!EventLog.SourceExists(sSource))
                {
                    EventLog.CreateEventSource(sSource, sLog);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("When running AllMyMusic for the first time, please use 'Run As Administrator'", "Error", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Error(string message, int eventId)
        {
            if (IsEnabled == true)
            {
                EventLog.WriteEntry(sSource, message, EventLogEntryType.Error, eventId);
            }
        }

        public void Error(Exception err, string message, int eventId)
        {
            if (IsEnabled == true)
            {
                EventLog.WriteEntry(sSource, message + Environment.NewLine + Environment.NewLine + err.Message, EventLogEntryType.Error, eventId);
            }
        }

        public void Error(AggregateException ae, string message, int eventId)
        {
            if (IsEnabled == true)
            {
                StringBuilder sbExceptionText = new StringBuilder();
                sbExceptionText.Append(message);
                sbExceptionText.Append(Environment.NewLine + Environment.NewLine);

                foreach (var e in ae.InnerExceptions)
                {
                    sbExceptionText.Append(e.Message);
                    sbExceptionText.Append(Environment.NewLine + Environment.NewLine);
                }

                EventLog.WriteEntry(sSource, sbExceptionText.ToString(), EventLogEntryType.Error, eventId);
            }
        }

        public void Warning(string message, int eventId)
        {
            if (IsEnabled == true)
            {
                EventLog.WriteEntry(sSource, message, EventLogEntryType.Warning, eventId);
            }
        }

        public void Information(string message, int eventId)
        {
            if (IsEnabled == true)
            {
                EventLog.WriteEntry(sSource, message, EventLogEntryType.Information, eventId);
            }
        }

        public void SuccessAudit(string message, int eventId)
        {
            if (IsEnabled == true)
            {
                EventLog.WriteEntry(sSource, message, EventLogEntryType.SuccessAudit, eventId);
            }
        }

        public void FailureAudit(string message, int eventId)
        {
            if (IsEnabled == true)
            {
                EventLog.WriteEntry(sSource, message, EventLogEntryType.FailureAudit, eventId);
            }
        }
    }
}
