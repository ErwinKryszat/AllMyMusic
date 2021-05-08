

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllMyMusic.ViewModel
{
    public class PagerViewModel : ViewModelBase
    {
        #region Fields
        private Int32 _page;
        private Int32 _pages;
        private Int32 _itemsPerPage;
        private Int32 _itemCount;
        private Int32 _startIndex;
        private Int32 _endIndex;
        private Boolean _pagerVisisble;
        private String _labelItems;
        private String _itemsName;
        private Boolean _showAllItems;

        private RelayCommand<object> _firstPageCommand;
        private RelayCommand<object> _prevPageCommand;
        private RelayCommand<object> _nextPageCommand;
        private RelayCommand<object> _lastPageCommand;
        private RelayCommand<object> _showAllCommand;
        #endregion // Fields

        #region Presentation Properties
        public String LabelPages
        {
            get { return "Page " + _page.ToString() + "/" + _pages.ToString(); }
        }

        public String LabelItems
        {
            get { return _labelItems; }
        }

        public Int32 Page
        {
            get { return _page; }
        }

        public Int32 Pages
        {
            get { return _pages; }
        }
        
        public Int32 ItemCount
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;
                RaisePropertyChanged("ItemCount");
            }
        }

        public Int32 ItemsPerPage
        {
            get { return _itemsPerPage; }
            set 
            { 
                _itemsPerPage = value;
                RaisePropertyChanged("ItemsPerPage");
            }
        }

        public Int32 StartIndex
        {
            get { return _startIndex; }
        }

        public Int32 EndIndex
        {
            get { return _endIndex; }
        }

        public Boolean PagerVisisble
        {
            get { return _pagerVisisble; }
        }

        public Boolean ShowAllItems
        {
            get { return _showAllItems; }
            set
            {
                _showAllItems = value;
                RaisePropertyChanged("ShowAllItems");
                RaisePropertyChanged("ShowPageNumber");
            }
        }
        public Boolean ShowPageNumber
        {
            get { return !_showAllItems; }
        }
        #endregion // Presentation Properties

        #region Commands
        public ICommand FirstPageCommand
        {
            get
            {
                if (null == _firstPageCommand)
                    _firstPageCommand = new RelayCommand<object>(ExecuteFirstPageCommand, CanFirstPageCommand);

                return _firstPageCommand;
            }
        }
        private void ExecuteFirstPageCommand(object notUsed)
        {
            _page = 1;
            OnPageChanged(this, new EventArgs());
        }
        private bool CanFirstPageCommand(object notUsed)
        {
            return ((Page > 1) && (_showAllItems == false));
        }

        public ICommand PrevPageCommand
        {
            get
            {
                if (null == _prevPageCommand)
                    _prevPageCommand = new RelayCommand<object>(ExecutePrevPageCommand, CanPrevPageCommand);

                return _prevPageCommand;
            }
        }
        private void ExecutePrevPageCommand(object notUsed)
        {
            _page--;
            OnPageChanged(this, new EventArgs());
        }
        private bool CanPrevPageCommand(object notUsed)
        {
            return ((Page > 1)  && (_showAllItems == false));
        }

        public ICommand NextPageCommand
        {
            get
            {
                if (null == _nextPageCommand)
                    _nextPageCommand = new RelayCommand<object>(ExecuteNextPageCommand, CanNextPageCommand);

                return _nextPageCommand;
            }
        }
        private void ExecuteNextPageCommand(object notUsed)
        {
            _page++;
            OnPageChanged(this, new EventArgs());
        }
        private bool CanNextPageCommand(object notUsed)
        {
            return ((_itemCount > _itemsPerPage) && ((_page * _itemsPerPage) < _itemCount)  && (_showAllItems == false));
        }

        public ICommand LastPageCommand
        {
            get
            {
                if (null == _lastPageCommand)
                    _lastPageCommand = new RelayCommand<object>(ExecuteLastPageCommand, CanLastPageCommand);

                return _lastPageCommand;
            }
        }
        private void ExecuteLastPageCommand(object notUsed)
        {
            _page = _pages;
            OnPageChanged(this, new EventArgs());
        }
        private bool CanLastPageCommand(object notUsed)
        {
            return ((_page < _pages) && (_showAllItems == false));
        }

        public ICommand ShowAllCommand
        {
            get
            {
                if (null == _showAllCommand)
                    _showAllCommand = new RelayCommand<object>(ExecuteShowAllCommand, CanShowAllCommand);

                return _showAllCommand;
            }
        }
        private void ExecuteShowAllCommand(object notUsed)
        {
            ShowAll();
            OnPageChanged(this, new EventArgs());
        }
        private bool CanShowAllCommand(object notUsed)
        {
            return (_itemCount <= 250);
        }

        #endregion // Commands

        #region Constructor
        public PagerViewModel()
        {

        }
        public PagerViewModel(String itemsName, Int32 itemCount, Int32 itemsPerPage)
        {
            this._itemsName = itemsName;
            this._itemCount = itemCount;
            this._itemsPerPage = itemsPerPage;
            this._page = 1;

            OnItemCountChanged();
        }
        
        #endregion  // Constructor
        private void OnItemCountChanged()
        {
            this._pages = (Int32)Math.Ceiling((double)_itemCount / (double)_itemsPerPage);
            if (_itemCount < _itemsPerPage)
            {
                _pagerVisisble = false;
            }
            else
            {
                _pagerVisisble = true;
            }
            CalculateIndex();
        }
        private void CalculateIndex()
        {
            _startIndex = ((_page - 1) * _itemsPerPage);
            _endIndex = Math.Min(((_page * _itemsPerPage) - 1), _itemCount - 1);
            _labelItems = _itemsName + " " + (_startIndex + 1).ToString() + "-" + (_endIndex + 1).ToString();
        }


        private Int32 _itemsPerPageMemorized = 0;
        private Int32 _pageMemorized = 1;
        private void ShowAll()
        {
            ShowAllItems = !ShowAllItems;

            if (_showAllItems == true)
            {
                _itemsPerPageMemorized = _itemsPerPage;
                _pageMemorized = _page;

                _page = 1;
                _itemsPerPage = _itemCount;
            }
            else
            {
                _itemsPerPage = _itemsPerPageMemorized;
                _page = _pageMemorized;
            }
        }
        #region Events
        public delegate void PageChangedEventHandler(object sender, EventArgs e);
        public event PageChangedEventHandler PageChanged;
        protected virtual void OnPageChanged(object sender, EventArgs e)
        {
            CalculateIndex();

            RaisePropertyChanged("LabelPages");
            RaisePropertyChanged("LabelItems");
            RaisePropertyChanged("Page");
            RaisePropertyChanged("StartIndex");
            RaisePropertyChanged("EndIndex");

            if (this.PageChanged != null)
            {
                this.PageChanged(this, e);
            }
        }
        #endregion // Events
    }
}
