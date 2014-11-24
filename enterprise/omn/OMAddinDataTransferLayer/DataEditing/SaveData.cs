using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.DataProcessing;
using OManager.DataLayer.QueryParser;

namespace OMAddinDataTransferLayer.DataEditing
{
    internal class SaveData : MarshalByRefObject, ISaveData
    {
        public void EditObject(long id, string attribute, object value)
        {
            ModifyObjects.EditObjects(id, attribute, value);
        }

        public void SaveObjects(long id,int depth)
        {
            ModifyObjects.SaveObjects(id, depth);


        }

        public void SaveCollection(long id, string attribName, object newValue, int offset)
        {
            ModifyCollections.SaveValues(id, attribName, newValue, offset);


        }
        public void AddObjectToObjectCache(long id)
        {
            object obj=ModifyObjects.GetObjects(id);
            ObjectCache.AddObject(id, obj);


        }
        public void RemoveObjectsFromCache(long id)
        {
           
            ObjectCache.RemoveObject(id);

        }

        public void GetObject(long id)
        {
            ModifyObjects.GetObjects(id);
        }

        public void RefreshObject(long id, int level)
        {
            ModifyObjects.RefreshObject(id, level);
        }

        public void DeleteObject(long id)
        {
            ModifyObjects.DeleteObject(id);
        }

        public void SetFieldToNull(long id, string fieldName)
        {
            dbInteraction.SetFieldToNull(id, fieldName);
        }

        public void SaveCollection(long id, int depth)
        {
            dbInteraction.SaveCollection(id, depth);
        }
        public int GetDepth(long id)
        {
            return dbInteraction.GetDepth(id);
        }
         public bool BackUpData(string filePath )
         {
             return dbInteraction.BackUpDatabase(filePath);
         }

        public override object InitializeLifetimeService()
        {

            return null;
        }
    }
}
