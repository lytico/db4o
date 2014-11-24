/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class InvalidOffsetInDeleteTestCase : Db4oTestWithTempFile, IDiagnosticListener
	{
		public class Parent
		{
			public string _parentName;
		}

		public class Item : InvalidOffsetInDeleteTestCase.Parent
		{
			public string _itemName;
		}

		public virtual void Test()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			Configure(config);
			IObjectContainer objectContainer = Db4oEmbedded.OpenFile(config, TempFile());
			InvalidOffsetInDeleteTestCase.Item item = new InvalidOffsetInDeleteTestCase.Item(
				);
			item._itemName = "item";
			item._parentName = "parent";
			objectContainer.Store(item);
			objectContainer.Close();
			config = NewConfiguration();
			Configure(config);
			objectContainer = Db4oEmbedded.OpenFile(config, TempFile());
			IQuery query = objectContainer.Query();
			query.Constrain(typeof(InvalidOffsetInDeleteTestCase.Item));
			IObjectSet objectSet = query.Execute();
			item = (InvalidOffsetInDeleteTestCase.Item)objectSet.Next();
			objectContainer.Store(item);
			objectContainer.Close();
		}

		private void Configure(IEmbeddedConfiguration config)
		{
			config.Common.Diagnostic.AddListener(this);
			config.File.GenerateCommitTimestamps = true;
			config.File.GenerateUUIDs = ConfigScope.Globally;
			config.Common.ObjectClass(typeof(InvalidOffsetInDeleteTestCase.Item)).ObjectField
				("_itemName").Indexed(true);
			config.Common.ObjectClass(typeof(InvalidOffsetInDeleteTestCase.Parent)).ObjectField
				("_parentName").Indexed(true);
		}

		public virtual void OnDiagnostic(IDiagnostic d)
		{
			if (d is DeletionFailed)
			{
				Assert.Fail("No deletion failed diagnostic message expected.");
			}
		}
	}
}
