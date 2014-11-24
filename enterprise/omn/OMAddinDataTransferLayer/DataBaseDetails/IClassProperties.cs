using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMAddinDataTransferLayer.DataBaseDetails
{
	public interface IClassProperties
	{
		ClassProperties GetClassProperties(string className);
		int GetObjectCountForAClass(string classname);
		void SetIndexedConfiguration(ArrayList fieldname, string className, ArrayList isIndexed, string dbPath, bool customConfig);
	}
}
