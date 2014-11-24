using System.Collections;
using OManager.BusinessLayer.Login;
namespace OMAddinDataTransferLayer.Connection
{
	public interface IConnection
	{
        string ConnectToDatabase(ConnectionDetails currConnectionDetails, bool customConfig);		
		Hashtable FetchAllStoredClasses();
		Hashtable FetchAllStoredClassesForAssembly();
		Hashtable FetchStoredFields(string nodeName);
		int NoOfClassesInDb();
		long GetFreeSizeOfDb();
		long GetTotalDbSize();
		long DbCreationTime();
		int GetFieldCount(string classname);
		void Closedb();
		bool DbConnectionStatus();
	    bool CheckForClientServer();
	    void SaveIndex(ArrayList fieldnames, string classname, ArrayList indexed, string path, bool customConfig);
	    bool CheckForCustomConfig(); 
	}
}
