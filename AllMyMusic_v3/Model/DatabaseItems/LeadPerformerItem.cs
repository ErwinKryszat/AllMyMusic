using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic
{
    public class LeadPerformerItem 
    {
        private String _leadPerformer = String.Empty;
        private Int32 _leadPerformerId;

        /// <summary>
        /// The name of the Artist/leadPerformer that created the album
        /// </summary>
        public String LeadPerformer
        {
            get { return _leadPerformer; }
            set { _leadPerformer = value; }
        }

        /// <summary>
        /// The Database ID of the Artist/leadPerformer
        /// </summary>
        public Int32 LeadPerformerId
        {
            get { return _leadPerformerId; }
            set { _leadPerformerId = value; }
        }
       

        public LeadPerformerItem()
        {
            _leadPerformer = String.Empty;
            _leadPerformerId = 0;
        }

        public LeadPerformerItem(String leadPerformer, Int32 leadPerformerId)
        {
            _leadPerformer = leadPerformer;
            _leadPerformerId = leadPerformerId;
        }
    }
}
