using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using AllMyMusic;

namespace Metadata.ID3
{

    /// <summary>
    /// This class is used to:
    /// Read the ID3V2 Tags into a TagCollection
    /// </summary>
    public class TagReader : IDisposable
    {
        // Date: 22 May 2011
        // Name: Erwin Kryszat
        // class created for code review
        private int id3v2TagSize = 0;
        private int dataLength = 0;
        private Id3TagCollection tagCollection = null;
        private Boolean tlenExists = false;
        private String tagIdentifier;
        private byte tagVersion;
        private byte tagRevision;
        private byte tagFlags;
        private byte[] headerBytes;
        private BinaryReader reader;

        #region Properties
        /// <summary>
        /// Size of needed to store all ID3V2 tags including the ID3V2 header
        /// </summary>
        public Int32 DataLength
        {
            get { return dataLength; }
        }

        /// <summary>
        /// Size of needed to store all ID3V2 tags
        /// </summary>
        public Int32 ID3v2TagSize
        {
            get { return id3v2TagSize; }
        }
        
        /// <summary>
        /// Collection of ID3V2 tags
        /// </summary>
        public Id3TagCollection TagCollection
        {
            get { return tagCollection; }
        }
        
        /// <summary>
        /// Wether the TLEN tag exists in the TagCollection
        /// </summary>
        public Boolean TLEN_Exists
        {
            get { return tlenExists; }
            set { tlenExists = value; }
        }

        /// <summary>
        /// Identifier Tag ("ID3")
        /// </summary>
        public String TagIdentifier
        {
            get { return tagIdentifier; }
        }

       
        /// <summary>
        /// Version of the ID3V2 Tag 
        /// </summary>
        public byte TagVersion
        {
            get { return tagVersion; }
        }

        /// <summary>
        /// Revision of the ID3V2 Tag 
        /// </summary>
        public byte TagRevision
        {
            get { return tagRevision; }
        }

        /// <summary>
        /// Flags. Currently not used in AllMyMusic
        /// </summary>
        public byte TagFlags
        {
            get { return tagFlags; }
        }
        #endregion

        #region TagParser
        /// <summary>
        /// Read the ID3V2 tags
        /// </summary>
        /// <param name="mp3Stream"></param>
        public TagReader(Stream mp3Stream)
        {
            ReadTags(mp3Stream);
        }

        /// <summary>
        /// Read the ID3V2 tags
        /// </summary>
        /// <param name="fileName"></param>
        public TagReader(String fileName)
        {
            FileStream mp3Stream = null;
            try
            {
                mp3Stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                ReadTags(mp3Stream);
            }
            catch (FieldAccessException)
            {
               
            }
            finally
            {
                if (mp3Stream != null)
                {
                    mp3Stream.Close();
                }
            }
        }

        /// <summary>
        /// Read the ID3V2 tags
        /// </summary>
        /// <param name="mp3Stream"></param>
        private void ReadTags(Stream mp3Stream)
        {
            try
            {
                tagCollection = new Id3TagCollection();
                this.reader = new BinaryReader(mp3Stream);
                this.headerBytes = reader.ReadBytes(10);

                if ((headerBytes[0] == (byte)'I') &&
                    (headerBytes[1] == (byte)'D') &&
                    (headerBytes[2] == '3'))
                {
                    ReadHeader();
                    ParseData();
                }
                else
                {
                    mp3Stream.Position -= 10;
                }
            }
            catch (EndOfStreamException)
            {

            }
        }

        /// <summary>
        /// Read the header that identifies the bytes as ID3 tags uncluding the synchsafe size information
        /// </summary>
        private void ReadHeader()
        {
            // http://www.id3.org/develop.html
            // OK found an ID3 tag
            // bytes 3 & 4 are ID3v2 version
            tagIdentifier = "ID3";
            tagVersion = headerBytes[3];
            tagRevision = headerBytes[4];
            tagFlags = headerBytes[5];

            if ((headerBytes[5] & 0x40) == 0x40)
            {
                // extended header present
                byte[] extendedHeader = reader.ReadBytes(4);
                int extendedHeaderLength = extendedHeader[0] * (1 << 21);
                extendedHeaderLength += extendedHeader[1] * (1 << 14);
                extendedHeaderLength += extendedHeader[2] * (1 << 7);
                extendedHeaderLength += extendedHeader[3];
            }

            // synchsafe
            id3v2TagSize = headerBytes[6] * (1 << 21);
            id3v2TagSize += headerBytes[7] * (1 << 14);
            id3v2TagSize += headerBytes[8] * (1 << 7);
            id3v2TagSize += headerBytes[9];

            dataLength = id3v2TagSize + 10;

            if ((headerBytes[5] & 0x10) == 0x10)
            {
                // footer present
                byte[] footer = reader.ReadBytes(10);
            }
        }

