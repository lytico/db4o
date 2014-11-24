/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class PeekPersistedTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new PeekPersistedTestCase().RunConcurrency();
		}

		public string name;

		public PeekPersistedTestCase child;

		protected override void Store()
		{
			PeekPersistedTestCase current = this;
			current.name = "1";
			for (int i = 2; i < 11; i++)
			{
				current.child = new PeekPersistedTestCase();
				current.child.name = string.Empty + i;
				current = current.child;
			}
			Store(this);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(PeekPersistedTestCase));
			q.Descend("name").Constrain("1");
			IObjectSet objectSet = q.Execute();
			PeekPersistedTestCase pp = (PeekPersistedTestCase)objectSet.Next();
			for (int i = 0; i < 10; i++)
			{
				Peek(oc, pp, i);
			}
		}

		private void Peek(IExtObjectContainer oc, PeekPersistedTestCase original, int depth
			)
		{
			PeekPersistedTestCase peeked = (PeekPersistedTestCase)((PeekPersistedTestCase)oc.
				PeekPersisted(original, depth, true));
			for (int i = 0; i <= depth; i++)
			{
				Assert.IsNotNull(peeked);
				Assert.IsFalse(oc.IsStored(peeked));
				peeked = peeked.child;
			}
			Assert.IsNull(peeked);
		}
	}
}
#endif // !SILVERLIGHT
