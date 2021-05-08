using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml;

using System.Globalization;
using AllMyMusic;

namespace AllMyMusic.Settings
{
    public class FormSettings
    {
        #region Properties
        //public List<ViewParameter> viewParameterList;
        //public List<ViewParameter> ViewParameterList
        //{
        //    get { return viewParameterList; }
        //    set { viewParameterList = value; }
        //}

        #endregion

        public FormSettings()
        {
            //viewParameterList = new List<ViewParameter>();
        }

        #region Main Form
        private Size frmMain_Size;
        public Size FrmMain_Size
        {
            get { return frmMain_Size; }
            set { frmMain_Size = value; }
        }
        private Point frmMain_Position;
        public Point FrmMain_Position
        {
            get { return frmMain_Position; }
            set { frmMain_Position = value; }
        }
        private double frmMain_AlbumListViewWidth = 400;
        public double FrmMain_AlbumListViewWidth
        {
            get { return frmMain_AlbumListViewWidth; }
            set { frmMain_AlbumListViewWidth = value; }
        }
        private double frmMain_AlbumAndAlbumlistViewWidth = 1000;
        public double FrmMain_AlbumAndAlbumListViewWidth
        {
            get { return frmMain_AlbumAndAlbumlistViewWidth; }
            set { frmMain_AlbumAndAlbumlistViewWidth = value; }
        }
        private Int32 frmMain_RibbonTab = 0;
        public Int32 FrmMain_RibbonTab
        {
            get { return frmMain_RibbonTab; }
            set { frmMain_RibbonTab = value; }
        }

        private Boolean frmMain_ViewVaBands = true;
        public Boolean FrmMain_ViewVaBands
        {
            get { return frmMain_ViewVaBands; }
            set { frmMain_ViewVaBands = value; }
        }

        private Boolean frmMain_AlbumListViewExpanded = true;
        public Boolean FrmMain_AlbumListViewExpanded
        {
            get { return frmMain_AlbumListViewExpanded; }
            set { frmMain_AlbumListViewExpanded = value; }
        }
        #endregion

        #region FrmFolderSelect
        private Size frmFolderSelect_Size;
        public Size FrmFolderSelect_Size
        {
            get { return frmFolderSelect_Size; }
            set { frmFolderSelect_Size = value; }
        }

        private Point frmFolderSelect_Position;
        public Point FrmFolderSelect_Position
        {
            get { return frmFolderSelect_Position; }
            set { frmFolderSelect_Position = value; }
        }

        private List<String> expandedDrives;
        public List<String> frmFolderSelect_ExpandedDrives
        {
            get { return expandedDrives; }
            set { expandedDrives = value; }
        }

        private List<String> expandedFolders;
        public List<String> frmFolderSelect_ExpandedFolders
        {
            get { return expandedFolders; }
            set { expandedFolders = value; }
        }
        #endregion

        #region FrmTools
        private Size frmTools_Size;
        public Size FrmTools_Size
        {
            get { return frmTools_Size; }
            set { frmTools_Size = value; }
        }
        private Point frmTools_Position;
        public Point FrmTools_Position
        {
            get { return frmTools_Position; }
            set { frmTools_Position = value; }
        }

        private double frmTools_PropertiesViewWidth = 400;
        public double FrmTools_PropertiesViewWidth
        {
            get { return frmTools_PropertiesViewWidth; }
            set { frmTools_PropertiesViewWidth = value; }
        }
        private double frmTools_SongTabelViewWidth = 400;
        public double FrmTools_SongTabelViewWidth
        {
            get { return frmTools_SongTabelViewWidth; }
            set { frmTools_SongTabelViewWidth = value; }
        }
        private Boolean frmTools_ColumnSelectorVisible = true;
        public Boolean FrmTools_ColumnSelectorVisible
        {
            get { return frmTools_ColumnSelectorVisible; }
            set { frmTools_ColumnSelectorVisible = value; }
        }

        private Boolean frmTools_TooltipsEnabled = true;
        public Boolean FrmTools_TooltipsEnabled
        {
            get { return frmTools_TooltipsEnabled; }
            set { frmTools_TooltipsEnabled = value; }
        }
        private String frmTools_AutoTagSelectedPattern;
        public String FrmTools_AutoTagSelectedPattern
        {
            get { return frmTools_AutoTagSelectedPattern; }
            set { frmTools_AutoTagSelectedPattern = value; }
        }

