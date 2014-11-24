/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class CommitAfterDroppedFieldIndexTestCase : Db4oClientServerTestCase
	{
		public class Item
		{
			public int _id;

			public Item(int id)
			{
				_id = id;
			}
		}

		private const int ObjectCount = 100;

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().PrefetchIDCount(1);
			config.ClientServer().BatchMessages(false);
			config.BTreeNodeSize(4);
		}

		public virtual void Test()
		{
			for (int i = 0; i < ObjectCount; i++)
			{
				Store(new CommitAfterDroppedFieldIndexTestCase.Item(1));
			}
			IStoredField storedField = FileSession().StoredClass(typeof(CommitAfterDroppedFieldIndexTestCase.Item
				)).StoredField("_id", null);
			storedField.CreateIndex();
			FileSession().Commit();
			IExtObjectContainer session = OpenNewSession();
			IObjectSet allItems = session.Query(typeof(CommitAfterDroppedFieldIndexTestCase.Item
				));
			for (IEnumerator itemIter = allItems.GetEnumerator(); itemIter.MoveNext(); )
			{
				CommitAfterDroppedFieldIndexTestCase.Item item = ((CommitAfterDroppedFieldIndexTestCase.Item
					)itemIter.Current);
				item._id++;
				session.Store(item);
			}
			// Making sure all storing has been processed.
			session.SetSemaphore("anySemaphore", 0);
			storedField.DropIndex();
			session.Commit();
			storedField.CreateIndex();
		}
	}
}
#endif // !SILVERLIGHT
