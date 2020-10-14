using System;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    public class Id3Tag
    {
        // Date: 22 May 2011
        // Name: Erwin Kryszat
        // class created for code review

        private TagType tagType;
        private String stringValue;
        private String description;


        private Boolean allowEncoding;
        private Boolean isStandardTag;
        private Boolean isString;
        private Byte[] tagData;

        #region Properties
        
        public TagType TagType
        {
            get { return tagType; }
            set { tagType = value; }
        }

        
        public Boolean AllowEncoding
        {
            get { return allowEncoding; }
            set { allowEncoding = value; }
        }

       
        public Boolean IsString
        {
            get { return isString; }
            set { isString = value; }
        }

        public Boolean IsStandardTag
        {
            get { return isStandardTag; }
            set { isStandardTag = value; }
        }
        
        public String StringValue
        {
            get { return stringValue; }
            set 
            {
                stringValue = value;
                this.tagData = UnicodeData.EncodeStringValue(stringValue, true, CodingType.ISO_8859_1);
            }
        }

        public String Description
        {
            get  { return description; }
            set { description = value; }
        }

        public Int32 DataLength
        {
            get { return tagData.Length; }
        }

        public Int32 TotalSize
        {
            get { return (10 + tagData.Length); }
        }
        
        public Byte[] TagData
        {
            get { return tagData; }
            set
            {
                tagData = value;
            }
        }
        #endregion

        #region Constructor
        public Id3Tag()
        {

        }

        public Id3Tag(String tagName, Byte[] tagData)
        {
            this.tagData = tagData;
            DefineTagProperties(tagName);
            
        }

        public void SetStringValue(String value)
        {
            stringValue = value;
        }
        #endregion

        private void DefineTagProperties(String tagName)
        {
            for (int i = 0; i < TagProperties.Item.Length; i++)
            {
                if (TagProperties.Item[i].Name == tagName)
	            {
                    allowEncoding = TagProperties.Item[i].AllowEncoding;
                    isString = TagProperties.Item[i].IsStringValue;
                    isStandardTag = TagProperties.Item[i].IsStandardTag;
                    tagType = TagProperties.Item[i].TagType;
                    description = TagProperties.Item[i].Description;

                    if (isString == true)
                    {
                        stringValue = UnicodeData.DecodeStringValue(tagData);
                    }

                    if (tagType == TagType.PRIV)
                    {
                        if ((stringValue.IndexOf("AllMyMusic") >= 0) || (stringValue.IndexOf("AllMyMp3") >= 0))
                        {
                            isStandardTag = true;
                        }
                    }
	            }
            }
        }
    }
}
