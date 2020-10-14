using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Read the APIC tag from an ID3V2 Tag. Convert the embedded byte data to an image object
    /// </summary>
    public class APIC : Id3Tag
    {
        //private String stringValue;
        private Image coverImage = null;
        private PictureType pictureType;

        /// <summary>
        /// Mime types supported in this class
        /// </summary>
        private static String[] mimeTypes = new String[5] 
        {
            "jpg",
            "image/jpg",
            "image/jpeg",
            "png",
            "image/png"
        };

        /// <summary>
        /// The image 
        /// </summary>
        public Image CoverImage
        {
            get 
            {
                return coverImage;
            }

            set 
            {
                coverImage = value;
                ConvertImageToBytes();
            }
        }

        /// <summary>
        /// Picure Type: Frontal or Backside
        /// </summary>
        public PictureType PictureType
        {
            get
            {
                return pictureType;
            }
        }

        /// <summary>
        /// Create an APIC tag
        /// </summary>
        /// <param name="coverImage"></param>
        /// <param name="picType"></param>
        public APIC(Image coverImage, PictureType pictureType)
        {
            this.pictureType = pictureType;
            this.coverImage = coverImage;

            ConvertImageToBytes();

            this.TagType = TagType.APIC;
            this.AllowEncoding = false;
        }

        /// <summary>
        /// Convert the Image object to a series of bytes
        /// The image is saved to a MemoryStream. 
        /// Then the mime type is copied to the byte array
        /// Then the MemoryStream is read into the byte array
        /// </summary>
        private void ConvertImageToBytes()
        {
            Byte[] MimeType = UnicodeData.EncodeStringValue("image/jpeg", true, 0);
            MemoryStream ms = new MemoryStream();
            this.coverImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            ms.Position = 0;

            this.TagData = new byte[MimeType.Length + 3 + ms.Length];
            MimeType.CopyTo(this.TagData, 0);
            this.TagData[MimeType.Length + 1] = (byte)this.pictureType;

            Int32 bytesRead = ms.Read(this.TagData, MimeType.Length + 3, (Int32)ms.Length);
            ms.Close();
        }

        /// <summary>
        /// Evaluate the series of bytes and read them into a new Image oobject
        /// </summary>
        /// <param name="tagData"></param>
        public APIC(Byte[] tagData)
        {
            this.TagData = tagData;
            this.TagType = TagType.APIC;
            this.AllowEncoding = false;

            Int32 imageStart = 0;
            String mimeType = UnicodeData.DecodeStringValue(this.TagData);
            Int32 pictureTypePosition = UnicodeData.IndexOfByte(this.TagData, 1, this.TagData.Length, 0) + 1;
            this.pictureType = (PictureType)this.TagData[pictureTypePosition];
            this.IsString = true;
            this.SetStringValue(mimeType);

            pictureTypePosition++;
            if (IsKnownMimeType(mimeType) == true)
            {
                imageStart = FindImageStart(pictureTypePosition);
            }

            if (imageStart > 0)
            {
                coverImage = ReadImage(imageStart);
            }
        }


        /// <summary>
        /// Check if the detected Mime type is supported in this class
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static Boolean IsKnownMimeType(String mimeType)
        {
            if (String.IsNullOrEmpty(mimeType) == false) 
            {
                mimeType = mimeType.ToLower();

                for (int i = 0; i < mimeTypes.Length; i++)
                {
                    if (mimeTypes[i] == mimeType)
                    {
                        return true;
                    }
                }
            } 
            return false;
        }

        /// <summary>
        /// Search a byte pattern that determines the start of the image data
        /// </summary>
        /// <param name="startSearchPosition"></param>
        /// <returns></returns>
        private Int32 FindImageStart(Int32 startSearchPosition)
        {
            for (int i = startSearchPosition; i < 100; i++)
            {
                // Search for FF D8 FF E0
                if ((this.TagData[i] == 0xFF) && (this.TagData[i + 1] == 0xD8) && (this.TagData[i + 2] == 0xFF) && (this.TagData[i + 3] == 0xE0))
                {
                    return i;
                }

                // Search for FF D8 FF E1
                if ((this.TagData[i] == 0xFF) && (this.TagData[i + 1] == 0xD8) && (this.TagData[i + 2] == 0xFF) && (this.TagData[i + 3] == 0xE1))
                {
                    return i;
                }

                // Search for 89 50 4E 47
                if ((this.TagData[i] == 0x89) && (this.TagData[i + 1] == 0x50) && (this.TagData[i + 2] == 0x4E) && (this.TagData[i + 3] == 0x47))
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// Read the image data from the TagData bytes
        /// </summary>
        /// <param name="startImageData"></param>
        /// <returns></returns>
        private Image ReadImage(Int32 startImageData)
        {
            try
            {
                MemoryStream reader = new MemoryStream(this.TagData, startImageData, this.TagData.Length - startImageData);
                return Image.FromStream(reader);
            }
            catch (EndOfStreamException Err)
            {
                throw new EndOfStreamException(Err.ToString());
            }
        }
    }
}
