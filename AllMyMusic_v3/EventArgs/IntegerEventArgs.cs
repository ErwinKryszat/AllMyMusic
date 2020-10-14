using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AllMyMusic_v3
{
    public class IntegerEventArgs : EventArgs
    {
        private Int32 _value;
        public Int32 Value
        {
            get { return _value; }
        }


        public IntegerEventArgs(Int32 value)
        {
            this._value = value;
        }
    }
}
