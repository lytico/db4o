/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using OManager.Business.Config;
using OManager.DataLayer.Reflection;
using Sharpen.Lang;
using Type=System.Type;

namespace OMNUnitTest
{
	public class ExcludingReflector : Db4objects.Db4o.Reflect.Net.NetReflector
	{
		private readonly Collection4 _excludedClasses;
    
		public ExcludingReflector(Type[] excludedClasses)
		{
			_excludedClasses = new Collection4();
			for (int claxxIndex = 0; claxxIndex < excludedClasses.Length; ++claxxIndex)
			{
				Type claxx = excludedClasses[claxxIndex];
				_excludedClasses.Add(claxx.FullName);
			}
        }

		public override IReflectClass ForName(string className)
		{
			if (_excludedClasses.Contains(className))
			{
				return null;
			}
			return base.ForName(className);
		}
        
		public override IReflectClass ForClass(Type clazz)
		{
			if (_excludedClasses.Contains(clazz.FullName))
			{
				return null;
			}
			return base.ForClass(clazz);
		}
	}

	public static class OMNUnitTestExtensions
	{
		public static IType NewGenericType(this Type type)
		{
			string databaseFileName = Path.GetTempFileName();
			StoreInstanceOf(databaseFileName, type); 

			IEmbeddedConfiguration config2 = Db4oEmbedded.NewConfiguration();
			config2.Common.ReflectWith(new ExcludingReflector(new[] { type }));
			using (IObjectContainer db = Db4oEmbedded.OpenFile(config2, databaseFileName))
			{
				TypeResolver excludingResolver = new TypeResolver(db.Ext().Reflector());
				return excludingResolver.Resolve(TypeReference.FromType(type).GetUnversionedName());
			}
		}

		private static void StoreInstanceOf(string databaseFileName, Type type)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			using (IObjectContainer db = Db4oEmbedded.OpenFile(config, databaseFileName))
			{
				db.Store(Activator.CreateInstance(type));
			}
		}
	}
}
