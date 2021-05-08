using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AllMyMusic
{
    public class ConnectionInfoEventArgs : EventArgs
    {
        private ConnectionInfo _dbConInfo;
        public ConnectionInfo DbConInfo
        {
            get { return _dbConInfo; }
        }


        public ConnectionInfoEventArgs(ConnectionInfo dbConInfo)
        {
            this._dbConInfo = dbConInfo;
        }
    }
}
