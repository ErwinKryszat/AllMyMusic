using System;
using System.Text;
using System.Diagnostics.Tracing;

namespace AllMyMusic
{
    [EventSource(Name = "AllMyMusic")]
    internal sealed class PerformanceLogging : EventSource
    {
        // public static readonly PerformanceLogging Write = new PerformanceLogging();

        public class Keywords
        {
            public const EventKeywords General = (EventKeywords)1;
            public const EventKeywords Database = (EventKeywords)2;
            public const EventKeywords FileSystem = (EventKeywords)3;
            public const EventKeywords AudioProcessing = (EventKeywords)4;
            public const EventKeywords UserInterface = (EventKeywords)5;
        }

        internal const int ErrorId = 1;
        internal const int WarningId = 2;
        internal const int ApplicationStartId = 3;
        internal const int ApplicationFinishedId = 4;


        [Event(ErrorId, Level = EventLevel.Informational, Keywords = Keywords.General)]
        public void Error(string message)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(ErrorId, message ?? string.Empty);
            }
        }

        [Event(WarningId, Level = EventLevel.Informational, Keywords = Keywords.General)]
        public void Warning(string message)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(WarningId, message ?? string.Empty);
            }
        }

        [Event(ApplicationStartId, Level = EventLevel.Informational, Keywords = Keywords.General)]
        public void ApplicationStart()
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(ApplicationStartId);
            }
        }

        [Event(ApplicationFinishedId, Level = EventLevel.Informational, Keywords = Keywords.General)]
        public void ApplicationFinished()
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(ApplicationFinishedId);
            }
        }


    }
}
