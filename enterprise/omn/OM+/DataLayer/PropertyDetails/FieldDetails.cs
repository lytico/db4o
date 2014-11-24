using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.Business.Config;
using OManager.DataLayer.Connection;
using OManager.DataLayer.CommonDatalayer;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;

namespace OManager.DataLayer.PropertyDetails
{
    public class FieldDetails 
    {
        private readonly IObjectContainer objectContainer;
        private readonly string m_classname; 
        private readonly string m_fieldname;

        public FieldDetails(string classname, string fieldname)
        {
            string CorrectfieldName = fieldname;
            int intIndexof = CorrectfieldName.LastIndexOf('.') + 1;
            CorrectfieldName = CorrectfieldName.Substring(intIndexof, CorrectfieldName.Length - intIndexof);
            m_classname = DataLayerCommon.RemoveGFromClassName(classname);
            m_fieldname = CorrectfieldName;
            objectContainer = Db4oClient.Client; 
        }

        public bool IsIndexed()
        {
			try
			{
				IStoredClass storedClass = objectContainer.Ext().StoredClass(m_classname);
				if (null == storedClass)
					return false;

				IStoredField field = DataLayerCommon.GetDeclaredStoredFieldInHeirarchy(storedClass, m_fieldname);
				if (field == null)
					return false;

				return field.HasIndex();
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return false;
			}
        }

        public  bool IsPrimitive()
        {
            try
            {             
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_classname);
                IReflectField rField = DataLayerCommon.GetDeclaredFieldInHeirarchy(rClass, m_fieldname);
                return rField.GetFieldType().IsPrimitive();
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false ;
            }
        }

        public bool GetModifier()
        {
            try
            {
                IReflectClass rClass = DataLayerCommon.ReflectClassForName(m_classname);
                if (rClass != null)
                {
                    IReflectField rField = DataLayerCommon.GetDeclaredFieldInHeirarchy(rClass, m_fieldname);
                  
					if (rField != null)
                    {

                        return rField.IsPublic();
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

        public IType GetFieldType()
        {
            GenericReflector reflecotr = objectContainer.Ext().Reflector();
            IReflectClass klass = reflecotr.ForName(m_classname);

            IReflectField rfield = DataLayerCommon.GetDeclaredFieldInHeirarchy(klass, m_fieldname);

            return Db4oClient.TypeResolver.Resolve(rfield.GetFieldType());
        }

       
    }
}
