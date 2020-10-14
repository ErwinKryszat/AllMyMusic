using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Wave;

namespace Metadata.Mp3
{
    /// <summary>
    /// This class is used to:
    /// Handle the Xing-Frame, also known as LameHeader
    /// </summary>
    public class LameHeader
    {
        [Flags]
        enum LameHeaderOptions
        {
            Frames = 1,
            Bytes = 2,
            Toc = 4,
            VbrScale = 8
        }

        private int vbrScale = -1;
        private int startOffset;
        private int framesOffset = -1;
        private int bytesOffset = -1;
        private Mp3Frame frame;

        private static Encoding encLatin1 = Encoding.GetEncoding(1252);    // ISO-8859-1

        private static int ReadBigEndian(byte[] buffer, int offset)
        {
            int x;
            // big endian extract
            x = buffer[offset+0];
            x <<= 8;
            x |= buffer[offset+1];
            x <<= 8;
            x |= buffer[offset+2];
            x <<= 8;
            x |= buffer[offset+3];

            return x;
        }

        private void WriteBigEndian(byte[] buffer, int offset, int value)
        {
            byte[] littleEndian = BitConverter.GetBytes(value);
            for (int n = 0; n < 4; n++)
            {
                buffer[offset + 4 - n] = littleEndian[n];
            }
        }

        /// <summary>
        /// Load Lame Header
        /// </summary>
        /// <param name="frame">Frame</param>
        /// <returns>Lame Header</returns>
        public static LameHeader LoadLameHeader(Mp3Frame frame)
        {
            LameHeader lameHeader = new LameHeader();
            lameHeader.frame = frame;
            int offset = 0;

            if (frame.MpegVersion == MpegVersion.Version1)
            {
                if (frame.ChannelMode != ChannelMode.Mono)
                    offset = 32 + 4;
                else
                    offset = 17 + 4;
            }
            else if (frame.MpegVersion == MpegVersion.Version2)
            {
                if (frame.ChannelMode != ChannelMode.Mono)
                    offset = 17 + 4;
                else
                    offset = 9 + 4;
            }
            else
            {
                return null;
                // throw new FormatException("Unsupported MPEG Version");
            }

            if (frame.FrameLength > 36)
            {
                

                String lameString = DecodeLatinString(frame.RawData, 36, frame.RawData.Length - 36).ToUpper();

                if (lameString.LastIndexOf("LAME") >= 0)
                {
                    lameHeader.startOffset = offset;
                    return lameHeader;
                }
            }
            return null;
        }

        private static String DecodeLatinString(Byte[] data, int index, int count)
        {
            return encLatin1.GetString(data, index, count);
        }

        /// <summary>
        /// Sees if a frame contains a Lame header
        /// </summary>
        private LameHeader()
        {
        }

        /// <summary>
        /// Number of frames
        /// </summary>
        public int Frames
        {
            get 
            { 
                if(framesOffset == -1) 
                    return -1;
                return ReadBigEndian(frame.RawData, framesOffset); 
            }
            set
            {
                if (framesOffset == -1)
                    throw new InvalidOperationException("Frames flag is not set");
                WriteBigEndian(frame.RawData, framesOffset, value);
            }
        }

        /// <summary>
        /// Number of bytes
        /// </summary>
        public int Bytes
        {
            get 
            { 
                if(bytesOffset == -1) 
                    return -1;
                return ReadBigEndian(frame.RawData, bytesOffset); 
            }
            set
            {
                if (framesOffset == -1)
                    throw new InvalidOperationException("Bytes flag is not set");
                WriteBigEndian(frame.RawData, bytesOffset, value);
            }
        }

        /// <summary>
        /// VBR Scale property
        /// </summary>
        public int VbrScale
        {
            get { return vbrScale; }
        }

        /// <summary>
        /// The MP3 frame
        /// </summary>
        public Mp3Frame Mp3Frame
        {
            get { return frame; }
        }

    }
}
