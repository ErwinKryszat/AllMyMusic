using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AllMyMusic
{
    public class CountryItem : INotifyPropertyChanged 
    {
        private String abbreviation;
        private String _country = String.Empty;
        private Int32 _countryId;
        private String _flagPath;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged(string propertyName, object propertyValue)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool SetProperty<T>(ref T storage, T value, String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName, value);
            return true;
        }

        

        public String Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public Int32 CountryId
        {
            get { return _countryId; }
            set { _countryId = value; }
        }

        
        public String Abbreviation
        {
            get { return abbreviation; }
            set { abbreviation = value; }
        }

        
        public String FlagPath
        {
            get { return _flagPath; }
            set { SetProperty(ref _flagPath, value, "FlagPath"); }
        }

        public CountryItem()
        {
            _country = String.Empty;
            _countryId = 0;
        }

        public CountryItem(String country, Int32 countryId)
        {
            _country = country;
            _countryId = countryId;
        }
    }
}
