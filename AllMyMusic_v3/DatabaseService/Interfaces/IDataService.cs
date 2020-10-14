using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AllMyMusic_v3.DataService
{
    interface IDataService 
    {
        void ChangeDatabase(ConnectionInfo conInfo);

        void Close();
    }
}