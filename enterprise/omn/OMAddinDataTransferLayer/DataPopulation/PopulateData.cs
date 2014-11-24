using System;
using System.Collections;
using System.Collections.Generic;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.QueryRenderer;
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.PropertyDetails;
using OManager.DataLayer.QueryParser;


namespace OMAddinDataTransferLayer.DataPopulation
{

	public class PopulateData : MarshalByRefObject, IPopulateData
	{
		
		public long[] ExecuteQueryResults(OMQuery omQuery)
		{
			return RunQuery.ExecuteQuery(omQuery);

		}
		public List<Hashtable> ReturnQueryResults(PagingData pagData, bool refresh, string baseclass, Hashtable attributeList)
		{
			return RunQuery.ReturnResults(pagData, refresh, baseclass, attributeList);
		}

		public ProxyTreeGridRenderer GetTreeGridViewDetails(bool readOnly, long selectedObj, string classname)
		{
			TreeGridViewRenderer item = new RenderHierarcyDetails(readOnly).GetRootObject(selectedObj, classname);
			ProxyTreeGridRenderer proxy = new ProxyTreeGridRenderer().ConvertObject(item );
			return proxy;
		}

		public List<ProxyTreeGridRenderer> TransverseTreeGridViewDetails(bool readOnly,long id, string classname)
		{
			List<TreeGridViewRenderer> list = new RenderHierarcyDetails(readOnly).TraverseObjTree(id, classname);
			return ConvertToProxyTreeGridRendererList(list );
		}

		private List<ProxyTreeGridRenderer> ConvertToProxyTreeGridRendererList(List<TreeGridViewRenderer> list)
		{
            if (list == null)
                return null;
            List<ProxyTreeGridRenderer> proxylist = new List<ProxyTreeGridRenderer>();

			foreach (TreeGridViewRenderer item in list)
			{
				ProxyTreeGridRenderer proxy = new ProxyTreeGridRenderer().ConvertObject(item);
				proxylist.Add(proxy);
			}
			return proxylist;
		}

		public List<ProxyTreeGridRenderer> ExpandTreeGidNode(bool readOnly,object id, bool activate)
		{
			List<TreeGridViewRenderer> list = new dbInteraction().ExpandTreeGidNode(readOnly,id, activate);
			return ConvertToProxyTreeGridRendererList(list);
		}

        public bool CheckIfObjectExists(long id)
        {
            ObjectDetails objDetails = new ObjectDetails(id);
            object obj= objDetails.GetObjById(id);
            if( obj!=null)
                return true ;
            return false;
        }
		public override object InitializeLifetimeService()
		{

			return null;
		} 
		
		
	}
}
