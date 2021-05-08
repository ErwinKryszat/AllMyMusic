using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

using System.Diagnostics;

using AllMyMusic.DataService;
using AllMyMusic.Settings;
using AllMyMusic.QueryBuilder;
using Metadata.Mp3;
using Metadata.ID3;

namespace AllMyMusic
{
    public class CoverImageTools : IDisposable
    {
        // Rename CD Cover Images and add the filename to the Images table
        // Naming Convention:
        // Groupname - albumname - Frontal.jpg

        

        #region Fields
        private DataServiceAlbums_SQL _dataServiceAlbums;
        #endregion


        public CoverImageTools(ConnectionInfo conInfo)
        {
            _dataServiceAlbums = new DataServiceAlbums_SQL(conInfo);
        }

        public void Close()
        {
            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Close();
            }
        }

        public async Task<Int32> ManageImages(String albumFolderPath)
        {
            CoverImageOptions options = new CoverImageOptions();
            options.CreateFolderImage = AppSettings.CoverImageSettings.CreateStamps;
            options.CreateStampsFiles = AppSettings.CoverImageSettings.CreateStamps;
            options.InsertFrontImage = AppSettings.CoverImageSettings.InsertFrontImage;
            options.InsertStampImage = AppSettings.CoverImageSettings.InsertStampImage;
            options.RemoveImages = AppSettings.CoverImageSettings.RemoveImages;
            options.RenameImageFiles = AppSettings.CoverImageSettings.RenameImages;
            options.SaveImageToDisk = AppSettings.CoverImageSettings.SaveToDisk;
            options.StampSize = AppSettings.CoverImageSettings.StampResolution;


            Int32 count = await ManageImagesForFolderpath(albumFolderPath, options);
            return count;
        }

        private async Task<Int32> ManageImagesForFolderpath(String albumFolderPath, CoverImageOptions options)
        {
            ObservableCollection<AlbumItem> albums = await _dataServiceAlbums.GetAlbumsByPath(albumFolderPath);
            AlbumItem album = albums[0];
            List<FileInfo> imageFiles = GetImageFileList(album.AlbumPath);

            Int32 count = 0;

            if ((imageFiles != null) && (imageFiles.Count == 0))
            {
                LaunchAlbumArtTool(album);
                imageFiles = GetImageFileList(album.AlbumPath);
            }

            if ((imageFiles != null) && (imageFiles.Count > 0))
            {

                if (options.RenameImageFiles == true)
                {
                    count += RenameImages(album.BandName, album.AlbumName, imageFiles);
                }
                if (options.CreateStampsFiles == true)
                {
                    count += CreateStamps(album.BandName, album.AlbumName, imageFiles);
                }

                if (options.CreateFolderImage == true)
                {
                    count += CreateFolderImage(imageFiles);
                }

                imageFiles = GetImageFileList(albumFolderPath);
                album = ParseImageFilename(imageFiles, album);

                AddImage(album);
            }

            if (options.RemoveImages == true)
            {
                count += RemoveAPIC(albumFolderPath);
            }

            if ((options.InsertStampImage == true) || (options.InsertFrontImage == true))
            {
                count += InsertAPIC(albumFolderPath, album, options);
            }

            if (options.SaveImageToDisk == true)
            {
                String ImagefileName = AppSettings.CoverImageSettings.ImageFilename;
                SaveAPIC(albumFolderPath, ImagefileName);
                imageFiles = GetImageFileList(albumFolderPath);
            }

            return count;
        }
        private void LaunchAlbumArtTool(AlbumItem album)
        {
            String pathAlbumArt = @"C:\Program Files\AlbumArtDownloader\AlbumArt.exe";
            if (File.Exists(pathAlbumArt) == true)
            {
                // Prepare the process to run
                ProcessStartInfo start = new ProcessStartInfo();
                // Enter in the command line arguments, everything you would enter after the executable name itself
                start.Arguments = GetAlbumArtArguments(album);
                // Enter the executable to run, including the complete path
                start.FileName = pathAlbumArt;
                // Do you want to show a console window?
                start.WindowStyle = ProcessWindowStyle.Normal;
                start.CreateNoWindow = false;
                int exitCode;


                // Run the external process & wait for it to finish
                using (Process proc = Process.Start(start))
                {
                    proc.WaitForExit();

                    // Retrieve the app's exit code
                    exitCode = proc.ExitCode;
                }
            }
        }

