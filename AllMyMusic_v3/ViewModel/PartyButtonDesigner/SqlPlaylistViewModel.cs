using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Microsoft.Win32;
using AllMyMusic.Settings;
using AllMyMusic.DataService;

namespace AllMyMusic.ViewModel
{
    public class SqlPlaylistViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ConnectionInfo _conInfo;
        private IDataServiceSongs _dataServiceSongs;
        private IDataServiceListItems _dataServiceListItems;
        private PartyButtonConfigViewModel _playlistConfiguration;

        private String _oldQuery = String.Empty;


        private QueryComposerViewModel _queryComposerViewModel;
        private String _sqlPlaylistQuery;
        private Int32 _textBoxSelectionStart;
        private ObservableCollection<String> _operators;
        private String _selectedOperator;

        private RelayCommand<object> _insertOperatorCommand;
        private RelayCommand<object> _insertQueryCommand;
        private RelayCommand<object> _testQueryCommand;
        private RelayCommand<object> _showExampleCommand;
        #endregion // Fields

        #region Commands
        public ICommand InsertQueryCommand
        {
            get
            {
                if (null == _insertQueryCommand)
                    _insertQueryCommand = new RelayCommand<object>(ExecuteInsertQueryCommand, CanInsertQueryCommand);

                return _insertQueryCommand;
            }
        }
        private void ExecuteInsertQueryCommand(object notUsed)
        {
            Insert_Query();
        }
        private bool CanInsertQueryCommand(object notUsed)
        {
            return ((_queryComposerViewModel != null) && (String.IsNullOrEmpty(_queryComposerViewModel.SqlPlaylistQuery) == false));
        }

        public ICommand InsertOperatorCommand
        {
            get
            {
                if (null == _insertOperatorCommand)
                    _insertOperatorCommand = new RelayCommand<object>(ExecuteInsertOperatorCommand, CanInsertOperatorCommand);

                return _insertOperatorCommand;
            }
        }
        private void ExecuteInsertOperatorCommand(object notUsed)
        {
            Insert_Operator();
        }
        private bool CanInsertOperatorCommand(object notUsed)
        {
            return ((_queryComposerViewModel != null) && (String.IsNullOrEmpty(_selectedOperator) == false));
        }
   
        public ICommand TestQueryCommand
        {
            get
            {
                if (null == _testQueryCommand)
                    _testQueryCommand = new RelayCommand<object>(ExecuteTestQueryCommand, CanTestQueryCommand);

                return _testQueryCommand;
            }
        }
        private void ExecuteTestQueryCommand(object notUsed)
        {
            Task.Run(() => TestQuery());
        }
        private bool CanTestQueryCommand(object notUsed)
        {
            return true;
        }

        public ICommand ShowExampleCommand
        {
            get
            {
                if (null == _showExampleCommand)
                    _showExampleCommand = new RelayCommand<object>(ExecuteShowExampleCommand, CanShowExampleCommand);

                return _showExampleCommand;
            }
        }
        private void ExecuteShowExampleCommand(object notUsed)
        {
            Task.Run(() => Insert_Example());
        }
        private bool CanShowExampleCommand(object notUsed)
        {
            return true;
        }
        #endregion

        #region Presentation Properties
        public QueryComposerViewModel QueryComposerViewModel
        {
            get { return _queryComposerViewModel; }
            set
            {
                _queryComposerViewModel = value;

                RaisePropertyChanged("QueryComposerViewModel");
            }
        }

        public String SqlPlaylistQuery
        {
            get { return _sqlPlaylistQuery; }
            set
            {
                _sqlPlaylistQuery = value;

                RaisePropertyChanged("SqlPlaylistQuery");

                StringEventArgs args = new StringEventArgs(_sqlPlaylistQuery);
                OnSqlPlaylistQueryChanged(this,args);
            }
        }

        public PartyButtonConfigViewModel PlaylistConfiguration
        {
            get { return _playlistConfiguration; }
            set 
            { 
                _playlistConfiguration = value;
                OnPlaylistConfigurationChanged();
            }
        }

