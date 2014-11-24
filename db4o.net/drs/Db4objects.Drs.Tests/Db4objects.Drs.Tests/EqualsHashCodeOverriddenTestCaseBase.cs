/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Drs;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class EqualsHashCodeOverriddenTestCaseBase : ITestCase
	{
		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj == null || GetType() != obj.GetType())
				{
					return false;
				}
				return _name.Equals(((EqualsHashCodeOverriddenTestCaseBase.Item)obj)._name);
			}

			public override int GetHashCode()
			{
				return _name.GetHashCode();
			}
		}

		private IStorage _storage = new MemoryStorage();

		public EqualsHashCodeOverriddenTestCaseBase() : base()
		{
		}

		protected virtual void AssertReplicates(object holder)
		{
			IEmbeddedObjectContainer sourceDb = OpenContainer("source");
			sourceDb.Store(holder);
			sourceDb.Commit();
			IEmbeddedObjectContainer targetDb = OpenContainer("target");
			try
			{
				Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(sourceDb
					);
				Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(targetDb
					);
				IReplicationSession replication = Replication.Begin(providerA, providerB);
				IObjectSet changed = replication.ProviderA().ObjectsChangedSinceLastReplication();
				while (changed.HasNext())
				{
					object o = changed.Next();
					if (holder.GetType() == o.GetType())
					{
						replication.Replicate(o);
						break;
					}
				}
				replication.Commit();
			}
			finally
			{
				sourceDb.Close();
				targetDb.Close();
			}
		}

		private IEmbeddedObjectContainer OpenContainer(string filePath)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = _storage;
			config.File.GenerateUUIDs = ConfigScope.Globally;
			config.File.GenerateCommitTimestamps = true;
			return Db4oEmbedded.OpenFile(config, filePath);
		}
	}
}
