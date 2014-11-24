/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class ClassMetadataTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		public class Item
		{
		}

		public virtual void TestDropClassIndex()
		{
			ClassMetadataTestCase.Item item = new ClassMetadataTestCase.Item();
			Store(item);
			AssertOccurrences(typeof(ClassMetadataTestCase.Item), 1);
			ClassMetadata classMetadata = Container().ClassMetadataForObject(item);
			classMetadata.DropClassIndex();
			AssertOccurrences(typeof(ClassMetadataTestCase.Item), 0);
		}
	}
}
