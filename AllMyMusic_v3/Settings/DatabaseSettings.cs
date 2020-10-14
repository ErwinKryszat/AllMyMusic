using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace AllMyMusic_v3.Settings
{
    public class DatabaseSettings
    {
        private String _dbCurrentVersion = "1.8.23.0";
        public String DbCurrentVersion
        {
            get { return _dbCurrentVersion; }
        }

        public DatabaseSettings()
        {
            _databaseConnections = new ObservableCollection<ConnectionInfo>();
        }
        public String GetConnectionString()
        {
            return defaultDatabase.GetConnectionString();
        }


        #region Properties
        private ConnectionInfo defaultDatabase;
        public ConnectionInfo DefaultDatabase
        {
            get { return defaultDatabase; }
            set { defaultDatabase = value; }
        }

        private ObservableCollection<ConnectionInfo> _databaseConnections;
        public ObservableCollection<ConnectionInfo> DatabaseConnections
        {
            get { return _databaseConnections; }
            set { _databaseConnections = value; }
        }
        #endregion

        public void Save(XmlDocument doc, XmlNode node)
        {
            try
            {
                XmlNode nodeDatabases = doc.CreateElement("databases");
                node.AppendChild(nodeDatabases);

                XmlAttribute attrDefaultDatabase = doc.CreateAttribute("defaultDatabase");
                attrDefaultDatabase.InnerText = defaultDatabase.DatabaseName;
                nodeDatabases.Attributes.Append(attrDefaultDatabase);

                for (int i = 0; i < _databaseConnections.Count; i++)
                {
                    XmlNode nodeDatabase = doc.CreateElement("database");

                    XmlAttribute attributeProductName = doc.CreateAttribute("productName");
                    attributeProductName.InnerText = _databaseConnections[i].ProductName;
                    nodeDatabase.Attributes.SetNamedItem(attributeProductName);

                    XmlAttribute attributeServerName = doc.CreateAttribute("serverName");
                    attributeServerName.InnerText = _databaseConnections[i].ServerName;
                    nodeDatabase.Attributes.SetNamedItem(attributeServerName);

                    XmlAttribute attributeName = doc.CreateAttribute("databaseName");
                    attributeName.InnerText = _databaseConnections[i].DatabaseName;
                    nodeDatabase.Attributes.SetNamedItem(attributeName);

                    XmlAttribute attributePath = doc.CreateAttribute("path");
                    attributePath.InnerText = _databaseConnections[i].DatabasePath;
                    nodeDatabase.Attributes.SetNamedItem(attributePath);

                    XmlAttribute attributeCollation = doc.CreateAttribute("collation");
                    attributeCollation.InnerText = _databaseConnections[i].Collation;
                    nodeDatabase.Attributes.SetNamedItem(attributeCollation);

                    XmlAttribute attributeCharacterset = doc.CreateAttribute("characterset");
                    attributeCharacterset.InnerText = _databaseConnections[i].Characterset;
                    nodeDatabase.Attributes.SetNamedItem(attributeCharacterset);

                    XmlAttribute attributeUser = doc.CreateAttribute("user");
                    attributeUser.InnerText = _databaseConnections[i].User;
                    nodeDatabase.Attributes.SetNamedItem(attributeUser);

                    XmlAttribute attributePassword = doc.CreateAttribute("password");
                    attributePassword.InnerText = _databaseConnections[i].Password;
                    nodeDatabase.Attributes.SetNamedItem(attributePassword);

                    nodeDatabases.AppendChild(nodeDatabase);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error saving Database settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load(XmlNode node)
        {
            try
            {
                String defaultDatabaseName = node.Attributes["defaultDatabase"].InnerText;
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "database":
                            ConnectionInfo connection = GetDatabaseConnection(childNode);
                            _databaseConnections.Add(connection);
                            break;
                        default:
                            break;
                    }
                }

                defaultDatabase = GetDatabaseConnection(defaultDatabaseName);
            }
            catch(Exception Err)
            {
                String errorMessage = "Error loading Database settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        private ConnectionInfo GetDatabaseConnection(XmlNode node)
        {
            ConnectionInfo dbConInfo = new ConnectionInfo();
            foreach (XmlAttribute attr in node.Attributes)
            {
                switch (attr.Name)
                {
                    case "productName":
                        dbConInfo.ProductName = attr.InnerText;
                        break;
                    case "serverName":
                        dbConInfo.ServerName = attr.InnerText;
                        break;
                    case "name":    // ToDo Remove this in a higher version
                        dbConInfo.DatabaseName = attr.InnerText;
                        break;
                    case "databaseName":
                        dbConInfo.DatabaseName = attr.InnerText;
                        break;
                    case "path":
                        dbConInfo.DatabasePath = attr.InnerText;
                        break;
                    case "collation":
                        dbConInfo.Collation = attr.InnerText;
                        break;
                    case "characterset":
                        dbConInfo.Characterset = attr.InnerText;
                        break;                 
                    case "user":
                        dbConInfo.User = attr.InnerText;
                        break;
                    case "password":
                        dbConInfo.Password = attr.InnerText;
                        break;
                    default:
                        break;
                }
            }
            return dbConInfo;
        }
        private ConnectionInfo GetDatabaseConnection(String databaseName)
        {
            foreach (ConnectionInfo dbConInfo in _databaseConnections)
            {
                if (dbConInfo.DatabaseName == databaseName)
                {
                    return dbConInfo;
                }
            }
            return null;
        }
    }

}
