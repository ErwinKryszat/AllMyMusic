using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AllMyMusic_v3
{
    public class ToolTypeEventArgs : EventArgs
    {
        private ToolType _toolType;
        public ToolType ToolType
        {
            get { return _toolType; }
        }


        public ToolTypeEventArgs(ToolType toolType)
        {
            this._toolType = toolType;
        }
    }
}
