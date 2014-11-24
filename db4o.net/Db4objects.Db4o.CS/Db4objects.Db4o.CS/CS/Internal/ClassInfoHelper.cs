/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.CS.Internal
{
	public class ClassInfoHelper
	{
		private Hashtable4 _classMetaTable = new Hashtable4();

		private Hashtable4 _genericClassTable = new Hashtable4();

		private Config4Impl _config;

		public ClassInfoHelper(Config4Impl config)
		{
			_config = config;
		}

		public virtual ClassInfo GetClassMeta(IReflectClass claxx)
		{
			if (IsObjectClass(claxx))
			{
				return ClassInfo.NewSystemClass(claxx.GetName());
			}
			ClassInfo existing = LookupClassMeta(claxx.GetName());
			if (existing != null)
			{
				return existing;
			}
			return NewUserClassMeta(claxx);
		}

		private ClassInfo NewUserClassMeta(IReflectClass claxx)
		{
			ClassInfo classMeta = ClassInfo.NewUserClass(claxx.GetName());
			classMeta.SetSuperClass(MapSuperclass(claxx));
			RegisterClassMeta(claxx.GetName(), classMeta);
			classMeta.SetFields(MapFields(claxx.GetDeclaredFields(), ShouldStoreTransientFields
				(claxx)));
			return classMeta;
		}

		private bool ShouldStoreTransientFields(IReflectClass claxx)
		{
			Config4Class configClass = _config.ConfigClass(claxx.GetName());
			return configClass == null ? false : configClass.StoreTransientFields();
		}

		private ClassInfo MapSuperclass(IReflectClass claxx)
		{
			IReflectClass superClass = claxx.GetSuperclass();
			if (superClass != null)
			{
				return GetClassMeta(superClass);
			}
			return null;
		}

		private FieldInfo[] MapFields(IReflectField[] fields, bool shouldStoreTransientFields
			)
		{
			if (!shouldStoreTransientFields)
			{
				fields = FilterTransientFields(fields);
			}
			FieldInfo[] fieldsMeta = new FieldInfo[fields.Length];
			for (int i = 0; i < fields.Length; ++i)
			{
				IReflectField field = fields[i];
				bool isArray = field.GetFieldType().IsArray();
				IReflectClass fieldClass = isArray ? field.GetFieldType().GetComponentType() : field
					.GetFieldType();
				bool isPrimitive = fieldClass.IsPrimitive();
				// TODO: need to handle NArray, currently it ignores NArray and alway sets NArray flag false.
				fieldsMeta[i] = new FieldInfo(field.GetName(), GetClassMeta(fieldClass), isPrimitive
					, isArray, false);
			}
			return fieldsMeta;
		}

		private IReflectField[] FilterTransientFields(IReflectField[] fields)
		{
			IList filteredFields = new ArrayList();
			for (int fieldIndex = 0; fieldIndex < fields.Length; ++fieldIndex)
			{
				IReflectField field = fields[fieldIndex];
				if (!field.IsTransient())
				{
					filteredFields.Add(field);
				}
			}
			return ((IReflectField[])Sharpen.Collections.ToArray(filteredFields, new IReflectField
				[filteredFields.Count]));
		}

		private static bool IsObjectClass(IReflectClass claxx)
		{
			// TODO: We should send the whole class meta if we'd like to support
			// java and .net communication (We have this request in our user forum
			// http://developer.db4o.com/forums/thread/31504.aspx). If we only want
			// to support java & .net platform separately, then this method should
			// be moved to Platform4.
			//return className.startsWith("java.lang.Object") || className.startsWith("System.Object");
			return claxx.Reflector().ForClass(Const4.ClassObject) == claxx;
		}

		private ClassInfo LookupClassMeta(string className)
		{
			return (ClassInfo)_classMetaTable.Get(className);
		}

		private void RegisterClassMeta(string className, ClassInfo classMeta)
		{
			_classMetaTable.Put(className, classMeta);
		}

		public virtual GenericClass ClassMetaToGenericClass(GenericReflector reflector, ClassInfo
			 classMeta)
		{
			if (classMeta.IsSystemClass())
			{
				return (GenericClass)reflector.ForName(classMeta.GetClassName());
			}
			string className = classMeta.GetClassName();
			// look up from generic class table.
			GenericClass genericClass = LookupGenericClass(className);
			if (genericClass != null)
			{
				return genericClass;
			}
			IReflectClass reflectClass = reflector.ForName(className);
			if (reflectClass != null)
			{
				return (GenericClass)reflectClass;
			}
			GenericClass genericSuperClass = null;
			ClassInfo superClassMeta = classMeta.GetSuperClass();
			if (superClassMeta != null)
			{
				genericSuperClass = ClassMetaToGenericClass(reflector, superClassMeta);
			}
			genericClass = new GenericClass(reflector, null, className, genericSuperClass);
			RegisterGenericClass(className, genericClass);
			FieldInfo[] fields = classMeta.GetFields();
			GenericField[] genericFields = new GenericField[fields.Length];
			for (int i = 0; i < fields.Length; ++i)
			{
				ClassInfo fieldClassMeta = fields[i].GetFieldClass();
				string fieldName = fields[i].GetFieldName();
				GenericClass genericFieldClass = ClassMetaToGenericClass(reflector, fieldClassMeta
					);
				genericFields[i] = new GenericField(fieldName, genericFieldClass, fields[i]._isPrimitive
					);
			}
			genericClass.InitFields(genericFields);
			return genericClass;
		}

		private GenericClass LookupGenericClass(string className)
		{
			return (GenericClass)_genericClassTable.Get(className);
		}

		private void RegisterGenericClass(string className, GenericClass classMeta)
		{
			_genericClassTable.Put(className, classMeta);
			((GenericReflector)classMeta.Reflector()).Register(classMeta);
		}
	}
}
