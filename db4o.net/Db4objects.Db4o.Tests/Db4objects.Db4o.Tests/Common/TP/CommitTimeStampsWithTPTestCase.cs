/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TP;

namespace Db4objects.Db4o.Tests.Common.TP
{
	public class CommitTimeStampsWithTPTestCase : AbstractDb4oTestCase
	{
		public virtual void TestWorksWithoutTP()
		{
			AssertUpdate(false);
		}

		private void AssertUpdate(bool isTP)
		{
			CommitTimeStampsWithTPTestCase.NamedItem item = new CommitTimeStampsWithTPTestCase.NamedItem
				();
			Store(item);
			Commit();
			long firstTS = CommitTimestampFor(item);
			item.SetName("New Name");
			if (!isTP)
			{
				Store(item);
			}
			Commit();
			long secondTS = CommitTimestampFor(item);
			AssertChangesHaveBeenStored(Db());
			Assert.IsTrue(secondTS > firstTS);
		}

		private long CommitTimestampFor(CommitTimeStampsWithTPTestCase.NamedItem item)
		{
			return Db().Ext().GetObjectInfo(item).GetCommitTimestamp();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestWorksWithTP()
		{
			Fixture().ConfigureAtRuntime(new _IRuntimeConfigureAction_38());
			Reopen();
			AssertUpdate(true);
		}

		private sealed class _IRuntimeConfigureAction_38 : IRuntimeConfigureAction
		{
			public _IRuntimeConfigureAction_38()
			{
			}

			public void Apply(IConfiguration config)
			{
				config.Add(new TransparentPersistenceSupport());
			}
		}

		private void AssertChangesHaveBeenStored(IObjectContainer container)
		{
			IObjectContainer session = container.Ext().OpenSession();
			try
			{
				CommitTimeStampsWithTPTestCase.NamedItem item = ((CommitTimeStampsWithTPTestCase.NamedItem
					)session.Query(typeof(CommitTimeStampsWithTPTestCase.NamedItem))[0]);
				Assert.AreEqual("New Name", item.GetName());
			}
			finally
			{
				session.Close();
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateCommitTimestamps(true);
			config.Storage = new MemoryStorage();
		}

		public class NamedItem : IActivatable
		{
			[System.NonSerialized]
			private IActivator _activator;

			public string name = "default";

			public virtual void SetName(string name)
			{
				Activate(ActivationPurpose.Write);
				this.name = name;
			}

			public virtual string GetName()
			{
				Activate(ActivationPurpose.Read);
				return this.name;
			}

			public virtual void Activate(ActivationPurpose purpose)
			{
				if (_activator != null)
				{
					_activator.Activate(purpose);
				}
			}

			public virtual void Bind(IActivator activator)
			{
				if (_activator == activator)
				{
					return;
				}
				if (activator != null && _activator != null)
				{
					throw new InvalidOperationException();
				}
				_activator = activator;
			}
		}
	}
}
