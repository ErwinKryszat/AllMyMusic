using System;
using System.Collections.Generic;
using System.Text;

namespace Metadata.Mp3
{
    /// <summary>
    /// This class is used to:
    /// Store the option for handling changes in the ID3V2 frames
    /// </summary>
    public class ID3Option
    {
        private Boolean addTLENtag = false;
        private Boolean deleteUnsupportedTags = false;
        private Boolean removeXingFrames = false;
        private Boolean writeID3V1Tags = false;

        /// <summary>
        /// Wether or not to add the TLEN tag in VBR file in order to save time we must read the file the next time
        /// To determine the length in seconds for a VBR file we must read the whole file and count the samples.
        /// </summary>
        public Boolean AddTLENtag
        {
            get { return addTLENtag; }
            set { addTLENtag = value; }
        }

        /// <summary>
        /// Wether or not the user want to delete Non-Standard tags
        /// </summary>
        public Boolean DeleteUnsupportedTags
        {
            get { return deleteUnsupportedTags; }
            set { deleteUnsupportedTags = value; }
        }

        /// <summary>
        /// Wether or not the Xing Frame shall be deleted.
        /// In my opinion this frame is obsolete.
        /// </summary>
        public Boolean RemoveXingFrames
        {
            get { return removeXingFrames; }
            set { removeXingFrames = value; }
        }

        /// <summary>
        /// Wether or not the ID3V1 tag at the end of the MP3 shall be updated when we save changes
        /// </summary>
        public Boolean WriteID3V1Tags
        {
            get { return writeID3V1Tags; }
            set { writeID3V1Tags = value; }
        }
    }
}
