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


using AllMyMusic.DataService;

namespace AllMyMusic.ViewModel
{
    public class QueryComposerViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private IDataServiceListItems _dataServiceListItems;

        private ObservableCollection<String> _compareOperators;
        private ObservableCollection<String> _fieldNames;
        private ObservableCollection<String> _fieldValues;
        private ObservableCollection<String> _alphabet;

        private String _sqlPlaylistQuery;
        private String _selectedCompareOperator;
        private String _selectedFieldName;
        private String _selectedFieldValue;
        private String _selectedAlphabet;

        private Boolean _alphabetVisible;
        #endregion // Fields

        #region Commands
      
        #endregion


        #region Presentation Properties
        public String SqlPlaylistQuery
        {
            get { return _sqlPlaylistQuery; }
            set
            {
                _sqlPlaylistQuery = value;
                RaisePropertyChanged("SqlPlaylistQuery");
            }
        }

        public ObservableCollection<String> CompareOperators
        {
            get { return _compareOperators; }
            set
            {
                _compareOperators = value;
                RaisePropertyChanged("CompareOperators");
            }
        }
        public ObservableCollection<String> FieldNames
        {
            get { return _fieldNames; }
            set
            {
                _fieldNames = value;
                RaisePropertyChanged("FieldNames");
            }
        }
        public ObservableCollection<String> FieldValues
        {
            get { return _fieldValues; }
            set
            {
                _fieldValues = value;
                RaisePropertyChanged("FieldValues");
            }
        }
        public ObservableCollection<String> Alphabet
        {
            get { return _alphabet; }
            set
            {
                _alphabet = value;
                RaisePropertyChanged("Alphabet");
            }
        }

        public String SelectedCompareOperator
        {
            get { return _selectedCompareOperator; }
            set
            {
                if (value == _selectedCompareOperator)
                    return;

                _selectedCompareOperator = value;
                RaisePropertyChanged("SelectedCompareOperator");

                OnSelectedCompareOperatorChanged();
            }
        }
        public String SelectedFieldName
        {
            get { return _selectedFieldName; }
            set
            {
                if (value == _selectedFieldName)
                { 
                    return; 
                }
                else
                {
                    _selectedFieldName = value;

                    Task.Run(() => OnSelectedFieldNameChanged());
                    RaisePropertyChanged("SelectedFieldName");
                }
            }
        }
        public String SelectedFieldValue
        {
            get { return _selectedFieldValue; }
            set
            {
                if (value == _selectedFieldValue)
                    return;

                _selectedFieldValue = value;
                RaisePropertyChanged("SelectedFieldValue");

                OnSelectedFieldValueChanged();
            }
        }
        public String SelectedAlphabet
        {
            get { return _selectedAlphabet; }
            set
            {
                if (value == _selectedAlphabet)
                    return;

                _selectedAlphabet = value;
                RaisePropertyChanged("SelectedAlphabet");

                OnSelectedAlphabetChanged();
            }
        }

