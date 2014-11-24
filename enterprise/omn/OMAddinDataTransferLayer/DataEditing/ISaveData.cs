using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMAddinDataTransferLayer.DataEditing
{
	public interface ISaveData 
	{
		void EditObject(long id, string attribute, object value);
		void SaveObjects(long id, int depth);
		void GetObject(long id);
	    int GetDepth(long obj);
        void AddObjectToObjectCache(long id);
		void RefreshObject(long id, int level);
		void DeleteObject(long id);
        void SetFieldToNull(long id, string fieldName);
	    void SaveCollection(long id, int depth);
	    void SaveCollection(long id, string attribName, object newValue, int offset);
	    bool BackUpData(string filePath );
	    void RemoveObjectsFromCache(long id);
	}
}
