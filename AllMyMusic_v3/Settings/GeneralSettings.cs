using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;


namespace AllMyMusic_v3.Settings
{
    public class GeneralSettings
    {
        #region Properties
        private String _applicationDataPath = String.Empty;
        public String ApplicationDataPath
        {
            get { return _applicationDataPath; }
            set { _applicationDataPath = value; }
        }

        private String activePlaylist = String.Empty;
        public String ActivePlaylist
        {
            get { return activePlaylist; }
            set { activePlaylist = value; }
        }

        private Boolean addTLENtag = true;
        public Boolean AddTLENtag
        {
            get { return addTLENtag; }
            set { addTLENtag = value; }
        }

        private Boolean checkForUpdate = false;
        public Boolean CheckForUpdate
        {
            get { return checkForUpdate; }
            set { checkForUpdate = value; }
        }

        private Int32 countryFlagHeight = 56;
        public Int32 CountryFlagHeight
        {
            get { return countryFlagHeight; }
            set { countryFlagHeight = value; }
        }

        private String languageGUI = "German";
        public String LanguageGUI
        {
            get { return languageGUI; }
            set { languageGUI = value; }
        }

        private Boolean deleteUnsupportedTags = false;
        public Boolean DeleteUnsupportedTags
        {
            get { return deleteUnsupportedTags; }
            set { deleteUnsupportedTags = value; }
        }

        private String downloadPath = String.Empty;
        public String DownloadPath
        {
            get { return downloadPath; }
            set { downloadPath = value; }
        }

        private String excelImportPath = String.Empty;
        public String ExcelImportPath
        {
            get { return excelImportPath; }
            set { excelImportPath = value; }
        }

        private String playlistActive = String.Empty;
        public String PlaylistActive
        {
            get { return playlistActive; }
            set { playlistActive = value; }
        }

        private String playlistPath = String.Empty;
        public String PlaylistPath
        {
            get { return playlistPath; }
            set { playlistPath = value; }
        }

        private Boolean playlistAutoplay = false;
        public Boolean PlaylistAutoplay
        {
            get { return playlistAutoplay; }
            set { playlistAutoplay = value; }
        }

        private Boolean removeXingFrames = false;
        public Boolean RemoveXingFrames
        {
            get { return removeXingFrames; }
            set { removeXingFrames = value; }
        }

        private Int32 sleeveImageSize = 400;
        public Int32 SleeveImageSize
        {
            get { return sleeveImageSize; }
            set { sleeveImageSize = value; }
        }

        private Int32 stampImageSize = 150;
        public Int32 StampImageSize
        {
            get { return stampImageSize; }
            set { stampImageSize = value; }
        }

        private String version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public String Version
        {
            get { return version; }
        }

        private Boolean writeID3V1Tags = false;
        public Boolean WriteID3V1Tags
        {
            get { return writeID3V1Tags; }
            set { writeID3V1Tags = value; }
        }

        private String wikipediaLanguage = "English";
        public String WikipediaLanguage
        {
            get { return wikipediaLanguage; }
            set { wikipediaLanguage = value; }
        }

        
        #endregion

