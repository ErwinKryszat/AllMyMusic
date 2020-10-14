using System;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Read the POPM tag from an ID3V2 Tag. This is the popularimeter. Better known as "Rating" as a series of star images
    /// </summary>
    public class POPM : Id3Tag
    {
        private String stringValue;
        private Int32 rating = 0;

        /// <summary>
        /// The rating for the song. Values 0 to 255.
        /// </summary>
        public Int32 Rating
        {
            get 
            {
                return rating;
            }

            set 
            { 
                rating = value;
                this.TagData = new Byte[10];
                Byte[] email = UnicodeData.EncodeStringValue("no@email", false, 0);
                email.CopyTo(this.TagData, 0);
                this.TagData[9] = Convert.ToByte(rating);
                this.stringValue = rating.ToString();
            }
        }


        /// <summary>
        /// Create a new POPM tag from a rating value
        /// </summary>
        /// <param name="rating"></param>
        public POPM(Int32 rating)
        {
            this.rating = rating;
            this.TagData = new Byte[10];
            Byte[] email = UnicodeData.EncodeStringValue("no@email", false, 0);
            email.CopyTo(this.TagData, 0);
            this.TagData[9] = Convert.ToByte(rating);
            this.TagType = TagType.POPM;
            this.IsStandardTag = true;
            this.AllowEncoding = false;
            this.IsString = true;
            this.SetStringValue(rating.ToString());
        }

        /// <summary>
        /// Create a new POPM tag from the byte data
        /// </summary>
        /// <param name="tagData"></param>
        public POPM(Byte[] tagData)
        {
            this.TagData = tagData;
            this.TagType = TagType.POPM;
            this.IsStandardTag = true;
            this.AllowEncoding = false;
            this.IsString = true;

            int pos = UnicodeData.IndexOfByte(this.TagData, 0, this.DataLength, 0) + 1;
            rating = this.TagData[pos];
            this.SetStringValue(rating.ToString());
        }
    }
}
