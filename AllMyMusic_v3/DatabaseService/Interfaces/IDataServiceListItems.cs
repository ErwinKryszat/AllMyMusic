﻿using System;
using System.Collections.ObjectModel;
using System.IO;

using System.Data;

using AllMyMusic.QueryBuilder;
using AllMyMusic.Settings;

using System.Threading;
using System.Threading.Tasks;

namespace AllMyMusic.DataService
{
    interface IDataServiceListItems 
    {
        ServerType SelectedServerType { get; }

        Task<ObservableCollection<String>> GetListItems(String strSQL);

        Task<ObservableCollection<String>> GetCountries();

        Task<ObservableCollection<String>> GetLanguages();

        Task<ObservableCollection<String>> GetGenres();

        Task<ObservableCollection<String>> GetAlphabet(TreeviewCategory tvCategory);

        Task<ObservableCollection<String>> GetListItemsIntByColumn(String ColumName);

        Task<ObservableCollection<String>> GetListItemsByColumn(String ColumName);

        Task<ObservableCollection<String>> GetStringItemsByAlphabet(String columName, String firstCharacter);

        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();

        void Dispose();
    }
}