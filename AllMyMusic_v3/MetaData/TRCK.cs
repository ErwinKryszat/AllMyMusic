using System;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Read the TRCK tag from an ID3V2 Tag. This is the track number
    /// As this is a sring we might find strings like 03/12 or 03\12 or 03.12
    /// For AllMyMusic we only want the track number, not the count of songs in the album
    /// </summary>
    public class TRCK : Id3Tag
    {
        private Char[] separator = new Char[3] { '.', '/', '\\' };

        /// <summary>
        /// Create a new TRCK tag from the track number
        /// </summary>
        /// <param name="track"></param>
        public TRCK(String trackNumber)
        {
            this.TagType = TagType.TRCK;
            this.StringValue = trackNumber;
            this.TagData = UnicodeData.EncodeStringValue_Latin(trackNumber);
            this.AllowEncoding = true;
            this.IsString = true;
            this.IsStandardTag = true;
        }

        /// <summary>
        /// Create a new TRCK tag from the byte data
        /// </summary>
        /// <param name="tagData"></param>
        public TRCK(Byte[] tagData)
        {
            this.TagType = TagType.TRCK;
            this.TagData = tagData;
            this.AllowEncoding = true;
            this.IsString = true;
            this.IsStandardTag = true;
            DefineValue();
        }

        private void DefineValue()
        {
            this.StringValue = UnicodeData.DecodeStringValue(this.TagData);
            Int32 position = this.StringValue.IndexOfAny(separator);
            if (position > 0)
            {
                this.StringValue = this.StringValue.Substring(0, position);
            }
        }
    }
}
