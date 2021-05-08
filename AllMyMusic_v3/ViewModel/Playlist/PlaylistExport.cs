using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;


namespace AllMyMusic.ViewModel
{
    public static class PlaylistExport
    {
        /// <summary>
        /// Export a PlaylistItemCollection file (.m3u, .pls, .xspf)
        /// </summary>
        public static void Export(String fileName, ObservableCollection<SongItem> songs)
        {
            String playlistType = Path.GetExtension(fileName).ToUpper();

            switch (playlistType)
            {
                case ".M3U":
                    Export_M3U(fileName, songs);
                    break;

                case ".PLS":
                    Export_PLS(fileName, songs);
                    break;

                case ".XSPF":
                    Export_XSPF(fileName, songs);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported PlaylistItemCollection Format: " + playlistType);
            }
        }

        /// <summary>
        /// Export a PlaylistItemCollection file (.m3u)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="M3U_EXTENDED"></param>
        /// <returns></returns>
        private static void Export_M3U(String fileName, ObservableCollection<SongItem> songs)
        {
            Boolean M3U_EXTENDED = false;
            StreamWriter streamOut = new StreamWriter(fileName);

            if (M3U_EXTENDED)
            {
                streamOut.WriteLine("#EXTM3U");
            }

            for (int i = 0; i < songs.Count; i++)
            {
                if (M3U_EXTENDED)
                {
                    streamOut.WriteLine("#EXTINF:"
                           + songs[i].Seconds.ToString() + ","
                           + songs[i].BandName + " - "
                           + songs[i].SongTitle);
                    streamOut.WriteLine(songs[i].SongPath + songs[i].SongFilename);
                }
                else
                {
                    streamOut.WriteLine(songs[i].SongPath + songs[i].SongFilename);
                }
            }

            streamOut.Close();
        }

        /// <summary>
        /// Export a PlaylistItemCollection file ( .pls)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static void Export_PLS(String fileName, ObservableCollection<SongItem> songs)
        {
            StreamWriter streamOut = new StreamWriter(fileName);
            streamOut.WriteLine("[playlist]");
            streamOut.WriteLine("NumberOfEntries=" + songs.Count.ToString());
            streamOut.WriteLine();

            Int32 number;
            for (int i = 0; i < songs.Count; i++)
            {
                number = i + 1;
                streamOut.WriteLine("File " + number.ToString() + "=" + songs[i].SongPath + songs[i].SongFilename);
                streamOut.WriteLine("Title " + number.ToString() + "=" + songs[i].SongTitle);
                streamOut.WriteLine("Length " + number.ToString() + "=" + songs[i].Seconds.ToString());
                streamOut.WriteLine();
            }

            streamOut.WriteLine("Version=2");

            streamOut.Close();
        }

        /// <summary>
        /// Export a PlaylistItemCollection file (.xspf)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static void Export_XSPF(String fileName, ObservableCollection<SongItem> songs)
        {

            //<?xml version="1.0" encoding="UTF-8"?>
            //<PlaylistItemCollection version="1" xmlns="http://xspf.org/ns/0/">
            //  <trackList>
            //    <track>
            //      <title>Nobody Move, Nobody Get Hurt</title>
            //      <creator>We Are Scientists</creator>
            //      <location>file:///mp3s/titel_1.mp3</location>
            //    </track>
            //    <track>
            //      <title>See The World</title>
            //      <creator>The Kooks</creator>
            //      <location>http://www.beispiel.com/musik/world.ogg</location>
            //    </track>
            //  </trackList>
            //</playlist>

            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declaration);
            String strNamespace = "http://xspf.org/ns/0/";
            XmlNode root = doc.CreateElement("playlist", strNamespace);
            XmlAttribute version = doc.CreateAttribute("version");
            version.InnerText = "1";
            root.Attributes.Append(version);
            doc.AppendChild(root);


            XmlNode trackList = doc.CreateNode(XmlNodeType.Element, "trackList", strNamespace);
            root.AppendChild(trackList);
            for (int i = 0; i < songs.Count; i++)
            {
                XmlNode track = doc.CreateNode(XmlNodeType.Element, "track", strNamespace);

                XmlNode title = doc.CreateNode(XmlNodeType.Element, "title", strNamespace);
                XmlNode creator = doc.CreateNode(XmlNodeType.Element, "creator", strNamespace);
                XmlNode location = doc.CreateNode(XmlNodeType.Element, "location", strNamespace);

                title.InnerText = songs[i].SongTitle;
                creator.InnerText = songs[i].BandName;
                location.InnerText = songs[i].SongPath + songs[i].SongFilename;

                track.AppendChild(title);
                track.AppendChild(creator);
                track.AppendChild(location);

                trackList.AppendChild(track);
            }


            doc.Save(fileName);
        }

       
    }
}
