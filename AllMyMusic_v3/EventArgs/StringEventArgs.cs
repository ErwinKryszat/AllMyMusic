using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AllMyMusic
{
    public class StringEventArgs : EventArgs
    {
        private String _name;
        public String Name
        {
            get { return _name; }
        }


        public StringEventArgs(String name)
        {
            this._name = name;
        }
    }
}
