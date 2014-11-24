using System;
using System.Collections.Generic;
using System.Linq;
using OManager.BusinessLayer.FavFolder;
using OManager.BusinessLayer.Login;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using OME.Logging.Common;
using OManager.DataLayer.Connection;

namespace OManager.DataLayer.FavFolderProcessing
{
    public class ManageFavouriteFolder
    {
        ConnParams m_connParam;

        public ManageFavouriteFolder(ConnParams connParam)
        {
            m_connParam = connParam;
        }
        internal void AddFolderToDatabase(FavouriteFolder favFolder)
        {
            try
            {
                FavouriteFolderList favList = FetchAllFavouritesForAConnection();
                if (favList == null)
                {
                    favList = new FavouriteFolderList(m_connParam)
                                  {TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis()};
                    favList.lstFavFolder.Add(favFolder);
                    Db4oClient.OMNConnection.Store(favList);
                    Db4oClient.OMNConnection.Commit();
                }
                else
                {
                    bool check = false;
                    foreach (
                        FavouriteFolder str in
                            favList.lstFavFolder.Where(str => str != null && str.FolderName.Equals(favFolder.FolderName))
                        )
                    {
                        favList.lstFavFolder.Remove(str);
                        favList.lstFavFolder.Add(favFolder);
                        Db4oClient.OMNConnection.Delete(str);
                        check = true;
                        break;
                    }
                    if (!check)
                        favList.lstFavFolder.Add(favFolder);
                }
                Db4oClient.OMNConnection.Store(favList);
                Db4oClient.OMNConnection.Commit();
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
        }


        internal void RemoveFolderfromDatabase(FavouriteFolder favFolder)
        {
            try
            {
                FavouriteFolderList favList = FetchAllFavouritesForAConnection();
                foreach (FavouriteFolder str in favList.lstFavFolder.Where(str => str.FolderName.Equals(favFolder.FolderName)))
                {
                    favList.lstFavFolder.Remove(str);
                    Db4oClient.OMNConnection.Delete(str);
                    Db4oClient.OMNConnection.Store(favList);
                    Db4oClient.OMNConnection.Commit();
                    break;
                }
            }
            
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);  
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
        }


        internal void RenameFolderInDatabase(FavouriteFolder oldFavFolder, FavouriteFolder newFavFolder)
        {
            try
            {
             
                FavouriteFolderList favList = FetchAllFavouritesForAConnection();
                if (favList == null)
                    return;
                foreach (FavouriteFolder str in favList.lstFavFolder.Where(str => str != null && str.FolderName.Equals(oldFavFolder.FolderName)))
                {
                    favList.lstFavFolder.Remove(str);
                    Db4oClient.OMNConnection.Delete(str);
                    str.FolderName = newFavFolder.FolderName;
                    favList.lstFavFolder.Add(str);
                    Db4oClient.OMNConnection.Store(favList);
                    Db4oClient.OMNConnection.Commit();
                    break;
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
        }

        internal FavouriteFolder FindFolderWithClassesByFolderName(string folderName)
        {
            FavouriteFolder favFolder = null;
            try
            {

                IQuery query = Db4oClient.OMNConnection.Query();
                query.Constrain(typeof(FavouriteFolderList));
                query.Descend("m_connParam").Descend("m_connection").Constrain(m_connParam.Connection);
                IQuery query1 = query.Descend("m_lstFavfolder" ).Descend("_items" );
                query1.Constrain(typeof (FavouriteFolder));
                query1.Descend("m_folderName").Constrain(folderName);
                IObjectSet objSet = query1.Execute();
                if (objSet.Count != 0)
                {
                    favFolder = objSet.Next() as FavouriteFolder;
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
            return favFolder;
        }
      

        private FavouriteFolderList FetchAllFavouritesForAConnection()
        {

            try
            {
                IQuery query = Db4oClient.OMNConnection.Query();
                query.Constrain(typeof (FavouriteFolderList));
                query.Descend("m_connParam").Descend("m_connection").Constrain(m_connParam.Connection);
                IObjectSet  objSet = query.Execute();
                if (objSet != null && objSet.Count != 0)
                {
                    return (FavouriteFolderList) objSet.Next();
                }

            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            return null;
        }

        internal List<FavouriteFolder> ReturnFavouritFolderList()
        {
            List<FavouriteFolder> FavList = null;
            try
            {

                FavouriteFolderList Fav = FetchAllFavouritesForAConnection();
                if (Fav != null)
                {
                    FavList = Fav.lstFavFolder;                    
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
            return FavList;
        }

        internal long ReturnTimeWhenFavouriteListCreated()
        {
            FavouriteFolderList Fav = null;

            try
            {

                Fav = FetchAllFavouritesForAConnection();
                if (Fav != null)
                    return Fav.TimeOfCreation > 0 ? Fav.TimeOfCreation : 0;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
            return 0;
        }
        internal void RemoveFavouritFolderForAConnection()
        {

            try
            {
                FavouriteFolderList Fav = FetchAllFavouritesForAConnection();
                if (Fav != null)
                {
                   foreach (FavouriteFolder favFolder in Fav.lstFavFolder.Where(favFolder => favFolder != null))
                   {
                       Db4oClient.OMNConnection.Delete(favFolder);
                   }
                    Fav.lstFavFolder.Clear();
                    Fav.TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis();
                    Db4oClient.OMNConnection.Delete(Fav);
                    Db4oClient.OMNConnection.Store(Fav);
                    Db4oClient.OMNConnection.Commit();
                }
                
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }


        }

    }

   
}
