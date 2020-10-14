using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AllMyMusic_v3;

namespace Metadata.ID3
{
    public static class ID3V1Tag
    {
        private static Byte[] tag = new Byte[3];
        private static Byte[] title = new Byte[30];
        private static Byte[] artist = new Byte[30];
        private static Byte[] album = new Byte[30];
        private static Byte[] year = new Byte[4];
        private static Byte[] comment = new Byte[28];
        private static Byte[] track = new Byte[2];
        private static Byte[] genre = new Byte[1];

        /// <summary>
        /// Reads an ID3v1 tag from a stream
        /// </summary>
        public static SongItem ReadTag(Stream input, String fileName)
        {
            SongItem song = null;
            Byte[] tempData = new Byte[30];

            input.Position = input.Length - 128;
            input.Read(tag, 0, 3);
            if (tag[0] == 'T' && tag[1] == 'A' && tag[2] == 'G')
            {
                input.Read(title, 0, 30);
                input.Read(artist, 0, 30);
                input.Read(album, 0, 30);
                input.Read(year, 0, 4);

                // Determine if this is a v1.0 or v1.1 tag
                input.Read(tempData, 0, 30);
                if (tempData[28] == 0)
                {
                    input.Position -= 30;
                    input.Read(comment, 0, 28);
                    input.Read(track, 0, 2);
                }
                else
                {
                    CopyBytes(tempData, comment, 28);
                }
                input.Read(genre, 0, 1);

                song = GetSong();
                song.SongPath = Path.GetDirectoryName(fileName) + "\\";
                song.SongFilename = Path.GetFileName(fileName);
            }
            return song;
        }

        private static SongItem GetSong()
        {
            Encoding encLatin1 = Encoding.GetEncoding(1252);    // ISO-8859-1

            SongItem song = new SongItem();
            Int32 endOfString = GetLength(title);
            song.SongTitle = encLatin1.GetString(title,0,endOfString);

            endOfString = GetLength(artist);
            song.BandName = encLatin1.GetString(artist, 0, endOfString).Trim();

            endOfString = GetLength(album);
            song.AlbumName = encLatin1.GetString(album, 0, endOfString).Trim();

            String tmpYear = encLatin1.GetString(year, 0, 4).Trim();
            if (FormatValidation.IsYear.IsMatch(tmpYear) == true)
            {
                song.Year = tmpYear;
            }

            endOfString = GetLength(comment);
            song.Comment = encLatin1.GetString(comment, 0, endOfString).Trim();
            song.Track = track[1].ToString();
            song.Genre = GenreCollection.GetGenre((int)genre[0]);
            return song;
        }

        private static Int32 GetLength(Byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 0)
                {
                    return i;
                }
            }
            return array.Length;
        }

        private static void CopyData(SongItem song)
        {
            // TAG Marker
            tag[0] = (byte)'T';
            tag[1] = (byte)'A';
            tag[2] = (byte)'G';

            // Song Title
            Byte[] tagData = UnicodeData.EncodeStringValue(song.SongTitle, false, CodingType.ASCII);
            CopyBytes(tagData, title, 30);

            // Artist
            tagData = UnicodeData.EncodeStringValue(song.BandName, false, CodingType.ASCII);
            CopyBytes(tagData, artist, 30);

            // Album
            tagData = UnicodeData.EncodeStringValue(song.AlbumName, false, CodingType.ASCII);
            CopyBytes(tagData, album, 30);

            // Year
            tagData = UnicodeData.EncodeStringValue(song.Year.ToString(), false, CodingType.ASCII);
            CopyBytes(tagData, year, 30);

            // Comment
            tagData = UnicodeData.EncodeStringValue(song.Comment.ToString(), false, CodingType.ASCII);
            CopyBytes(tagData, comment, 28);

            // Track (ID3v1.1 Standard)
            track[0] = 0;
            if (String.IsNullOrEmpty(song.Track)==false)
            {
                track[1] = Convert.ToByte(song.Track);
            }
            else
            {
                track[1] = 0;
            }
            

            // Genre
            genre[0] = GenreCollection.GetGenreId(song.Genre);

        }
        private static void CopyBytes(Byte[] source, Byte[] destination, Int32 countMax)
        {
            Int32 limit = Math.Min(source.Length, countMax);
            for (int i = 0; i < limit; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Save(String fileName, SongItem song)
        {
            try
            {
                CopyData(song);
                FileStream outputStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

                outputStream.Position = outputStream.Length - 128;
                byte[] tagData = new byte[3];
                outputStream.Read(tagData, 0, 3);
                if (tagData[0] == 'T' && tagData[1] == 'A' && tagData[2] == 'G')
                {              
                    // Write ID3V1 Tag into the file
                    outputStream.Position = outputStream.Length - 128;
                }
                else
                {
                    // Append ID3V1 Tag into the file
                    outputStream.Position = outputStream.Length;
                }

                outputStream.Write(tag, 0, tag.Length);
                outputStream.Write(title, 0, title.Length);
                outputStream.Write(artist, 0, artist.Length);
                outputStream.Write(album, 0, album.Length);
                outputStream.Write(year, 0, year.Length);
                outputStream.Write(comment, 0, comment.Length);
                outputStream.Write(track, 0, track.Length);
                outputStream.Write(genre, 0, genre.Length);
                outputStream.Close();

            }
            catch (FieldAccessException)
            {
                throw;
            }
        }
    }
}
