/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class DatabaseUnicityTest : AbstractDb4oTestCase
	{
		public virtual void Test()
		{
			Hashtable4 ht = new Hashtable4();
			ObjectContainerBase container = Container();
			container.ShowInternalClasses(true);
			IQuery q = Db().Query();
			q.Constrain(typeof(Db4oDatabase));
			IObjectSet objectSet = q.Execute();
			while (objectSet.HasNext())
			{
				Db4oDatabase identity = (Db4oDatabase)objectSet.Next();
				Assert.IsFalse(ht.ContainsKey(identity.i_signature));
				ht.Put(identity.i_signature, string.Empty);
			}
			container.ShowInternalClasses(false);
		}
	}
}
