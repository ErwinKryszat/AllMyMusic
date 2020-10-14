using System;
using System.Collections.Generic;

using System.IO;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Write the ID3V2 Tags into a MP3 file
    /// </summary>
    public static class TagWriter 
    {
        private static String fileName;
        private static Id3TagCollection tagCollection;
        private static Int32 requiredSize = 0;
        private static Int32 availableSize = 0;
        private static Int32 dataStartPosition = 0;
        private static Int32 id3v2TagSize = 0;

        private static int rwBufferSize;
        private static byte[] musicDataBytes;

        /// <summary>
        /// Write the information from "tagCollection" into the ID3V2Tags of the MP3 file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tagCollection"></param>
        public static void WriteTags(String FileName, Id3TagCollection TagCollection, Int32 DataStartPosition)
        {
            fileName = FileName;
            tagCollection = TagCollection;
            dataStartPosition = DataStartPosition;
            requiredSize = tagCollection.GetRequiredSize();
            availableSize = DataStartPosition;
            id3v2TagSize = DefineID3TagSize(requiredSize) - 10;

            if ((availableSize < id3v2TagSize) || ((availableSize > id3v2TagSize) && ((availableSize - id3v2TagSize) > 0x0200)))
            {
                // Enlarge or Shrink the file
                ResizeFile();
            }
            else
            {
                id3v2TagSize = availableSize - 10;
                FileStream dstStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                WriteID3V2Tags(dstStream);
                dstStream.Close();
            }
        }
       
        /// <summary>
        /// Define the size for the ID3V2 tag in some "even" borders. i.e. not let the size be something like 713 bytes but then 800
        /// </summary>
        /// <param name="required"></param>
        /// <returns></returns>
        private static Int32 DefineID3TagSize(Int32 totalRequireSize)
        {
            Int32 totalTagSize = 0x0200;
            while (totalTagSize < (totalRequireSize + 10))
            {
                totalTagSize += 0x0100;
            }
            return totalTagSize;
        }

        

        /// <summary>
        /// Shrink the file if the available size for the ID3V2 tag is greater then the required size
        /// </summary>
        private static void ResizeFile()
        {
            // Größe erste Datei als Puffer festlegen
            rwBufferSize = (int)(new FileInfo(fileName).Length);
            musicDataBytes = new byte[rwBufferSize];

            Int32 bytesRead = ReadAudioData();

            WriteID3TagAndAudioData(bytesRead);
        }

        /// <summary>
        /// Read the audio data 
        /// </summary>
        /// <returns></returns>
        private static Int32 ReadAudioData()
        {
            int bytesRead = -1;
            FileStream srcStream = null;

            try
            {
                // Streams festlegen
                srcStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                // Komplette Datei Lesen, ab startposition der musik daten
                int bytesToRead = (int)srcStream.Length - dataStartPosition;
                srcStream.Position = dataStartPosition;

                bytesRead = srcStream.Read(musicDataBytes, 0, bytesToRead);
            }
            catch (Exception)
            {
                
            }
            finally
            {
                srcStream.Close();
            }
            return bytesRead;
        }

        /// <summary>
        /// Write ID3V2 Tag and then the audio data
        /// </summary>
        private static void WriteID3TagAndAudioData(Int32 bytesRead)
        {
            FileStream dstStream = null;

            // Quelldateiname und Pfad zwischenspeichern
            string srcFilename = Path.GetFileNameWithoutExtension(fileName);
            string srcPath = Path.GetDirectoryName(fileName) + @"\";
            string tempFile = srcPath + srcFilename + ".tmp";

            try
            {
                dstStream = new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                WriteID3V2Tags(dstStream);
                dstStream.Write(musicDataBytes, 0, bytesRead);
            }
            catch (Exception)
            {
                
            }
            finally
            {
                dstStream.Close();
                File.Delete(fileName);
                File.Move(tempFile, fileName);
            }
        }

       
        /// <summary>
        /// Write the ID3V2 tag to the output stream
        /// </summary>
        /// <param name="outputStream"></param>
        private static void WriteID3V2Tags(Stream outputStream)
        {
            try
            {
                Byte[] fileBufferID3Tag = new Byte[id3v2TagSize + 10];
                fileBufferID3Tag.Initialize();

                WriteHeader(fileBufferID3Tag);
                int position = WriteTagCollection(fileBufferID3Tag);

                outputStream.Write(fileBufferID3Tag, 0, fileBufferID3Tag.Length);
            }
            catch (Exception)
            {
                
            }
        }
         
        /// <summary>
        /// Write the ID3 identifier and the SynchSafe Size Information for the wholde ID3V2 tag
        /// </summary>
        /// <param name="fileBuffer"></param>
        private static void WriteHeader(Byte[] fileBuffer)
        {
            // Write ID3 header
            fileBuffer[0] = (byte)'I';
            fileBuffer[1] = (byte)'D';
            fileBuffer[2] = (byte)'3';
            fileBuffer[3] = 0x03;
            fileBuffer[4] = 0x00;
            fileBuffer[5] = 0x00;

            Byte[] SynchSafeBytes = IntToSynchsafeBytes(id3v2TagSize);
            SynchSafeBytes.CopyTo(fileBuffer, 6);
        }

        /// <summary>
        /// Write the ID3 tags
        /// </summary>
        /// <param name="fileBuffer"></param>
        /// <returns></returns>
        private static Int32 WriteTagCollection(Byte[] fileBuffer)
        {
            int position = 10;

            foreach (Id3Tag tag in tagCollection)
            {
                if (tag.StringValue != String.Empty)
                {
                    // Tag Name
                    Byte[] nameBytes = UnicodeData.EncodeStringValue_Latin(tag.TagType.ToString());
                    nameBytes.CopyTo(fileBuffer, position);
                    position += nameBytes.Length;

                    // Write the tag size as integer  
                    Byte[] sizeBytes = IntToBytes(tag.DataLength);
                    sizeBytes.CopyTo(fileBuffer, position);
                    position += 4;

                    // Write the flags  (ToDo, not yet implemented)
                    UInt16 flags = 0x0000;
                    Byte highbyte = (Byte)(flags / 256);
                    Byte lowbyte = (Byte)flags;
                    fileBuffer[position] = highbyte; position++;
                    fileBuffer[position] = lowbyte; position++;

                    tag.TagData.CopyTo(fileBuffer, position);
                    position += tag.DataLength;
                }
            }
            return position;
        }



        /// <summary>
        /// Convert an Integer (32 bit) to a synchsafe series of 4 bytes. Bit8 is always zero.
        /// </summary>
        /// <param name="Size"></param>
        /// <returns></returns>
        private static Byte[] IntToSynchsafeBytes(Int32 Size)
        {
            Int32 Output = 0;
            Int32 Mask = 0;
            Mask = 0x0000007F;

            for (int i = 0; i < 4; i++)
            {
                Output |= Size & Mask;
                Mask <<= 8;
                Size <<= 1;
            }

            // Convert Int32 to Byte Array
            Byte[] bb = BitConverter.GetBytes(Output);
            Byte[] res = new Byte[4];

            // Reverse the Byte Array
            res[0] = bb[3];
            res[1] = bb[2];
            res[2] = bb[1];
            res[3] = bb[0];
            return res;
        }

        /// <summary>
        /// Convert an Integer (32 bit) to a series of 4 bytes.
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns></returns>
        private static Byte[] IntToBytes(Int32 intValue)
        {
            // Convert Int32 to Byte Array
            Byte[] intBytes = BitConverter.GetBytes(intValue);
            Byte[] resBytes = new Byte[4];

            // Reverse the Byte Array
            resBytes[0] = intBytes[3];
            resBytes[1] = intBytes[2];
            resBytes[2] = intBytes[1];
            resBytes[3] = intBytes[0];
            return resBytes;
        }
    }
}
