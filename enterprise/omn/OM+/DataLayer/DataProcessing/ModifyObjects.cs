using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.Business.Config;
using OManager.DataLayer.Connection;
using OManager.DataLayer.CommonDatalayer;
using OManager.DataLayer.QueryParser;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;

namespace OManager.DataLayer.DataProcessing
{
    public class ModifyObjects
    {

        public static void EditObjects(long id, string attribute, object  value)
        {
            try
            {
                string fieldName = attribute;
                int intIndexof = fieldName.IndexOf('.') + 1;
                fieldName = fieldName.Substring(intIndexof, fieldName.Length - intIndexof);
                string[] splitstring = fieldName.Split('.');
                object holdParentObject = Db4oClient.Client.Ext().GetByID(id);
                Db4oClient.Client.Ext().Activate(holdParentObject, 2);
                foreach (string str in splitstring)
                {
                    holdParentObject = SetField(str, holdParentObject, value);
                }
               

            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
             

            }
        }

        private static object SetField(string attribName, object subObject, object  newValue)
        {
            try
            {
               
                IReflectClass rclass = DataLayerCommon.ReflectClassFor(subObject);
                if (rclass == null)
                    return null;

                IReflectField rfield = DataLayerCommon.GetDeclaredFieldInHeirarchy(rclass, attribName);
                if (rfield == null)
                    return null;

                if (rfield is GenericVirtualField || rfield.IsStatic())
                    return null;



                IType fieldType = Db4oClient.TypeResolver.Resolve(rfield.GetFieldType());
                if (!fieldType.IsEditable)
                {
                    if (!fieldType.IsCollection && !fieldType.IsArray)
                    {
                        subObject = rfield.Get(subObject);
                        Db4oClient.Client.Ext().Activate(subObject, 2);
                        return subObject;
                    }
                }
                else if (subObject != null)
                {
                    rfield.Set(subObject, fieldType.Cast(newValue));
                    return subObject;
                }
                return null;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }

       

        public static object GetObjects(long id)
        {
            IObjectContainer objectContainer = Db4oClient.Client;

            if (id == 0)
                return null;
            object obj = objectContainer.Ext().GetByID(id);
            Db4oClient.Client.Ext().Activate(obj, 1);
            return obj;

        }


        public static void SaveObjects(long id, int depth)
        {
        	IObjectContainer objectContainer = Db4oClient.Client;
			try
            {
            	if (id == 0)
            		return ;
            	object obj=objectContainer.Ext().GetByID(id);
                Db4oClient.Client.Ext().Activate(obj, depth);
                objectContainer.Ext().Store(obj, depth);
            	objectContainer.Commit();
                ObjectCache.RemoveObject(id);
            }
            catch (Exception oEx)
            {
                Db4oClient.Client.Rollback();
                LoggingHelper.HandleException(oEx);

            }
           
        }

    	public static void DeleteObject(long id)
        {
			IObjectContainer objectContainer = Db4oClient.Client;
			try
			{
				if (id == 0)
					return;
				object obj = objectContainer.Ext().GetByID(id);
                Db4oClient.Client.Delete(obj);
                Db4oClient.Client.Ext().Purge();
                Db4oClient.Client.Commit();

                ObjectCache.RemoveObject(id);
			}
			catch (Exception oEx)
			{
				objectContainer.Rollback();
				LoggingHelper.HandleException(oEx);
			}
        }

		public static void RefreshObject(long id, int level)
		{
			if (id == 0)
				return;
			object obj = Db4oClient.Client.Ext().GetByID(id);
			Db4oClient.Client.Ext().Refresh(obj, level);
		}
    }
}
