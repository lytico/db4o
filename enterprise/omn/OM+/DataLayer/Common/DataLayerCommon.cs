using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.Business.Config;
using OManager.DataLayer.Connection;
using OME.Logging.Common;

namespace OManager.DataLayer.CommonDatalayer
{
    public class DataLayerCommon
    {
        public static string Db4o_Version
        {
            get { return Db4oVersion.Name; }
        }

        public static IReflectField GetDeclaredFieldInHeirarchy(IReflectClass aClass, string attribute)
        {
            try
            {
                while (aClass != null)
                {
                    
                    
                    IReflectField refField = GetDeclaredField(aClass, attribute);
                    if (refField != null)
                        return refField;

                    aClass = aClass.GetSuperclass();
                }
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;

        }

        public static IStoredField GetDeclaredStoredFieldInHeirarchy(IStoredClass aClass, string attribute)
        {
            try
            {
                IStoredField sField = null;
                if (aClass == null)
                    return null;

                while (aClass != null)
                {

                    sField = aClass.StoredField(attribute, null);
                    if (sField != null)
                        break;
                    aClass = aClass.GetParentStoredClass();

                }
                return sField;
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;

        }

        public static IReflectField GetDeclaredField(IReflectClass aClass, string attribute)
        {
            try
            {
                return aClass.GetDeclaredField(attribute);
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }

        public static IReflectField[] GetDeclaredFieldsInHeirarchy(IReflectClass aClass) //67
        {
            try
            {
                return GetFieldList(aClass).ToArray();
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }

        public static List<IReflectField> GetFieldList(IReflectClass aClass)
        {
            try
            {
                if (aClass == null)
                    return null;

                List<IReflectField> ret = NonVirtualFieldsFor(aClass);
                IReflectClass parent = aClass.GetSuperclass();
                if (parent != null && !(parent.GetName().StartsWith("System.")))
                    ret.AddRange(GetFieldList(parent));

                return ret;
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }

        public static List<IReflectField> NonVirtualFieldsFor(IReflectClass aClass)
        {
            try
            {
                List<IReflectField> ret = new List<IReflectField>();
                foreach (IReflectField field in aClass.GetDeclaredFields())
                {
                    if (!(field is GenericVirtualField))
                        ret.Add(field);
                }
                return ret;
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }


        public static string RemoveGFromClassName(string name)
        {
            return name.Contains("(G) ") ? name.Replace("(G) ", "") : name;
        }

        public static IReflectClass ReflectClassForName(string classname)
        {
            try
            {
                IObjectContainer objectContainer = Db4oClient.Client;
                if (classname != string.Empty)
                {
                    classname = RemoveGFromClassName(classname);
                    return objectContainer.Ext().Reflector().ForName(classname);
                }
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;
        }
        public static IReflectClass ReflectClassFor(object obj)
        {
            try
            {
                if (obj != null)
                {
                    IObjectContainer objectContainer = Db4oClient.Client;

                    return objectContainer.Ext().Reflector().ForObject(obj);
                }
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
            return null;

        }
        public static bool IsCollection(object expandedObj)
        {
            try
            {
                if (expandedObj != null)
                {
                   IReflectClass refClass = Db4oClient.Client.Ext().Reflector().ForObject(expandedObj);
                    IType type = Db4oClient.TypeResolver.Resolve(refClass);
                    return type.IsCollection;
                }
                return false;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }

        public static bool IsArray(object expandedObj)
        {
            try
            {
                if (expandedObj != null)
                {
                    IReflectClass refClass = ReflectClassFor(expandedObj);
                    if (refClass != null)
                    {
                      
                        IType type = Db4oClient.TypeResolver.Resolve(refClass);
                         return type.IsArray ;
                
                    }
                }
                return false;

            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }


        public static bool IsPrimitive(object expandedObj)
        {
            try
            {
                 IReflectClass refClass = ReflectClassFor(expandedObj);
                if (refClass != null)
                {
                    IType type = Db4oClient.TypeResolver.Resolve(refClass);
                    return type.IsPrimitive;  
                }
                return false;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
        }
       
       

    }
}
