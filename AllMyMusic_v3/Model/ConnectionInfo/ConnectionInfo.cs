using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace AllMyMusic_v3
{
    public class ConnectionInfo : IDataErrorInfo
    {
        #region State Properties
        private Boolean _connectionTested = false;
        public Boolean ConnectionTested
        {
            get { return _connectionTested; }
            set { _connectionTested = value; }
        }

        private String _collation = String.Empty;
        public String Collation
        {
            get { return _collation; }
            set { _collation = value; }
        }

        private String _characterset = String.Empty;
        public String Characterset
        {
            get { return _characterset; }
            set { _characterset = value; }
        }

        private String _productName = String.Empty;
        public String ProductName
        {
            get { return _productName; }
            set 
            { 
                _productName = value;
                if (_productName == "SQL_Server")
                {
                    _serverType = ServerType.SqlServer;
                }
                if (_productName == "MySQL")
                {
                    _serverType = ServerType.MySql;
                }
            }
        }

        private ServerType _serverType = ServerType.Unknown;
        public ServerType ServerType
        {
            get 
            {
                if (_productName == "SQL_Server")
                {
                    _serverType = ServerType.SqlServer;
                    return _serverType;
                }
                if (_productName == "MySQL")
                {
                    _serverType = ServerType.MySql;
                    return _serverType;
                }
                return ServerType.Unknown;
            }
            set 
            {
                _serverType = value;
                if (_serverType == ServerType.SqlServer)
                {
                    _productName = "SQL_Server";

                    if (String.IsNullOrEmpty(_databaseName) == true)
                    {
                        _databaseName = "master";
                    }
                }
                if (_serverType == ServerType.MySql)
                {
                    _productName = "MySQL";

                    if (String.IsNullOrEmpty(_databaseName) == true)
                    {
                        _databaseName = "mysql";
                    }
                }
            }
        }

        private String _serverName = String.Empty;
        public String ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        private String _databaseName = String.Empty;
        public String DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        private String _databasePath = String.Empty;
        public String DatabasePath
        {
            get { return _databasePath; }
            set { _databasePath = value; }
        }

        private String _user = String.Empty;
        public String User
        {
            get { return _user; }
            set { _user = value; }
        }

        private String _password = String.Empty;
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }
        #endregion

        public ConnectionInfo()
        { 
        
        }

        public ConnectionInfo(ConnectionInfo _dbConInfo, String _databaseName)
        {
            this._characterset = _dbConInfo.Characterset;
            this._collation = _dbConInfo.Collation;
            this._databaseName = _dbConInfo.DatabaseName;
            this._databasePath = _dbConInfo.DatabasePath;
            this._password = _dbConInfo.Password;
            this._productName = _dbConInfo.ProductName;
            this._serverName = _dbConInfo.ServerName;
            this._serverType = _dbConInfo.ServerType;
            this._user = _dbConInfo.User;

            if (String.IsNullOrEmpty(_databaseName) == false)
            {
                this._databaseName = _databaseName;
            }
        }

        public ConnectionInfo Copy()
        {
            ConnectionInfo newDbConInfo = new ConnectionInfo();
            newDbConInfo._characterset = this._characterset;
            newDbConInfo._collation = this._collation;
            newDbConInfo._databaseName = this._databaseName;
            newDbConInfo._databasePath = this._databasePath;
            newDbConInfo._password = this._password;
            newDbConInfo._productName = this._productName;
            newDbConInfo._serverName = this._serverName;
            newDbConInfo._serverType = this._serverType;
            newDbConInfo._user = this._user;

            return newDbConInfo;
        }

        public String GetConnectionString()
        {
            Int32 dbTimeOut = 30; // Seconds

            String connectionString = String.Empty;
            if (this._serverType == ServerType.SqlServer)
            {
                SqlConnectionStringBuilder sqlConnection = new SqlConnectionStringBuilder();
                sqlConnection.DataSource = this._serverName;
                sqlConnection.InitialCatalog = this._databaseName;
                sqlConnection.IntegratedSecurity = true;
                sqlConnection.ConnectTimeout = dbTimeOut;
                sqlConnection.MultipleActiveResultSets = true;
                connectionString = sqlConnection.ToString();
            }

            if (this._serverType == ServerType.MySql)
            {
                MySqlConnectionStringBuilder sqlConnection = new MySqlConnectionStringBuilder();
                sqlConnection.Server = this._serverName;
                sqlConnection.Database = this._databaseName;
                sqlConnection.UserID = this._user;
                sqlConnection.Password = this._password;
                sqlConnection.AllowUserVariables = true;
                connectionString = sqlConnection.ToString();
            }

            return connectionString;
        }

        #region IDataErrorInfo Members
        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.GetValidationError(propertyName);
            }
        }
        #endregion // IDataErrorInfo Members

        #region Validation

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return true;
            }
        }

        static readonly string[] ValidatedProperties = 
        { 
            "ServerName", 
            "ServerType", 
            "DatabaseName",
            "User",
            "Password"
        };

        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "ServerName":
                    error = this.ValidateServerName();
                    break;

                case "ServerType":
                    error = this.ValidateServerType();
                    break;

                case "DatabaseName":
                    error = this.ValidateDatabaseName();
                    break;

                case "User":
                    error = this.ValidateUserName();
                    break;

                case "Password":
                    error = this.ValidatePassword();
                    break;

                default:
                    Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
                    break;
            }

            return error;
        }

        string ValidateServerName()
        {
            if (IsStringMissing(this.ServerName))
            {
                return "ServerName missing";
            }
            else if (!IsValidServerName(this.ServerName))
            {
                return "Invalid ServerName";
            }
            return null;
        }

        string ValidateServerType()
        {
            if (ServerType == ServerType.Unknown) 
            {
                return "Invalid Server Type";
            }
            return null;
        }

        string ValidateDatabaseName()
        {
            if (IsStringMissing(this.DatabaseName))
                return "DatabaseName missing";

            return null;
        }

        string ValidateUserName()
        {
            if (IsStringMissing(this.User) && ServerType == ServerType.MySql)
                return "User Name missing";

            return null;
        }

        string ValidatePassword()
        {
            if (IsStringMissing(this.Password) && ServerType == ServerType.MySql)
                return "Password missing";

            return null;
        }


        static bool IsStringMissing(string value)
        {
            return  String.IsNullOrEmpty(value) || value.Trim() == String.Empty;
        }

        static bool IsValidServerName(string serverName)
        {
            if (IsStringMissing(serverName))
                return false;

            // This regex pattern came from: http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            //string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            //string pattern = @"^(?=.{1,255})([0-9A-Za-z]|_{1}|\*{1}$)(?:(?:[0-9A-Za-z]|\b-){0,61}[0-9A-Za-z])?(?:\.[0-9A-Za-z](?:(?:[0-9A-Za-z]|\b-){0,61}[0-9A-Za-z])?)*\.?$";
            //return Regex.IsMatch(serverName, pattern, RegexOptions.IgnoreCase);

            return true;
        }

        #endregion // Validation

    }
}
