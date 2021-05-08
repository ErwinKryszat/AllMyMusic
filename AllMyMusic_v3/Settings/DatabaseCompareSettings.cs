using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AllMyMusic.Settings
{
    public class DatabaseCompareSettings
    {
        #region Properties
        private Boolean createBandFolder = false;
        public Boolean CreateBandFolder
        {
            get { return createBandFolder; }
            set { createBandFolder = value; }
        }

        private String leftDB = String.Empty;
        public String LeftDB
        {
            get { return leftDB; }
            set { leftDB = value; }
        }

        private String rightDB = String.Empty;
        public String RightDB
        {
            get { return rightDB; }
            set { rightDB = value; }
        }

        private Boolean overwriteExistingFiles = false;
        public Boolean OverwriteExistingFiles
        {
            get { return overwriteExistingFiles; }
            set { overwriteExistingFiles = value; }
        }

        private Boolean purgeTargetFolder = false;
        public Boolean PurgeTargetFolder
        {
            get { return purgeTargetFolder; }
            set { purgeTargetFolder = value; }
        }
        #endregion

        public void Save(XmlDocument doc, XmlNode node)
        {
            try
            {
                XmlNode nodeDatabaseCompareSettings = doc.CreateElement("databaseCompare");
                node.AppendChild(nodeDatabaseCompareSettings);


                XmlNode nodeLeftDB = doc.CreateElement("leftDB");
                nodeLeftDB.InnerText = LeftDB;
                nodeDatabaseCompareSettings.AppendChild(nodeLeftDB);

                XmlNode nodeRightDB = doc.CreateElement("rightDB");
                nodeLeftDB.InnerText = RightDB;
                nodeDatabaseCompareSettings.AppendChild(nodeLeftDB);

                XmlNode nodeOverwriteExistingFiles = doc.CreateElement("overwriteExistingFiles");
                nodeOverwriteExistingFiles.InnerText = OverwriteExistingFiles.ToString();
                nodeDatabaseCompareSettings.AppendChild(nodeOverwriteExistingFiles);

                XmlNode nodePurgeTargetFolder = doc.CreateElement("purgeTargetFolder");
                nodePurgeTargetFolder.InnerText = PurgeTargetFolder.ToString();
                nodeDatabaseCompareSettings.AppendChild(nodePurgeTargetFolder);

                XmlNode nodeCreateBandFolder = doc.CreateElement("createBandFolder");
                nodeCreateBandFolder.InnerText = CreateBandFolder.ToString();
                nodeDatabaseCompareSettings.AppendChild(nodeCreateBandFolder);
               
            }
            catch (Exception Err)
            {
                String errorMessage = "Error saving Database Compare settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
                throw new SettingsException(errorMessage, Err.ToString(), Err.InnerException);
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
                        case "leftDB":
                            LeftDB = childNode.InnerText;
                            break;

                        case "rightDB":
                            RightDB = childNode.InnerText;
                            break;

                        case "overwriteExistingFiles":
                            OverwriteExistingFiles = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "purgeTargetFolder":
                            PurgeTargetFolder = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "createBandFolder":
                            CreateBandFolder = Convert.ToBoolean(childNode.InnerText);
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error loading Database Compare settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
                throw new SettingsException(errorMessage, Err.ToString(), Err.InnerException);
            }
        }
    }
}