        private String frmTools_RenameSelectedPattern = @"<band>\<year> <album>\<track> - <song>";
        public String FrmTools_RenameSelectedPattern
        {
            get { return frmTools_RenameSelectedPattern; }
            set { frmTools_RenameSelectedPattern = value; }
        }
        #endregion

        #region FrmManageCoverImages
        private Size frmManageCoverImages_Size;
        public Size FrmManageCoverImages_Size
        {
            get { return frmManageCoverImages_Size; }
            set { frmManageCoverImages_Size = value; }
        }
        private Point frmManageCoverImages_Position;
        public Point FrmManageCoverImages_Position
        {
            get { return frmManageCoverImages_Position; }
            set { frmManageCoverImages_Position = value; }
        }
        private String frmManageCoverImages_SelectedPattern;
        public String FrmManageCoverImages_SelectedPattern
        {
            get { return frmManageCoverImages_SelectedPattern; }
            set { frmManageCoverImages_SelectedPattern = value; }
        }
        #endregion

        #region FrmMessage
        private Size frmMessage_Size;
        public Size FrmMessage_Size
        {
            get { return frmMessage_Size; }
            set { frmMessage_Size = value; }
        }
        private Point frmMessage_Position;
        public Point FrmMessage_Position
        {
            get { return frmMessage_Position; }
            set { frmMessage_Position = value; }
        }
        #endregion

        #region FrmPartyButtonDesigner
        private Size frmPartyButtonDesigner_Size;
        public Size FrmPartyButtonDesigner_Size
        {
            get { return frmPartyButtonDesigner_Size; }
            set { frmPartyButtonDesigner_Size = value; }
        }
        private Point frmPartyButtonDesigner_Position;
        public Point FrmPartyButtonDesigner_Position
        {
            get { return frmPartyButtonDesigner_Position; }
            set { frmPartyButtonDesigner_Position = value; }
        }
        private double frmPartyButtonDesigner_LeftAlbumsWidth = 400;
        public double FrmPartyButtonDesigner_LeftAlbumsWidth
        {
            get { return frmPartyButtonDesigner_LeftAlbumsWidth; }
            set { frmPartyButtonDesigner_LeftAlbumsWidth = value; }
        }
        private double frmPartyButtonDesigner_LeftSongsWidth = 400;
        public double FrmPartyButtonDesigner_LeftSongsWidth
        {
            get { return frmPartyButtonDesigner_LeftSongsWidth; }
            set { frmPartyButtonDesigner_LeftSongsWidth = value; }
        }
        #endregion

        #region FrmConnectServer
        private Size frmConnectServer_Size = new Size(600,600);
        public Size FrmConnectServer_Size
        {
            get { return frmConnectServer_Size; }
            set { frmConnectServer_Size = value; }
        }
        private Point frmConnectServer_Position;
        public Point FrmConnectServer_Position
        {
            get { return frmConnectServer_Position; }
            set { frmConnectServer_Position = value; }
        }
        #endregion

        #region Another Form

        #endregion

        private String ConvertSizeToString(Size size)
        { 
            Size intSize = new Size(Convert.ToInt32(size.Width),Convert.ToInt32(size.Height));
            return intSize.Width.ToString() + "," + intSize.Height.ToString();
        }

        private String ConvertPositionToString(Point point)
        {
            if (Double.IsNaN(point.X) != true)
            {
                 Point intPoint = new Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y));
                return intPoint.X.ToString() + "," + intPoint.Y.ToString();
            }
            else
            {
                return "0,0";
            }
             //Point intPoint = new Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y));
             //   return intPoint.X.ToString() + "," + intPoint.Y.ToString();

            //if (point.X != Int32.)
            //{
               
