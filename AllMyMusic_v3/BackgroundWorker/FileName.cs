using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic
{
    /// <summary>
    /// This class is used to:
    /// Eliminate character that are forbidden in a filename but might exist in Meta-Information for songs
    /// </summary>
    public static class FileName
    {
        /// <summary>
        /// Remove all characters that are forbidden in a filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static String RemoveForbiddenChars(String fileName)
        {
            fileName = fileName.Replace("?", "");
            fileName = fileName.Replace("\\", "");
            fileName = fileName.Replace("/", "");
            fileName = fileName.Replace(":", "");
            fileName = fileName.Replace("*", "");
            fileName = fileName.Replace("<", "");
            fileName = fileName.Replace(">", "");
            fileName = fileName.Replace("|", "");
            fileName = fileName.Replace("\"", "");

            return fileName;
        }

        public static String ReplaceForbiddenChars(String fileName)
        {
            fileName = fileName.Replace('\\', '_');
            fileName = fileName.Replace('/', '_');
            fileName = fileName.Replace(':', '_');
            fileName = fileName.Replace('*', '_');
            fileName = fileName.Replace('?', '_');
            fileName = fileName.Replace('"', '_');
            fileName = fileName.Replace('<', '_');
            fileName = fileName.Replace('>', '_');
            fileName = fileName.Replace('|', '_');

            return fileName;
        }
    }
}
