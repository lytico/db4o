/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class TimeStampIdGeneratorTestCase : ITestCase
	{
		public virtual void TestObjectCounterPartOnlyUses6Bits()
		{
			long[] ids = GenerateIds();
			for (int i = 1; i < ids.Length; i++)
			{
				Assert.IsGreater(ids[i] - 1, ids[i]);
				long creationTime = TimeStampIdGenerator.IdToMilliseconds(ids[i]);
				long timePart = TimeStampIdGenerator.MillisecondsToId(creationTime);
				long objectCounter = ids[i] - timePart;
				// 6 bits
				Assert.IsSmallerOrEqual(Binary.LongForBits(6), objectCounter);
			}
		}

		private long[] GenerateIds()
		{
			int count = 500;
			TimeStampIdGenerator generator = new TimeStampIdGenerator();
			long[] ids = new long[count];
			for (int i = 0; i < ids.Length; i++)
			{
				ids[i] = generator.Generate();
			}
			return ids;
		}

		public virtual void TestContinousIncrement()
		{
			TimeStampIdGenerator generator = new TimeStampIdGenerator();
			AssertContinousIncrement(generator);
		}

		private void AssertContinousIncrement(TimeStampIdGenerator generator)
		{
			long oldId = generator.Generate();
			for (int i = 0; i < 1000000; i++)
			{
				long newId = generator.Generate();
				Assert.IsGreater(oldId, newId);
				oldId = newId;
			}
		}

		public virtual void TestTimeStaysTheSame()
		{
			TimeStampIdGenerator generatorWithSameTime = new _TimeStampIdGenerator_51();
			AssertContinousIncrement(generatorWithSameTime);
		}

		private sealed class _TimeStampIdGenerator_51 : TimeStampIdGenerator
		{
			public _TimeStampIdGenerator_51()
			{
			}

			protected override long Now()
			{
				return 1;
			}
		}
	}
}
