using System;
using System.Collections.ObjectModel;


namespace AllMyMusic.DataService
{
    interface IDatabasesServerManagement 
    {
        ConnectionInfo DatabaseConnectionInformation
        { get;  set; }

        ServerType SelectedServerType
        { get; set; }

        void ConnectServer(ConnectionInfo dbCI);
       
        Boolean ConnectDatabase(ConnectionInfo dbCI, Boolean _silent);
       
        Boolean TestConnection(ConnectionInfo dbCI);
       
        ObservableCollection<String> GetUserDatabases();
       
        Boolean CreateDatabase(ConnectionInfo dbCI);
       
        Boolean PurgeDatabase(ConnectionInfo dbCI);
        
        Boolean DeleteDatabase(ConnectionInfo dbCI);
        
        void Close();

        void Dispose();
    }
}