        /// <summary>
        /// Read the individual ID3V2 tags and add them to the TagCollection
        /// </summary>
        private void ParseData()
        {
            String tagName = String.Empty;
            Int32 tagSize = 0;

            try
            {
                do
                {
                    byte[] tagData = null;
                    byte[] tagHeader = reader.ReadBytes(10);

                    // Get the Tag Name
                    tagName = GetTagName(tagHeader);

                    // Get the Tag Size
                    if (String.IsNullOrEmpty(tagName) == false)
                    {
                        if (tagName == "TLEN")
                        {
                            tlenExists = true;
                        }
                        tagSize = GetTagSize(tagHeader);

                        // Get the Tag Data portion and create the ID3Tag
                        if (tagSize > 0)
                        {
                            tagData = reader.ReadBytes(tagSize);
                            Id3Tag tag = CreateTag(tagName, tagData);
                            tagCollection.Add(tag);
                        }
                    }
                } while ((reader.BaseStream.Position < dataLength) && (tagName != String.Empty));
            }
            catch (EndOfStreamException)
            {
                throw;
            }
        }

        /// <summary>
        /// Create the individual ID3V2 tags from the bytes read
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="tagData"></param>
        /// <returns></returns>
        private static Id3Tag CreateTag(String tagName, Byte[] tagData)
        {
            Id3Tag tag = null;
 
            switch (tagName)
            {
                case "APIC":
                    if (tagData.Length > 2048)
                    {
                        tag = new APIC(tagData);
                        tag.Description = TagProperties.GetTagDescription("APIC");
                    }
                    else
                    {
                        tag = new Id3Tag("XXXX", tagData);
                    }
                    break;
                case "POPM":
                    tag = new POPM(tagData);
                    tag.Description = TagProperties.GetTagDescription("POPM");
                    break;
                case "PRIV":
                    tag = new PRIV(tagData);
                    tag.Description = TagProperties.GetTagDescription("PRIV");
                    break;
                case "TRCK":
                    tag = new TRCK(tagData);
                    tag.Description = TagProperties.GetTagDescription("TRCK");
                    break;
                default:
                    tag = new Id3Tag(tagName, tagData);
                    break;
            }
            return tag;
        }

        /// <summary>
        /// Get the name of the ID3V2 tag (4 chars, all upper case for Version ID3V2.3 and version ID3V2.4)
        /// (3 chars, all upper case for Version ID3V2.2)
        /// </summary>
        /// <param name="tagHeader"></param>
        /// <returns></returns>
        private String GetTagName(Byte[] tagHeader)
        {
            String tagName = String.Empty;
            if (this.tagVersion == 2)
            {
                tagName = UnicodeData.DecodeLatinString(tagHeader, 0, 3);
                
                if (FormatValidation.IsID3V20Tag.IsMatch(tagName) == true)
                {
                    tagName = TranslateOldTagname(tagName);
                }
                else
                {
                    tagName = String.Empty;
                }
            }
            if ((this.tagVersion == 3) || (this.tagVersion == 4))
            {
                tagName = UnicodeData.DecodeLatinString(tagHeader, 0, 4);

                if (FormatValidation.IsID3V2xTag.IsMatch(tagName) == false)
                {
                    tagName = String.Empty;
                }
            }

            return tagName;
        }

        /// <summary>
        /// Total frame size minus ten for the size of the frame header
        /// </summary>
        /// <param name="tagHeader"></param>
        /// <returns></returns>
        private Int32 GetTagSize(Byte[] tagHeader)
        {
            Int32 tagSize = 0;
            if (this.TagVersion == 2)
            {
                tagSize = (0x10000 * tagHeader[3]) + (0x100 * tagHeader[4]) + tagHeader[5];
            }

            if ((this.TagVersion == 3) || (this.TagVersion == 4))
            {
                tagSize = (0x1000000 * tagHeader[4]) + (0x10000 * tagHeader[5]) + (0x100 * tagHeader[6]) + tagHeader[7];
                Int16 Flags = (Int16)((0x100 * tagHeader[8]) + tagHeader[9]);
            }
            return tagSize;
        }

