/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class TAWithGCBeforeCommitTestCase : AbstractDb4oTestCase
	{
		private static readonly string UpdatedId = "X";

		private static readonly string OrigId = "U";

		public class Item : IActivatable
		{
			public string _id;

			public Item(string id)
			{
				_id = id;
			}

			public virtual void Id(string id)
			{
				Activate(ActivationPurpose.Write);
				_id = id;
			}

			public virtual string Id()
			{
				Activate(ActivationPurpose.Read);
				return _id;
			}

			[System.NonSerialized]
			private IActivator _activator;

			public virtual void Bind(IActivator activator)
			{
				if (this._activator == activator)
				{
					return;
				}
				if (activator != null && this._activator != null)
				{
					throw new InvalidOperationException();
				}
				this._activator = activator;
			}

			public virtual void Activate(ActivationPurpose purpose)
			{
				if (this._activator == null)
				{
					return;
				}
				this._activator.Activate(purpose);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentPersistenceSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new TAWithGCBeforeCommitTestCase.Item(OrigId));
		}

		public virtual void TestCommit()
		{
			TAWithGCBeforeCommitTestCase.Item item = (TAWithGCBeforeCommitTestCase.Item)((TAWithGCBeforeCommitTestCase.Item
				)RetrieveOnlyInstance(typeof(TAWithGCBeforeCommitTestCase.Item)));
			item.Id(UpdatedId);
			item = null;
			Runtime.Gc();
			Db().Commit();
			item = (TAWithGCBeforeCommitTestCase.Item)((TAWithGCBeforeCommitTestCase.Item)RetrieveOnlyInstance
				(typeof(TAWithGCBeforeCommitTestCase.Item)));
			Db().Refresh(item, int.MaxValue);
			Assert.AreEqual(UpdatedId, item.Id());
		}

		public virtual void TestRollback()
		{
			TAWithGCBeforeCommitTestCase.Item item = (TAWithGCBeforeCommitTestCase.Item)((TAWithGCBeforeCommitTestCase.Item
				)RetrieveOnlyInstance(typeof(TAWithGCBeforeCommitTestCase.Item)));
			item.Id(UpdatedId);
			item = null;
			Runtime.Gc();
			Db().Rollback();
			item = (TAWithGCBeforeCommitTestCase.Item)((TAWithGCBeforeCommitTestCase.Item)RetrieveOnlyInstance
				(typeof(TAWithGCBeforeCommitTestCase.Item)));
			Db().Refresh(item, int.MaxValue);
			Assert.AreEqual(OrigId, item.Id());
		}

		public static void Main(string[] args)
		{
			new TAWithGCBeforeCommitTestCase().RunAll();
		}
	}
}