        private String GetAlbumArtArguments(AlbumItem album)
        {
            String arguments = String.Empty;
            arguments = "/artist " + "\"" + album.BandName + "\" "
                + "/album " + "\"" + album.AlbumName + "\" "
                + "/path " + "\"" + album.AlbumPath + "folder.jpg\" "
                + "/minsize 500 ";

            return arguments;
        }

        private void AddImage(AlbumItem album)
        {
            Task.Run(() => _dataServiceAlbums.AddImage(album));
        }

        private List<FileInfo> GetImageFileList(String folderPath)
        {
            List<FileInfo> imageFiles = new List<System.IO.FileInfo>();
            if (Directory.Exists(folderPath) == true)
            {
                // Collect all image Files in this folder

                DirectoryInfo di = new DirectoryInfo(folderPath);
                FileInfo[] allFiles = di.GetFiles("*.*", SearchOption.TopDirectoryOnly);

                foreach (FileInfo coverFile in allFiles)
                {
                    String extension = Path.GetExtension(coverFile.Name).ToLower();
                    if ((extension == ".jpg") || (extension == ".png"))
                    {
                        imageFiles.Add(coverFile);
                    }
                }
            }
           
            return imageFiles;
        }
        private Int32 InsertAPIC(String folderPath, AlbumItem album, CoverImageOptions options)
        {
            String fileName = String.Empty;
            Image coverImage = null;
            Int32 Count = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                FileInfo[] MusicFiles = di.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
                if (MusicFiles.Length == 0) { return 0; }

                String imageFilename = String.Empty;
                if ((options.InsertStampImage == true) && (album.StampImageFullpath != String.Empty))
                {
                    imageFilename = album.StampImageFullpath;
                }

                if ((options.InsertFrontImage == true) && (album.FrontImageFullpath != String.Empty))
                {
                    imageFilename = album.FrontImageFullpath ;
                }

                if (File.Exists(imageFilename))
                {
                    coverImage = Image.FromFile(imageFilename);
                }

                foreach (FileInfo MusicFile in MusicFiles)
                {
                    fileName = MusicFile.FullName;
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Length < AppSettings.CoverImageSettings.MinimalFileSize)
                    {
                        // Ignore small files and files with size = 10000
                        continue;
                    }

                    if (coverImage != null)
                    {
                        Mp3Metaedit metaData = new Mp3Metaedit(fileName);
                        metaData.UpdatePicture(coverImage, PictureType.FrontalCover);
                        metaData.SaveMetadata();

                        Count++;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error InsertAPIC for file: " + fileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            finally 
            {
                if (coverImage != null)
                {
                    coverImage.Dispose();
                }
            }
            
            return Count;
        }
        private Int32 RemoveAPIC(String folderPath)
        {
            String fileName = String.Empty;
            Int32 Count = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                FileInfo[] MusicFiles = di.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
                if (MusicFiles.Length == 0) { return 0; }

                foreach (FileInfo MusicFile in MusicFiles)
                {
                    fileName = MusicFile.FullName;
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Length < AppSettings.CoverImageSettings.MinimalFileSize)
                    {
                        // Ignore small files and files with size = 0
                        continue;
                    }

                    Mp3Metaedit metaData = new Mp3Metaedit(fileName);
                    Int32 countDone =  metaData.DeletePictureTags();
                    metaData.SaveMetadata();

                    if (countDone > 0)
                    {
                        Count++; 
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error RemoveAPIC for file: " + fileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            return Count;
        }
        private void SaveAPIC(String folderPath, String imageFilename)
        {
            String fileName = String.Empty;
            try
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                FileInfo[] MusicFiles = di.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
                if (MusicFiles.Length == 0) { return; }

                foreach (FileInfo MusicFile in MusicFiles)
                {
                    fileName = MusicFile.FullName;
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Length < AppSettings.CoverImageSettings.MinimalFileSize)
                    {
                        // Ignore small files and files with size = 0
                        continue;
                    }

                    if (File.Exists(folderPath + "\\" + imageFilename) == false)
                    {
                        TagReader tagReader = new TagReader(fileName);
                        Image image = tagReader.GetAttachedPicture(PictureType.FrontalCover);
                        if (image != null)
                        {
                            FileStream fs = new FileStream(folderPath + "\\" + imageFilename, FileMode.Create, FileAccess.Write, FileShare.Write);
                            image.Save(fs, ImageFormat.Jpeg);
                            fs.Close();
                            break; 
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error SaveAPIC for file: " + fileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        private int RenameImages(String bandName, String albumName, List<FileInfo> imageFiles)
        {
            String OldFilename = String.Empty;
            
            String FileType = String.Empty;

            int count = 0;  // Counter for number of rename files ( just 1 or 0 at present)

            try
            {
                foreach (FileInfo file in imageFiles)
                {
                    OldFilename = Path.GetFileNameWithoutExtension(file.Name);

                    if (OldFilename.Length >= 2)
                    {
                        if (OldFilename.Substring(OldFilename.Length - 2, 2) == "_a")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Frontal");
                            continue;
                        }
                        if (OldFilename.Substring(OldFilename.Length - 2, 2) == "_b")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Back");
                            continue;
                        }
                        if (OldFilename.Substring(OldFilename.Length - 2, 2) == "-a")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Frontal");
                            continue;
                        }
                        if (OldFilename.Substring(OldFilename.Length - 2, 2) == "-b")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Back");
                            continue;
                        }
                        if (OldFilename.Substring(OldFilename.Length - 2, 2) == "-f")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Frontal");
                            continue;
                        }
                        if (OldFilename.Substring(OldFilename.Length - 2, 2) == "-b")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Back");
                            continue;
                        }
                        if (OldFilename.Substring(OldFilename.Length - 2, 2).ToUpper() == "CD")
                        {
                            count += RenameImageFile(file, bandName, albumName, "CD");
                            continue;
                        }
                    }

                    if (OldFilename.Length >= 3)
                    {

                    }

                    if (OldFilename.Length >= 4)
                    {
                        if (OldFilename.Substring(OldFilename.Length - 4, 4).ToUpper() == "BACK")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Back");
                            continue;
                        }
                    }

                    if (OldFilename.Length >= 5)
                    {
                        if (OldFilename.Substring(OldFilename.Length - 5, 5).ToUpper() == "FRONT")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Frontal");
                            continue;
                        }
                        if (OldFilename.Substring(OldFilename.Length - 5, 5).ToUpper() == "STAMP")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Stamp");
                            continue;
                        }

