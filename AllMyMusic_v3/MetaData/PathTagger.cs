using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using AllMyMusic_v3;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Guess song Meta-Information from the filename if we hav't a ID3V2 frame nor an ID3V1 frame
    /// </summary>
    static public class PathTagger
    {
        /// <summary>
        /// Guess the track number
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Int32 GetTrackFromFileName(String fileName)
        {
            for (int i = 0; i < 5; i++)
            {
                if ((fileName[i].CompareTo('0') < 0) || (fileName[i].CompareTo('9') > 0))
                {
                    if (i > 0)
                    {
                        String Track = fileName.Substring(0, i);
                        return Convert.ToInt32(Track);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Get the album folder name
        /// </summary>
        /// <param name="Folder"></param>
        /// <returns></returns>
        public static String GetAlbumFromFolder(String Folder)
        {
            if (Folder[Folder.Length - 1] == '\\')
            {
                Folder = Folder.Substring(0, Folder.Length - 1);
            }
            String albumFolder = GetFolder(Folder, 1);
            return albumFolder;
        }

        /// <summary>
        /// Guess some informations from the filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static SongItem GuessTagsFromFileName(String fileName)
        {
            SongItem song = new SongItem();
            song.SongPath = Path.GetDirectoryName(fileName) + "\\";
            song.SongFilename = Path.GetFileName(fileName);
            String filenameOnly = Path.GetFileNameWithoutExtension(fileName);
            String folderName = Path.GetDirectoryName(fileName);
            String albumFolder = PathTagger.GetFolder(folderName, 1);

            ArrayList SplittedFileName = SplitFileName(filenameOnly);
            ArrayList SplittedFolderName = SplitFolderName(albumFolder);

            Int32 IndexTrack = -1;
            Int32 Index = 0;
            
            foreach (String item in SplittedFolderName)
            {
                if (FormatValidation.IsNumber.IsMatch(item) == true)
                {
                    if (item.Length == 4)
                    {
                        song.Year = item;
                        break;
                    }
                }
                Index++;
            }

            foreach (String item in SplittedFileName)
            {
                if (FormatValidation.IsNumber.IsMatch(item) == true)
                {
                    if (item.Length == 4)
                    {
                        song.Year = item;
                        break;
                    }
                    if (item.Length < 4)
                    {
                        song.Track = item;
                        break;
                    }
                }
                Index++;
            }

            if (SplittedFileName.Count == 1)
            {
                song.SongTitle = (String)SplittedFileName[0];
            }

            if (SplittedFileName.Count == 2)
            {
                if (IndexTrack > 0)
                {
                    song.BandName = (String)SplittedFileName[0];
                }
                if (IndexTrack < 1)
                {
                    song.SongTitle = (String)SplittedFileName[1];
                }
            }

            if (SplittedFileName.Count > 2)
            {
                if (IndexTrack == 0)
                {
                    song.BandName = (String)SplittedFileName[1];
                    song.SongTitle = (String)SplittedFileName[2];
                }

                if (IndexTrack == 1)
                {
                    song.BandName = (String)SplittedFileName[0];
                    song.SongTitle = (String)SplittedFileName[2];
                }

            }
            //song.AlbumBand = song.BandName;
            song.AlbumName = albumFolder;
            song.Year = GetYearFromFolder(albumFolder);

            if (String.IsNullOrEmpty(song.BandName) == true)
            {
                song.BandName = song.SongTitle;
            }
            return song;
        }

        /// <summary>
        /// Try to guess the year from the folder name
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static String GetYearFromFolder(String folderName)
        {
            ArrayList SplittedFoldername = SplitFolderName(folderName);
            foreach (String item in SplittedFoldername)
            {
                if (FormatValidation.IsYear.IsMatch(item))
                {
                    return item;
                }
            }
            return "";
        }

        /// <summary>
        /// Split the filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static ArrayList SplitFileName(String fileName)
        {
            Int32 pos = 0;
            ArrayList SplittedPattern = new ArrayList();

            Boolean PointDelimiter = false;

            Int32 limit = fileName.Length - 1;
            for (int i = 0; i < limit; i++)
            {
                String Delimiter = fileName.Substring(i, 2);
                switch (Delimiter)
                {
                    case ". ":
                        if (PointDelimiter == false)
                        {
                            SplittedPattern.Add(fileName.Substring(pos, i - pos));
                            PointDelimiter = true;
                            while ((i < fileName.Length) && (fileName[i] == ' '))
                            {
                                i++;
                            }
                            i += 2;
                            pos = i;
                        }
                        break;
                    case "- ":
                        SplittedPattern.Add(fileName.Substring(pos, i - pos));
                        i += 2;
                        while ((i < fileName.Length) && (fileName[i] == ' '))
                        {
                            i++;
                        }

                        pos = i;

                        break;
                    case " -":
                        SplittedPattern.Add(fileName.Substring(pos, i - pos));
                        i += 2;
                        while ((i < fileName.Length) && (fileName[i] == ' '))
                        {
                            i++;
                        }
                        pos = i;

                        break;
                }
            }

            if (pos < fileName.Length)
            {
                SplittedPattern.Add(fileName.Substring(pos, fileName.Length - pos));
            }
            return SplittedPattern;
        }

        /// <summary>
        /// Split the folder name
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private static ArrayList SplitFolderName(String folderName)
        {
            Int32 pos = 0;
            ArrayList SplittedPattern = new ArrayList();
            // Gaetane Abrial - Cheyenne [2008]

            for (int i = 0; i < folderName.Length; i++)
            {
                switch (folderName[i])
                {
                    case ' ':
                    case '-':
                    case '(':
                    case ')':
                    case '[':
                    case ']':

                        String _temp = folderName.Substring(pos, i - pos);
                        if (String.IsNullOrEmpty(_temp) == false)
                        {
                            SplittedPattern.Add(folderName.Substring(pos, i - pos));
                        }
                        pos = i + 1;

                        break;
                }
            }

            if (pos < folderName.Length)
            {
                SplittedPattern.Add(folderName.Substring(pos, folderName.Length - pos));
            }
            return SplittedPattern;
        }

        /// <summary>
        /// Get the name of th efolder that is higher by the determined number of levels
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="reverseLevel"></param>
        /// <returns></returns>
        public static String GetFolder(String folder, Int32 reverseLevel)
        {
            // ReverseLevel is the folder Level started counting from the last subdirectory
            // Path = "Z:\Music\Hardrock\Danzig\1988 Danzig"
            // 1998 Danzig is Level 1
            // Danzig is Level 2
            // Hardrock is Level 3

            if ((reverseLevel > 10) || (reverseLevel < 0)) { return null; }
            int[] PosSeparator = new int[10];
            int CountSeparator = 0;

            try
            {
                // Identify position of the folder separators
                for (int i = folder.Length; i > 1; i--)
                {
                    if (folder[i - 1] == Path.DirectorySeparatorChar)
                    {
                        PosSeparator[CountSeparator] = i;
                        CountSeparator++;
                    }
                }
            }
            catch (Exception)
            {
               
            }


            // Extract the substring between two separartors
            reverseLevel--;
            if (reverseLevel > 0)
            {
                if ((PosSeparator[reverseLevel] != 0) && (PosSeparator[reverseLevel - 1] != 0))
                {
                    int len = PosSeparator[reverseLevel - 1] - PosSeparator[reverseLevel] - 1;
                    return folder.Substring(PosSeparator[reverseLevel], len);
                }
            }
            else
            {
                int len = folder.Length - PosSeparator[reverseLevel];
                string subpath = folder.Substring(PosSeparator[reverseLevel], len);
                return folder.Substring(PosSeparator[reverseLevel], len);
            }

            return "Unknown";
        }
    }
}
