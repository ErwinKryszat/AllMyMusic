using System;
using System.Collections.Generic;
using System.Text;

using AllMyMusic;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Convert Text To/From a series of bytes according to a specific coding table
    /// </summary>
    public static class UnicodeData
    {
        public static DatabaseCollation collation = DatabaseCollation.Latin;

        private static Encoding encLatin1 = Encoding.GetEncoding(1252);    // ISO-8859-1
        private static Encoding encKOI8 = Encoding.GetEncoding(1251);
        private static UnicodeEncoding unicodeLittleEndianBOM = new UnicodeEncoding(false, true);
        private static UnicodeEncoding unicodeBigEndianBOM = new UnicodeEncoding(true, true);
        private static UTF8Encoding unicodeUTF8 = new UTF8Encoding();
        private static ASCIIEncoding encASCII = new ASCIIEncoding();

        #region Character Encoding and Decoding

        /// <summary>
        /// Convert a series of bytes to text.
        /// The first byte determines the coding type
        /// </summary>
        /// <param name="data"></param>
        /// <returns>String representation of the byte sequence</returns>
        public static String DecodeStringValue(Byte[] data)
        {
            Int32 length = data.Length;
            CodingType codingType = (CodingType)data[0];

            String value = String.Empty;
            Int32 PosZero = 0;


            switch (codingType)
            {
                case CodingType.ISO_8859_1:
                    {
                        PosZero = IndexOfByte(data, 1, length, 0);
                        if (PosZero > 0)
                        {
                            length = PosZero;
                        }

                        if (collation == DatabaseCollation.Latin)
                        {
                            value = encLatin1.GetString(data, 1, length - 1);
                        }
                        if (collation == DatabaseCollation.Cyrillic)
                        {
                            value = encKOI8.GetString(data, 1, length - 1);
                        }

                        break;
                    }
                case CodingType.UTF_16:
                    {
                        if (length > 2)
                        {
                            if ((data[length - 2] == 0) && (data[length - 1] == 0))
                            {
                                length -= 2;
                            }
                            if ((data[1] == 0xFF) && (data[2] == 0xFE))
                            {

                                value = unicodeLittleEndianBOM.GetString(data, 3, length - 3);
                            }
                            if ((data[1] == 0xFE) && (data[2] == 0xFF))
                            {
                                value = unicodeBigEndianBOM.GetString(data, 3, length - 3);
                            }
                        }
                        break;
                    }
                case CodingType.UTF_16BE:
                    {
                        if ((data[length - 2] == 0) && (data[length - 1] == 0))
                        {
                            length -= 2;
                        }
                        if (length > 2)
                        {
                            if ((data[1] == 0xFF) && (data[2] == 0xFE))
                            {
                                value = unicodeLittleEndianBOM.GetString(data, 3, length - 3);
                            }
                            if ((data[1] == 0xFE) && (data[2] == 0xFF))
                            {
                                value = unicodeBigEndianBOM.GetString(data, 3, length - 3);
                            }
                        }
                        break;
                    }
                case CodingType.UTF_8:
                    {
                        PosZero = IndexOfByte(data, 1, length, 0);
                        if (PosZero > 0)
                        {
                            length = PosZero;
                        }
                        value = unicodeUTF8.GetString(data, 1, length - 1);
                        break;
                    }
                case CodingType.ASCII:
                    {
                        PosZero = IndexOfByte(data, 1, length, 0);
                        if (PosZero > 0)
                        {
                            length = PosZero;
                        }
                        if (collation == DatabaseCollation.Latin)
                        {
                            value = encASCII.GetString(data, 1, length - 1);
                        }
                        if (collation == DatabaseCollation.Cyrillic)
                        {
                            value = encKOI8.GetString(data, 1, length - 1);
                        }
                        break;
                    }
                case CodingType.KOI8:
                    {
                        PosZero = IndexOfByte(data, 1, length, 0);
                        if (PosZero > 0)
                        {
                            length = PosZero;
                        }
                        value = encKOI8.GetString(data, 1, length - 1);
                        break;
                    }
                default:
                    {
                        // No encoding byte was specified, so read the string from the first byte
                        PosZero = IndexOfByte(data, 0, length, 0);
                        if (PosZero > 0)
                        {
                            length = PosZero;
                        }
                        value = encLatin1.GetString(data, 0, length);
                        break;
                    }
            }
            return value;
        }

        /// <summary>
        /// Convert a series of bytes to text using the code page 1252 / ISO-8859-1
        /// </summary>
        /// <param name="data"></param>
        /// <returns>String representation of the byte sequence</returns>
        public static String DecodeLatinString(Byte[] data, int index, int count)
        {
            return encLatin1.GetString(data, index, count);
        }

        /// <summary>
        /// Convert a series of bytes to text. Each character is represented by one or by two bytes
        /// </summary>
        /// <param name="data"></param>
        /// <returns>String representation of the byte sequence</returns>
        public static String DecodeUTF8String(Byte[] data, int index, int count)
        {
            return unicodeUTF8.GetString(data, index, count);
        }

        public static Byte[] EncodeStringValue_Latin(String value)
        {
            Byte[] Data = encLatin1.GetBytes(value);
            return Data;
        }

        public static Byte[] EncodeStringValue_ASCII(String value)
        {
            Byte[] Data = encASCII.GetBytes(value);
            return Data;
        }

        /// <summary>
        /// Convert a string to a series of bytes.
        /// </summary>
        /// <param name="value">The string that shall by converted to bytes</param>
        /// <param name="AllowEncoding">Coding is permitted or not.</param>
        /// <param name="codingType">The type of coding to be applied</param>
        /// <returns></returns>
        public static Byte[] EncodeStringValue(String value, Boolean allowEncoding, CodingType codingType)
        {
            Byte[] Data = null;

            if (allowEncoding == true)
            {
                Boolean UnicodeRequired = false;
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] > 127) { UnicodeRequired = true; }
                }
                
                if ((codingType == CodingType.ISO_8859_1 || codingType == CodingType.UTF_8) && UnicodeRequired) { codingType = CodingType.UTF_16; }
                if ((codingType == CodingType.UTF_16 || codingType == CodingType.UTF_16BE) && !UnicodeRequired) { codingType = CodingType.ISO_8859_1; }
                if ((codingType > CodingType.ASCII) && UnicodeRequired) { codingType = CodingType.UTF_16; }
                if ((codingType > CodingType.ASCII) && !UnicodeRequired) { codingType = CodingType.ISO_8859_1; }


                if (collation == DatabaseCollation.Cyrillic)
                {
                    codingType = CodingType.UTF_16;
                }

                switch (codingType)
                {
                    case CodingType.ISO_8859_1:
                        {
                            Byte[] b1 = encLatin1.GetBytes(value);

                            if (collation == DatabaseCollation.Cyrillic)
                            {
                                b1 = encKOI8.GetBytes(value);
                            }
                            Byte[] b2 = new Byte[b1.Length + 1];
                            b2[0] = 0;
                            b1.CopyTo(b2, 1);
                            Data = b2;
                            break;
                        }
                    case CodingType.UTF_16:
                        {
                            Byte[] byteOrderMark = unicodeLittleEndianBOM.GetPreamble();
                            Byte[] b1 = unicodeLittleEndianBOM.GetBytes(value);
                            Byte[] b2 = new Byte[b1.Length + 3];
                            b2[0] = 1;
                            byteOrderMark.CopyTo(b2, 1);
                            b1.CopyTo(b2, 3);
                            Data = b2;
                            break;
                        }
                    case CodingType.UTF_16BE:
                        {
                            Byte[] byteOrderMark = unicodeBigEndianBOM.GetPreamble();
                            Byte[] b1 = unicodeBigEndianBOM.GetBytes(value);
                            Byte[] b2 = new Byte[b1.Length + 3];
                            b2[0] = 2;
                            byteOrderMark.CopyTo(b2, 1);
                            b1.CopyTo(b2, 3);
                            Data = b2;
                            break;
                        }
                    case CodingType.UTF_8:
                        {
                            Byte[] b1 = unicodeUTF8.GetBytes(value);
                            Byte[] b2 = new Byte[b1.Length + 2];
                            b2[0] = 3;
                            b1.CopyTo(b2, 1);
                            Data = b2;
                            break;
                        }
                    case CodingType.ASCII:
                        {
                            if (collation == DatabaseCollation.Latin)
                            {
                                Byte[] b1 = encASCII.GetBytes(value);
                                Byte[] b2 = new Byte[b1.Length + 2];
                                b2[0] = 4;
                                b1.CopyTo(b2, 1);
                                Data = b2;
                            }
                            if (collation == DatabaseCollation.Cyrillic)
                            {
                                Byte[] b1 = encKOI8.GetBytes(value);
                                Byte[] b2 = new Byte[b1.Length + 2];
                                b2[0] = 4;
                                b1.CopyTo(b2, 1);
                                Data = b2;
                            }
                            break;
                        }
                    default:
                        {
                            Data = encLatin1.GetBytes(value);
                            //MessageBox.Show("ID3Tag.StringToByteArray - Unsupported coding: " + codingtype.ToString());
                            break;
                        }
                }
            }
            else
            {
                if (collation == DatabaseCollation.Latin)
                {
                    Data = encLatin1.GetBytes(value);
                }
                if (collation == DatabaseCollation.Cyrillic)
                {
                    Data = encKOI8.GetBytes(value);
                }
            }
            return Data;
        }

        /// <summary>
        /// Retrieve the index of a byte with a specified value inside a series of bytes
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <returns>index of a byte with a specified value</returns>
        public static Int32 IndexOfByte(Byte[] data, Int32 index, Int32 length, Byte value)
        {
            for (int i = index; i < length; i++)
            {
                if (data[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Retrieve the index of a character with a specified value inside a series of bytes
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="character"></param>
        /// <returns>index of a character with a specified value</returns>
        public static Int32 IndexOfChar(Byte[] data, Int32 index, Int32 length, Char character)
        {
            for (int i = index; i < index + length; i++)
            {
                if (data[i] == character)
                {
                    return i - index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Seraches the position of the first byte that is not zero
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns>Index of first byte > 0</returns>
        public static Int32 IndexStringStart(Byte[] data, Int32 index, Int32 length)
        {
            for (int i = index; i < index + length; i++)
            {
                if (data[i] > 0)
                {
                    return i - index;
                }
            }
            return -1;
        }
        #endregion
    }
}
