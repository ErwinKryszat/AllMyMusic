using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security.Principal;


namespace AllMyMusic.Database
{
    public class dbSchema_SQL : IDisposable
    {
        private ConnectionInfo _dbConnectionInfo;
        private String _dbCurrentVersion;
        private SqlConnection con = null;
        private String assemblyBasePath = "AllMyMusic.DatabaseService.DatabaseSchema.";

        /// <summary>
        /// This class provides functions to create and to update the database schema
        /// </summary>
        /// <param name="connnection"></param>
        public dbSchema_SQL(ConnectionInfo dbCI, String dbCurrentVersion)
        {
            _dbConnectionInfo = dbCI;
            _dbCurrentVersion = dbCurrentVersion;
        }

        public void CreateSchema()
        {
            try
            {
                con = new SqlConnection(_dbConnectionInfo.GetConnectionString());
                con.Open();

                if (IsAdminLoggedIn() == true)
                {
                    CreateSchemaFromScript("SQL.Tables.sql", "GO");
                    CreateSchemaFromScript("SQL.Views.sql", "GO");
                    CreateSchemaFromScript("SQL.Procedures.sql", "GO");

                    UpdateDBversion(_dbCurrentVersion);
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error creating database schema for Server: " + _dbConnectionInfo.ServerName + "\\n\\n";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            finally
            {
                con.Close();
            }
        }
        public void UpdateSchema()
        {
            try
            {
                con = new SqlConnection(_dbConnectionInfo.GetConnectionString());
                con.Open();

                String paramValue = ReadParameter("dbVersion");
                Version dbVersion = new Version(paramValue);

           
                if (IsAdminLoggedIn() == true)
                {
                    if (dbVersion.CompareTo(new Version("1.8.15.0")) < 0)
                    {
                        UpdateSchemaFromScript("SQL.Tables_1_8_15_0.sql", "GO");
                        UpdateSchemaFromScript("SQL.Views_1_8_15_0.sql", "GO");
                        UpdateSchemaFromScript("SQL.Procedures_1_8_15_0.sql", "GO");
                        UpdateDBversion("1.8.15.0");
                    }

                    if (dbVersion.CompareTo(new Version("1.8.20.0")) < 0)
                    {
                        UpdateSchemaFromScript("SQL.Tables_1_8_20_0.sql", "GO");
                        UpdateSchemaFromScript("SQL.Views_1_8_20_0.sql", "GO");
                        UpdateSchemaFromScript("SQL.Procedures_1_8_20_0.sql", "GO");
                        UpdateDBversion("1.8.20.0");
                    }

                    if (dbVersion.CompareTo(new Version("1.8.21.0")) < 0)
                    {
                        UpdateSchemaFromScript("SQL.Procedures_1_8_21_0.sql", "GO");
                        UpdateDBversion("1.8.21.0");
                    }

                    if (dbVersion.CompareTo(new Version("1.8.23.0")) < 0)
                    {
                        //UpdateSchemaFromScript("SQL.Tables_1_8_23_0.sql", "GO");
                        //UpdateSchemaFromScript("SQL.Views_1_8_23_0.sql", "GO");
                        UpdateSchemaFromScript("SQL.Procedures_1_8_23_0.sql", "GO");
                        UpdateDBversion("1.8.23.0");
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error updating database schema for Server: " + _dbConnectionInfo.ServerName + "\\n\\n";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            finally
            {
                con.Close();
            }
        }
        private void CreateSchemaFromScript(String assemblyPath, String delimiter)
        {
            
            String strScript = String.Empty;
            String strSubScript = String.Empty;
            Int32 pos = 0;
            Int32 posStart = 0;
            Int32 posEnd = 0;

            try
            {

                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(assemblyBasePath + assemblyPath);
                Int32 nLength = (Int32)stream.Length - 2; // subtract the length of the unicode preamble
                Byte[] filebytes = new Byte[nLength];
                stream.Read(filebytes, 0, 2);   // Read the unicode preamble
                stream.Read(filebytes, 0, nLength - 2);
                strScript = Encoding.Unicode.GetString(filebytes);

                while (pos < nLength)
                {
                    pos = strScript.IndexOf("CREATE ", pos);
                    if (pos >= 0)
                    {
                        posStart = pos;
                        posEnd = strScript.IndexOf(delimiter, posStart);
                        if (posEnd == -1)
                        {
                            posEnd = (nLength / 2) - 2;
                        }
                        strSubScript = strScript.Substring(posStart, posEnd - posStart);

                        pos = posEnd + delimiter.Length;
                        ExecuteNonQuery(strSubScript);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error creating database schema from Script: " + assemblyPath + "\\n\\n";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        private Int32 UpdateSchemaFromScript(String assemblyPath, String delimiter)
        {
            String strScript = String.Empty;
            String strSubScript = String.Empty;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(assemblyBasePath + assemblyPath);
                Int32 nLength = (Int32)stream.Length;
                Byte[] filebytes = new Byte[nLength];
                stream.Read(filebytes, 0, 2);
                stream.Read(filebytes, 0, nLength - 2);
                strScript = Encoding.Unicode.GetString(filebytes);

                Int32 pos1 = -1;
                Int32 pos2 = -1;
                Int32 pos3 = -1;
                Int32 pos = 0;
                Int32 posStart = 0;
                Int32 posEnd = 0;
                while (pos < nLength)
                {
                    pos1 = strScript.IndexOf("DROP ", pos);
                    pos2 = strScript.IndexOf("ALTER ", pos);
                    pos3 = strScript.IndexOf("CREATE ", pos);

                    if (pos1 >= 0) { pos = pos1; }
                    if (pos2 >= 0)
                    {
                        if (pos >= 0) { pos = Math.Min(pos, pos2); }
                        else { pos = pos2; }
                    }
                    if (pos3 >= 0)
                    {
                        if (pos >= 0) { pos = Math.Min(pos, pos3); }
                        else { pos = pos3; }
                    }



                    if ((pos >= 0) && (pos1 >= 0 || pos2 >= 0 || pos3 >= 0))
                    {
                        posStart = pos;
                        posEnd = strScript.IndexOf(delimiter, posStart);
                        if (posEnd == -1)
                        {
                            posEnd = nLength - 1;
                        }
                        strSubScript = strScript.Substring(posStart, posEnd - posStart);
                        pos = posEnd + delimiter.Length;
                        ExecuteNonQuery(strSubScript);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error updating database schema from Script: " + assemblyPath + "\\n\\n";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            return 1;
        }
        private void ExecuteNonQuery(String strSQL)
        {
            try
            {
                SqlCommand cmd1 = new SqlCommand(strSQL, con);
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandTimeout = 180;
                cmd1.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error executing query: " + strSQL + "\\n\\n";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        private Boolean UpdateDBversion(String version)
        {
            try
            {
                WriteParameter("dbVersion", version);
                return true;
            }
            catch (Exception Err)
            {
                String errorMessage = "Error updating database version parameter: " + version + "\\n\\n";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
                return false;
            }
        }
        private Boolean IsAdminLoggedIn()
        {
            switch (Environment.OSVersion.Version.Major)
            {
                case 5: // Windows XP, Windows Server 2003
                    WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                    if (wp.IsInRole(WindowsBuiltInRole.Administrator) == true)
                    {
                        return true;
                    }
                    else
                    {
                        // MyDialog.InformationMessage("Please login with Administrator permissions to create a database user.");
                        return false;
                    }

                case 6: // Windows Vista
                    return true;

                default:
                    break;
            }
            return true;
        }


        private String ReadParameter(String name)
        {
            String Value = String.Empty;
            String strSQL = "SELECT Value FROM Parameter WHERE Name = '" + name + "'";

            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            Value = reader.GetString(0).TrimEnd();
                        }
                        else { Value = "NOT FOUND"; }
                    }
                }
                reader.Close();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error getting parameter value by following query: " + strSQL;
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
            return Value;
        }
        private void WriteParameter(String name, String value)
        {
            SqlParameter param = null;

            try
            {
                SqlCommand cmd = new SqlCommand("AddParameter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 16);
                param.Value = name;

                param = cmd.Parameters.Add("@Value", SqlDbType.NVarChar, 16);
                param.Value = value;

                cmd.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
                String errorMessage = "Error adding Parameter data\\n" +
                    "Parameter Name: " + name + "\\n" +
                    "Parameter Value: " + value;

                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();
        }

        ~dbSchema_SQL()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (con != null)
            {
                con.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
