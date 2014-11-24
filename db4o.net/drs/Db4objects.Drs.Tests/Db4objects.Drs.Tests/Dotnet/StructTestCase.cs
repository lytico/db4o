/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Dotnet
{
	public class StructTestCase : DrsTestCase
	{
		internal Container template = new Container(new Value(42));

		public virtual void Test()
		{
			StoreToProviderA();
			ReplicateAllToProviderB();
		}

		internal virtual void StoreToProviderA()
		{
			ITestableReplicationProviderInside provider = A().Provider();
			provider.StoreNew(template);
			provider.Commit();
			EnsureContent(template, provider);
		}

		internal virtual void ReplicateAllToProviderB()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureContent(template, B().Provider());
		}

		private void EnsureContent(Container container, ITestableReplicationProviderInside
			 provider)
		{
			IObjectSet result = provider.GetStoredObjects(container.GetType());
			Assert.AreEqual(1, result.Count);
			Container c = Next(result);
			Assert.AreEqual(template.GetValue(), c.GetValue());
		}

		private Container Next(IObjectSet result)
		{
			IEnumerator iterator = result.GetEnumerator();
			if (iterator.MoveNext())
			{
				return (Container)iterator.Current;
			}
			return null;
		}
	}
}
