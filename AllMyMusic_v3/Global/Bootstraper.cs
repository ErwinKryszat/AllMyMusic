using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AllMyMusic_v3.Settings;
using System.IO;


namespace AllMyMusic_v3
{
    public class Bootstraper
    {
        #region Fields
        private String _applicationDataPath = String.Empty;
        private String _applicationSettingsFile;
        private String _applicationSettingsFile_v2;
        private String _renamePatternFile;
        private String _autoTagPatternFile;
        private String _dbSetupFile;
        private String _partyButtonConfigFile;
        private String _worldCountriesFile;
        private String _flagsPath;
        private String _partyButtonImages;
        private String _playlistPath;

        //private String _applicationLogfilePath;
        //private String _cachePath;
        //private String _cachePathImages;

        #endregion

        #region Constructor
        public Bootstraper()
        {
                       
        }
        #endregion

        #region Properties
        #endregion
        
      
 
        public Boolean CreateApplicationDirectories()
        {
            SetChildDirectories();

            if ( String.IsNullOrEmpty(_applicationSettingsFile) == true)
            {
                return false;
            }

            DirectoryInfo CommonApplicationData = null;
            if (Directory.Exists(_applicationDataPath) == false)
            {
                CommonApplicationData = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                CommonApplicationData.CreateSubdirectory("AllMyMusic");
            }

          
            if (Directory.Exists(_flagsPath) == false)
            {
                DirectoryInfo applicationDirectory = new DirectoryInfo(_applicationDataPath);
                applicationDirectory.CreateSubdirectory("Flags");

                CopyFlagImagesFromUserToProgDirectory();
            }

            if (Directory.Exists(_partyButtonImages) == false)
            {
                DirectoryInfo applicationDirectory = new DirectoryInfo(_applicationDataPath);
                applicationDirectory.CreateSubdirectory("PlaylistImages");
            }

            if (Directory.Exists(_playlistPath) == false)
            {
                String pathMyMusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

                DirectoryInfo myMusicDirectory = new DirectoryInfo(pathMyMusic);
                myMusicDirectory.CreateSubdirectory("Playlists");
            }

            //if (Directory.Exists(_applicationLogfilePath) == false)
            //{
            //    DirectoryInfo applicationDirectory = new DirectoryInfo(_applicationDataPath);
            //    applicationDirectory.CreateSubdirectory("Log");
            //}

            //if (Directory.Exists(_cachePath) == false)
            //{
            //    DirectoryInfo applicationDirectory = new DirectoryInfo(_applicationDataPath);
            //    applicationDirectory.CreateSubdirectory("Cache");
            //}

            //if (Directory.Exists(_cachePath + "\\discogs") == false)
            //{
            //    DirectoryInfo DiscogsDataPath = new DirectoryInfo(_cachePath);
            //    DiscogsDataPath.CreateSubdirectory("discogs");
            //}

            //if (Directory.Exists(_cachePath + "\\musicbrainz") == false)
            //{
            //    DirectoryInfo MusicbrainzDataPath = new DirectoryInfo(_cachePath);
            //    MusicbrainzDataPath.CreateSubdirectory("musicbrainz");
            //}

            //if (Directory.Exists(_cachePath + "\\Images") == false)
            //{
            //    DirectoryInfo ImagesPath = new DirectoryInfo(_cachePath);
            //    ImagesPath.CreateSubdirectory("Images");
            //}

            return true;
        }

        private void CopyFlagImagesFromUserToProgDirectory()
        {
            String userDirectory =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AllMyMusic"; //  C:\Users\Erwin\AppData\Roaming\AllMyMusic
            String userFlagImages = userDirectory + @"\Flags";

            if (Directory.Exists(userFlagImages))
            {
                DirectoryInfo dirSource = new DirectoryInfo(userFlagImages);
                FileInfo[] files = dirSource.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(_flagsPath, file.Name);
                    file.CopyTo(temppath, false);
                }
            }
        }

