using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using System.Collections;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.UIHelper;
using OME.Logging.Common;

namespace OManager.DataLayer.QueryParser
{
    public class RunQuery
    {
    	public static long[] ExecuteQuery(OMQuery query)
        {
            QueryParser qParser = new QueryParser(query);
            IObjectSet objSet = qParser.ExecuteOMQueryList();
            
			return objSet != null ? objSet.Ext().GetIDs() : null;
        }

        public static List<Hashtable> ReturnResults(PagingData pgData, bool refresh,string baseclass,Hashtable AttributeList)
        {
            try
            {
            	if (pgData.ObjectId.Count <= 0)
            		return null;

            	IObjectsetConverter objSetConvertor = new IObjectsetConverter(baseclass, refresh);
            	return objSetConvertor.ObjectIDToUIObjects(pgData, AttributeList);
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }
    }
}
