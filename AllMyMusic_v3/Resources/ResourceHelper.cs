using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;

namespace AllMyMusic
{
    public static class ResourceHelper 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourcePath">The resourcePath is case sensitive!</param>
        /// <param name="DestinationFolder"></param>
        public static void CopyImageToFolder(String resourcePath, String DestinationFolder)
        {
            Byte[] filebytes = null;
            Int32 nLength = 0;

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream streamResource = assembly.GetManifestResourceStream(resourcePath);
                if (streamResource != null)
                {
                    nLength = (Int32)streamResource.Length;
                    filebytes = new Byte[nLength];
                    streamResource.Read(filebytes, 0, nLength);
                    streamResource.Close();
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }

            try
            {
                if ((filebytes != null) && (nLength > 0))
                {
                    String extension = resourcePath.Substring(resourcePath.LastIndexOf(".") + 1, resourcePath.Length - resourcePath.LastIndexOf(".") - 1);
                    String resourcePathWithoutExtension = resourcePath.Substring(0, resourcePath.Length - extension.Length - 1);
                    String filename = resourcePath.Substring(resourcePathWithoutExtension.LastIndexOf(".") + 1, resourcePathWithoutExtension.Length - resourcePathWithoutExtension.LastIndexOf(".") - 1);

                    String fullPath = DestinationFolder + "\\" + filename + "." + extension;
                    FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    fileStream.Write(filebytes, 0, nLength);
                    fileStream.Close();
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public static BitmapImage GetImage(Uri resorcePath)
        {
            BitmapImage bi = new BitmapImage();

            try
            {
                bi.BeginInit();
                bi.UriSource = resorcePath;
                bi.EndInit();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error Loading Image from Assembly Path: " + resorcePath;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
          
            return bi;
        }
        public static BitmapImage GetImage(String localPath)
        {
            BitmapImage bi = null;
            try
            {
                Uri uri = null;
                if (File.Exists(localPath) == true)
                {
                    uri = new Uri(localPath);

                    bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = uri;
                    bi.EndInit();
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error Loading Image from directory: " + localPath;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }

            return bi;
        }

        /// <summary>
        /// Copy a resource text / xml file to the local hard disc
        /// not: resource mus be set as "Embedded Resource"
        /// </summary>
        /// <param name="resourcePath">The resourcePath is case sensitive!</param>
        /// <param name="TargetFilename"></param>
        public static void CopyResourceTextFileToFilesystem(String resourcePath, String TargetFilename)
        {
            try
            {
                Encoding encLatin1 = Encoding.GetEncoding(1252);    // ISO-8859-1
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream streamResource = assembly.GetManifestResourceStream(resourcePath);
                if (streamResource != null)
                {
                    StreamReader reader = new StreamReader(streamResource, encLatin1);

                    StreamWriter writer = new StreamWriter(TargetFilename, false, encLatin1);
                    while (reader.EndOfStream != true)
                    {
                        String pattern = reader.ReadLine();
                        writer.WriteLine(pattern);
                    }
                    reader.Close();
                    writer.Close();
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public static ArrayList ReadTextFileResource(String resourcePath)
        {
            try
            {
                ArrayList textFile = new ArrayList();
                Encoding encLatin1 = Encoding.GetEncoding(1252);    // ISO-8859-1
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream streamResource = assembly.GetManifestResourceStream(resourcePath))
                {
                    StreamReader reader = new StreamReader(streamResource, encLatin1);

                    while (reader.EndOfStream != true)
                    {
                        String pattern = reader.ReadLine();
                        textFile.Add(pattern);
                    }
                }

                return textFile;
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            return null;
        }
        public static ArrayList ReadTextFile(String filename)
        {
            try
            {
                ArrayList textFile = new ArrayList();
                Encoding encLatin1 = Encoding.GetEncoding(1252);    // ISO-8859-1

                using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    StreamReader reader = new StreamReader(fileStream, encLatin1);

                    while (reader.EndOfStream != true)
                    {
                        String pattern = reader.ReadLine();
                        textFile.Add(pattern);
                    }
                }

                return textFile;
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            return null;
        }

        public static void WriteTextFile(String filename, ObservableCollection<String> textFile)
        {
            Encoding encLatin1 = Encoding.GetEncoding(1252);    // ISO-8859-1

            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    fs = null;

                    for (int i = 0; i < textFile.Count; i++)
                    {
                        writer.WriteLine(textFile[i]);
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
    }
}