        public void Save(XmlDocument doc, XmlNode node)
        {
            try
            {
                XmlNode nodeGeneral = doc.CreateElement("general");
                node.AppendChild(nodeGeneral);

                XmlNode nodePlaylist = doc.CreateElement("activePlaylist");
                nodePlaylist.InnerText = activePlaylist;
                nodeGeneral.AppendChild(nodePlaylist);

                XmlNode nodeUpdateTLEN = doc.CreateElement("addTLEN");
                nodeUpdateTLEN.InnerText = addTLENtag.ToString();
                nodeGeneral.AppendChild(nodeUpdateTLEN);

                XmlNode nodeCheckForUpdate = doc.CreateElement("checkForUpdate");
                nodeCheckForUpdate.InnerText = checkForUpdate.ToString();
                nodeGeneral.AppendChild(nodeCheckForUpdate);

                XmlNode nodeCountryFlagHeight = doc.CreateElement("countryFlagHeight");
                nodeCountryFlagHeight.InnerText = countryFlagHeight.ToString();
                nodeGeneral.AppendChild(nodeCountryFlagHeight);

                XmlNode nodeLanguageGUI = doc.CreateElement("languageGUI");
                nodeLanguageGUI.InnerText = languageGUI;
                nodeGeneral.AppendChild(nodeLanguageGUI);

                XmlNode nodeDeleteUnsupportedTags = doc.CreateElement("deleteUnsupportedTags");
                nodeDeleteUnsupportedTags.InnerText = deleteUnsupportedTags.ToString();
                nodeGeneral.AppendChild(nodeDeleteUnsupportedTags);

                XmlNode nodeDownloadPath = doc.CreateElement("downloadPath");
                nodeDownloadPath.InnerText = downloadPath;
                nodeGeneral.AppendChild(nodeDownloadPath);

                XmlNode nodeExcelImportPath = doc.CreateElement("excelImportPath");
                nodeExcelImportPath.InnerText = excelImportPath;
                nodeGeneral.AppendChild(nodeExcelImportPath);

                XmlNode nodeRemoveXingFrames = doc.CreateElement("removeXingFrames");
                nodeRemoveXingFrames.InnerText = removeXingFrames.ToString();
                nodeGeneral.AppendChild(nodeRemoveXingFrames);

                XmlNode nodePlaylistPath = doc.CreateElement("playlistPath");
                nodePlaylistPath.InnerText = playlistPath;
                nodeGeneral.AppendChild(nodePlaylistPath);

                XmlNode nodePlaylistAutoplay = doc.CreateElement("playlistAutoplay");
                nodePlaylistAutoplay.InnerText = PlaylistAutoplay.ToString();
                nodeGeneral.AppendChild(nodePlaylistAutoplay);

                XmlNode nodeSleeveImageSize = doc.CreateElement("sleeveImageSize");
                nodeSleeveImageSize.InnerText = sleeveImageSize.ToString();
                nodeGeneral.AppendChild(nodeSleeveImageSize);

                XmlNode nodeStampImageSize = doc.CreateElement("stampImageSize");
                nodeStampImageSize.InnerText = stampImageSize.ToString();
                nodeGeneral.AppendChild(nodeStampImageSize);


                XmlNode nodeWikipediaLanguage = doc.CreateElement("wikipediaLanguage");
                nodeWikipediaLanguage.InnerText = wikipediaLanguage;
                nodeGeneral.AppendChild(nodeWikipediaLanguage);

                XmlNode nodeWriteID3V1Tags = doc.CreateElement("writeID3V1Tags");
                nodeWriteID3V1Tags.InnerText = writeID3V1Tags.ToString();
                nodeGeneral.AppendChild(nodeWriteID3V1Tags);

                XmlNode nodeVersion = doc.CreateElement("version");
                nodeVersion.InnerText = version;
                nodeGeneral.AppendChild(nodeVersion);

            }
            catch (Exception Err)
            {
                String errorMessage = "Error saving General settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "activePlaylist":
                            activePlaylist = childNode.InnerText;
                            break;

                        case "addTLEN":
                            addTLENtag = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "checkForUpdate":
                            checkForUpdate = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "countryFlagHeight":
                            countryFlagHeight = Convert.ToInt32(childNode.InnerText);
                            break;

                        case "languageGUI":
                            languageGUI = childNode.InnerText;
                            break;
                                    
                        case "deleteUnsupportedTags":
                            deleteUnsupportedTags = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "downloadPath":
                            downloadPath = childNode.InnerText;
                            break;

                        case "excelImportPath":
                            excelImportPath = childNode.InnerText;
                            break;

                        case "playlistPath":
                            PlaylistPath = childNode.InnerText;
                            break;

                        case "playlistAutoplay":
                            playlistAutoplay = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "removeXingFrames":
                            removeXingFrames = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "sleeveImageSize":
                            sleeveImageSize = Convert.ToInt32(childNode.InnerText);
                            break;

                        case "stampImageSize":
                            stampImageSize = Convert.ToInt32(childNode.InnerText);
                            break;

                        case "wikipediaLanguage":
                            wikipediaLanguage = childNode.InnerText;
                            break;

                        case "writeID3V1Tags":
                            writeID3V1Tags = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "version":
                            //  don't load the version from the config file.
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error loading General settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
    }
}