        private void SetChildDirectories()
        {
            _applicationDataPath = GetApplicationDataPath();


            _applicationSettingsFile = _applicationDataPath + @"\AllMyMusic_v3.xml";
            _applicationSettingsFile_v2 = _applicationDataPath + @"\AllMyMusic_v2.xml";

            _renamePatternFile = _applicationDataPath + @"\renameFiles_v3.txt";
            _autoTagPatternFile = _applicationDataPath + @"\autotagFiles_v3.txt";
            _dbSetupFile = _applicationDataPath + @"\db_setup.xml";
            _partyButtonConfigFile = _applicationDataPath + @"\partyButtonConfig";
            _worldCountriesFile = _applicationDataPath + @"\world.xml";
            _flagsPath = _applicationDataPath + @"\Flags";
            _partyButtonImages = _applicationDataPath + @"\PlaylistImages";
            //_applicationLogfilePath = _applicationDataPath + @"\Log";
            //_cachePath = _applicationDataPath + @"\Cache";
            //_cachePathImages = _applicationDataPath + @"\Cache\Images";

            String pathMyMusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            _playlistPath = pathMyMusic + @"\Playlists";


            Global.ApplicationSettingsPath = _applicationSettingsFile;

            Global.RenamePatternFile = _renamePatternFile;
            Global.AutoTagPatternFile = _autoTagPatternFile;
            Global.DbSetupFile = _dbSetupFile;
            Global.PartyButtonConfigFile = _partyButtonConfigFile;
            Global.WorldCountriesFile = _worldCountriesFile;
            Global.FlagsPath = _flagsPath;
            //Global.ApplicationLogfilePath = _applicationLogfilePath;
            //Global.CachePath = _cachePath;
            //Global.CachePathImages = _cachePathImages;
            Global.PartyButtonImages = _partyButtonImages;
        }

        private String GetApplicationDataPath()
        {
            //return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AllMyMusic"; //  C:\Users\Erwin\AppData\Roaming\AllMyMusic

            InstallationContexts appContext = InstallerHelper.GetInstalledContext("AllMyMusic_v3");

            switch (appContext)
            {
                case InstallationContexts.NotInstalled:
                    return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\AllMyMusic"; // C:\ProgramData
                case InstallationContexts.Everyone:
                    return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\AllMyMusic"; // C:\ProgramData
                case InstallationContexts.JustMe:
                    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AllMyMusic"; // C:\Users\Erwin\AppData\Roaming\AllMyMusic
                default:
                    return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\AllMyMusic"; //C:\ProgramData
            }
        }

        public void CopyResources()
        {
            if (File.Exists(_worldCountriesFile) == false)
            {
                ResourceHelper.CopyResourceTextFileToFilesystem("AllMyMusic_v3.Resources.Text.world.xml", _worldCountriesFile);
            }

            if (File.Exists(_renamePatternFile) == false)
            {
                ResourceHelper.CopyResourceTextFileToFilesystem("AllMyMusic_v3.Resources.Text.renameFiles_v3.txt", _renamePatternFile);
            }

            if (File.Exists(_autoTagPatternFile) == false)
            {
                ResourceHelper.CopyResourceTextFileToFilesystem("AllMyMusic_v3.Resources.Text.autotagFiles_v3.txt", _autoTagPatternFile);
            }
        }
        public void CopyPartybuttonImages()
        {
            String PartyButtonImagePath = _partyButtonImages;

            DirectoryInfo di = new DirectoryInfo(PartyButtonImagePath);
            FileInfo[] fi = di.GetFiles("*.png");
            if ((fi.Length == 0) && (File.Exists(PartyButtonImagePath + "\\bass1.png") == false))
            {
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.bass1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.bass2.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.bass3.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.drums1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.drums2.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.drums3.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.drums4.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g3.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g4.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g5.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g6.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g7.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g8.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g9.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.g10.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.harp1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.sax1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.sax2.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.balalaika1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.bandola1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.e-cello1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.grand-piano1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.hat1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.hat2.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.hat3.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.horn1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.keyboard1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.keyboard2.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.keyboard3.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.contrabass1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.mic1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.mic2.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.piano1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.speaker1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.speaker2.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.trumpet1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.tuba1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.violin1.png", PartyButtonImagePath);
                ResourceHelper.CopyImageToFolder("AllMyMusic_v3.Resources.Instruments.violin2.png", PartyButtonImagePath);
            }
        }

        public void LoadApplicationSettings()
        {
            if (File.Exists(_applicationSettingsFile) == true)
            {
                AppSettings.Load(_applicationSettingsFile);
            }
            else if (File.Exists(_applicationSettingsFile_v2) == true)
            {
                AppSettings.Load(_applicationSettingsFile_v2);
            }
            else
            {
                AppSettings._fileName = _applicationSettingsFile;
            }

            AppSettings.GeneralSettings.ApplicationDataPath = _applicationDataPath;
            AppSettings.GeneralSettings.PlaylistPath = _playlistPath;
        }
    }
}
