using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using OManager.Business.Config;
using OManager.DataLayer.Connection;
using OManager.DataLayer.CommonDatalayer;
using OME.Logging.Common;
using OManager.DataLayer.Reflection;

namespace OManager.DataLayer.PropertyDetails
{
    public class ClassDetails 
    {
        private readonly IObjectContainer objectContainer;
        private readonly string m_className;

        public  ClassDetails(string className)
        {
            m_className = DataLayerCommon.RemoveGFromClassName(className);
            objectContainer = Db4oClient.Client; 
        }

        public int GetNumberOfObjects()
        {
            try
            {
                return objectContainer.Ext().StoredClass(m_className).GetIDs().Length;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return 0;
            }
        }

        public Hashtable GetFields()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_className);
                IReflectField[] rFields = DataLayerCommon.GetDeclaredFieldsInHeirarchy(rClass);
                Hashtable FieldList = new Hashtable();

                foreach (IReflectField field in rFields)
                {
                    if (!FieldList.ContainsKey(field.GetName()))
                    {

                        FieldList.Add(
                            field.GetName(),
                            field.GetFieldType().GetName());
                    }
                }
                return FieldList;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }


       
        public int GetFieldCount()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_className);
                if (rClass != null)
                {
                    IType type = Db4oClient.TypeResolver.Resolve(rClass);
                    if (!type.IsEditable)
                    {
                        IReflectField[] rFields = DataLayerCommon.GetDeclaredFieldsInHeirarchy(rClass);
                        return rFields.Length;
                    }
                }
                return 0;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return 0; 
            }
        }

        public IReflectField[] GetFieldList()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_className); 
                IReflectField[] rFields = DataLayerCommon.GetDeclaredFieldsInHeirarchy(rClass);
                return rFields;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
           
        }
        public void SetIndex(ArrayList fieldnames, string className, ArrayList Indexed, string path, bool customConfig)
        {
            IObjectContainer con = null;
            try
            {
                IEmbeddedConfiguration embeddedConfig = !customConfig ? Db4oEmbedded.NewConfiguration() : Config();

                for (int i = 0; i < fieldnames.Count; i++)
                {
                    embeddedConfig.Common.ObjectClass(className).ObjectField(fieldnames[i].ToString()).Indexed(
                        Convert.ToBoolean(Indexed[i]));
                }

                con = Db4oEmbedded.OpenFile(embeddedConfig, path);
                IReflectClass clazz = con.Ext().Reflector().ForName(className);
                con.QueryByExample(clazz);
            }
            finally
            {
                con.Close();
            }
        }

        public static IEmbeddedConfiguration Config()
        {
            IEmbeddedConfiguration config1 = ManageCustomConfig.ConfigureEmbeddedCustomConfig();
            return config1;
        }
    }
}
