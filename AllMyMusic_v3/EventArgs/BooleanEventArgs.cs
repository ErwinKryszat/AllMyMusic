using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AllMyMusic
{
    public class BooleanEventArgs : EventArgs
    {
        private Boolean _value;
        public Boolean Value
        {
            get { return _value; }
        }


        public BooleanEventArgs(Boolean value)
        {
            this._value = value;
        }
    }
}