        /// <summary>
        /// Translate ID3V2.2 tag names to ID3V2.3 tag names
        /// </summary>
        /// <param name="oldTagName"></param>
        /// <returns></returns>
        private String TranslateOldTagname(String oldTagName)
        {
            String [,] tagTable = new [,] { 
                {"BUF", "RBUF"},
                {"CNT", "PCNT"},
                {"COM", "COMM"},
                {"CRA", "AENC"},
                {"CRM", "ENCR"},
                {"ETC", "ETCO"},
                {"EQU", "EQU2"},
                {"GEO", "GEOB"},
                {"IPL", "TIPL"},
                {"LNK", "LINK"},
                {"MCI", "MCDI"},
                {"MLL", "MLLT"},
                {"PIC", "APIC"},
                {"POP", "POPM"},
                {"REV", "REVB"},
                {"RVA", "RVAD"},
                {"SLT", "SYLT"},
                {"STC", "SYTC"},
                {"TAL", "TALB"},
                {"TBP", "TBPM"},
                {"TCM", "TCOM"},
                {"TCO", "TCON"},
                {"TCR", "TCOP"},
                {"TDA", "TDAT"},
                {"TDY", "TDLY"},
                {"TEN", "TENC"},
                {"TFT", "TFLT"},
                {"TIM", "TIME"},
                {"TKE", "TKEY"},
                {"TLA", "TLAN"},
                {"TLE", "TLEN"},
                {"TMT", "TMED"},
                {"TOA", "TOPE"},
                {"TOF", "TOFN"},
                {"TOL", "TOLY"},
                {"TOR", "TDOR"},
                {"TOT", "TOAL"},
                {"TP1", "TPE1"},
                {"TP2", "TPE2"},
                {"TP3", "TPE3"},
                {"TP4", "TPE4"},
                {"TPA", "TPOS"},
                {"TPB", "TPUB"},
                {"TRC", "TSRC"},
                {"TRD", "TRDC"},
                {"TRK", "TRCK"},
                {"TSI", "TSIZ"},
                {"TSS", "TSSE"},
                {"TT1", "TIT1"},
                {"TT2", "TIT2"},
                {"TT3", "TIT3"},
                {"TXT", "TEXT"},
                {"TXX", "TXXX"},
                {"TYE", "TYER"},
                {"UFI", "COMM"},
                {"ULT", "COMM"},
                {"WAF", "WOAF"},
                {"WAR", "WOAR"},
                {"WAS", "WOAS"},
                {"WCM", "WCOM"},
                {"WCP", "WCOP"},
                {"WPB", "WPUB"},
                {"WXX", "WXXX"},
            };

            Int32 tableRows = tagTable.GetLength(1);
            for (int i = 0; i < tableRows; i++)
            {
                if (tagTable[0,i] == oldTagName)
                {
                    return tagTable[1, i];
                }
            }
            return "This frame type is not supported in ID3 v2.00: " + oldTagName;

        }
        #endregion 

        #region Methods

        /// <summary>
        /// Get the TagCollection. Filter Non-Standard tags if desired. The definition of "Standard" is found in class ID3Tag 
        /// </summary>
        /// <param name="onlyStandardTags"></param>
        /// <returns></returns>
        public Id3TagCollection BuildTagCollection(Boolean onlyStandardTags)
        {
            if (this.tagCollection.Count == 0)
            {
                return null;
            }

            if (onlyStandardTags == true)
            {
                Id3TagCollection newTagCollection = new Id3TagCollection();
                foreach (Id3Tag tag in tagCollection)
                {
                    if (tag.IsStandardTag == true)
                    {
                        newTagCollection.Add(tag);
                    }
                }
                tagCollection = newTagCollection;
            }

            return tagCollection;  
        }