                        if (OldFilename.Substring(OldFilename.Length - 5, 5).ToUpper() == "INLAY")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Inlay");
                            continue;
                        }
                    }

                    if (OldFilename.Length >= 6)
                    {
                        if (OldFilename.Substring(OldFilename.Length - 6, 6).ToUpper() == "FOLDER")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Frontal");
                            continue;
                        }
                        if (OldFilename.Substring(OldFilename.Length - 6, 6).ToUpper() == "INSIDE")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Inside");
                            continue;
                        }
                    } 

                    if (OldFilename.Length >= 7)
                    {
                        if (OldFilename.Substring(OldFilename.Length - 7, 7).ToUpper() == "FRONTAL")
                        {
                            count += RenameImageFile(file, bandName, albumName, "Frontal");
                            continue;
                        }
                    }

                    // default action if none of the filters above has matched the filename
                    count += RenameImageFile(file, bandName, albumName, "Frontal");
                    continue;
                }
            }

            catch (Exception Err)
            {
                String errorMessage = "Error RenameImages for file: " + OldFilename;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }

            return count;
        }
        private Int32 RenameImageFile(FileInfo file, String bandName, String albumName, String FileType)
        {
            String NewFilename = String.Empty;
            if (FileType == "")
            {
                NewFilename = file.DirectoryName + "\\"
                    + FileName.RemoveForbiddenChars(bandName) + " - "
                    + FileName.RemoveForbiddenChars(albumName) + " - Frontal";
            }
            else
            {
                NewFilename = file.DirectoryName + "\\"
                    + FileName.RemoveForbiddenChars(bandName) + " - "
                    + FileName.RemoveForbiddenChars(albumName) + " - " + FileType;
            }

            String extension = Path.GetExtension(file.FullName).ToLower();
            if (file.FullName != (NewFilename + extension))
            {
                if (!File.Exists(NewFilename + extension))
                {
                    file.MoveTo(NewFilename + extension);
                    return 1;
                }
            }
            return 0;
        }

        private int CreateStamps(String bandName, String albumName, List<FileInfo> imageFiles)
        {
            int count = 0;

            String folderPath = imageFiles[0].DirectoryName;
            String FrontimageFilename = String.Empty;
            String StampimageFilename = String.Empty;
            String FolderimageFilename = String.Empty;
            Int32 imgFileSize = AppSettings.CoverImageSettings.StampResolution;

            foreach (FileInfo file in imageFiles)
            {
                String Filename = Path.GetFileNameWithoutExtension(file.Name);

                if (Filename.Length > 7)
                {
                    if (Filename.Substring(Filename.Length - 7, 7) == "Frontal")
                    {
                        FrontimageFilename = file.FullName;
                    }
                }

                if (Filename.Length == 6)
                {
                    if (Filename == "Folder")
                    {
                        FolderimageFilename = file.FullName;
                    }
                }

                if (Filename.Length > 5)
                {
                    if (Filename.Substring(Filename.Length - 5, 5) == "Stamp")
                    {
                        StampimageFilename = file.FullName;
                    }
                }

            }

            // Stamp Image already exist or there is no frontal cover found
            if (FrontimageFilename != "")
            {
                String StampFilename = String.Empty;
                try
                {
                    // Create the thumbnail image for this album
                    String extension = Path.GetExtension(FrontimageFilename).ToLower();
                    StampFilename = FileName.RemoveForbiddenChars(bandName) + " - " + FileName.RemoveForbiddenChars(albumName) + " - Stamp" + extension;

                    PictureBox pb = new PictureBox();
                    pb.Image = Thumbnail.GetThumbnail(FrontimageFilename, imgFileSize, imgFileSize);
                    if (pb.Image != null)
                    {
                        pb.Image.Save(folderPath + '\\' + StampFilename);
                    }
                    pb.Dispose();
                    count++;
                }
                catch (Exception Err)
                {
                    String errorMessage = "Error CreateStamps for file: " + FrontimageFilename;
                    ShowError.ShowAndLog(Err, errorMessage, 2001);
                }
            }
            return count;
        }
        private int CreateFolderImage(List<FileInfo> imageFiles)
        {
            int count = 0;

            String folderPath = imageFiles[0].DirectoryName;
            String FrontimageFilename = String.Empty;;
            String FolderimageFilename = String.Empty;

            foreach (FileInfo file in imageFiles)
            {
                String Filename = Path.GetFileNameWithoutExtension(file.Name);

                if (Filename.Length > 7)
                {
                    if (Filename.Substring(Filename.Length - 7, 7) == "Frontal")
                    {
                        FrontimageFilename = file.FullName;
                    }
                }

                if (Filename.Length == 6)
                {
                    if (Filename == "Folder")
                    {
                        FolderimageFilename = file.FullName;
                    }
                }
            }

            // Stamp Image already exist or there is no frontal cover found
            if (FrontimageFilename != "")
            {
                String Filename = "folder.jpg";

                // To Be Deleted **********************************************************************************************
                if (File.Exists(folderPath + '\\' + Filename) == true)
                {
                    File.Delete(folderPath + '\\' + Filename);
                }

                if (File.Exists(folderPath + '\\' + Filename) == false)
                {
                    try
                    {


                        // Copy the front image to a file called "folder.jpg" with max 1024 resolution
                        // Resize to 953 pixe if it is bigger

                        PictureBox pb = new PictureBox();
                        pb.Image = Image.FromFile(FrontimageFilename);

                        if ((pb.Image.Width <= 1024) && (pb.Image.Width <= 1024))
                        {
                            File.Copy(FrontimageFilename, folderPath + '\\' + Filename);
                        }
                        else
                        {
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);    // 100% Quality
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            ImageCodecInfo myImageCodecInfo = GetEncoder(ImageFormat.Jpeg);


                            pb.Image = Thumbnail.GetFolderImageSonos(FrontimageFilename);
                            pb.Image.Save(folderPath + '\\' + Filename, myImageCodecInfo, myEncoderParameters);
                        }
                        

                        pb.Dispose();
                        count++;
                    }
                    catch (Exception Err)
                    {
                        String errorMessage = "Error CreateFolderImage for file: " + folderPath + '\\' + Filename;
                        ShowError.ShowAndLog(Err, errorMessage, 2001);
                    }
                }
            }
            return count;
        }

        private  ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private AlbumItem ParseImageFilename(List<FileInfo> imageFiles, AlbumItem album)
        {
            foreach (FileInfo CoverFile in imageFiles)
            {
                if (CoverFile.Name.Length >= 11)
                {
                    if (CoverFile.Name.Substring(CoverFile.Name.Length - 11, 7).ToLower() == "frontal")
                    {
                        album.FrontImageFileName = CoverFile.Name;
                        continue;
                    }
                }
                if (CoverFile.Name.Length >= 10)
                {
                    if ((CoverFile.Name.Substring(CoverFile.Name.Length - 10, 6).ToLower() == "folder") && (String.IsNullOrEmpty(album.FrontImageFileName) == true))
                    {
                        album.FrontImageFileName = CoverFile.Name;
                        continue;
                    }
                }
                if (CoverFile.Name.Length >= 9)
                {
                    if (CoverFile.Name.Substring(CoverFile.Name.Length - 9, 5).ToLower() == "large")
                    {
                        album.FrontImageFileName = CoverFile.Name;
                        continue;
                    }
                    if (CoverFile.Name.Substring(CoverFile.Name.Length - 9, 5).ToLower() == "stamp")
                    {
                        album.StampImageFileName = CoverFile.Name;
                        continue;
                    }
                    if (CoverFile.Name.Substring(CoverFile.Name.Length - 9, 5).ToLower() == "small")
                    {
                        album.StampImageFileName = CoverFile.Name;
                        continue;
                    }
                }
                if (CoverFile.Name.Length >= 8)
                {
                    if (CoverFile.Name.Substring(CoverFile.Name.Length - 8, 4).ToLower() == "back")
                    {
                        album.BackImageFileName = CoverFile.Name;
                        continue;
                    }
                }
                if (CoverFile.Name.Length >= 5)
                {
                    if (CoverFile.Name.Substring(CoverFile.Name.Length - 5, 1).ToLower() == "a")
                    {
                        album.FrontImageFileName = CoverFile.Name;
                        continue;
                    }
                    if (CoverFile.Name.Substring(CoverFile.Name.Length - 5, 1).ToLower() == "b")
                    {
                        album.BackImageFileName = CoverFile.Name;
                        continue;
                    }
                    if (CoverFile.Name.Substring(CoverFile.Name.Length - 5, 1).ToLower() == "f")
                    {
                        album.FrontImageFileName = CoverFile.Name;
                        continue;
                    }
                }
            }

            return album;
      
        }
     
  
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

        ~CoverImageTools()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_dataServiceAlbums != null)
            {
                _dataServiceAlbums.Dispose();
            } 
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
