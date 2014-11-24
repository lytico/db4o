/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class CommitTimestampTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateCommitTimestamps(true);
		}

		public class Item
		{
		}

		public virtual void TestUpdateAndQuery()
		{
			CommitTimestampTestCase.Item item1 = new CommitTimestampTestCase.Item();
			Store(item1);
			CommitTimestampTestCase.Item item2 = new CommitTimestampTestCase.Item();
			Store(item2);
			Commit();
			long initialCommitTimestamp1 = Db().GetObjectInfo(item1).GetCommitTimestamp();
			long initialCommitTimestamp2 = Db().GetObjectInfo(item2).GetCommitTimestamp();
			Assert.AreEqual(initialCommitTimestamp1, initialCommitTimestamp2);
			Store(item2);
			Commit();
			long secondCommitTimestamp1 = Db().GetObjectInfo(item1).GetCommitTimestamp();
			long secondCommitTimestamp2 = Db().GetObjectInfo(item2).GetCommitTimestamp();
			Assert.AreEqual(initialCommitTimestamp1, secondCommitTimestamp1);
			Assert.AreNotEqual(initialCommitTimestamp2, secondCommitTimestamp2);
			AssertQueryForTimestamp(item1, initialCommitTimestamp1);
			AssertQueryForTimestamp(item2, secondCommitTimestamp2);
		}

		private void AssertQueryForTimestamp(CommitTimestampTestCase.Item expected, long 
			timestamp)
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(CommitTimestampTestCase.Item));
			query.Descend(VirtualField.CommitTimestamp).Constrain(timestamp);
			IObjectSet objectSet = query.Execute();
			Assert.AreEqual(1, objectSet.Count);
			CommitTimestampTestCase.Item actual = (CommitTimestampTestCase.Item)objectSet.Next
				();
			Assert.AreSame(expected, actual);
		}
	}
}