        // TextBoxSelectionStart is set from viewSqlPlaylistPanel code behind
        public Int32 TextBoxSelectionStart
        {
            get { return _textBoxSelectionStart; }
            set { _textBoxSelectionStart = value;}
        }
        public ObservableCollection<String> Operators
        {
            get { return _operators; }
            set
            {
                _operators = value;
                RaisePropertyChanged("CompareOperators");
            }
        }
        public String SelectedOperator
        {
            get { return _selectedOperator; }
            set
            {
                if (value == _selectedOperator)
                    return;

                _selectedOperator = value;
                RaisePropertyChanged("SelectedCompareOperator");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public SqlPlaylistViewModel(ConnectionInfo conInfo)
        {
            _conInfo = conInfo;

            _queryComposerViewModel = new QueryComposerViewModel(_conInfo);

            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceSongs = new DataServiceSongs_SQL(conInfo);
                _dataServiceListItems = new DataServiceListItems_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceSongs = new DataServiceSongs_MYSQL(conInfo);
                _dataServiceListItems = new DataServiceListItems_MYSQL(conInfo);
            }

            LoadOperators();

            SqlPlaylistQuery = "SELECT * FROM viewSongs WHERE ";

            Localize();
        }
        #endregion  // Constructor

        private void OnPlaylistConfigurationChanged()
        {
            SqlPlaylistQuery = _playlistConfiguration.SqlQuery;
        }

        private void LoadOperators()
        {
            String[] operators = { " AND ", " OR ", " AND NOT ", " OR NOT ", " ( ", " ) " };

            Operators = new ObservableCollection<string>(operators);

            SelectedOperator = " OR ";
        }
        private void Insert_Query()
        {
            String newQuery = _queryComposerViewModel.SqlPlaylistQuery;

            Int32 cursorPosition = _textBoxSelectionStart;
            Int32 textLength = _sqlPlaylistQuery.Length;

            if (newQuery.Trim() == String.Empty)
            {
                newQuery = "SELECT * FROM viewSongs WHERE ";
            }

            if (cursorPosition < textLength)
            {
                String leftPart = _sqlPlaylistQuery.Substring(0, cursorPosition);
                String rightPart = _sqlPlaylistQuery.Substring(cursorPosition, textLength - cursorPosition);
                newQuery = leftPart + _queryComposerViewModel.SqlPlaylistQuery + rightPart;
            }
            else
            {
                newQuery = _sqlPlaylistQuery + _queryComposerViewModel.SqlPlaylistQuery;
            }

            SqlPlaylistQuery = newQuery;
        }
        private void Insert_Operator()
        {
            Int32 cursorPosition = _textBoxSelectionStart;
            Int32 textLength = _sqlPlaylistQuery.Length;

            if (cursorPosition < textLength)
            {
                String leftPart = _sqlPlaylistQuery.Substring(0, cursorPosition);
                String rightPart = _sqlPlaylistQuery.Substring(cursorPosition, textLength - cursorPosition);
                SqlPlaylistQuery = leftPart + _selectedOperator + rightPart;
            }
            else
            {
                SqlPlaylistQuery = _sqlPlaylistQuery + _selectedOperator;
            }
        }

        private async Task Insert_Example()
        {
            _playlistConfiguration.ButtonType = PartyButtonType.Query;
            _playlistConfiguration.Randomize = true;
            _playlistConfiguration.ButtonImagePath = GetRandomImagePath();
            _playlistConfiguration.ToolTipText = "This query was created with AllMyMusic random Party Button generator";

            Application.Current.Dispatcher.Invoke((Action)(delegate
               {
                   _playlistConfiguration.LargeButtonImage = ResourceHelper.GetImage(_playlistConfiguration.ButtonImagePath);
                   _playlistConfiguration.SmallButtonImage = ResourceHelper.GetImage(_playlistConfiguration.ButtonImagePath);
               }));

            Int32 randomIndex = Global.rnd.Next(6);
            switch (randomIndex)
            {
                case 0:
                    _playlistConfiguration.ButtonLabel = "Years";
                    _playlistConfiguration.SqlQuery = await RandomYearQuery();
                    break;
                case 1:
                    _playlistConfiguration.ButtonLabel = "Countries";
                    _playlistConfiguration.SqlQuery = await RandomCountryQuery();
                    break;
                case 2:
                    _playlistConfiguration.ButtonLabel = "Genres";
                    _playlistConfiguration.SqlQuery = await RandomGenreQuery();
                    break;
                case 3:
                    _playlistConfiguration.ButtonLabel = "Bands";
                    _playlistConfiguration.SqlQuery = await RandomBandQuery();
                    break;
                case 4:
                    _playlistConfiguration.ButtonLabel = "Albums";
                    _playlistConfiguration.SqlQuery = await RandomAlbumQuery();
                    break;
                case 5:
                    _playlistConfiguration.ButtonLabel = "Search";
                    _playlistConfiguration.SqlQuery = RandomSearchQuery();
                    break;

                default:
                    _playlistConfiguration.SqlQuery = await RandomBandQuery();
                    break;
            }

            SqlPlaylistQuery = _playlistConfiguration.SqlQuery;

        }

        private async Task TestQuery()
        {
            String result = String.Empty; ;


            if (String.IsNullOrEmpty(_sqlPlaylistQuery.Trim()) == false)
            {
                result = await TestQuery(_sqlPlaylistQuery);
                if (result.IndexOf("Query found") >= 0)
                {
                    MessageBox.Show(result, "Success");
                }
                else
                {
                    MessageBox.Show(result, "Error");
                }
            }
        }
        private async Task<String> TestQuery(String strSQL)
        {
            String result = String.Empty;
            try
            {
                ObservableCollection<SongItem>  songs = await _dataServiceSongs.GetSongs(strSQL);
                result = String.Format("Query found {0} songs.", songs.Count);
            }
            catch (Exception Err)
            {
                if (Err.InnerException != null)
                {
                    result = Err.InnerException.Message;
                }
                else
                {
                    result = Err.Message;
                }
            }
            return result;
        }

        #region Random Queries
        private async Task<String> RandomBandQuery()
        {
            String strQuery = "SELECT * FROM viewSongs WHERE ";
            ObservableCollection<String> bandNames = await GetRandomValues("BandName", 5);

            String[] conditions = new String[5];

            if (bandNames.Count > 0)
            {
                conditions[0] = "BandName = '" + bandNames[0] + "'";
                strQuery = strQuery + conditions[0];
            }

            if (bandNames.Count > 1)
            {
                conditions[1] = " OR BandName = '" + bandNames[1] + "'";
                strQuery = strQuery + conditions[1];
            }
            if (bandNames.Count > 2)
            {
                conditions[2] = " OR BandName = '" + bandNames[2] + "'";
                strQuery = strQuery + conditions[2];
            }
            if (bandNames.Count > 3)
            {
                conditions[3] = " OR BandName = '" + bandNames[3] + "'";
                strQuery = strQuery + conditions[3];
            }
            if (bandNames.Count > 4)
            {
                conditions[4] = " OR BandName = '" + bandNames[4] + "'";
                strQuery = strQuery + conditions[4];
            }

            return strQuery;
        }
        private async Task<String> RandomAlbumQuery()
        {
            String strQuery = "SELECT * FROM viewSongs WHERE ";
            ObservableCollection<String> albumNames = await GetRandomValues("AlbumName", 5);

            String[] conditions = new String[5];

            if (albumNames.Count > 0)
            {
                conditions[0] = "AlbumName = '" + albumNames[0] + "'";
                strQuery = strQuery + conditions[0];
            }

            if (albumNames.Count > 1)
            {
                conditions[1] = " OR AlbumName = '" + albumNames[1] + "'";
                strQuery = strQuery + conditions[1];
            }
            if (albumNames.Count > 2)
            {
                conditions[2] = " OR AlbumName = '" + albumNames[2] + "'";
                strQuery = strQuery + conditions[2];
            }
            if (albumNames.Count > 3)
            {
                conditions[3] = " OR AlbumName = '" + albumNames[3] + "'";
                strQuery = strQuery + conditions[3];
            }
            if (albumNames.Count > 4)
            {
                conditions[4] = " OR AlbumName = '" + albumNames[4] + "'";
                strQuery = strQuery + conditions[4];
            }

            return strQuery;
        }
        private async Task<String> RandomGenreQuery()
        {
            String strQuery = "SELECT * FROM viewSongs WHERE ";
            ObservableCollection<String> genreNames = await GetRandomValues("Genre", 5);

            String[] conditions = new String[5];

            if (genreNames.Count > 0)
            {
                conditions[0] = "Genre = '" + genreNames[0] + "'";
                strQuery = strQuery + conditions[0];
            }

            if (genreNames.Count > 1)
            {
                conditions[1] = " OR Genre = '" + genreNames[1] + "'";
                strQuery = strQuery + conditions[1];
            }
            if (genreNames.Count > 2)
            {
                conditions[2] = " OR Genre = '" + genreNames[2] + "'";
                strQuery = strQuery + conditions[2];
            }
            if (genreNames.Count > 3)
            {
                conditions[3] = " OR Genre = '" + genreNames[3] + "'";
                strQuery = strQuery + conditions[3];
            }
            if (genreNames.Count > 4)
            {
                conditions[4] = " OR Genre = '" + genreNames[4] + "'";
                strQuery = strQuery + conditions[4];
            }

            return strQuery;
        }
        private async Task<String> RandomYearQuery()
        {
            String strQuery = "SELECT * FROM viewSongs WHERE ";
            ObservableCollection<String> yearNames = await GetRandomValues("Year", 5);

            String[] conditions = new String[5];

            if (yearNames.Count > 0)
            {
                conditions[0] = "Year = '" + yearNames[0] + "'";
                strQuery = strQuery + conditions[0];
            }

            if (yearNames.Count > 1)
            {
                conditions[1] = " OR Year = '" + yearNames[1] + "'";
                strQuery = strQuery + conditions[1];
            }
            if (yearNames.Count > 2)
            {
                conditions[2] = " OR Year = '" + yearNames[2] + "'";
                strQuery = strQuery + conditions[2];
            }
            if (yearNames.Count > 3)
            {
                conditions[3] = " OR Year = '" + yearNames[3] + "'";
                strQuery = strQuery + conditions[3];
            }
            if (yearNames.Count > 4)
            {
                conditions[4] = " OR Year = '" + yearNames[4] + "'";
                strQuery = strQuery + conditions[4];
            }

            return strQuery;
        }
        private async Task<String> RandomCountryQuery()
        {
            String strQuery = "SELECT * FROM viewSongs WHERE ";
            ObservableCollection<String> countryNames = await GetRandomValues("Country", 5);

            String[] conditions = new String[5];

            if (countryNames.Count > 0)
            {
                conditions[0] = "Country = '" + countryNames[0] + "'";
                strQuery = strQuery + conditions[0];
            }

            if (countryNames.Count > 1)
            {
                conditions[1] = " OR Country = '" + countryNames[1] + "'";
                strQuery = strQuery + conditions[1];
            }
            if (countryNames.Count > 2)
            {
                conditions[2] = " OR Country = '" + countryNames[2] + "'";
                strQuery = strQuery + conditions[2];
            }
            if (countryNames.Count > 3)
            {
                conditions[3] = " OR Country = '" + countryNames[3] + "'";
                strQuery = strQuery + conditions[3];
            }
            if (countryNames.Count > 4)
            {
                conditions[4] = " OR Country = '" + countryNames[4] + "'";
                strQuery = strQuery + conditions[4];
            }

            return strQuery;
        }
        private String RandomSearchQuery()
        {
            ObservableCollection<String> searchWords = new ObservableCollection<String>();
            searchWords.Add("%water%");
            searchWords.Add("%fire%");
            searchWords.Add("%earth%");
            searchWords.Add("%air%");
            searchWords.Add("%love%");
            searchWords.Add("%hate%");
            searchWords.Add("%god%");
            searchWords.Add("%devil%");
            searchWords.Add("%black%");
            searchWords.Add("%white%");
            searchWords.Add("%happy%");
            searchWords.Add("%lucky%");

            Int32 randomIndex = Global.rnd.Next(searchWords.Count);
            String search = searchWords[randomIndex];

            String strQuery = "SELECT * FROM viewSongs WHERE BandName like '" + search + "' OR AlbumName like '" + search + "' OR SongName like '" + search + "'";

            return strQuery;
        }

        private async Task<ObservableCollection<String>> GetRandomValues(String fieldName, Int32 reqNumValues)
        {
            ObservableCollection<String> values = new ObservableCollection<string>();
            ObservableCollection<String> fieldValues = await  _dataServiceListItems.GetListItemsByColumn(fieldName);

            Int32 randomIndex = Global.rnd.Next(fieldValues.Count);

            if (reqNumValues < fieldValues.Count)
            {
                for (int i = 0; i < reqNumValues; i++)
                {
                    randomIndex = Global.rnd.Next(fieldValues.Count);
                    values.Add(fieldValues[randomIndex]);
                }
            }
            else
            {
                values = fieldValues;
            }

            return values;
        }
        private String GetRandomImagePath()
        {
            DirectoryInfo di = new DirectoryInfo(Global.PartyButtonImages);
            FileInfo[] fi = di.GetFiles();
            Int32 randomIndex = Global.rnd.Next(fi.Length);
            return fi[randomIndex].FullName;
        }
        #endregion

        #region Events
        public delegate void SqlPlaylistQueryChangedEventHandler(object sender, StringEventArgs e);
        public event SqlPlaylistQueryChangedEventHandler SqlPlaylistQueryChanged;
        protected virtual void OnSqlPlaylistQueryChanged(object sender, StringEventArgs e)
        {
            if (this.SqlPlaylistQueryChanged != null)
            {
                this.SqlPlaylistQueryChanged(this, e);
            }
        }
        #endregion

        #region Localization
        private String _groupInsertField = "Insert a Field";
        private String _groupOperator = "Operator";
        private String _groupTest = "Test this query";
        private String _groupExample = "Show Example";

        private String _cmdInsertQuery = "Insert";
        private String _cmdInsertOperator = "Insert";
        private String _cmdTest = "Test";
        private String _cmdExample = "Example";

        public String GroupInsertField
        {
            get { return _groupInsertField; }
            set
            {
                if (value == _groupInsertField)
                    return;

                _groupInsertField = value;

                RaisePropertyChanged("GroupInsertField");
            }
        }
        public String GroupOperator
        {
            get { return _groupOperator; }
            set
            {
                if (value == _groupOperator)
                    return;

                _groupOperator = value;

                RaisePropertyChanged("GroupOperator");
            }
        }
        public String GroupTest
        {
            get { return _groupTest; }
            set
            {
                if (value == _groupTest)
                    return;

                _groupTest = value;

                RaisePropertyChanged("GroupTest");
            }
        }
        public String GroupExample
        {
            get { return _groupExample; }
            set
            {
                if (value == _groupExample)
                    return;

                _groupExample = value;

                RaisePropertyChanged("GroupExample");
            }
        }

        public String CmdInsertQuery
        {
            get { return _cmdInsertQuery; }
            set
            {
                if (value == _cmdInsertQuery)
                    return;

                _cmdInsertQuery = value;

                RaisePropertyChanged("CmdInsertQuery");
            }
        }
        public String CmdInsertOperator
        {
            get { return _cmdInsertOperator; }
            set
            {
                if (value == _cmdInsertOperator)
                    return;

                _cmdInsertOperator = value;

                RaisePropertyChanged("CmdInsertOperator");
            }
        }
        public String CmdTest
        {
            get { return _cmdTest; }
            set
            {
                if (value == _cmdTest)
                    return;

                _cmdTest = value;

                RaisePropertyChanged("CmdTest");
            }
        }
        public String CmdExample
        {
            get { return _cmdExample; }
            set
            {
                if (value == _cmdExample)
                    return;

                _cmdExample = value;

                RaisePropertyChanged("CmdExample");
            }
        }
        public void Localize()
        {
            GroupInsertField = AmmLocalization.GetLocalizedString("sqlPlaylist_GroupInsertField");
            GroupOperator = AmmLocalization.GetLocalizedString("sqlPlaylist_GroupOperator");
            GroupTest = AmmLocalization.GetLocalizedString("sqlPlaylist_GroupTest");
            GroupExample = AmmLocalization.GetLocalizedString("sqlPlaylist_GroupExample");

            CmdInsertQuery = AmmLocalization.GetLocalizedString("cmd_Insert");
            CmdInsertOperator = AmmLocalization.GetLocalizedString("cmd_Insert");
            CmdTest = AmmLocalization.GetLocalizedString("cmd_Test");
            CmdExample = AmmLocalization.GetLocalizedString("cmd_Example");
        }
        #endregion // Localization

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

        ~SqlPlaylistViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_dataServiceSongs != null)
            {
                _dataServiceSongs.Dispose();
            }
            if (_dataServiceListItems != null)
            {
                _dataServiceListItems.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
