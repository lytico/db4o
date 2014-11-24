using System;
using System.Collections;
using System.Collections.Generic;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.UIHelper;

namespace OMAddinDataTransferLayer.DataPopulation
{
	public interface IPopulateData
	{
		long[] ExecuteQueryResults(OMQuery omQuery);
		List<Hashtable> ReturnQueryResults(PagingData pagData, bool refresh, string baseclass, Hashtable attributeList);
		ProxyTreeGridRenderer GetTreeGridViewDetails(bool readOnly,long id, string classname);
		List<ProxyTreeGridRenderer> TransverseTreeGridViewDetails(bool readOnly,long id, string classname);
		List<ProxyTreeGridRenderer> ExpandTreeGidNode(bool readOnl,object id, bool activate);
        bool CheckIfObjectExists(long id);
	}
}
