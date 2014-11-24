/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class DeleteDeepTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new DeleteDeepTestCase().RunConcurrency();
		}

		public string name;

		public DeleteDeepTestCase child;

		protected override void Store()
		{
			AddNodes(10);
			name = "root";
			Store(this);
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(DeleteDeepTestCase)).CascadeOnDelete(true);
		}

		// config.objectClass(DeleteDeepTestCase.class).cascadeOnActivate(true);
		private void AddNodes(int count)
		{
			if (count > 0)
			{
				child = new DeleteDeepTestCase();
				child.name = string.Empty + count;
				child.AddNodes(count - 1);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(DeleteDeepTestCase));
			q.Descend("name").Constrain("root");
			IObjectSet os = q.Execute();
			if (os.Count == 0)
			{
				// already deleted
				return;
			}
			Assert.AreEqual(1, os.Count);
			if (!os.HasNext())
			{
				return;
			}
			DeleteDeepTestCase root = (DeleteDeepTestCase)os.Next();
			// wait for other threads
			// Thread.sleep(500);
			oc.Delete(root);
			oc.Commit();
			AssertOccurrences(oc, typeof(DeleteDeepTestCase), 0);
		}

		public virtual void Check(IExtObjectContainer oc)
		{
			AssertOccurrences(oc, typeof(DeleteDeepTestCase), 0);
		}
	}
}
#endif // !SILVERLIGHT
