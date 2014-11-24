/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.Business.Config;
using OManager.DataLayer.Connection;
using OManager.DataLayer.CommonDatalayer;
using OManager.DataLayer.PropertyDetails;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;

namespace OManager.DataLayer.DataProcessing
{
    public class ModifyCollections
    {


        public static void SaveCollections(long id, int level)
        {
            try
            {
                IObjectContainer objectContainer = Db4oClient.Client;
                if (id == 0)
                    return;
                object topObject = objectContainer.Ext().GetByID(id);
                Db4oClient.Client.Ext().Activate(topObject, level);
                Db4oClient.Client.Ext().Store(topObject, level);
                Db4oClient.Client.Commit();
            }
            catch (Exception oEx)
            {
                Db4oClient.Client.Rollback();
                LoggingHelper.HandleException(oEx);

            }
        }



        private static object KeyAtIndex(IDictionary dictionary, int index)
        {
           
            foreach (DictionaryEntry entry in dictionary)
            {
                if (index == 0)
                    return entry.Key;

                index--;
            }

            return null;
        }

        public static void SaveDictionary(object targetObject, string attribName, object newValue, object key)
        {
            try
            {
                IReflectClass rclass = DataLayerCommon.ReflectClassFor(targetObject);
                if (rclass != null)
                {
                    IReflectField rfield = DataLayerCommon.GetDeclaredFieldInHeirarchy(rclass, attribName);
                    if (rfield != null)
                    {
                        if (!(rfield is GenericVirtualField || rfield.IsStatic()))
                        {
                            object obj = rfield.Get(targetObject);
                            ICollection col = (ICollection)obj;
                            if (col is Hashtable)
                            {
                                Hashtable hash = (Hashtable) col;
                                hash.Remove(key);
                                hash.Add(key, newValue);
                                rfield.Set(targetObject, hash);
                            }
                            else if (col is IDictionary)
                            {
                                IDictionary dict = (IDictionary) col;
                                dict.Remove(key);
                                dict.Add(key, newValue);
                                rfield.Set(targetObject, dict);
                            }

                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                Db4oClient.Client.Rollback();
                LoggingHelper.HandleException(oEx);

            }

        }

        public static void SaveValues(long id, string attribName, object newValue, int offset)
        {
            try
            {
                object targetObject = Db4oClient.Client.Ext().GetByID(id);
                Db4oClient.Client.Ext().Activate(targetObject, 2);
                IReflectClass rclass = DataLayerCommon.ReflectClassFor(targetObject);
                IReflectField rfield = DataLayerCommon.GetDeclaredFieldInHeirarchy(rclass, attribName);
                IType type = new FieldDetails(rclass.GetName(), attribName).GetFieldType();
                object obj = rfield.Get(targetObject);

                if (obj is IDictionary)
                {
                    SaveDictionary(targetObject, attribName, newValue, KeyAtIndex((IDictionary)obj, offset/2));
                }
                else
                {

                    if (rfield != null && !(rfield is GenericVirtualField || rfield.IsStatic()))
                    {
                        if (type.IsArray || type.IsCollection)
                        {
                            IList list = obj as IList;
                            if (null != list)
                            {
                                list[offset] = newValue;
                                rfield.Set(targetObject, list);
                            }
                        }

                    }
                }
            }
            catch (Exception oEx)
            {
                Db4oClient.Client.Rollback();
                LoggingHelper.HandleException(oEx);
            }
        }



    }
}
