using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;


namespace AllMyMusic_v3
{
    public static class PlaylistImport
    {
        /// <summary>
        /// Import a PlaylistItemCollection file (.m3u, .pls, .xspf)
        /// </summary>
        public static ObservableCollection<String> Import(String fileName)
        {
            String playlistType = Path.GetExtension(fileName).ToUpper();

            ObservableCollection<String>  songCollection = null;

            switch (playlistType)
            {
                case ".M3U":
                    songCollection = Import_M3U(fileName);
                    break;

                case ".PLS":
                    songCollection = Import_PLS(fileName);
                    break;

                case ".XSPF":
                    songCollection = Import_XSPF(fileName);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported PlaylistItemCollection Format: " + playlistType);
            }

            return songCollection;
        }
        private static ObservableCollection<String> Import_M3U(String fileName)
        {
            ObservableCollection<String> songCollection = new ObservableCollection<string>();

            StreamReader streamIn = new StreamReader(fileName);

            String line = streamIn.ReadLine();
            while (line != null)
            {
                if ((line.IndexOf("#EXTM3U") == -1) && (line.IndexOf("#EXTINF") == -1))
                {
                    songCollection.Add(line); 
                }
                line = streamIn.ReadLine();
            }
            streamIn.Close();

            return songCollection;
        }
        private static ObservableCollection<String> Import_PLS(String fileName)
        {
            ObservableCollection<String> songCollection = new ObservableCollection<string>();

            StreamReader streamIn = new StreamReader(fileName);

            String line = streamIn.ReadLine();
            while (line != null)
            {
                Int32 IndexOfFile = line.ToUpper().IndexOf("FILE");
                if (IndexOfFile >= 0)
                {
                    Int32 IndexOfEqual = line.IndexOf("=", IndexOfFile);
                    Int32 SequenceNumber = Convert.ToInt32(line.Substring(IndexOfFile + 5, IndexOfEqual - IndexOfFile - 5));
                    line = line.Substring(IndexOfEqual + 1, line.Length - IndexOfEqual - 1);

                    songCollection.Add(line); 
                }
                line = streamIn.ReadLine();
            }

            streamIn.Close();

            return songCollection;
        }
        private static ObservableCollection<String> Import_XSPF(String fileName)
        {
            ObservableCollection<String> songCollection = new ObservableCollection<string>();

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlElement rootNode = doc.DocumentElement;
            XmlNode trackList = rootNode.FirstChild;

            String title;
            String creator;
            String location;

            for (int i = 0; i < trackList.ChildNodes.Count; i++)
            {
                XmlNode track = trackList.ChildNodes[i];

                foreach (XmlNode node in track.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "title":
                            title = node.InnerText;
                            break;

                        case "creator":
                            creator = node.InnerText;
                            break;

                        case "location":
                            location = node.InnerText;
                            songCollection.Add(location); 
                            break;

                        default:
                            // unsupported node
                            break;
                    }
                }
            }
            return songCollection;
        }
    }
}
