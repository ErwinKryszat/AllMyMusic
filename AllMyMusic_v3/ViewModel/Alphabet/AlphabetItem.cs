using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic.ViewModel
{
    public class AlphabetItem : ViewModelBase
    {
        private String _character = String.Empty;
        private Boolean _isSelected = false;

        public String Character
        {
            get { return _character; }
            set
            {
                if (value == _character)
                    return;

                _character = value;

                RaisePropertyChanged("Character");
            }
        }
        public Boolean IsSelected 
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                RaisePropertyChanged("IsSelected");
            }
        }
        public AlphabetItem(String character)
        {
            _character = character;
        }
    }
}
