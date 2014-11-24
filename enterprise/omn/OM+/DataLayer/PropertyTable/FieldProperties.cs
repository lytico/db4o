/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using Db4objects.Db4o.Reflect;
using OManager.Business.Config;
using OManager.DataLayer.Connection;
using Db4objects.Db4o.Reflect.Generic;
using OManager.DataLayer.PropertyDetails;
using OME.Logging.Common;

namespace OManager.DataLayer.PropertyTable
{
	
    public class FieldProperties
    {
		private string m_fieldName;
		private bool m_isIndexed;
		private bool m_isPublic;
		private readonly string m_dataType;
        private readonly IType m_type;
		public FieldProperties(string fieldName, string fieldType)
		{
			m_fieldName = fieldName;
            m_type = Db4oClient.TypeResolver.Resolve(fieldType);
            m_dataType = m_type.DisplayName;
		}

        public bool Indexed
        {
            get { return m_isIndexed; }
            set { m_isIndexed = value; }
        }

        public bool Public
        {
            get { return m_isPublic; }
            set { m_isPublic = value; }
        }

        public string Field
        {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        public string DataType
        {
            get { return m_dataType; }
        }

		public IType Type
		{
			get { return m_type; }
		}

		public static List<FieldProperties> FieldsFrom(string className)
        {
            try
            {
				List<FieldProperties> listFieldProperties = new List<FieldProperties>();
                ClassDetails clDetails = new ClassDetails(className);
                IReflectField[] fields = clDetails.GetFieldList();
                if (fields == null)
                {
                    return null;
                }
                
                    foreach (IReflectField field in clDetails.GetFieldList())
                    {
                        if (!(field is GenericVirtualField || field.IsStatic()))
                        {
                            FieldProperties fp = FieldPropertiesFor(className, field);
                            listFieldProperties.Add(fp);
                        }
                    }
                    return listFieldProperties;
                }
            
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }

    	private static FieldProperties FieldPropertiesFor(string className, IReflectField field)
    	{
    		FieldProperties fp = new FieldProperties(field.GetName(), field.GetFieldType().GetName());
    		FieldDetails fd = new FieldDetails(className, fp.Field);
    		fp.m_isPublic = fd.GetModifier();
    		fp.m_isIndexed = fd.IsIndexed();
    		
			return fp;
    	}
    }

}
