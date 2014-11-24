/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	/// <exclude></exclude>
	public class CascadedDeleteFileFormatUpdateTestCase : FormatMigrationTestCaseBase
	{
		private bool _failed;

		protected override void ConfigureForStore(IConfiguration config)
		{
			config.ObjectClass(typeof(CascadedDeleteFileFormatUpdateTestCase.ParentItem)).CascadeOnDelete
				(true);
		}

		protected override void ConfigureForTest(IConfiguration config)
		{
			ConfigureForStore(config);
			config.Diagnostic().AddListener(new _IDiagnosticListener_24(this));
		}

		private sealed class _IDiagnosticListener_24 : IDiagnosticListener
		{
			public _IDiagnosticListener_24(CascadedDeleteFileFormatUpdateTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (d is DeletionFailed)
				{
					// Can't assert directly here, db4o eats the exception. :/
					this._enclosing._failed = true;
				}
			}

			private readonly CascadedDeleteFileFormatUpdateTestCase _enclosing;
		}

		protected override void DeconfigureForTest(IConfiguration config)
		{
			config.Diagnostic().RemoveAllListeners();
		}

		public class ParentItem
		{
			public CascadedDeleteFileFormatUpdateTestCase.ChildItem[] _children;

			public static CascadedDeleteFileFormatUpdateTestCase.ParentItem NewTestInstance()
			{
				CascadedDeleteFileFormatUpdateTestCase.ParentItem item = new CascadedDeleteFileFormatUpdateTestCase.ParentItem
					();
				item._children = new CascadedDeleteFileFormatUpdateTestCase.ChildItem[] { new CascadedDeleteFileFormatUpdateTestCase.ChildItem
					(), new CascadedDeleteFileFormatUpdateTestCase.ChildItem() };
				return item;
			}
		}

		public class ChildItem
		{
		}

		protected override void AssertObjectsAreReadable(IExtObjectContainer objectContainer
			)
		{
			CascadedDeleteFileFormatUpdateTestCase.ParentItem parentItem = (CascadedDeleteFileFormatUpdateTestCase.ParentItem
				)RetrieveInstance(objectContainer, typeof(CascadedDeleteFileFormatUpdateTestCase.ParentItem
				));
			Assert.IsNotNull(parentItem._children);
			Assert.IsNotNull(parentItem._children[0]);
			Assert.IsNotNull(parentItem._children[1]);
			objectContainer.Delete(parentItem);
			Assert.IsFalse(_failed);
			objectContainer.Store(CascadedDeleteFileFormatUpdateTestCase.ParentItem.NewTestInstance
				());
		}

		private object RetrieveInstance(IExtObjectContainer objectContainer, Type clazz)
		{
			return objectContainer.Query(clazz).Next();
		}

		protected override string FileNamePrefix()
		{
			return "migrate_cascadedelete_";
		}

		protected override void Store(IObjectContainerAdapter objectContainer)
		{
			objectContainer.Store(CascadedDeleteFileFormatUpdateTestCase.ParentItem.NewTestInstance
				());
		}
	}
}
