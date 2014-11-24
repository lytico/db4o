/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Reflect.Custom;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	/// <summary>
	/// Custom class information is stored to db4o itself as
	/// a CustomClassRepository singleton.
	/// </summary>
	/// <remarks>
	/// Custom class information is stored to db4o itself as
	/// a CustomClassRepository singleton.
	/// </remarks>
	public class Db4oPersistenceProvider : IPersistenceProvider
	{
		internal class MyContext
		{
			public readonly CustomClassRepository repository;

			public readonly IObjectContainer metadata;

			public readonly IObjectContainer data;

			public MyContext(CustomClassRepository repository, IObjectContainer metadata, IObjectContainer
				 data)
			{
				this.repository = repository;
				this.metadata = metadata;
				this.data = data;
			}
		}

		public virtual void CreateEntryClass(PersistenceContext context, string className
			, string[] fieldNames, string[] fieldTypes)
		{
			LogMethodCall("createEntryClass", context, className);
			CustomClassRepository repository = Repository(context);
			repository.DefineClass(className, fieldNames, fieldTypes);
			UpdateMetadata(context, repository);
		}

		public virtual void CreateIndex(PersistenceContext context, string className, string
			 fieldName)
		{
			MarkIndexedField(context, className, fieldName, true);
		}

		public virtual void DropIndex(PersistenceContext context, string className, string
			 fieldName)
		{
			MarkIndexedField(context, className, fieldName, false);
		}

		private void MarkIndexedField(PersistenceContext context, string className, string
			 fieldName, bool indexed)
		{
			CustomField field = CustomClass(context, className).CustomField(fieldName);
			field.Indexed(indexed);
			UpdateMetadata(context, field);
			Restart(context);
		}

		private void Restart(PersistenceContext context)
		{
			CloseContext(context);
			InitContext(context);
		}

		public virtual int Delete(PersistenceContext context, string className, object uid
			)
		{
			// TODO Auto-generated method stub
			return 0;
		}

		public virtual void DropEntryClass(PersistenceContext context, string className)
		{
		}

		// TODO Auto-generated method stub
		public virtual void InitContext(PersistenceContext context)
		{
			LogMethodCall("initContext", context);
			IObjectContainer metadata = OpenMetadata(context.Url());
			try
			{
				CustomClassRepository repository = InitializeClassRepository(metadata);
				CustomReflector reflector = new CustomReflector(repository);
				IObjectContainer data = OpenData(reflector, context.Url());
				context.SetProviderContext(new Db4oPersistenceProvider.MyContext(repository, metadata
					, data));
			}
			catch (Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
				// make sure metadata container is not left open
				// in case something goes wrong with the setup
				CloseIgnoringExceptions(metadata);
				// cant use exception chaining here because the
				// test must run in jdk 1.1
				throw new Db4oException(e);
			}
		}

		private void CloseIgnoringExceptions(IObjectContainer container)
		{
			try
			{
				container.Close();
			}
			catch (Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		public virtual void Insert(PersistenceContext context, PersistentEntry entry)
		{
			LogMethodCall("insert", context, entry);
			// clone the entry because clients are allowed to reuse
			// entry objects
			DataContainer(context).Store(Clone(entry));
		}

		public virtual IEnumerator Select(PersistenceContext context, PersistentEntryTemplate
			 template)
		{
			LogMethodCall("select", context, template);
			IQuery query = QueryFromTemplate(context, template);
			return new ObjectSetIterator(query.Execute());
		}

		public virtual void Update(PersistenceContext context, PersistentEntry entry)
		{
			PersistentEntry existing = SelectByUid(context, entry.className, entry.uid);
			existing.fieldValues = entry.fieldValues;
			DataContainer(context).Store(existing);
		}

		private PersistentEntry SelectByUid(PersistenceContext context, string className, 
			object uid)
		{
			IQuery query = NewQuery(context, className);
			query.Descend("uid").Constrain(uid);
			return (PersistentEntry)query.Execute().Next();
		}

		private void AddClassConstraint(PersistenceContext context, IQuery query, string 
			className)
		{
			query.Constrain(CustomClass(context, className));
		}

		private CustomClass CustomClass(PersistenceContext context, string className)
		{
			return Repository(context).ForName(className);
		}

		private IConstraint AddFieldConstraint(IQuery query, PersistentEntryTemplate template
			, int index)
		{
			return query.Descend(template.fieldNames[index]).Constrain(template.fieldValues[index
				]);
		}

		private void AddFieldConstraints(IQuery query, PersistentEntryTemplate template)
		{
			if (template.fieldNames.Length == 0)
			{
				return;
			}
			IConstraint c = AddFieldConstraint(query, template, 0);
			for (int i = 1; i < template.fieldNames.Length; ++i)
			{
				c = c.And(AddFieldConstraint(query, template, i));
			}
		}

		private PersistentEntry Clone(PersistentEntry entry)
		{
			return new PersistentEntry(entry.className, entry.uid, entry.fieldValues);
		}

		public virtual void CloseContext(PersistenceContext context)
		{
			LogMethodCall("closeContext", context);
			Db4oPersistenceProvider.MyContext customContext = My(context);
			if (null != customContext)
			{
				CloseIgnoringExceptions(customContext.metadata);
				CloseIgnoringExceptions(customContext.data);
				context.SetProviderContext(null);
			}
		}

		private Db4oPersistenceProvider.MyContext My(PersistenceContext context)
		{
			return ((Db4oPersistenceProvider.MyContext)context.GetProviderContext());
		}

		private IConfiguration DataConfiguration(CustomReflector reflector)
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.ReflectWith(reflector);
			ConfigureCustomClasses(config, reflector);
			return config;
		}

		private void ConfigureCustomClasses(IConfiguration config, CustomReflector reflector
			)
		{
			IEnumerator classes = reflector.CustomClasses();
			while (classes.MoveNext())
			{
				CustomClass cc = (CustomClass)classes.Current;
				ConfigureFields(config, cc);
			}
		}

		private void ConfigureFields(IConfiguration config, CustomClass cc)
		{
			IEnumerator fields = cc.CustomFields();
			while (fields.MoveNext())
			{
				CustomField field = (CustomField)fields.Current;
				config.ObjectClass(cc).ObjectField(field.GetName()).Indexed(field.Indexed());
			}
		}

		public virtual IObjectContainer DataContainer(PersistenceContext context)
		{
			return My(context).data;
		}

		private CustomClassRepository InitializeClassRepository(IObjectContainer container
			)
		{
			CustomClassRepository repository = QueryClassRepository(container);
			if (repository == null)
			{
				Log("Initializing new class repository.");
				repository = new CustomClassRepository();
				Store(container, repository);
			}
			else
			{
				Log("Found existing class repository: " + repository);
			}
			return repository;
		}

		private IConfiguration MetaConfiguration()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.ExceptionsOnNotStorable(true);
			// the following line is only necessary for the tests to run
			// in OSGi environment
			config.ReflectWith(Platform4.ReflectorForType(typeof(CustomClassRepository)));
			Cascade(config, typeof(CustomClassRepository));
			Cascade(config, typeof(Hashtable4));
			Cascade(config, typeof(CustomClass));
			// FIXME: [TA] this is necessary because the behavior
			// on .net differs with regards to cascade activation
			// remove the following two lines and run the test
			// on .net to see it fail
			Cascade(config, typeof(CustomField));
			Cascade(config, typeof(CustomUidField));
			return config;
		}

		private void Cascade(IConfiguration config, Type klass)
		{
			config.ObjectClass(klass).CascadeOnUpdate(true);
			config.ObjectClass(klass).CascadeOnActivate(true);
		}

		private IObjectContainer MetadataContainer(PersistenceContext context)
		{
			return My(context).metadata;
		}

		private string MetadataFile(string fname)
		{
			return fname + ".metadata";
		}

		private IObjectContainer OpenData(CustomReflector reflector, string fname)
		{
			return Db4oFactory.OpenFile(DataConfiguration(reflector), fname);
		}

		private IObjectContainer OpenMetadata(string fname)
		{
			return Db4oFactory.OpenFile(MetaConfiguration(), MetadataFile(fname));
		}

		public virtual void Purge(string url)
		{
			File4.Delete(url);
			File4.Delete(MetadataFile(url));
		}

		private CustomClassRepository QueryClassRepository(IObjectContainer container)
		{
			IObjectSet found = container.Query(typeof(CustomClassRepository));
			if (!found.HasNext())
			{
				return null;
			}
			return (CustomClassRepository)found.Next();
		}

		private IQuery QueryFromTemplate(PersistenceContext context, PersistentEntryTemplate
			 template)
		{
			IQuery query = NewQuery(context, template.className);
			AddFieldConstraints(query, template);
			return query;
		}

		private IQuery NewQuery(PersistenceContext context, string className)
		{
			IQuery query = DataContainer(context).Query();
			AddClassConstraint(context, query, className);
			return query;
		}

		private CustomClassRepository Repository(PersistenceContext context)
		{
			return My(context).repository;
		}

		private void Store(IObjectContainer container, object obj)
		{
			container.Store(obj);
			container.Commit();
		}

		private void UpdateMetadata(PersistenceContext context, object metadata)
		{
			Store(MetadataContainer(context), metadata);
		}

		private void Log(string message)
		{
			Logger.Log("Db4oPersistenceProvider: " + message);
		}

		private void LogMethodCall(string methodName, object arg)
		{
			Logger.LogMethodCall("Db4oPersistenceProvider", methodName, arg);
		}

		private void LogMethodCall(string methodName, object arg1, object arg2)
		{
			Logger.LogMethodCall("Db4oPersistenceProvider", methodName, arg1, arg2);
		}
	}
}
