/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class TypeHandlerConfiguration
	{
		protected readonly Config4Impl _config;

		private ITypeHandler4 _listTypeHandler;

		private ITypeHandler4 _mapTypeHandler;

		public abstract void Apply();

		public TypeHandlerConfiguration(Config4Impl config)
		{
			_config = config;
		}

		protected virtual void ListTypeHandler(ITypeHandler4 listTypeHandler)
		{
			_listTypeHandler = listTypeHandler;
		}

		protected virtual void MapTypeHandler(ITypeHandler4 mapTypehandler)
		{
			_mapTypeHandler = mapTypehandler;
		}

		protected virtual void RegisterCollection(Type clazz)
		{
			RegisterListTypeHandlerFor(clazz);
		}

		protected virtual void RegisterMap(Type clazz)
		{
			RegisterMapTypeHandlerFor(clazz);
		}

		protected virtual void IgnoreFieldsOn(Type clazz)
		{
			RegisterTypeHandlerFor(clazz, IgnoreFieldsTypeHandler.Instance);
		}

		protected virtual void IgnoreFieldsOn(string className)
		{
			RegisterTypeHandlerFor(className, IgnoreFieldsTypeHandler.Instance);
		}

		private void RegisterListTypeHandlerFor(Type clazz)
		{
			RegisterTypeHandlerFor(clazz, _listTypeHandler);
		}

		private void RegisterMapTypeHandlerFor(Type clazz)
		{
			RegisterTypeHandlerFor(clazz, _mapTypeHandler);
		}

		protected virtual void RegisterTypeHandlerFor(Type clazz, ITypeHandler4 typeHandler
			)
		{
			_config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(clazz), typeHandler
				);
		}

		protected virtual void RegisterTypeHandlerFor(string className, ITypeHandler4 typeHandler
			)
		{
			_config.RegisterTypeHandler(new SingleNamedClassTypeHandlerPredicate(className), 
				typeHandler);
		}
	}
}
