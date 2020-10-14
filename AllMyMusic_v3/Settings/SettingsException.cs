using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace AllMyMusic_v3.Settings
{
    [Serializable()]
    public class SettingsException : ApplicationException
    {
        private DateTime errorTime = DateTime.Now;
        public DateTime ErrorTime 
        {
            get { return this.errorTime; }
        }

        private String detailedMessage = String.Empty;
        public String DetailedMessage
        {
            get { return this.detailedMessage; }
        }

        public SettingsException() : base() { }

         public SettingsException( String message) 
            : base(message)
        {
            this.errorTime = DateTime.Now;
            this.detailedMessage = message;
        }
       
        public SettingsException( String message, Exception inner) 
            : base(message, inner)
        {
            this.errorTime = DateTime.Now;
            this.detailedMessage = message;
        }

        public SettingsException(String message, String detailed)
            : base(message)
        {
            this.errorTime = DateTime.Now;
            this.detailedMessage = detailed;
        }

        public SettingsException(String message, String detailed, Exception inner)
            : base(message, inner)
        {
            this.errorTime = DateTime.Now;
            this.detailedMessage = detailed;
        }

        public SettingsException(SerializationInfo info, StreamingContext context) 
            : base(info, context) 
        {
            this.errorTime = info.GetDateTime("ErrorTime");
            this.detailedMessage = info.GetString("DetailedMessage");
        }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("ErrorTime", this.errorTime);
            info.AddValue("DetailedMessage", this.detailedMessage);
        }
    }
}
