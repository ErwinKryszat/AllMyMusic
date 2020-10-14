using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace AllMyMusic_v3.Settings
{
    public static class MyXML
    {
        // Date: 31 January 2011
        // Name: Erwin Kryszat
        // code review session

        public static XmlDocument OpenFile(String filename, String rootNode)
        {
            XmlDocument doc;

            if (File.Exists(filename))
            {
                try
                {
                    doc = new XmlDocument();
                    doc.Load(filename);
                }
                catch (FieldAccessException Err)
                {
                    throw new SettingsException(Err.ToString());
                }
            }
            else
            {
                doc = CreateFile(filename, rootNode);
            }
            return doc;
        }

        public static XmlDocument CreateFile(String filename, String rootNode)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(dec);   // Create the root element
                XmlElement root = doc.CreateElement(rootNode);
                doc.AppendChild(root);


                doc.Save(filename);
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }

            return doc;
        }

        public static XmlNode AddInformation(XmlDocument doc, XmlNode parentNode, String newNodeName, String[] attributNames, String[] attributValues)
        {
            XmlNode newNode = null;

            try
            {
                newNode = doc.CreateElement(newNodeName);
                XmlAttribute[] attribute = new XmlAttribute[attributNames.Length];

                // Create the attributes
                for (int i = 0; i < attributNames.Length; i++)
                {
                    // Create a new attribute
                    attribute[i] = doc.CreateAttribute(attributNames[i]);

                    // Set the attribute value
                    attribute[i].InnerText = attributValues[i];

                    // Add the new attributes to the node
                    newNode.Attributes.SetNamedItem(attribute[i]);

                }

                // add the node to the document
                parentNode.AppendChild(newNode);

            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }

            return newNode;
        }

        public static XmlNode UpdateInformation(XmlDocument doc, XmlNode updateNode, String[] attributNames, String[] attributValues)
        {
            // Assign new values to _Node
            try
            {
                for (int i = 0; i < attributNames.Length; i++)
                {
                    Boolean foundAttribute = false;
                    foreach (XmlAttribute item in updateNode.Attributes)
                    {
                        if (item.Name == attributNames[i] )
                        {
                            foundAttribute = true;
                            updateNode.Attributes[attributNames[i]].InnerText = attributValues[i];
                        }
                    }
                    if (foundAttribute == false)
                    {
                        XmlAttribute attr = doc.CreateAttribute(attributNames[i]);
                        attr.InnerText = attributValues[i];
                        updateNode.Attributes.Append(attr);
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }

            return updateNode;
        }

        public static XmlNode FindNode(XmlDocument doc, XmlNode parentNode, String nodeName, String attributName, String attributValue)
        {
            // find a node where the specified attribute has the specified value
            XmlNode desiredNode = null;
            try
            {
                foreach (XmlNode childNode in parentNode.ChildNodes)
                {
                    if (childNode.Name == nodeName)
                    {
                        foreach (XmlAttribute attr in childNode.Attributes)
                        {
                            if (attr.Name == attributName)
                            {
                                if (childNode.Attributes[attributName].InnerText == attributValue)
                                {
                                    desiredNode = childNode;
                                    break;
                                }  
                            }
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            return desiredNode;
        }

        public static XmlNode FindNodeByName(XmlDocument doc, XmlNode parentNode, String nodeName)
        {
            // find a node where the specified attribute has the specified value
            XmlNode desiredNode = null;
            try
            {
                foreach (XmlNode childNode in parentNode.ChildNodes)
                {
                    if (childNode.Name == nodeName)
                    {
                        desiredNode = childNode;
                        break;
                    }
                }
            }
            catch (SettingsException Err)
            {
                throw new SettingsException(Err.ToString());
            }
            return desiredNode;
        }

        public static void RemoveInformation(XmlDocument doc, XmlNode parentNode, String nodeName, String attributName, String attributValue)
        {
            XmlNode removeNode = FindNode(doc, parentNode, nodeName, attributName, attributValue);
            if (removeNode != null)
            {
                parentNode.RemoveChild(removeNode);
            }
        }

        public static String[,] GetInformation(XmlNode node)
        {
            String[,] values = new String[2, node.Attributes.Count];
            for (int i = 0; i < node.Attributes.Count; i++)
            {
                values[0, i] = node.Attributes[i].Name;
                values[1, i] = node.Attributes[i].InnerText;
            }
            return values;
        }

        public static String XmlNodeValue(XmlNode node)
        {
            if (node == null)
            {
                return "";
            }

            String value = String.Empty;
            String row = String.Empty;
            try
            {
                if (node.ChildNodes.Count == 1)
                {
                    value = node.FirstChild.Value.Trim();
                    if (value.Length > 1)
                    {
                        if (value.Substring(0, 1) == "\"")
                        {
                            value = value.Substring(1);
                        }
                        if (value.Substring(value.Length - 1, 1) == "\"")
                        {
                            value = value.Substring(0, value.Length - 1);
                        }
                    }
                    return value;
                }

                if (node.ChildNodes.Count > 1)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.FirstChild == null)
                        {
                            value += '\n';
                            continue;
                        }
                        else
                        {
                            row = childNode.FirstChild.Value.Trim();
                        }

                        if (row.Length > 1)
                        {
                            if (row.Substring(0, 1) == "\"")
                            {
                                row = row.Substring(1);
                            }
                            if (row.Substring(row.Length - 1, 1) == "\"")
                            {
                                row = row.Substring(0, row.Length - 1);
                            }
                        }
                        value += row + '\n';
                    }
                    return value;
                }
            }
            catch (Exception Err)
            {
                String errorMessage = String.Empty;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }

            return "";
        }

    }
}
