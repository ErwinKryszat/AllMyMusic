using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AllMyMusic.DataService
{
    interface IDataService 
    {
        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();
    }
}