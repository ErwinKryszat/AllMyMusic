using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic_v3
{
    public class LanguageItem 
    {
        private String _language = String.Empty;
        private Int32 _languageId;

        /// <summary>
        /// The name of the Artist/language that created the album
        /// </summary>
        public String Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// The Database ID of the Artist/language
        /// </summary>
        public Int32 LanguageId
        {
            get { return _languageId; }
            set { _languageId = value; }
        }
       

        public LanguageItem()
        {
            _language = String.Empty;
            _languageId = 0;
        }

        public LanguageItem(String language, Int32 languageId)
        {
            _language = language;
            _languageId = languageId;
        }
    }
}
