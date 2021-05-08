using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace AllMyMusic.Database
{
    public class dbSchemaValidation
    {
        private ConnectionInfo _dbCI;
        
        #region Properties
        private Boolean validatedTables = false;
        public Boolean ValidatedTables
        {
            get { return validatedTables; }
        }

        private Boolean validatedViews = false;
        public Boolean ValidatedViews
        {
            get { return validatedViews; }
        }

        private Boolean validatedProcedures = false;
        public Boolean ValidatedProcedures
        {
            get { return validatedProcedures; }
        }

        private Boolean validatedSchema = false;
        public Boolean ValidatedSchema
        {
            get { return validatedSchema; }
        }

        #endregion

        public dbSchemaValidation(ConnectionInfo dbCI)
        {
            _dbCI = dbCI;
        }

        public Boolean CheckDatabaseSchema()
        {
            DataTable tables = null;
            DataTable views = null;
            DataTable procedures = null;

            if (_dbCI.ServerType == ServerType.SqlServer)
            {
                SqlConnection con = new SqlConnection(_dbCI.GetConnectionString());
                con.Open();

                tables = con.GetSchema("Tables");
                views = con.GetSchema("Views");
                procedures = con.GetSchema("Procedures");

                con.Close();
            }

            if (_dbCI.ServerType == ServerType.MySql)
            {
                MySqlConnection con = new MySqlConnection(_dbCI.GetConnectionString());
                con.Open();

                tables = con.GetSchema("Tables");
                views = con.GetSchema("Views");
                procedures = con.GetSchema("Procedures");

                con.Close();
            }


            validatedTables = CheckSchemaTables(tables);
            validatedViews = CheckSchemaViews(views);
            validatedProcedures = CheckSchemaProcedures(procedures);

            validatedSchema = validatedTables && validatedViews && validatedProcedures;


            if (validatedTables == false)
            {
                String errorMessage = "Databaseschema verification returned an error for checking: TABLES" + "\\n\\n";
                ShowError.ShowAndLog(errorMessage, 2001);
            }
            if (validatedViews == false)
            {
                String errorMessage = "Databaseschema verification returned an error for checking: VIEWS" + "\\n\\n";
                ShowError.ShowAndLog(errorMessage, 2001);
            }
            if (validatedProcedures == false)
            {
                String errorMessage = "Databaseschema verification returned an error for checking: PROCEDURES" + "\\n\\n";
                ShowError.ShowAndLog(errorMessage, 2001);
            }

            return validatedSchema;
        }

        private Boolean CheckSchemaTables(DataTable tables)
        {
            if (tables == null)
            {
                return false;
            }
            Int32 TableValidation = 0;
            foreach (DataRow row in tables.Rows)
            {
                String tableName = row["TABLE_NAME"].ToString().ToLower();
                switch (tableName)
                {
                    case "albumgenres":
                        TableValidation++;
                        break;
                    case "albums":
                        TableValidation++;
                        break;
                    case "bands":
                        TableValidation++;
                        break;
                    case "bookmarks":
                        TableValidation++;
                        break;
                    case "composer":
                        TableValidation++;
                        break;
                    case "conductor":
                        TableValidation++;
                        break;
                    case "countries":
                        TableValidation++;
                        break;
                    case "genres":
                        TableValidation++;
                        break;
                    case "images":
                        TableValidation++;
                        break;
                    case "id3tags":
                        TableValidation++;
                        break;
                    case "languages":
                        TableValidation++;
                        break;
                    case "leadperformer":
                        TableValidation++;
                        break;
                    case "parameter":
                        TableValidation++;
                        break;
                    case "playlistnames":
                        TableValidation++;
                        break;
                    case "playlistsongs":
                        TableValidation++;
                        break;
                    case "songs":
                        TableValidation++;
                        break;
                    case "websites":
                        TableValidation++;
                        break;
                    default:
                        break;
                }
            }

            if (TableValidation >= 17)
            {
                return true;
            }
            return false;
        }
        private Boolean CheckSchemaViews(DataTable views)
        {
            if (views == null)
            {
                return false;
            }

            Int32 ViewValidation = 0;
            foreach (DataRow row in views.Rows)
            {
                String tableName = row["TABLE_NAME"].ToString().ToLower();
                switch (tableName)
                {
                    case "viewalbums":
                        ViewValidation++;
                        break;
                    case "viewbands":
                        ViewValidation++;
                        break;
                    case "viewplaylist":
                        ViewValidation++;
                        break;
                    case "viewsongs":
                        ViewValidation++;
                        break;
                    case "viewalbumquality":
                        ViewValidation++;
                        break;
                    default:
                        break;
                }
            }

            if (ViewValidation >= 5)
            {
                return true;
            }
            return false;
        }
        private Boolean CheckSchemaProcedures(DataTable procedures)
        {
            if (procedures == null)
            {
                return false;
            }

            Int32 ProcValidation = 0;
            foreach (DataRow row in procedures.Rows)
            {
                String routineName = row["ROUTINE_NAME"].ToString().ToLower();
                switch (routineName)
                {
                    case "addalbum":
                        ProcValidation++;
                        break;
                    case "addband":
                        ProcValidation++;
                        break;
                    case "addbookmark":
                        ProcValidation++;
                        break;
                    case "addcomposer":
                        ProcValidation++;
                        break;
                    case "addconductor":
                        ProcValidation++;
                        break;
                    case "addcountry":
                        ProcValidation++;
                        break;
                    case "addimage":
                        ProcValidation++;
                        break;
                    case "addleadperformer":
                        ProcValidation++;
                        break;
                    case "addparameter":
                        ProcValidation++;
                        break;
                    case "addplaylist":
                        ProcValidation++;
                        break;
                    case "addplaylistsong":
                        ProcValidation++;
                        break;
                    case "addsong":
                        ProcValidation++;
                        break;
                    case "addwebsite":
                        ProcValidation++;
                        break;
                    case "deletealbum":
                        ProcValidation++;
                        break;
                    case "deletealbumgenre":
                        ProcValidation++;
                        break;
                    case "deleteband":
                        ProcValidation++;
                        break;
                    case "deletebookmark":
                        ProcValidation++;
                        break;
                    case "deletecomposer":
                        ProcValidation++;
                        break;
                    case "deleteconductor":
                        ProcValidation++;
                        break;
                    case "deletecountry":
                        ProcValidation++;
                        break;
                    case "deletedecade":
                        ProcValidation++;
                        break;
                    case "deletegenre":
                        ProcValidation++;
                        break;
                    case "deletelanguage":
                        ProcValidation++;
                        break;
                    case "deleteleadperformer":
                        ProcValidation++;
                        break;
                    case "deleteplaylist":
                        ProcValidation++;
                        break;
                    case "deleteplaylistsong":
                        ProcValidation++;
                        break;
                    case "deletesong":
                        ProcValidation++;
                        break;
                    case "deletevariousartists":
                        ProcValidation++;
                        break;
                    case "deleteyear":
                        ProcValidation++;
                        break;
                    case "purgedatabase":
                        ProcValidation++;
                        break;
                    case "playlistmoverows":
                        ProcValidation++;
                        break;

                    default:
                        break;
                }
            }

            if (ProcValidation >= 31)
            {
                return true;
            }
            return false;
        }
        
    }
}