            //}
            //else
            //{
            //    return "0,0";
            //}
        }

        public void Save(XmlDocument doc, XmlNode node)
        {
            XmlNode nodeFormSettings = doc.CreateElement("formSettings");
            node.AppendChild(nodeFormSettings);

            Save_frmMain(doc, nodeFormSettings);
            Save_frmFolderSelect(doc, nodeFormSettings);
            Save_frmTools(doc, nodeFormSettings);
            Save_frmManageCoverImages(doc, nodeFormSettings);
            Save_frmMessage(doc, nodeFormSettings);
            Save_frmPartyButtonDesigner(doc, nodeFormSettings);
            Save_frmConnectServer(doc, nodeFormSettings);
         
        }
        public void Save_frmMain(XmlDocument doc, XmlNode nodeFormSettings)
        {
            try
            {
                // Main Form
                XmlNode nodefrmMain = doc.CreateElement("frmMain");
                nodeFormSettings.AppendChild(nodefrmMain);

                XmlNode nodefrmMain_Size = doc.CreateElement("size");
                nodefrmMain_Size.InnerText = ConvertSizeToString(frmMain_Size);
                nodefrmMain.AppendChild(nodefrmMain_Size);

                XmlNode nodefrmMain_Position = doc.CreateElement("position");
                nodefrmMain_Position.InnerText = ConvertPositionToString(frmMain_Position);
                nodefrmMain.AppendChild(nodefrmMain_Position);

                XmlNode nodefrmMain_AlbumListViewWidth = doc.CreateElement("frmMain_AlbumListViewWidth");
                nodefrmMain_AlbumListViewWidth.InnerText = frmMain_AlbumListViewWidth.ToString();
                nodefrmMain.AppendChild(nodefrmMain_AlbumListViewWidth);

                XmlNode nodefrmMain_AlbumAndAlbumlistViewWidth = doc.CreateElement("frmMain_AlbumAndAlbumlistViewWidth");
                nodefrmMain_AlbumAndAlbumlistViewWidth.InnerText = frmMain_AlbumAndAlbumlistViewWidth.ToString();
                nodefrmMain.AppendChild(nodefrmMain_AlbumAndAlbumlistViewWidth);

                XmlNode nodefrmMain_RibbonTab = doc.CreateElement("frmMain_RibbonTab");
                nodefrmMain_RibbonTab.InnerText = frmMain_RibbonTab.ToString();
                nodefrmMain.AppendChild(nodefrmMain_RibbonTab);

                XmlNode nodefrmMain_viewVaBands = doc.CreateElement("frmMain_viewVaBands");
                nodefrmMain_viewVaBands.InnerText = frmMain_ViewVaBands.ToString();
                nodefrmMain.AppendChild(nodefrmMain_viewVaBands);

                XmlNode nodefrmMain_AlbumListViewExpanded = doc.CreateElement("frmMain_AlbumListViewExpanded");
                nodefrmMain_AlbumListViewExpanded.InnerText = frmMain_AlbumListViewExpanded.ToString();
                nodefrmMain.AppendChild(nodefrmMain_AlbumListViewExpanded);

                
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Save_frmFolderSelect(XmlDocument doc, XmlNode nodeFormSettings)
        {
            try
            {
                XmlNode nodefrmFolderSelect = doc.CreateElement("frmFolderSelect");
                nodeFormSettings.AppendChild(nodefrmFolderSelect);

                XmlNode nodefrmFolderSelect_Size = doc.CreateElement("size");
                nodefrmFolderSelect_Size.InnerText = ConvertSizeToString(frmFolderSelect_Size);
                nodefrmFolderSelect.AppendChild(nodefrmFolderSelect_Size);

                XmlNode nodefrmFolderSelect_Position = doc.CreateElement("position");
                nodefrmFolderSelect_Position.InnerText = ConvertPositionToString(frmMain_Position);
                nodefrmFolderSelect.AppendChild(nodefrmFolderSelect_Position);


                if (expandedDrives != null)
                {
                    XmlNode nodeExpandedDrives = doc.CreateElement("expandedDrives");
                    nodefrmFolderSelect.AppendChild(nodeExpandedDrives);

                    for (int i = 0; i < expandedDrives.Count; i++)
                    {
                        XmlNode nodeExpandedDrive = doc.CreateElement("expandedDrive");

                        XmlAttribute attributeName = doc.CreateAttribute("driveName");
                        attributeName.InnerText = expandedDrives[i];
                        nodeExpandedDrive.Attributes.SetNamedItem(attributeName);

                        nodeExpandedDrives.AppendChild(nodeExpandedDrive);
                    }
                }

                if (expandedFolders != null)
                {
                    XmlNode nodeExpandedFolders = doc.CreateElement("expandedFolders");
                    nodefrmFolderSelect.AppendChild(nodeExpandedFolders);

                    for (int i = 0; i < expandedFolders.Count; i++)
                    {
                        XmlNode nodeExpandedFolder = doc.CreateElement("expandedFolder");

                        XmlAttribute attributeName = doc.CreateAttribute("folderPath");
                        attributeName.InnerText = expandedFolders[i];
                        nodeExpandedFolder.Attributes.SetNamedItem(attributeName);

                        nodeExpandedFolders.AppendChild(nodeExpandedFolder);
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Save_frmTools(XmlDocument doc, XmlNode nodeFormSettings)
        {
            try
            {
                XmlNode nodefrmTools = doc.CreateElement("frmTools");
                nodeFormSettings.AppendChild(nodefrmTools);

                XmlNode nodefrmTools_Size = doc.CreateElement("size");
                nodefrmTools_Size.InnerText = ConvertSizeToString(frmTools_Size);
                nodefrmTools.AppendChild(nodefrmTools_Size);

                XmlNode nodefrmTools_Position = doc.CreateElement("position");
                nodefrmTools_Position.InnerText = ConvertPositionToString(frmTools_Position);
                nodefrmTools.AppendChild(nodefrmTools_Position);

                XmlNode nodefrmTools_PropertiesViewWidth = doc.CreateElement("propertiesViewWidth");
                nodefrmTools_PropertiesViewWidth.InnerText = frmTools_PropertiesViewWidth.ToString();
                nodefrmTools.AppendChild(nodefrmTools_PropertiesViewWidth);

                XmlNode nodefrmTools_ColumnSelectorVisible = doc.CreateElement("columnSelectorVisible");
                nodefrmTools_ColumnSelectorVisible.InnerText = frmTools_ColumnSelectorVisible.ToString();
                nodefrmTools.AppendChild(nodefrmTools_ColumnSelectorVisible);

                XmlNode nodefrmTools_TooltipsEnabled = doc.CreateElement("tooltipsEnabled");
                nodefrmTools_TooltipsEnabled.InnerText = frmTools_TooltipsEnabled.ToString();
                nodefrmTools.AppendChild(nodefrmTools_TooltipsEnabled);

                XmlNode nodefrmTools_RenameSelectedPattern = doc.CreateElement("renameSelectedPattern");
                nodefrmTools_RenameSelectedPattern.InnerText = frmTools_RenameSelectedPattern;
                nodefrmTools.AppendChild(nodefrmTools_RenameSelectedPattern);

                XmlNode nodefrmTools_AutoTagSelectedPattern = doc.CreateElement("autoTagSelectedPattern");
                nodefrmTools_AutoTagSelectedPattern.InnerText = frmTools_AutoTagSelectedPattern;
                nodefrmTools.AppendChild(nodefrmTools_AutoTagSelectedPattern);
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Save_frmManageCoverImages(XmlDocument doc, XmlNode nodeFormSettings)
        {
            try
            {
                XmlNode nodefrmManageCoverImages = doc.CreateElement("frmManageCoverImages");
                nodeFormSettings.AppendChild(nodefrmManageCoverImages);

                XmlNode nodefrmManageCoverImages_Size = doc.CreateElement("size");
                nodefrmManageCoverImages_Size.InnerText = ConvertSizeToString(frmManageCoverImages_Size);
                nodefrmManageCoverImages.AppendChild(nodefrmManageCoverImages_Size);

                XmlNode nodefrmManageCoverImages_Position = doc.CreateElement("position");
                nodefrmManageCoverImages_Position.InnerText = ConvertPositionToString(frmManageCoverImages_Position);
                nodefrmManageCoverImages.AppendChild(nodefrmManageCoverImages_Position);

                XmlNode nodefrmManageCoverImages_SelectedPattern = doc.CreateElement("selectedPattern");
                nodefrmManageCoverImages_SelectedPattern.InnerText = frmManageCoverImages_SelectedPattern;
                nodefrmManageCoverImages.AppendChild(nodefrmManageCoverImages_SelectedPattern);
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Save_frmMessage(XmlDocument doc, XmlNode nodeFormSettings)
        {
            try
            {
                XmlNode nodefrmMessage = doc.CreateElement("frmMessage");
                nodeFormSettings.AppendChild(nodefrmMessage);

                XmlNode nodefrmMessage_Size = doc.CreateElement("size");
                nodefrmMessage_Size.InnerText = ConvertSizeToString(frmMessage_Size);
                nodefrmMessage.AppendChild(nodefrmMessage_Size);

                XmlNode nodefrmMessage_Position = doc.CreateElement("position");
                nodefrmMessage_Position.InnerText = ConvertPositionToString(frmMessage_Position);
                nodefrmMessage.AppendChild(nodefrmMessage_Position);
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Save_frmPartyButtonDesigner(XmlDocument doc, XmlNode nodeFormSettings)
        {
            try
            {
                XmlNode nodefrmPartyButtonDesigner = doc.CreateElement("frmPartyButtonDesigner");
                nodeFormSettings.AppendChild(nodefrmPartyButtonDesigner);

                XmlNode nodefrmPartyButtonDesigner_Size = doc.CreateElement("size");
                nodefrmPartyButtonDesigner_Size.InnerText = ConvertSizeToString(frmPartyButtonDesigner_Size);
                nodefrmPartyButtonDesigner.AppendChild(nodefrmPartyButtonDesigner_Size);

                XmlNode nodefrmPartyButtonDesigner_Position = doc.CreateElement("position");
                nodefrmPartyButtonDesigner_Position.InnerText = ConvertPositionToString(frmPartyButtonDesigner_Position);
                nodefrmPartyButtonDesigner.AppendChild(nodefrmPartyButtonDesigner_Position);

                XmlNode nodefrmPartyButtonDesigner_LeftAlbumsWidth = doc.CreateElement("leftAlbums_width");
                nodefrmPartyButtonDesigner_LeftAlbumsWidth.InnerText = frmPartyButtonDesigner_LeftAlbumsWidth.ToString();
                nodefrmPartyButtonDesigner.AppendChild(nodefrmPartyButtonDesigner_LeftAlbumsWidth);

                XmlNode nodefrmPartyButtonDesigner_LeftSongsWidth = doc.CreateElement("leftSongs_width");
                nodefrmPartyButtonDesigner_LeftSongsWidth.InnerText = frmPartyButtonDesigner_LeftSongsWidth.ToString();
                nodefrmPartyButtonDesigner.AppendChild(nodefrmPartyButtonDesigner_LeftSongsWidth);
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Save_frmConnectServer(XmlDocument doc, XmlNode nodeFormSettings)
        {
            try
            {
                XmlNode nodefrmTools = doc.CreateElement("frmColumnsSelector");
                nodeFormSettings.AppendChild(nodefrmTools);

                XmlNode nodefrmTools_Size = doc.CreateElement("size");
                nodefrmTools_Size.InnerText = ConvertSizeToString(frmConnectServer_Size);
                nodefrmTools.AppendChild(nodefrmTools_Size);

                XmlNode nodefrmTools_Position = doc.CreateElement("position");
                nodefrmTools_Position.InnerText = ConvertPositionToString(frmConnectServer_Position);
                nodefrmTools.AppendChild(nodefrmTools_Position);
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

       
        public void Load(XmlNode node)
        {
            try
            {
                //viewParameterList = new List<ViewParameter>();
                
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "frmMain":
                            Load_frmMain(childNode);
                            break;

                        case "frmFolderSelect":
                            Load_frmFolderSelect(childNode);
                            break;

                        case "frmTools":
                            Load_frmTools(childNode);
                            break;

                        case "frmMessage":
                            Load_frmMessage(childNode);
                            break;

                        case "frmPartyButtonDesigner":
                            Load_frmPartyButtonDesigner(childNode);
                            break;

                        case  "frmColumnsSelector":
                            Load_frmConnectServer(childNode);
                            break;

                        case "frmManageCoverImages":
                            Load_frmManageCoverImages(childNode);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load_frmMain(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "size":
                            frmMain_Size = Size.Parse(childNode.InnerText);
                            break;
                        case "position":
                            frmMain_Position = Point.Parse(childNode.InnerText);
                            break;
                        case "viewParameters":
                            //GetViewParameterList(childNode);
                            break;

                        case "frmMain_AlbumListViewWidth":
                            frmMain_AlbumListViewWidth = Convert.ToDouble(childNode.InnerText);
                            break;

                        case "frmMain_AlbumAndAlbumlistViewWidth":
                            frmMain_AlbumAndAlbumlistViewWidth = Convert.ToDouble(childNode.InnerText);
                            break;

                        case "frmMain_RibbonTab":
                            frmMain_RibbonTab = Convert.ToInt32(childNode.InnerText);
                            break;

                        case "frmMain_viewVaBands":
                            frmMain_ViewVaBands = Convert.ToBoolean(childNode.InnerText);
                            break;

                        case "frmMain_AlbumListViewExpanded":
                            frmMain_AlbumListViewExpanded = Convert.ToBoolean(childNode.InnerText);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load_frmFolderSelect(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "size":
                            frmFolderSelect_Size = Size.Parse(childNode.InnerText);
                            break;
                        case "position":
                            frmFolderSelect_Position = Point.Parse(childNode.InnerText);
                            break;

                        case "expandedDrives":
                            GetExpandedDrives(childNode);
                            break;

                        case "expandedFolders":
                            GetExpandedFolders(childNode);
                            break;

                            
                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load_frmTools(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "size":
                            frmTools_Size = Size.Parse(childNode.InnerText);
                            break;
                        case "position":
                            frmTools_Position = Point.Parse(childNode.InnerText);
                            break;
                        case "propertiesViewWidth":
                            frmTools_PropertiesViewWidth = Double.Parse(childNode.InnerText);
                            break;
                        case "songTabelViewWidth":
                            frmTools_SongTabelViewWidth = Double.Parse(childNode.InnerText);
                            break;
                        case "columnSelectorVisible":
                            frmTools_ColumnSelectorVisible = Boolean.Parse(childNode.InnerText);
                            break;
                        case "tooltipsEnabled":
                            frmTools_TooltipsEnabled = Boolean.Parse(childNode.InnerText);
                            break;
                        case "renameSelectedPattern":
                            frmTools_RenameSelectedPattern = childNode.InnerText;
                            break;
                        case "autoTagSelectedPattern":
                            frmTools_AutoTagSelectedPattern = childNode.InnerText;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load_frmManageCoverImages(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "size":
                            frmManageCoverImages_Size = Size.Parse(childNode.InnerText);
                            break;
                        case "position":
                            frmManageCoverImages_Position = Point.Parse(childNode.InnerText);
                            break;
                        case "selectedPattern":
                            frmManageCoverImages_SelectedPattern = childNode.InnerText;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load_frmMessage(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "size":
                            frmMessage_Size = Size.Parse(childNode.InnerText);
                            break;
                        case "position":
                            frmMessage_Position = Point.Parse(childNode.InnerText);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load_frmPartyButtonDesigner(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "size":
                            frmPartyButtonDesigner_Size = Size.Parse(childNode.InnerText);
                            break;
                        case "position":
                            frmPartyButtonDesigner_Position = Point.Parse(childNode.InnerText);
                            break;
                        case "leftAlbums_width":
                            frmPartyButtonDesigner_LeftAlbumsWidth = Int32.Parse(childNode.InnerText);
                            break;
                        case "leftSongs_width":
                            frmPartyButtonDesigner_LeftSongsWidth = Int32.Parse(childNode.InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load_frmConnectServer(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "size":
                            frmConnectServer_Size = Size.Parse(childNode.InnerText);
                            break;
                        case "position":
                            frmConnectServer_Position = Point.Parse(childNode.InnerText);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        private void GetExpandedDrives(XmlNode node)
        {
            expandedDrives = new List<String>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "expandedDrive")
                {
                    foreach (XmlAttribute attr in childNode.Attributes)
                    {
                        switch (attr.Name)
                        {
                            case "driveName":
                                expandedDrives.Add(attr.InnerText);
                                break;
                        }
                    }
                }
            }
        }
        private void GetExpandedFolders(XmlNode node)
        {
            expandedFolders = new List<String>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "expandedFolder")
                {
                    foreach (XmlAttribute attr in childNode.Attributes)
                    {
                        switch (attr.Name)
                        {
                            case "folderPath":
                                expandedFolders.Add(attr.InnerText);
                                break;
                        }
                    }
                }
            }
        }
    }
}
