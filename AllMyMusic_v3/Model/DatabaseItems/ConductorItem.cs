using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic
{
    public class ConductorItem 
    {
        private String _conductor = String.Empty;
        private Int32 _conductorId;

        /// <summary>
        /// The name of the Artist/conductor that created the album
        /// </summary>
        public String Conductor
        {
            get { return _conductor; }
            set { _conductor = value; }
        }

        /// <summary>
        /// The Database ID of the Artist/conductor
        /// </summary>
        public Int32 ConductorId
        {
            get { return _conductorId; }
            set { _conductorId = value; }
        }
       

        public ConductorItem()
        {
            _conductor = String.Empty;
            _conductorId = 0;
        }

        public ConductorItem(String conductor, Int32 conductorId)
        {
            _conductor = conductor;
            _conductorId = conductorId;
        }
    }
}
