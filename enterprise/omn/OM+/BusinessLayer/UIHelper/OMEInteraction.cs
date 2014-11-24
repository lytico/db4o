using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OManager.BusinessLayer.FavFolder;
using OManager.BusinessLayer.Login;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.SearchString;
using OManager.DataLayer.Connection;
using OManager.DataLayer.FavFolderProcessing;
using OManager.DataLayer.SearchStringProcessing;

namespace OManager.BusinessLayer.UIHelper
{
    public class OMEInteraction
    {
        public static ConnParams GetCurrentConnParams()
        {
            return Db4oClient.CurrentConnParams; 
        }
        public static ConnectionDetails GetRefreshedCurrentRecentConnection()
        {
            ManageConnectionDetails manage = new ManageConnectionDetails(GetCurrentConnParams());
            return manage.GetConnectionDetails();
        }

        public static void SetCurrentRecentConnection(ConnParams  conn)
        {
            Db4oClient.CurrentConnParams = conn;
        }

        public static void SaveRecentConnection(ConnectionDetails connDetails)
        {
            ManageConnectionDetails manage = new ManageConnectionDetails(GetCurrentConnParams());
            manage.SaveConnectionDetails(connDetails);
        }
        public static void CloseOMEdb()
        {
            Db4oClient.CloseRecentConnectionFile();
        }
        public static long ChkIfRecentConnIsInDb()
        {
         ManageConnectionDetails manage = new ManageConnectionDetails(GetCurrentConnParams());
            return manage.ChkIfRecentConnIsInDb();
        }
        public static long ChkIfRecentConnIsInDb(ConnParams connectionDetails)
        {

            ManageConnectionDetails manage = new ManageConnectionDetails(connectionDetails);
            return manage.ChkIfRecentConnIsInDb();
        }
        public static ConnectionDetails GetConnectionDetailsObject(long id)
        {
            ConnectionDetails connectionDetails= Db4oClient.OMNConnection.Ext().GetByID(id) as ConnectionDetails;
            Db4oClient.OMNConnection.Ext().Activate(connectionDetails, 3);
            return connectionDetails;
        }

        public static void RemoveFavFolder()
        {
            ManageFavouriteFolder favouriteList = new ManageFavouriteFolder(GetCurrentConnParams());
            favouriteList.RemoveFavouritFolderForAConnection();
        }

        public static void RemoveSearchString()
        {
            ManageSearchStrings SearchStringList = new ManageSearchStrings(GetCurrentConnParams());
            SearchStringList.RemovesSearchStringsForAConnection();
        }

        public static void RemoveRecentQueries()
        {
            
            ManageConnectionDetails manage = new ManageConnectionDetails(GetCurrentConnParams());
            manage.DeleteRecentQueriesForAConnection();
        }

        public static void SaveFavourite(FavouriteFolder favFolder)
        {
            ManageFavouriteFolder favouriteList = new ManageFavouriteFolder(GetCurrentConnParams());
            favouriteList.AddFolderToDatabase(favFolder);
        }

        public static void UpdateFavourite(FavouriteFolder favFolder)
        {
            ManageFavouriteFolder favouriteList = new ManageFavouriteFolder(GetCurrentConnParams());
            favouriteList.RemoveFolderfromDatabase(favFolder);
        }

        public static void RenameFolderInDatabase(FavouriteFolder oldFav, FavouriteFolder newFav)
        {
            ManageFavouriteFolder lstFav = new ManageFavouriteFolder(GetCurrentConnParams());
            lstFav.RenameFolderInDatabase(oldFav, newFav);
        }


        public static FavouriteFolder GetFolderfromDatabaseByFoldername(string folderName)
        {
            ManageFavouriteFolder manageFav = new ManageFavouriteFolder(GetCurrentConnParams());
            return manageFav.FindFolderWithClassesByFolderName(folderName);
          
        }

        public static List<string> GetSearchString()
        {
            ManageSearchStrings searchStrings = new ManageSearchStrings(GetCurrentConnParams());
            return searchStrings.ReturnStringList();
        }

        public static void SaveSearchString(SeachString searchString)
        {
            ManageSearchStrings searchStrings = new ManageSearchStrings(GetCurrentConnParams());
                searchStrings.AddSearchStringToList(searchString);
        }

        public static List<FavouriteFolder> GetFavourites()
        {
            ManageFavouriteFolder lstFav = new ManageFavouriteFolder(GetCurrentConnParams());
            return lstFav.ReturnFavouritFolderList();
        }

        public static long GetTimeforFavCreation()
        {
            ManageFavouriteFolder lstFav = new ManageFavouriteFolder(GetCurrentConnParams());
            return lstFav.ReturnTimeWhenFavouriteListCreated();
        }

        public static long GetTimeforSearchStringCreation()
        {
            ManageSearchStrings lstsearchstring = new ManageSearchStrings(GetCurrentConnParams());
            return lstsearchstring.ReturnTimeWhenSearchStringCreated();
        }

        public static long GetTimeforRecentQueriesCreation()
        {
          
            ManageConnectionDetails manage = new ManageConnectionDetails(GetCurrentConnParams());
            return manage.ReturnTimeWhenRecentQueriesCreated();
        }
        public static void AddQueriesToList(OMQuery query)
        {
           
            ManageConnectionDetails manage = new ManageConnectionDetails(GetCurrentConnParams());
            manage.AddQueryToList(query);
        }
        public static void DeleteConfigConnection(string path, ConnParams connnection)
        {
          
            ManageConnectionDetails manage = new ManageConnectionDetails(connnection);
            manage.RemoveCustomConfigPath(path); 
        }

        public static List<ConnectionDetails> FetchAllConnections(bool checkRemote)
        {
            ManageConnectionDetails manage = new ManageConnectionDetails(GetCurrentConnParams());
            return manage.GetAllConnections(checkRemote);
        }

        public static void SetProxyInfo(ProxyAuthentication proxyInfo)
		{
			ProxyAuthenticator proxyAuth = new ProxyAuthenticator();
			proxyAuth.AddProxyInfoToDb(proxyInfo);
		}

		public static ProxyAuthentication RetrieveProxyInfo()
		{
			ProxyAuthenticator proxyAuth = new ProxyAuthenticator();
			proxyAuth = proxyAuth.ReturnProxyAuthenticationInfo();
			if (proxyAuth != null)
				return proxyAuth.ProxyAuthObj;
			
			return null;
		}
    }
}
