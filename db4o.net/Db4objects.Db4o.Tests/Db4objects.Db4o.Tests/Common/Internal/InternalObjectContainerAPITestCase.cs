/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class InternalObjectContainerAPITestCase : AbstractDb4oTestCase
	{
		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new InternalObjectContainerAPITestCase.Item());
		}

		public virtual void TestClassMetadataForName()
		{
			string className = CrossPlatformServices.FullyQualifiedName(typeof(InternalObjectContainerAPITestCase.Item
				));
			ClassMetadata clazz = ((IInternalObjectContainer)Db()).ClassMetadataForName(className
				);
			Assert.AreEqual(className, clazz.GetName());
			Assert.AreEqual(Reflector().ForClass(typeof(InternalObjectContainerAPITestCase.Item
				)), clazz.ClassReflector());
		}

		public static void Main(string[] args)
		{
			new InternalObjectContainerAPITestCase().RunAll();
		}
	}
}
