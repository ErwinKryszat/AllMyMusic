using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Threading;

using AllMyMusic.DataService;
using AllMyMusic.Settings;
using AllMyMusic.WebServices;

namespace AllMyMusic.ViewModel
{
    public class CountrySelectorViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ObservableCollection<CountryItem> _countryItems;
        private CountryItem _selectedCountry;
        private String _abbreviation;
        private RelayCommand<object> _searchHarddisk;
        private RelayCommand<object> _searchInternet;
        private RelayCommand<object> _cmdOK;
        private RelayCommand<object> _cmdCancel;
        private bool? _dialogResult;
        #endregion // Fields

        #region Commands
        public ICommand SearchHarddiskCommand
        {
            get
            {
                if (null == _searchHarddisk)
                    _searchHarddisk = new RelayCommand<object>(ExecuteSearchHarddiskCommand, CanSearchHarddiskCommand);

                return _searchHarddisk;
            }
        }
        private void ExecuteSearchHarddiskCommand(object notUsed)
        {
            GetFlaggFromHarddisk();
        }
        private bool CanSearchHarddiskCommand(object notUsed)
        {
            return true;
        }

        public ICommand SearchInternetCommand
        {
            get
            {
                if (null == _searchInternet)
                    _searchInternet = new RelayCommand<object>(ExecuteSearchInternetCommand, CanSearchInternetCommand);

                return _searchInternet;
            }
        }
        private void ExecuteSearchInternetCommand(object notUsed)
        {
            GetFlaggFromInternet();
        }
        private bool CanSearchInternetCommand(object notUsed)
        {
            return true;
        }

        public ICommand OKCommand
        {
            get
            {
                if (null == _cmdOK)
                    _cmdOK = new RelayCommand<object>(ExecuteOKCommand, CanOKCommand);

                return _cmdOK;
            }
        }
        private void ExecuteOKCommand(object notUsed)
        {
            DialogResult = true;
        }
        private bool CanOKCommand(object notUsed)
        {
            return true;
        }

        public ICommand CancelCommand
        {
            get
            {
                if (null == _cmdCancel)
                    _cmdCancel = new RelayCommand<object>(ExecuteCancelCommand, CanCancelCommand);

                return _cmdCancel;
            }
        }
        private void ExecuteCancelCommand(object notUsed)
        {
            DialogResult = false;
        }
        private bool CanCancelCommand(object notUsed)
        {
            return true;
        }
        #endregion


        #region Presentation Properties

        public ObservableCollection<CountryItem> CountryItems
        {
            get { return _countryItems; }
            set
            {
                _countryItems = value;
                RaisePropertyChanged("CountryItems");
            }
        }

        public CountryItem SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (value == _selectedCountry)
                    return;

                _selectedCountry = value;
                OnSelectedCountryChanged();
                RaisePropertyChanged("SelectedCountry");
            }
        }
        public String Abbreviation
        {
            get { return _abbreviation; }
            set
            {
                if (value == _abbreviation)
                    return;

                _abbreviation = value;
                RaisePropertyChanged("Abbreviation");
            }
        }

        public bool? DialogResult
        {
            get { return _dialogResult; }
            set
            {
                if (value == _dialogResult)
                    return;

                _dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public CountrySelectorViewModel(CountryItem country)
        {
            LoadInternationalCountryNames(country);

            Localize();
        }
        #endregion  // Constructor


        #region private
        private void OnSelectedCountryChanged()
        {
            Abbreviation = _selectedCountry.Abbreviation;
        }
        private void LoadInternationalCountryNames(CountryItem country)
        {
            _countryItems = new ObservableCollection<CountryItem>();

            CountryCollection WorldList = new CountryCollection(Global.WorldCountriesFile);

            for (int i = 0; i < WorldList.Count; i++)
            {
                _countryItems.Add(WorldList[i]);

                if (String.IsNullOrEmpty(country.Abbreviation) == false)
                {
                    if (_countryItems[i].Abbreviation == country.Abbreviation)
                    {
                        SelectedCountry = _countryItems[i];
                    }
                }
                else
                {
                    if (_countryItems[i].Country == country.Country)
                    {
                        SelectedCountry = _countryItems[i];
                    }
                }
            }

            RaisePropertyChanged("InterantionalCountryNames");
        }
        private void GetFlaggFromInternet()
        {
            // http://www.crwflags.com/fotw/images/a/ao.gif
            if (SelectedCountry.Abbreviation != "")
            {
                String AlphabetChar = SelectedCountry.Abbreviation.Substring(0, 1).ToLower();
                String remoteURL = Global.FlagDownloadURL
                    + AlphabetChar + "/"
                    + SelectedCountry.Abbreviation.ToLower() + ".gif";

                String filePath = Global.FlagsPath + "\\" + SelectedCountry.Abbreviation + ".gif";

                WebDownload.DownloadFile(remoteURL, filePath);

                if (File.Exists(filePath))
                {
                    SelectedCountry.FlagPath = filePath;
                }
            }
        }
        private void GetFlaggFromHarddisk()
        {
            Microsoft.Win32.OpenFileDialog dlgFlagPath = new Microsoft.Win32.OpenFileDialog();

            dlgFlagPath.DefaultExt = ".gif";
            dlgFlagPath.InitialDirectory = Global.FlagsPath;
            dlgFlagPath.Title = AmmLocalization.GetLocalizedString("frmCountries_BrowseFlag");
            dlgFlagPath.FileName = _abbreviation;
            dlgFlagPath.ShowDialog();
            dlgFlagPath.ValidateNames = true;
            dlgFlagPath.Multiselect = false;

            if (File.Exists(dlgFlagPath.FileName))
            {
                SelectedCountry.FlagPath = dlgFlagPath.FileName;
            }
        }
        #endregion
        #region Localization

        private String _labelInternationalCountryName;
        private String _labelCountryAbbreviation;

        private String _cmd_Harddisk;
        private String _cmd_Internet;


        public String LabelInternationalCountryName
        {
            get { return _labelInternationalCountryName; }
            set
            {
                if (value == _labelInternationalCountryName)
                    return;

                _labelInternationalCountryName = value;

                RaisePropertyChanged("LabelInternationalCountryName");
            }
        }
        public String LabelCountryAbbreviation
        {
            get { return _labelCountryAbbreviation; }
            set
            {
                if (value == _labelCountryAbbreviation)
                    return;

                _labelCountryAbbreviation = value;

                RaisePropertyChanged("LabelCountryAbbreviation");
            }
        }
        public String Cmd_Harddisk
        {
            get { return _cmd_Harddisk; }
            set
            {
                if (value == _cmd_Harddisk)
                    return;

                _cmd_Harddisk = value;

                RaisePropertyChanged("Cmd_Harddisk");
            }
        }
        public String Cmd_Internet
        {
            get { return _cmd_Internet; }
            set
            {
                if (value == _cmd_Internet)
                    return;

                _cmd_Internet = value;

                RaisePropertyChanged("Cmd_Internet");
            }
        }
       

        public void Localize()
        {
            LabelInternationalCountryName = AmmLocalization.GetLocalizedString("frmCountrySelector_Label1");
            LabelCountryAbbreviation = AmmLocalization.GetLocalizedString("frmCountrySelector_Label2");
            Cmd_Harddisk = AmmLocalization.GetLocalizedString("frmCountrySelector_cmdFlagHarddisk");
            Cmd_Internet = AmmLocalization.GetLocalizedString("frmCountrySelector_cmdFlagInternet");
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

        ~CountrySelectorViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
           
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}
