/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */
using Db4objects.Db4o.IO;
using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public partial class StorageTestSuite
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] 
						{
							new EnvironmentProvider(), 
							new SubjectFixtureProvider(
									new object[] 
									{ 
										Db4oUnitPlatform.NewPersistentStorage(),
										new MemoryStorage(), 
										new CachingStorage(Db4oUnitPlatform.NewPersistentStorage()), 
									})
						};
		}
	}
}
