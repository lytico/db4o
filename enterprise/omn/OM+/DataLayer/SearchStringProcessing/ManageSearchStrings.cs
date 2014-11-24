using System;
using System.Collections.Generic;
using System.Linq;
using OManager.BusinessLayer.Login;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using OME.Logging.Common;
using OManager.BusinessLayer.SearchString;
using OManager.DataLayer.Connection;

namespace OManager.DataLayer.SearchStringProcessing
{

    public class ManageSearchStrings
    {

         ConnParams m_connParam;

        public ManageSearchStrings(ConnParams connParam)
        {
            m_connParam = connParam;
        }

        internal void AddSearchStringToList(SeachString strToAdd)
        {
            try
            {
                
                SearchStringList searchStringList = FetchAllSearchStringsForAConnection();
                if (searchStringList == null)
                {
                    searchStringList = new SearchStringList(m_connParam);
                    searchStringList.ListSearchString.Add(strToAdd);
                    searchStringList.TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis();
                    Db4oClient.OMNConnection.Store(searchStringList);
                    Db4oClient.OMNConnection.Commit();
                    return;
                }
                if (searchStringList.ListSearchString.Count < 20)
                {
                    bool checkstr = false;
                    foreach (
                        SeachString str in
                            searchStringList.ListSearchString.Where(
                                str => str.SearchString.Equals(strToAdd.SearchString)))
                    {
                        searchStringList.ListSearchString.Remove(str);
                        strToAdd.Timestamp = DateTime.Now;
                        searchStringList.ListSearchString.Add(strToAdd);
                        Db4oClient.OMNConnection.Delete(str);
                        checkstr = true;
                        break;
                    }
                    if (!checkstr)
                        searchStringList.ListSearchString.Add(strToAdd);
                    Db4oClient.OMNConnection.Store(searchStringList);
                    Db4oClient.OMNConnection.Commit();
                }
                else
                    MaintainTwentySearchString(strToAdd, searchStringList);
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

        private static void MaintainTwentySearchString(SeachString strToAdd, SearchStringList searchStringList)
        {
            bool check = false;
            foreach (
                SeachString str in
                    searchStringList.ListSearchString.Where(str => str.SearchString.Equals(strToAdd.SearchString)))
            {
                searchStringList.ListSearchString.Remove(str);
                strToAdd.Timestamp = DateTime.Now;
                searchStringList.ListSearchString.Add(strToAdd);
                Db4oClient.OMNConnection.Delete(str);
                check = true;
                break;
            }
            if (!check)
            {
                SeachString temp = searchStringList.ListSearchString[searchStringList.ListSearchString.Count - 1];
                searchStringList.ListSearchString.Remove(temp );
                strToAdd.Timestamp = DateTime.Now;
                searchStringList.ListSearchString.Add(strToAdd);
                Db4oClient.OMNConnection.Delete(temp);
            }
            Db4oClient.OMNConnection.Store(searchStringList);
            Db4oClient.OMNConnection.Commit();
        }

        private  SearchStringList FetchAllSearchStringsForAConnection()
        {
            SearchStringList grpSearchStrings = null;
            try
            {
                IQuery query = Db4oClient.OMNConnection.Query();
                query.Constrain(typeof (SearchStringList));
                query.Descend("m_connParam").Descend("m_connection").Constrain(m_connParam.Connection);
                IObjectSet objSet = query.Execute();
                if (objSet.Count != 0)
                    grpSearchStrings = (SearchStringList) objSet.Next();
            }

            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            return grpSearchStrings;
        }

        internal long ReturnTimeWhenSearchStringCreated()
        {
            try
            {
                SearchStringList grpSearchStrings = FetchAllSearchStringsForAConnection();
                if (grpSearchStrings != null)
                    return grpSearchStrings.TimeOfCreation > 0 ? grpSearchStrings.TimeOfCreation : 0;
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
        internal List<string> ReturnStringList()
        {
            List<string> stringList = null;
            try
            {

                SearchStringList groupSearchList = FetchAllSearchStringsForAConnection();
                if (groupSearchList != null)
                {
                    CompareSearchStringTimestamps cmp = new CompareSearchStringTimestamps();
                    groupSearchList.ListSearchString.Sort(cmp);
                    stringList = groupSearchList.ListSearchString.Select(str => str.SearchString).ToList();
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

            return stringList;
        }
        internal void RemovesSearchStringsForAConnection()
        {
            try
            {
                SearchStringList grpSearchString = FetchAllSearchStringsForAConnection();
                
                if (grpSearchString != null)
                {
                    foreach (SeachString sString in grpSearchString.ListSearchString.Where(sString => sString != null))
                    {
                        Db4oClient.OMNConnection.Delete(sString);
                    }
                    grpSearchString.ListSearchString.Clear();
                    grpSearchString.TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis();
                    Db4oClient.OMNConnection.Store(grpSearchString);
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
    public class CompareSearchStringTimestamps : IComparer<SeachString>
    {
        public int Compare(SeachString s1, SeachString s2)
        {

            return s2.Timestamp.CompareTo(s1.Timestamp);
        }
    }
}
