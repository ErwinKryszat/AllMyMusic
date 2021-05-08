using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace AllMyMusic.Settings
{
    public static class AppSettings
    {
        #region Variables
        private static XmlDocument _doc;
        #endregion
        
        #region Properties
        public static String _fileName;
        public static AudioSettings AudioSettings = new AudioSettings();
        public static CoverImageSettings CoverImageSettings = new CoverImageSettings();
        public static DatabaseSettings DatabaseSettings = new DatabaseSettings();
        public static DatabaseCompareSettings DatabaseCompareSettings = new DatabaseCompareSettings();
        public static ColumnConfigCollection SongTableConfigPropertiesTool = new ColumnConfigCollection("propertiesTool");
        public static ColumnConfigCollection SongTableConfigRenameTool = new ColumnConfigCollection("renameTool");
        public static ColumnConfigCollection SongTableConfigAutotagTool = new ColumnConfigCollection("autotagTool");
        public static FormSettings FormSettings = new FormSettings();
        public static FreeDbSettings FreeDBSettings = new FreeDbSettings();
        public static GeneralSettings GeneralSettings = new GeneralSettings();
        #endregion

        #region Methods
        public static Boolean Load(String fileName)
        {
            _fileName = fileName;

            try
            {
                _doc = new XmlDocument();
                if (File.Exists(fileName) == false)
                {
                    return false;
                }
                _doc.Load(fileName);
                XmlElement nodeAppSettings = _doc.DocumentElement;

                foreach (XmlNode node in nodeAppSettings.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "audio":
                            AudioSettings.Load(node);
                            break;

                        case "coverImages":
                            CoverImageSettings.Load(node);
                            break;

                        case "databases":
                            DatabaseSettings.Load(node);
                            //DatabaseSettingsOld.Load(node);
                            break;

                        case "databaseCompare":
                            DatabaseCompareSettings.Load(node);
                            break;

                        case "formSettings":
                            FormSettings.Load(node);
                            break;

                        case "freedb":
                            FreeDBSettings.Load(node);
                            break;

                        case "general":
                            GeneralSettings.Load(node);
                            break;

                        case "songTable":
                            LoadSongTableCfg(node);
                            break;
                        default:
                            break;
                    }
                }
                return true;
            }
            catch (IOException Err)
            {
                String errorMessage = "Error accessing file: " + fileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
                throw new SettingsException(errorMessage, Err.ToString(), Err.InnerException);
            }
        }
        public static void Save()
        {
            try
            {
                _doc = new XmlDocument();

                XmlDeclaration dec = _doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                _doc.AppendChild(dec);
                XmlElement nodeAppSettings = _doc.CreateElement("appSettings");
                _doc.AppendChild(nodeAppSettings);

                GeneralSettings.Save(_doc, nodeAppSettings);
                FormSettings.Save(_doc, nodeAppSettings);
                DatabaseSettings.Save(_doc, nodeAppSettings);
                DatabaseCompareSettings.Save(_doc, nodeAppSettings);
                AudioSettings.Save(_doc, nodeAppSettings);
                FreeDBSettings.Save(_doc, nodeAppSettings);
                CoverImageSettings.Save(_doc, nodeAppSettings);
                SaveSongTableCfg(nodeAppSettings);

                _doc.Save(_fileName);
            }
            catch (IOException Err)
            {
                String errorMessage = "Error accessing file: " + _fileName;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
                throw new SettingsException(errorMessage, Err.ToString(), Err.InnerException);
            }
        }
      
        private static void SaveSongTableCfg(XmlNode _nodeRoot)
        {
            try
            {
                XmlNode nodeSongTable = _doc.CreateElement("songTable");
                _nodeRoot.AppendChild(nodeSongTable);

                XmlNode nodeConfigProperties = _doc.CreateElement("configPropertiesTool");
                nodeSongTable.AppendChild(nodeConfigProperties);

                XmlNode nodeConfigRename = _doc.CreateElement("configRenameTool");
                nodeSongTable.AppendChild(nodeConfigRename);

                XmlNode nodeConfigAutotag = _doc.CreateElement("configAutotagTool");
                nodeSongTable.AppendChild(nodeConfigAutotag);

                for (int i = 0; i < SongTableConfigPropertiesTool.Count; i++)
                {
                    dgColumnConfig columnCfg = SongTableConfigPropertiesTool[i];
                    XmlNode nodeColumn = _doc.CreateElement("column");

                    XmlAttribute attributeName = _doc.CreateAttribute("name");
                    attributeName.InnerText = columnCfg.ColumnName;
                    nodeColumn.Attributes.SetNamedItem(attributeName);

                    XmlAttribute attributeWidth = _doc.CreateAttribute("width");
                    attributeWidth.InnerText = columnCfg.Width.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeWidth);

                    XmlAttribute attributeVisible = _doc.CreateAttribute("visible");
                    attributeVisible.InnerText = columnCfg.Visible.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeVisible);

                    XmlAttribute attributeDispIndex = _doc.CreateAttribute("displayIndex");
                    attributeDispIndex.InnerText = columnCfg.DisplayIndex.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeDispIndex);

                    nodeConfigProperties.AppendChild(nodeColumn);
                }

                for (int i = 0; i < SongTableConfigRenameTool.Count; i++)
                {
                    dgColumnConfig columnCfg = SongTableConfigRenameTool[i];
                    XmlNode nodeColumn = _doc.CreateElement("column");

                    XmlAttribute attributeName = _doc.CreateAttribute("name");
                    attributeName.InnerText = columnCfg.ColumnName;
                    nodeColumn.Attributes.SetNamedItem(attributeName);

                    XmlAttribute attributeWidth = _doc.CreateAttribute("width");
                    attributeWidth.InnerText = columnCfg.Width.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeWidth);

                    XmlAttribute attributeVisible = _doc.CreateAttribute("visible");
                    attributeVisible.InnerText = columnCfg.Visible.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeVisible);

                    XmlAttribute attributeDispIndex = _doc.CreateAttribute("displayIndex");
                    attributeDispIndex.InnerText = columnCfg.DisplayIndex.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeDispIndex);

                    nodeConfigRename.AppendChild(nodeColumn);
                }

                for (int i = 0; i < SongTableConfigAutotagTool.Count; i++)
                {
                    dgColumnConfig columnCfg = SongTableConfigAutotagTool[i];
                    XmlNode nodeColumn = _doc.CreateElement("column");

                    XmlAttribute attributeName = _doc.CreateAttribute("name");
                    attributeName.InnerText = columnCfg.ColumnName;
                    nodeColumn.Attributes.SetNamedItem(attributeName);

                    XmlAttribute attributeWidth = _doc.CreateAttribute("width");
                    attributeWidth.InnerText = columnCfg.Width.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeWidth);

                    XmlAttribute attributeVisible = _doc.CreateAttribute("visible");
                    attributeVisible.InnerText = columnCfg.Visible.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeVisible);

                    XmlAttribute attributeDispIndex = _doc.CreateAttribute("displayIndex");
                    attributeDispIndex.InnerText = columnCfg.DisplayIndex.ToString();
                    nodeColumn.Attributes.SetNamedItem(attributeDispIndex);

                    nodeConfigAutotag.AppendChild(nodeColumn);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error saving song table configuration.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
                throw new SettingsException(errorMessage, Err.ToString(), Err.InnerException);
            }
        }
        private static void LoadSongTableCfg(XmlNode _nodeSongTable)
        {
            try
            {
                XmlNode nodeConfigProperties = null;
                XmlNode nodeConfigRename = null;
                XmlNode nodeConfigAutotag = null;

                foreach (XmlNode node in _nodeSongTable.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "configPropertiesTool":
                            nodeConfigProperties = node;
                            break;
                        case "configRenameTool":
                            nodeConfigRename = node;
                            break;
                        case "configAutotagTool":
                            nodeConfigAutotag = node;
                            break;
                        default:
                            break;
                    }
                }


                if (nodeConfigProperties != null)
                {
                    SongTableConfigPropertiesTool.Clear();
                    foreach (XmlNode node in nodeConfigProperties.ChildNodes)
                    {
                        dgColumnConfig columnCfg = new dgColumnConfig();

                        columnCfg.ColumnName = node.Attributes["name"].InnerText;
                        columnCfg.Width = Convert.ToInt32(node.Attributes["width"].InnerText);
                        columnCfg.Visible = Convert.ToBoolean(node.Attributes["visible"].InnerText);
                        columnCfg.DisplayIndex = Convert.ToInt32(node.Attributes["displayIndex"].InnerText);

                        SongTableConfigPropertiesTool.Add(columnCfg);
                    }
                }

                if (nodeConfigRename != null)
                {
                    SongTableConfigRenameTool.Clear();
                    foreach (XmlNode node in nodeConfigRename.ChildNodes)
                    {
                        dgColumnConfig columnCfg = new dgColumnConfig();

                        columnCfg.ColumnName = node.Attributes["name"].InnerText;
                        columnCfg.Width = Convert.ToInt32(node.Attributes["width"].InnerText);
                        columnCfg.Visible = Convert.ToBoolean(node.Attributes["visible"].InnerText);
                        columnCfg.DisplayIndex = Convert.ToInt32(node.Attributes["displayIndex"].InnerText);

                        SongTableConfigRenameTool.Add(columnCfg);
                    }
                }
             
                if (nodeConfigAutotag != null)
                {
                    SongTableConfigAutotagTool.Clear();
                    foreach (XmlNode node in nodeConfigAutotag.ChildNodes)
                    {
                        dgColumnConfig columnCfg = new dgColumnConfig();

                        columnCfg.ColumnName = node.Attributes["name"].InnerText;
                        columnCfg.Width = Convert.ToInt32(node.Attributes["width"].InnerText);
                        columnCfg.Visible = Convert.ToBoolean(node.Attributes["visible"].InnerText);
                        columnCfg.DisplayIndex = Convert.ToInt32(node.Attributes["displayIndex"].InnerText);

                        SongTableConfigAutotagTool.Add(columnCfg);
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error loading song table configuration.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
                throw new SettingsException(errorMessage, Err.ToString(), Err.InnerException);
            }
        }
    #endregion
    }
}