        public Boolean AlphabetVisible
        {
            get { return _alphabetVisible; }
            set
            {
                if (value == _alphabetVisible)
                    return;

                _alphabetVisible = value;
                RaisePropertyChanged("AlphabetVisible");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public QueryComposerViewModel(ConnectionInfo conInfo)
        {
            if (conInfo.ServerType == ServerType.SqlServer)
            {
                _dataServiceListItems = new DataServiceListItems_SQL(conInfo);
            }
            if (conInfo.ServerType == ServerType.MySql)
            {
                _dataServiceListItems = new DataServiceListItems_MYSQL(conInfo);
            }
            LoadCompareOperators();
            LoadFieldNames();

            _selectedFieldName = "BandName";
            _selectedCompareOperator = " like ";
            //SelectedAlphabet = "A";
            //SelectedFieldValue = "ABBA";
            Localize();
        }
        #endregion  // Constructor

        #region Public

        #endregion

        #region private helper
        private void LoadCompareOperators()
        {
            String[] compareOperators = { " like ", " not like ", " = ", " != ", " > ", " >= ", " < ", " <= " };

            CompareOperators = new ObservableCollection<string>(compareOperators);
        }
        private void LoadFieldNames()
        {
            String[] fieldNames = new string[]
            {
                "BandName", 
                "AlbumName", 
                "ComposerName", 
                "ConductorName", 
                "LeadPerformerName", 
                "SongName", 

                "Country", 
                "Language", 
                "Genre", 
                "AlbumGenre", 

                "BitRate", 
                "SampleRate",  
                "VariousArtists", 
                "Track", 
                "Rating", 
                "Year"
            };

            FieldNames = new ObservableCollection<string>(fieldNames);

        }


        private async Task OnSelectedFieldNameChanged()
        {
            _alphabet = new ObservableCollection<string>();
            if (_selectedFieldName == "BandName")
            {
                Alphabet = await _dataServiceListItems.GetAlphabet(TreeviewCategory.Band);
            }
            else if (_selectedFieldName == "AlbumName")
            {
                Alphabet = await _dataServiceListItems.GetAlphabet(TreeviewCategory.Album);
            }
            else if (_selectedFieldName == "ComposerName")
            {
                Alphabet = await _dataServiceListItems.GetAlphabet(TreeviewCategory.Composer);
            }
            else if (_selectedFieldName == "ConductorName")
            {
                Alphabet = await _dataServiceListItems.GetAlphabet(TreeviewCategory.Conductor);
            }
            else if (_selectedFieldName == "LeadPerformerName")
            {
                Alphabet = await _dataServiceListItems.GetAlphabet(TreeviewCategory.LeadPerformer);
            }
            else if (_selectedFieldName == "SongName")
            {
                Alphabet = await _dataServiceListItems.GetAlphabet(TreeviewCategory.SongName);
            }

            if (Alphabet.Count == 0)
            {
                if ((_selectedFieldName == "SampleRate") || (_selectedFieldName == "BitRate") || (_selectedFieldName == "Track") || (_selectedFieldName == "Rating") || (_selectedFieldName == "VariousArtists"))
                {
                    FieldValues = await _dataServiceListItems.GetListItemsIntByColumn(_selectedFieldName);
                    AlphabetVisible = false;
                }
                else if ((_selectedFieldName == "Country") || (_selectedFieldName == "Language") || (_selectedFieldName == "Genre") || (_selectedFieldName == "AlbumGenre") || (_selectedFieldName == "Year"))
                {
                    FieldValues = await _dataServiceListItems.GetListItemsByColumn(_selectedFieldName);
                    AlphabetVisible = false;
                }
            }
            else
            {
                AlphabetVisible = true;
            }
           

            AssembleString();

            if (String.IsNullOrEmpty(_selectedAlphabet) == false)
            {
                OnSelectedAlphabetChanged();
            }
        }
        private void OnSelectedCompareOperatorChanged()
        {
            AssembleString();
        }
        private void OnSelectedFieldValueChanged()
        {
            AssembleString();
        }
        private void OnSelectedAlphabetChanged()
        {
            Task.Run(async () =>
            {
                if (String.IsNullOrEmpty(_selectedFieldName.Trim()) == false)
                {
                    FieldValues = await _dataServiceListItems.GetStringItemsByAlphabet(_selectedFieldName, _selectedAlphabet);
                }
                AssembleString();
            });
            
        }

        private void AssembleString()
        {
            if ( (String.IsNullOrEmpty(_selectedFieldName) == false) && (String.IsNullOrEmpty(_selectedCompareOperator) == false) && (String.IsNullOrEmpty(_selectedFieldValue) == false) )
            {
                Int32 pos = _selectedFieldValue.IndexOf("'");
                if (pos >= 0)
                {
                    SelectedFieldValue = _selectedFieldValue.Substring(0, pos);
                    MessageBox.Show("The \"'\" is not allowed in this textbox");
                }
                _sqlPlaylistQuery = _selectedFieldName + _selectedCompareOperator + "'" + _selectedFieldValue + "'";
            }
            else
            {
                _sqlPlaylistQuery = String.Empty;
            }
        }
        #endregion

        #region Localization
        private String _tooltip_Fieldname = "Select the field that identifies the Songs you are searching";
        private String _tooltip_CompareOperators = "Select the operation that includes or excludes the Songs you are searching";
        private String _tooltip_Alphabet = "Select the value that identifies the Songs you are searching";
        private String _tooltip_FieldValues = "Select the value that identifies the Songs you are searching";

        public String Tooltip_Fieldname
        {
            get { return _tooltip_Fieldname; }
            set
            {
                if (value == _tooltip_Fieldname)
                    return;

                _tooltip_Fieldname = value;

                RaisePropertyChanged("Tooltip_Fieldname");
            }
        }
        public String Tooltip_CompareOperators
        {
            get { return _tooltip_CompareOperators; }
            set
            {
                if (value == _tooltip_CompareOperators)
                    return;

                _tooltip_CompareOperators = value;

                RaisePropertyChanged("Tooltip_CompareOperators");
            }
        }
        public String Tooltip_Alphabet
        {
            get { return _tooltip_Alphabet; }
            set
            {
                if (value == _tooltip_Alphabet)
                    return;

                _tooltip_Alphabet = value;

                RaisePropertyChanged("Tooltip_Alphabet");
            }
        }
        public String Tooltip_FieldValues
        {
            get { return _tooltip_FieldValues; }
            set
            {
                if (value == _tooltip_FieldValues)
                    return;

                _tooltip_FieldValues = value;

                RaisePropertyChanged("Tooltip_FieldValues");
            }
        }
        public void Localize()
        {
            Tooltip_Fieldname = AmmLocalization.GetLocalizedString("queryComposer_Tooltip_Fieldname");
            Tooltip_CompareOperators = AmmLocalization.GetLocalizedString("queryComposer_Tooltip_CompareOperators");
            Tooltip_Alphabet = AmmLocalization.GetLocalizedString("queryComposer_Tooltip_Alphabet");
            Tooltip_FieldValues = AmmLocalization.GetLocalizedString("queryComposer_Tooltip_FieldValues");
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

        ~QueryComposerViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
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
