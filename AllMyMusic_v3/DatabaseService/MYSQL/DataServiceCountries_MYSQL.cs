using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using AllMyMusic.QueryBuilder;


namespace AllMyMusic.DataService
{
    public class DataServiceCountries_MYSQL : IDataServiceCountries, IDisposable
    {
        private ObservableCollection<CountryItem> listCountriesDatabase = null;
        private CountryCollection listWorldCountriesXML = null;

        #region Properties
        private MySqlConnection _connection;
        public MySqlConnection Connection
        {
            get { return _connection; }
        }

        public ServerType SelectedServerType
        {
            get { return ServerType.MySql; }
        }
        #endregion

        #region Constructor
        public DataServiceCountries_MYSQL(ConnectionInfo conInfo)
        {
            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();
        }
        #endregion

        #region Public
        public async Task<CountryItem> GetCountry(Int32 countryId)
        {
            String strSQL = QueryBuilderCountries.Country(countryId);
            CountryItem country = new CountryItem();
            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // Country ID
                    if (!reader.IsDBNull(0)) { country.CountryId = (Int32)reader.GetInt32(0); }
                    else { country.CountryId = 0; }

                    // Country Name
                    if (!reader.IsDBNull(1))
                    {
                        country.Country = reader.GetString(1).TrimEnd();
                    }
                    else { country.Country = String.Empty; }

                    // Abbreviation
                    if (!reader.IsDBNull(2))
                    {
                        country.Abbreviation = reader.GetString(2).TrimEnd();
                    }
                    else { country.Abbreviation = String.Empty; }

                    // Flag Path
                    if (!reader.IsDBNull(3))
                    {
                        country.FlagPath = reader.GetString(3).TrimEnd();
                    }
                    else { country.FlagPath = String.Empty; }
                }
            }
            reader.Close();

            return country;
        }
        public async Task<CountryItem> GetCountry(String countryName)
        {
            String strSQL = QueryBuilderCountries.Country(countryName);
            CountryItem country = new CountryItem();
            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // Country ID
                    if (!reader.IsDBNull(0)) { country.CountryId = (Int32)reader.GetInt32(0); }
                    else { country.CountryId = 0; }

                    // Country Name
                    if (!reader.IsDBNull(1))
                    {
                        country.Country = reader.GetString(1).TrimEnd();
                    }
                    else { country.Country = String.Empty; }

                    // Abbreviation
                    if (!reader.IsDBNull(2))
                    {
                        country.Abbreviation = reader.GetString(2).TrimEnd();
                    }
                    else { country.Abbreviation = String.Empty; }

                    // Flag Path
                    if (!reader.IsDBNull(3))
                    {
                        country.FlagPath = reader.GetString(3).TrimEnd();
                    }
                    else { country.FlagPath = String.Empty; }
                }
            }
            reader.Close();

            return country;
        }
        public async Task<ObservableCollection<CountryItem>> GetCountries()
        {
            ObservableCollection<CountryItem> countryList = new ObservableCollection<CountryItem>();
            String strSQL = null;

            strSQL = QueryBuilderCountries.AllCountries();

            MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
            cmd.CommandType = CommandType.Text;

            System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();
            //MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CountryItem Country = new CountryItem();

                    // Country ID
                    if (!reader.IsDBNull(0)) { Country.CountryId = (Int32)reader.GetInt32(0); }
                    else { Country.CountryId = 0; }


                    // Country Name
                    if (!reader.IsDBNull(1))
                    {
                        Country.Country = reader.GetString(1).TrimEnd();
                    }
                    else { Country.Country = String.Empty; }

                    // Abbreviation
                    if (!reader.IsDBNull(2))
                    {
                        Country.Abbreviation = reader.GetString(2).TrimEnd();
                    }
                    else { Country.Abbreviation = String.Empty; }

                    // Flag Path
                    if (!reader.IsDBNull(3))
                    {
                        Country.FlagPath = reader.GetString(3).TrimEnd();
                    }
                    else { Country.FlagPath = String.Empty; }

                    countryList.Add(Country);
                }
            }
            reader.Close();

            return countryList;
        }
        public async Task<Int32> AddCountry(CountryItem country)
        {
            MySqlParameter param = null;

            MySqlCommand cmd = new MySqlCommand("AddCountry", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("var_Country", MySqlDbType.VarString, 50);
            param.Value = country.Country;

            param = cmd.Parameters.Add("var_Abbreviation", MySqlDbType.VarString, 10);
            param.Value = country.Abbreviation;

            param = cmd.Parameters.Add("var_FlagPath", MySqlDbType.VarString, 200);
            param.Value = country.FlagPath;


            param = cmd.Parameters.Add("var_ID", MySqlDbType.Int32);
            param.Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            country.CountryId = (int)param.Value;

            return country.CountryId;
        }
        public async Task UpdateCountries(ObservableCollection<CountryItem> countries)
        {
            listCountriesDatabase = countries;
            listWorldCountriesXML = new CountryCollection(Global.WorldCountriesFile);

            DirectoryInfo di = new DirectoryInfo(Global.FlagsPath);
            FileInfo[] files = di.GetFiles("*.gif");
            if (files.Length > 0)
            {
                for (int i = 0; i < listCountriesDatabase.Count; i++)
                {
                    CountryItem country = (CountryItem)listCountriesDatabase[i];

                    if (String.IsNullOrEmpty(country.Abbreviation) == true)
                    {
                        // Get abbreviation from world.xml file
                        country.Abbreviation = listWorldCountriesXML.GetAbbreviation(country.Country);

                        String flagFilename = Global.FlagsPath + "\\" + country.Abbreviation + ".gif";

                        foreach (FileInfo file in files)
                        {
                            if (String.Compare(file.FullName, flagFilename, StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                country.FlagPath = flagFilename;
                            }
                        }

                        await AddCountry(country);
                    }

                }
            }
        }
        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new MySqlConnection(conInfo.GetConnectionString());
            _connection.Open();

        }
        public void Close()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    MySqlConnection.ClearPool(_connection);
                }
                _connection.Dispose();
                _connection = null;
            }
        }
        #endregion

        #region private

        #endregion

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

        ~DataServiceCountries_MYSQL()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            Close();
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}