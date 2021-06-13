using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using AllMyMusic.QueryBuilder;


namespace AllMyMusic.DataService
{
    public class DataServiceCountries_SQL : IDataServiceCountries, IDisposable
    {

        private ObservableCollection<CountryItem> listCountriesDatabase = null;
        private CountryCollection listWorldCountriesXML = null;

        #region Properties
        private SqlConnection _connection;
        public SqlConnection Connection
        {
            get { return _connection; }
        }

        public ServerType SelectedServerType
        {
            get { return ServerType.SqlServer; }
        }
        #endregion

        #region Constructor
        public DataServiceCountries_SQL(ConnectionInfo conInfo)
        {
            _connection = new SqlConnection(conInfo.GetConnectionString());
            _connection.Open();
        }
        #endregion

        #region Public
        public async Task<CountryItem> GetCountry(Int32 countryId)
        {
            try
            {
                String strSQL = QueryBuilderCountries.Country(countryId);
                CountryItem country = new CountryItem();
                SqlCommand cmd = new SqlCommand(strSQL, _connection);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        // Country Name
                        if (!reader.IsDBNull(0))
                        {
                            country.Country = reader.GetString(0).TrimEnd();
                        }
                        else { country.Country = String.Empty; }

                        // Abbreviation
                        if (!reader.IsDBNull(1))
                        {
                            country.Abbreviation = reader.GetString(1).TrimEnd();
                        }
                        else { country.Abbreviation = String.Empty; }


                        // Country ID
                        if (!reader.IsDBNull(2)) { country.CountryId = (Int32)reader.GetInt32(2); }
                        else { country.CountryId = 0; }

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
            catch (Exception Err)
            {
                String errorMessage = "DataServiceCountries_SQL, Error in GetCountry by CountryId";
                throw new DatabaseLayerException(errorMessage, Err);
            }
          
        }
        public async Task<CountryItem> GetCountry(String countryName)
        {
            try
            {
                String strSQL = QueryBuilderCountries.Country(countryName);
                CountryItem country = new CountryItem();
                SqlCommand cmd = new SqlCommand(strSQL, _connection);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Country Name
                        if (!reader.IsDBNull(0))
                        {
                            country.Country = reader.GetString(0).TrimEnd();
                        }
                        else { country.Country = String.Empty; }

                        // Abbreviation
                        if (!reader.IsDBNull(1))
                        {
                            country.Abbreviation = reader.GetString(1).TrimEnd();
                        }
                        else { country.Abbreviation = String.Empty; }

                        // Country ID
                        if (!reader.IsDBNull(2)) { country.CountryId = (Int32)reader.GetInt32(2); }
                        else { country.CountryId = 0; }

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
            catch (Exception Err)
            {
                String errorMessage = "DataServiceCountries_SQL, Error in GetCountry by Name";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<ObservableCollection<CountryItem>> GetCountries()
        {
            try
            {
                ObservableCollection<CountryItem> countryList = new ObservableCollection<CountryItem>();
                String strSQL = null;

                strSQL = QueryBuilderCountries.AllCountries();

                SqlCommand cmd = new SqlCommand(strSQL, _connection);
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CountryItem Country = new CountryItem();

                        // Country Name
                        if (!reader.IsDBNull(0))
                        {
                            Country.Country = reader.GetString(0).TrimEnd();
                        }
                        else { Country.Country = String.Empty; }

                        // Abbreviation
                        if (!reader.IsDBNull(1))
                        {
                            Country.Abbreviation = reader.GetString(1).TrimEnd();
                        }
                        else { Country.Abbreviation = String.Empty; }

                        // Country ID
                        if (!reader.IsDBNull(2)) { Country.CountryId = (Int32)reader.GetInt32(2); }
                        else { Country.CountryId = 0; }

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
            catch (Exception Err)
            {
                String errorMessage = "DataServiceCountries_SQL, Error in GetCountries";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task<Int32> AddCountry(CountryItem country)
        {
            try
            {
                SqlParameter param = null;

                SqlCommand cmd = new SqlCommand("AddCountry", _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 50);
                param.Value = country.Country;

                param = cmd.Parameters.Add("@Abbreviation", SqlDbType.NVarChar, 10);
                param.Value = country.Abbreviation;

                param = cmd.Parameters.Add("@FlagPath", SqlDbType.NVarChar, 200);
                param.Value = country.FlagPath;


                param = cmd.Parameters.Add("@ID", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                country.CountryId = (int)param.Value;

                return country.CountryId;
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceCountries_SQL, Error in AddCountry";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }
        public async Task UpdateCountries(ObservableCollection<CountryItem> countries)
        {
            try
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

                        if ((String.IsNullOrEmpty(country.Abbreviation) == true) || (String.IsNullOrEmpty(country.FlagPath) == true))
                        {
                            // Get abbreviation from world.xml file
                            country.Abbreviation = listWorldCountriesXML.GetAbbreviation(country.Country);

                            String flagFilename = Global.FlagsPath + "\\" + country.Abbreviation + ".gif";

                            foreach (FileInfo file in files)
                            {
                                if (String.Compare(file.FullName, flagFilename, StringComparison.InvariantCultureIgnoreCase) == 0)
                                {
                                    country.FlagPath = flagFilename;
                                    break;
                                }
                            }

                            await AddCountry(country);
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "DataServiceCountries_SQL, Error in UpdateCountries";
                throw new DatabaseLayerException(errorMessage, Err);
            }
        }

        public void ChangeDatabase(ConnectionInfo conInfo)
        {
            Close();

            _connection = new SqlConnection(conInfo.GetConnectionString());
            _connection.Open();

        }
        public void Close()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    SqlConnection.ClearPool(_connection);
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

        ~DataServiceCountries_SQL()
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