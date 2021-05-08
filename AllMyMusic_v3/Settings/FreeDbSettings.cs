using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;


namespace AllMyMusic.Settings
{
    public class FreeDbSettings
    {
        public FreeDbSettings()
        {
            siteAdressCollection = new ArrayList();
        }

        #region Properties
        //  WebService
        private Boolean enabled = true;
        public Boolean Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        private String userName = "test";
        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private String hostName = "winisoft";
        public String HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private String clientName = "AllMyMusic";
        public String ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }

        private String version = "1.0";
        public String Version
        {
            get { return version; }
            set { version = value; }
        }

        private String defaultSite = "freedb.freedb.org";
        public String DefaultSite
        {
            get { return defaultSite; }
            set { defaultSite = value; }
        }

        private ArrayList siteAdressCollection;
        public ArrayList SiteAdressCollection
        {
            get { return siteAdressCollection; }
            set { siteAdressCollection = value; }
        }
        #endregion

        public void Save(XmlDocument doc, XmlNode node)
        {
            try
            {
                XmlNode nodeFreeDb = doc.CreateElement("freedb");
                node.AppendChild(nodeFreeDb);

                XmlNode nodeEnabled = doc.CreateElement("enabled");
                nodeEnabled.InnerText = enabled.ToString();
                nodeFreeDb.AppendChild(nodeEnabled);

                XmlNode nodeDefaultSite = doc.CreateElement("defaultSite");
                nodeDefaultSite.InnerText = defaultSite;
                nodeFreeDb.AppendChild(nodeDefaultSite);

                XmlNode nodeUserNamer = doc.CreateElement("userName");
                nodeUserNamer.InnerText = userName;
                nodeFreeDb.AppendChild(nodeUserNamer);

                XmlNode nodeHostName = doc.CreateElement("hostName");
                nodeHostName.InnerText = hostName;
                nodeFreeDb.AppendChild(nodeHostName);

                XmlNode nodeClientName = doc.CreateElement("clientName");
                nodeClientName.InnerText = clientName;
                nodeFreeDb.AppendChild(nodeClientName);

                XmlNode nodeVersion = doc.CreateElement("version");
                nodeVersion.InnerText = version;
                nodeFreeDb.AppendChild(nodeVersion);

                if (siteAdressCollection != null)
                {
                    if (siteAdressCollection.Count > 0)
                    {
                        XmlNode nodeSites = doc.CreateElement("sites");
                        nodeFreeDb.AppendChild(nodeSites);

                        for (int i = 0; i < siteAdressCollection.Count; i++)
                        {
                            String site = (String)siteAdressCollection[i];

                            XmlNode nodeSite = doc.CreateElement("site");
                            nodeSite.InnerText = site;
                            nodeSites.AppendChild(nodeSite);
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error saving FreeDB settings: ";
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
                        case "enabled":
                            enabled = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "defaultSite":
                            defaultSite = childNode.InnerText;
                            break;
                        case "userName":
                            userName = childNode.InnerText;
                            break;
                        case "hostName":
                            hostName = childNode.InnerText;
                            break;
                        case "clientName":
                            clientName = childNode.InnerText;
                            break;
                        case "version":
                            version = childNode.InnerText;
                            break;
                        case "sites":
                            XmlNode sitesCollection = childNode;
                            for (int i = 0; i < sitesCollection.ChildNodes.Count; i++)
                            {
                                String siteAddress = sitesCollection.ChildNodes[i].InnerText;
                                siteAdressCollection.Add(siteAddress);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error loading FreeDB settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
    }
}
