using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AllMyMusic_v3.Settings
{
    public class CoverImageSettings
    {
        #region Properties
        private Boolean createStamps = true;
        public Boolean CreateStamps
        {
            get { return createStamps; }
            set { createStamps = value; }
        }

        private String imageFilename = "Folder.jpg";
        public String ImageFilename
        {
            get { return imageFilename; }
            set { imageFilename = value; }
        }

        private Boolean insertStampImage = false;
        public Boolean InsertStampImage
        {
            get { return insertStampImage; }
            set { insertStampImage = value; }
        }

        private Boolean insertFrontImage = false;
        public Boolean InsertFrontImage
        {
            get { return insertFrontImage; }
            set { insertFrontImage = value; }
        }

        private Boolean noActionAPIC = false;
        public Boolean NoActionAPIC
        {
            get { return noActionAPIC; }
            set { noActionAPIC = value; }
        }

        private Boolean removeImages = false;
        public Boolean RemoveImages
        {
            get { return removeImages; }
            set { removeImages = value; }
        }

        private Boolean renameImages = true;
        public Boolean RenameImages
        {
            get { return renameImages; }
            set { renameImages = value; }
        }

        private Boolean saveToDisk = false;
        public Boolean SaveToDisk
        {
            get { return saveToDisk; }
            set { saveToDisk = value; }
        }

        private Int32 stampResolution = 200;
        public Int32 StampResolution
        {
            get { return stampResolution; }
            set { stampResolution = value; }
        }

        private Int32 minimalFileSize = 10000;
        public Int32 MinimalFileSize
        {
            get { return minimalFileSize; }
            set { minimalFileSize = value; }
        }
        #endregion

        public void Save(XmlDocument doc, XmlNode node)
        {
            try
            {
                XmlNode nodeCoverImages = doc.CreateElement("coverImages");
                node.AppendChild(nodeCoverImages);

                XmlNode nodeCreateStamps = doc.CreateElement("createStamps");
                nodeCreateStamps.InnerText = createStamps.ToString();
                nodeCoverImages.AppendChild(nodeCreateStamps);

                XmlNode nodeInsertStampImage = doc.CreateElement("insertStampImage");
                nodeInsertStampImage.InnerText = insertStampImage.ToString();
                nodeCoverImages.AppendChild(nodeInsertStampImage);

                XmlNode nodeInsertFrontImage = doc.CreateElement("insertFrontImage");
                nodeInsertFrontImage.InnerText = insertFrontImage.ToString();
                nodeCoverImages.AppendChild(nodeInsertFrontImage);

                XmlNode nodeNoActionAPIC = doc.CreateElement("noActionAPIC");
                nodeNoActionAPIC.InnerText = noActionAPIC.ToString();
                nodeCoverImages.AppendChild(nodeNoActionAPIC);

                XmlNode nodeRemoveImages = doc.CreateElement("removeImages");
                nodeRemoveImages.InnerText = removeImages.ToString();
                nodeCoverImages.AppendChild(nodeRemoveImages);

                XmlNode nodeRenameImages = doc.CreateElement("renameImages");
                nodeRenameImages.InnerText = renameImages.ToString();
                nodeCoverImages.AppendChild(nodeRenameImages);

                XmlNode nodeSaveToDisk = doc.CreateElement("saveToDisk");
                nodeSaveToDisk.InnerText = saveToDisk.ToString();
                nodeCoverImages.AppendChild(nodeSaveToDisk);

                XmlNode nodeStampSize = doc.CreateElement("stampSize");
                nodeStampSize.InnerText = stampResolution.ToString();
                nodeCoverImages.AppendChild(nodeStampSize);

                XmlNode nodeImageFilename = doc.CreateElement("imageFilename");
                nodeImageFilename.InnerText = imageFilename.ToString();
                nodeCoverImages.AppendChild(nodeImageFilename);

                XmlNode nodeMinimalFileSize = doc.CreateElement("minimalFileSize");
                nodeMinimalFileSize.InnerText = minimalFileSize.ToString();
                nodeCoverImages.AppendChild(nodeMinimalFileSize);
            }
            catch (Exception Err)
            {
                String errorMessage = "Error saving Cover Image settings.";
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
                        case "createStamps":
                            createStamps = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "imageFilename":
                            imageFilename = childNode.InnerText;
                            break;
                        case "insertStampImage":
                            insertStampImage = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "insertFrontImage":
                            insertFrontImage = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "noActionAPIC":
                            noActionAPIC = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "removeImages":
                            removeImages = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "renameImages":
                            renameImages = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "saveToDisk":
                            saveToDisk = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "stampSize":
                            stampResolution = Convert.ToInt32(childNode.InnerText);
                            break;
                        case "minimalFileSize":
                            minimalFileSize = Convert.ToInt32(childNode.InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error loading Cover Image settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
    }
}