        /// <summary>
        /// Get the song object from the TagCollection
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public SongItem GetSong(String fileName)
        {
            if (this.tagCollection.Count == 0)
            {
                return null;   
            }
            SongItem song = new SongItem();

            song.AlbumName = GetStringValue(TagType.TALB);
            song.AlbumSortName = GetStringValue(TagType.TSOA);
            song.BandName = GetStringValue(TagType.TPE2);
            song.BandSortName = GetStringValue(TagType.TSOP);
            song.Comment = GetStringValue(TagType.COMM);
            song.ComposerName = GetStringValue(TagType.TCOM);
            song.ConductorName = GetStringValue(TagType.TPE3);
            song.Country = GetStringValue(TagType.PRIV);
            song.Encoder = GetStringValue(TagType.TSSE);
            song.Language = GetStringValue(TagType.TLAN);
            song.LeadPerformer = GetStringValue(TagType.TPE1);
            song.UserDefinedInfo = GetStringValue(TagType.TXXX);
            song.Track = GetStringValue(TagType.TRCK);
            song.WebsiteCommercial = GetStringValue(TagType.WCOM);
            song.WebsiteCopyright = GetStringValue(TagType.WCOP);
            song.WebsiteAudioFile = GetStringValue(TagType.WOAF);
            song.WebsiteArtist = GetStringValue(TagType.WOAR);
            song.WebsiteAudioSource = GetStringValue(TagType.WOAS);
            song.WebsiteInternetRadio = GetStringValue(TagType.WORS);
            song.WebsitePayment = GetStringValue(TagType.WPAY);
            song.WebsitePublisher = GetStringValue(TagType.WPUB);
            song.WebsiteUser = GetStringValue(TagType.WXXX);
            song.SongFilename = Path.GetFileName(fileName);
            song.SongPath = Path.GetDirectoryName(fileName) + "\\";
            song.SongTitle = GetStringValue(TagType.TIT2);
            song.Year = GetStringValue(TagType.TYER);

            String tagValue = GetStringValue(TagType.TCON);
            int pos = tagValue.IndexOf(')');
            tagValue = tagValue.Substring(pos + 1);
            if (tagValue != song.Genre)
            {
                song.Genre = tagValue;
                song.GenreId = 0;
            }

            String ms = GetStringValue(TagType.TLEN);
            if (String.IsNullOrEmpty(ms) == false)
            {
                if (FormatValidation.IsNumber.IsMatch(ms) == true)
                {
                    double milliseconds = Convert.ToDouble(ms);
                    TimeSpan duration = TimeSpan.FromMilliseconds(milliseconds);
                    song.Seconds = (Int32)duration.TotalSeconds;
                }
            }

            String strRating = GetStringValue(TagType.POPM);
            if (String.IsNullOrEmpty(strRating) == false)
            {
                song.Rating = Convert.ToInt32(strRating);
            }

            return song;
        }


        /// <summary>
        /// Replace the image in an APIC tag that matches the picType
        /// </summary>
        /// <param name="coverImage"></param>
        /// <param name="picType"></param>
        public void UpdatePicture(Image coverImage, PictureType picType)
        {
            tagCollection.UpdatePicture(coverImage, picType);
        }

        /// <summary>
        /// Delete all APIC tags
        /// </summary>
        public Int32 DeletePictureTags()
        {
            Int32 countDone = 0;
            Id3TagCollection newTagCollection = new Id3TagCollection();
            foreach (Id3Tag tag in tagCollection)
            {
                if (tag.TagType == TagType.APIC)
                {
                    countDone++;
                }
                else
                {
                    newTagCollection.Add(tag); 
                }
            }
            tagCollection = newTagCollection;
            return countDone;
        }

        /// <summary>
        /// Get the image for the specified PictureType if it exists in the TagCollection
        /// </summary>
        /// <param name="picType"></param>
        /// <returns></returns>
        public Image GetAttachedPicture(PictureType pictureType)
        {
            return tagCollection.GetAttachedPicture(pictureType);
        }
       
        #endregion

        #region Tag access

        /// <summary>
        /// Get the string value for the specified ID3V2 tag
        /// </summary>
        /// <param name="tagTpye"></param>
        /// <returns></returns>
        public String GetStringValue(TagType tagTpye)
        {
            Id3Tag tag = tagCollection.FindTagByType(tagTpye);
            if (tag != null)
            {
                return GetStringValue(tag);
            }
            return String.Empty;
        }

        /// <summary>
        /// Get the string value for the ID3V2 tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static String GetStringValue(Id3Tag tag)
        {
            String stringValue = String.Empty;
            Int32 length = tag.DataLength;
            Int32 posZero = 0;

            switch (tag.TagType)
            {               
                case TagType.PRIV:
                    stringValue = ((PRIV)tag).Country;
                    break;
              
                case TagType.POPM:
                    stringValue = ((POPM)tag).Rating.ToString();
                    break;
              
                case TagType.UFID:
                    posZero = UnicodeData.IndexOfByte(tag.TagData, 0, length, 0);
                    if (posZero > 0)
                    {
                        stringValue = UnicodeData.DecodeLatinString(tag.TagData, 0, posZero);
                        stringValue += " " + UnicodeData.DecodeLatinString(tag.TagData, posZero + 1, length - posZero - 1);
                    }
                    break;
               
                default:
                    if (tag.IsString == true)
                    {
                        stringValue = tag.StringValue;
                    }
                    break;
            }

            return stringValue;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();
        }

        ~TagReader()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (reader != null)
            {
                reader.Dispose();
            }

            
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion

    }
}
