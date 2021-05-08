using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace AllMyMusic
{
    static class FileSystemTool
    {
        #region Fields

        #endregion

        public static void DeleteFilesFromFolder(String folderName)
        {
            // Purge Target folder
            DirectoryInfo di = new DirectoryInfo(folderName);
            FileInfo[] fi = di.GetFiles();
            foreach (FileInfo item in fi)
            {
                item.Delete();
            }
        }

        public static void CreateDirectory(String folderName)
        {
            try
            {
                if (Directory.Exists(folderName) == false)
                {
                    Directory.CreateDirectory(folderName);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error creating directory: " + folderName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        public static void CreateDirectoryOrPurge(String folderName)
        {
            try
            {
                if (Directory.Exists(folderName) == false)
                {
                    Directory.CreateDirectory(folderName);
                }
                else
                {
                    DeleteFilesFromFolder(folderName);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error creating directory: " + folderName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        public static Boolean DirectoryExists(String folderName)
        {
            return Directory.Exists(folderName);
        }

        public static void CopyDirectory(String sourceDirectory, String destinationDirectory)
        {
            String destinationFileName = String.Empty;
            try
            {
                DirectoryInfo di = new DirectoryInfo(sourceDirectory);
                FileInfo[] fiArray = di.GetFiles();
                foreach (FileInfo fi in fiArray)
                {
                    destinationFileName = destinationDirectory + "\\" + fi.Name;
                    fi.CopyTo(destinationFileName, true);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error copying file: " + destinationFileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        public static void MoveDirectory(String sourceDirectory, String destinationDirectory)
        {
            String destinationFileName = String.Empty;
            try
            {
                DirectoryInfo di = new DirectoryInfo(sourceDirectory);
                FileInfo[] fiArray = di.GetFiles();
                foreach (FileInfo fi in fiArray)
                {
                    destinationFileName = destinationDirectory + "\\" + fi.Name;
                    fi.MoveTo(destinationFileName);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error moving directory: " + destinationFileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public static void MoveFiles(String searchPatter, String sourceDirectory, String destinationDirectory)
        {
            String destinationFileName = String.Empty;

            try
            {
                DirectoryInfo di = new DirectoryInfo(sourceDirectory);
                FileInfo[] otherFiles = di.GetFiles(searchPatter, SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in otherFiles)
                {
                    String SourceFileFullName = file.FullName;
                    String SourcefileName = file.Name;
                    destinationFileName = destinationDirectory + "\\" + SourcefileName;
                    File.Move(SourceFileFullName, destinationFileName);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error moving files: " + destinationFileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        public static void DeleteDirectoryAndParent(String folderName)
        {
            try
            {
                if (Directory.Exists(folderName) == true)
                {
                    DirectoryInfo di = new DirectoryInfo(folderName);
                    di.Delete(true);


                    // Delete Parent Directory (Band Directory) if it is empty
                    DirectoryInfo[] diSubdirectories = di.Parent.GetDirectories();
                    FileInfo[] fiArray = di.Parent.GetFiles();
                    if ((diSubdirectories.Length == 0) && (fiArray.Length == 0))
                    {
                        di.Parent.Delete();
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error deleting directory: " + folderName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        public static void DeleteEmptyDirectory(String folderName)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(folderName);
                FileInfo[] fi = di.GetFiles();
                if (fi.Length == 0)
                {
                    DirectoryInfo[] subDires = di.GetDirectories();
                    if (subDires.Length == 0)
                    {
                        di.Delete();
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error deleting directory: " + folderName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
    }
}
