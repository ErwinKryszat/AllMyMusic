using System;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Read the PRIV tag from an ID3V2 Tag. This is the so called Private tag used for custom defined informations
    /// In AllMyMusic this tag is used to store the country name where the Band/Artist belongs to
    /// </summary>
    public class PRIV : Id3Tag
    {
        /// <summary>
        /// Country name where the Band/Artist belongs to
        /// </summary>
        private String country = String.Empty;
        public String Country
        {
            get 
            {
                return country;
            }

            set 
            { 
                country = value;
                base.StringValue = "AllMyMusic\0" + "TNAT=" + country;
                this.TagData = UnicodeData.EncodeStringValue(base.StringValue, true, CodingType.ISO_8859_1);
            }
        }

        /// <summary>
        /// Create a new PRIV tag from the country name
        /// </summary>
        /// <param name="country">Name of the country where the Band/Artist belongs to</param>
        public PRIV(String country)
        {
            this.country = country;
            base.StringValue = "AllMyMusic\0" + "TNAT=" + country;
            this.TagData = UnicodeData.EncodeStringValue(base.StringValue, true, CodingType.ISO_8859_1);
            this.TagType = TagType.PRIV;
            this.AllowEncoding = false;
            this.IsStandardTag = true;
        }

        /// <summary>
        /// Create a new PRIV tag from the byte data
        /// </summary>
        /// <param name="tagData"></param>
        public PRIV(Byte[] tagData)
        {
            this.TagData = tagData;
            this.TagType = TagType.PRIV;
            this.AllowEncoding = false;
            this.IsStandardTag = true;

            Int32 StartPos = 0;
            if (tagData[0] <= 5)
            {
                StartPos = 1;
            }
            base.StringValue = UnicodeData.DecodeLatinString(tagData, StartPos, DataLength - StartPos);
            if ((this.DataLength - StartPos) >= 10)
            {
                if ((base.StringValue.Substring(0, 10) == "AllMyMusic") || (base.StringValue.Substring(0, 8) == "AllMyMp3"))
                {
                    int pos = base.StringValue.IndexOf('=');
                    int len = base.StringValue.Length - pos - 1;
                    if (base.StringValue[base.StringValue.Length - 1] == 0)
                    {
                        len--;
                    }
                    if (pos > 0)
                    {
                        country = base.StringValue.Substring(pos + 1, len);
                    }
                }
            }
        }
   }
}


