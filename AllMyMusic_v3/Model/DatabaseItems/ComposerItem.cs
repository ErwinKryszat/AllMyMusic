using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic_v3
{
    public class ComposerItem 
    {
        private String _composer = String.Empty;
        private Int32 _composerId;

        /// <summary>
        /// The name of the Artist/composer that created the album
        /// </summary>
        public String Composer
        {
            get { return _composer; }
            set { _composer = value; }
        }

        /// <summary>
        /// The Database ID of the Artist/composer
        /// </summary>
        public Int32 ComposerId
        {
            get { return _composerId; }
            set { _composerId = value; }
        }
       

        public ComposerItem()
        {
            _composer = String.Empty;
            _composerId = 0;
        }

        public ComposerItem(String composer, Int32 composerId)
        {
            _composer = composer;
            _composerId = composerId;
        }
    }
}
