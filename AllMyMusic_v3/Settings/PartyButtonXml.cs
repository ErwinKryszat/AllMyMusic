using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;

using AllMyMusic_v3.ViewModel;


namespace AllMyMusic_v3.Settings
{
    public static class PartyButtonXml
    {
        #region Variables
        private static XmlDocument doc;
        #endregion
        
        #region Properties

        #endregion

        #region Methods
        public static void Save(ObservableCollection<PartyButtonConfigViewModel> playlistConfigurations, String fileName, String databaseName)
        {
            if (playlistConfigurations == null) { return; }
            fileName = fileName + "_" + databaseName + ".xml";

            try
            {
                doc = new XmlDocument();

                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);
                XmlElement nodePartyButtons = doc.CreateElement("partyButtons");
                doc.AppendChild(nodePartyButtons);

                for (int i = 0; i < playlistConfigurations.Count; i++)
                {
                    XmlNode nodePartyButton = SaveButton(doc, playlistConfigurations[i]);
                    nodePartyButtons.AppendChild(nodePartyButton);
                }

                doc.Save(fileName);
            }
            catch (IOException Err)
            {
                String errorMessage = "Error accessing file: " + fileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        private static XmlNode SaveButton(XmlDocument doc, PartyButtonConfigViewModel playlistConfiguration)
        {
            XmlNode nodePartyButton = doc.CreateElement("partyButton");

            XmlNode nodeButtonType = doc.CreateElement("buttonType");
            nodeButtonType.InnerText = playlistConfiguration.ButtonType.ToString();
            nodePartyButton.AppendChild(nodeButtonType);

            if (String.IsNullOrEmpty(playlistConfiguration.ButtonLabel) == false)
            {
                XmlNode nodeButtonLabel = doc.CreateElement("buttonLabel");
                nodeButtonLabel.InnerText = playlistConfiguration.ButtonLabel;
                nodePartyButton.AppendChild(nodeButtonLabel);
            }

            if (String.IsNullOrEmpty(playlistConfiguration.ButtonImagePath) == false)
            {
                XmlNode nodeButtonImagePath = doc.CreateElement("buttonImagePath");
                nodeButtonImagePath.InnerText = playlistConfiguration.ButtonImagePath;
                nodePartyButton.AppendChild(nodeButtonImagePath);
            }

            XmlNode nodeRandomize = doc.CreateElement("randomize");
            nodeRandomize.InnerText = playlistConfiguration.Randomize.ToString();
            nodePartyButton.AppendChild(nodeRandomize);

            if (String.IsNullOrEmpty(playlistConfiguration.SqlQuery) == false)
            {
                XmlNode nodeSqlQuery = doc.CreateElement("sqlQuery");
                nodeSqlQuery.InnerText = playlistConfiguration.SqlQuery;
                nodePartyButton.AppendChild(nodeSqlQuery);
            }

            if (String.IsNullOrEmpty(playlistConfiguration.ToolTipText)==false)
            {
                XmlNode nodeTooltipText = doc.CreateElement("tooltipText");
                nodeTooltipText.InnerText = playlistConfiguration.ToolTipText;
                nodePartyButton.AppendChild(nodeTooltipText);
            }

            if (String.IsNullOrEmpty(playlistConfiguration.TooltipImagePath) == false)
            {
                XmlNode nodeTooltipImagePath = doc.CreateElement("tooltipImagePath");
                nodeTooltipImagePath.InnerText = playlistConfiguration.TooltipImagePath;
                nodePartyButton.AppendChild(nodeTooltipImagePath);
            }


            if (playlistConfiguration.ButtonType == PartyButtonType.Albumlist)
            {
                SaveAlbumList(playlistConfiguration, nodePartyButton);
            }
            if (playlistConfiguration.ButtonType== PartyButtonType.Songlist)
            {
                SaveSongList(playlistConfiguration, nodePartyButton);
            }

            return nodePartyButton;
        }
        private static void SaveAlbumList(PartyButtonConfigViewModel playlistConfiguration, XmlNode nodePartyButton)
        {
            if ((playlistConfiguration.AlbumPathNames != null) && (playlistConfiguration.AlbumPathNames.Count > 0))
            {
                XmlNode nodeAlbums = doc.CreateElement("albums");
                nodePartyButton.AppendChild(nodeAlbums);

                for (int i = 0; i < playlistConfiguration.AlbumPathNames.Count; i++)
                {
                    XmlNode nodeAlbum = doc.CreateElement("album");

                    XmlAttribute attrAlbumPath = doc.CreateAttribute("albumPath");
                    attrAlbumPath.InnerText = playlistConfiguration.AlbumPathNames[i];
                    nodeAlbum.Attributes.Append(attrAlbumPath);

                    nodeAlbums.AppendChild(nodeAlbum);
                }
            }
        }
        private static void SaveSongList(PartyButtonConfigViewModel playlistConfiguration, XmlNode nodePartyButton)
        {
            if (playlistConfiguration.Playlist != null)
            {
                XmlNode nodeSongs = doc.CreateElement("songs");
                nodePartyButton.AppendChild(nodeSongs);

                for (int i = 0; i < playlistConfiguration.Playlist.Count; i++)
                {
                    XmlNode nodeSong = doc.CreateElement("song");

                    XmlAttribute attrSongPath = doc.CreateAttribute("songPath");
                    attrSongPath.InnerText = playlistConfiguration.Playlist[i].SongFullPath;
                    nodeSong.Attributes.Append(attrSongPath);

                    nodeSongs.AppendChild(nodeSong);
                }
            }
            else if ((playlistConfiguration.SongPathNames != null) && (playlistConfiguration.SongPathNames.Count > 0))
            {
                XmlNode nodeSongs = doc.CreateElement("songs");
                nodePartyButton.AppendChild(nodeSongs);

                for (int i = 0; i < playlistConfiguration.SongPathNames.Count; i++)
                {
                    XmlNode nodeSong = doc.CreateElement("song");

                    XmlAttribute attrSongPath = doc.CreateAttribute("songPath");
                    attrSongPath.InnerText = playlistConfiguration.SongPathNames[i];
                    nodeSong.Attributes.Append(attrSongPath);

                    nodeSongs.AppendChild(nodeSong);
                }
            }
        }

        public static ObservableCollection<PartyButtonConfigViewModel> Load(String fileName, String databaseName)
        {
            ObservableCollection<PartyButtonConfigViewModel>  playlistConfigurations = new ObservableCollection<PartyButtonConfigViewModel>();
            fileName = fileName + "_" + databaseName + ".xml";
            try
            {
                if (File.Exists(fileName) == false)
                {
                    return playlistConfigurations;
                }
                doc = new XmlDocument();
                doc.Load(fileName);
                XmlElement nodePartyButtons = doc.DocumentElement;

                foreach (XmlNode nodePartyButton in nodePartyButtons.ChildNodes)
                {
                    PartyButtonConfigViewModel buttonConfig = LoadButton(nodePartyButton);
                    buttonConfig.SmallButtonImage = ResourceHelper.GetImage(buttonConfig.ButtonImagePath);
                    buttonConfig.LargeButtonImage = ResourceHelper.GetImage(buttonConfig.ButtonImagePath);
                    if (buttonConfig.SmallButtonImage == null)
                    {
                        Uri resorcePath = new Uri(Global.Instruments + "g5.png", UriKind.Relative);
                        buttonConfig.SmallButtonImage = ResourceHelper.GetImage(resorcePath);
                        buttonConfig.LargeButtonImage = ResourceHelper.GetImage(resorcePath);
                    }

                    playlistConfigurations.Add(buttonConfig);
                }
                return playlistConfigurations;
            }
            catch (IOException Err)
            {
                String errorMessage = "Error accessing file: " + fileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            return playlistConfigurations;
        }
        private static PartyButtonConfigViewModel LoadButton(XmlNode nodePartyButton)
        {
            PartyButtonConfigViewModel button = new PartyButtonConfigViewModel();

            foreach (XmlNode childNode in nodePartyButton.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "buttonType":
                        button.ButtonType = (PartyButtonType)Enum.Parse(typeof(PartyButtonType), childNode.InnerText);
                    break;

                    case "buttonLabel":
                        button.ButtonLabel = childNode.InnerText;
                    break;

                    case "buttonImagePath":
                        button.ButtonImagePath = childNode.InnerText;
                    break;

                    case "randomize":
                        button.Randomize = Convert.ToBoolean(childNode.InnerText);
                    break;

                    case "sqlQuery":
                        button.SqlQuery = childNode.InnerText;
                    break;

                    case "tooltipText":
                        button.ToolTipText = childNode.InnerText;
                    break;

                    case "tooltipImagePath":
                        button.TooltipImagePath = childNode.InnerText;
                    break;

                    case "albums":
                        button.AlbumPathNames = LoadAlbumList(childNode, button);
                    break;

                    case "songs":
                    button.SongPathNames = LoadSongList(childNode, button);
                    break;

                    default:
                        break;
                }
            }

            return button;
        }
        private static ObservableCollection<String> LoadAlbumList(XmlNode albumsNode, PartyButtonConfigViewModel button)
        {
            ObservableCollection<String> albums = new ObservableCollection<String>();

            foreach (XmlNode albumNode in albumsNode.ChildNodes)
            {
                String albumPath = albumNode.Attributes["albumPath"].InnerText;
                albums.Add(albumPath);
            }
            return albums;
        }
        private static ObservableCollection<String> LoadSongList(XmlNode songsNode, PartyButtonConfigViewModel button)
        {
            ObservableCollection<String> songs = new ObservableCollection<String>();

            foreach (XmlNode songNode in songsNode.ChildNodes)
            {
                String songPath = songNode.Attributes["songPath"].InnerText;
                songs.Add(songPath);
            }
            return songs;
        }
    #endregion
    }
}
