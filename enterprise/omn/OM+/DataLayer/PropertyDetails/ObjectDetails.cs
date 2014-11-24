using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.Business.Config;
using OManager.DataLayer.CommonDatalayer;
using OManager.DataLayer.Connection;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;
using System.Collections;

namespace OManager.DataLayer.PropertyDetails
{
    public class ObjectDetails 
    {
        private readonly IObjectContainer objectContainer;

		int level;
        private readonly object m_genObject;
    	private long id;
		private readonly List<object> m_listforObjects = new List<object>();

        public ObjectDetails(long id)
        {
        	this.id = id;
			objectContainer = Db4oClient.Client;
        	m_genObject = objectContainer.Ext().GetByID(id);
        	objectContainer.Ext().Activate(m_genObject, 1);
           
        }
		public ObjectDetails(object obj)
		{
			m_genObject = obj;
		}

        public string GetUUID()
        {
            try
            {
				
                IObjectInfo objInfo = objectContainer.Ext().GetObjectInfo(m_genObject);
                if (objInfo != null)
                {
                    Db4oUUID uuid = objInfo.GetUUID() ;
                    return uuid != null ? uuid.GetLongPart().ToString() : "NA";
                }
            	
				return "NA";
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return "NA";
            }
        }

        public long GetLocalID()
        {
            try
            {
                IObjectInfo objInfo =  Db4oClient.Client.Ext().GetObjectInfo(m_genObject);
				return objInfo != null ? objInfo.GetInternalID() : 0;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return 0;
            }
        }

        public int GetDepth(long id)
        {

            try
            {

                object obj = Db4oClient.Client.Ext().GetByID(id);
                Db4oClient.Client.Ext().Activate(obj, 1);
                level++;
                IReflectClass rclass = DataLayerCommon.ReflectClassFor(obj);
                if (rclass != null)
                {
                    IReflectField[] fieldArr = DataLayerCommon.GetDeclaredFieldsInHeirarchy(rclass);
                    if (fieldArr != null)
                    {
                        foreach (IReflectField field in fieldArr)
                        {

                            object getObject = field.Get(obj);
                            string getFieldType = field.GetFieldType().GetName();
                            IType fieldType = Db4oClient.TypeResolver.Resolve(getFieldType);
                            if (getObject != null)
                            {
                                if (!fieldType.IsEditable)
                                {
                                    if (fieldType.IsCollection)
                                    {
                                        ICollection coll = (ICollection) field.Get(obj);
                                        ArrayList arrList = new ArrayList(coll);

                                        for (int i = 0; i < arrList.Count; i++)
                                        {
                                            object colObject = arrList[i];
                                            if (colObject != null)
                                            {
                                                if (colObject is GenericObject)
                                                {
                                                    if (!m_listforObjects.Contains(colObject))
                                                    {
                                                        m_listforObjects.Add(colObject);
                                                        long objId = new ObjectDetails(colObject).GetLocalID();

                                                        level = GetDepth(objId);

                                                    }
                                                }
                                            }
                                        }


                                    }
                                    else if (fieldType.IsArray)
                                    {

                                        int length = objectContainer.Ext().Reflector().Array().GetLength(field.Get(obj));
                                        for (int i = 0; i < length; i++)
                                        {
                                            object arrObject =
                                                objectContainer.Ext().Reflector().Array().Get(field.Get(obj), i);
                                            if (arrObject != null)
                                            {
                                                if (arrObject is GenericObject)
                                                {
                                                    if (!m_listforObjects.Contains(arrObject))
                                                    {
                                                        m_listforObjects.Add(arrObject);
                                                        long objId = new ObjectDetails(arrObject).GetLocalID();
                                                        level = GetDepth(objId);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!m_listforObjects.Contains(getObject))
                                        {
                                            m_listforObjects.Add(getObject);
                                            long objId = new ObjectDetails(getObject).GetLocalID();
                                            level = GetDepth(objId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            return level;
        }

        public object GetObjById(long id)
        {
            IObjectContainer objectContainer = Db4oClient.Client;
            return objectContainer.Ext().GetByID(id);
        }
     

        public long GetVersion()
        {
            const long version = 0;
            try
            {
            	IObjectInfo objInfo = objectContainer.Ext().GetObjectInfo(m_genObject);
            	return objInfo != null ? objInfo.GetCommitTimestamp()  : version;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return 0;
            }
        }
    }
}
