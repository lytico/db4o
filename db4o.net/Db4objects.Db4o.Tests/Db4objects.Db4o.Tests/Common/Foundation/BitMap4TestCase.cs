/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class BitMap4TestCase : ITestCase
	{
		public virtual void Test()
		{
			byte[] buffer = new byte[100];
			for (int i = 0; i < 17; i++)
			{
				BitMap4 map = new BitMap4(i);
				map.WriteTo(buffer, 11);
				BitMap4 reReadMap = new BitMap4(buffer, 11, i);
				for (int j = 0; j < i; j++)
				{
					TBit(map, j);
					TBit(reReadMap, j);
				}
			}
		}

		private void TBit(BitMap4 map, int bit)
		{
			map.SetTrue(bit);
			Assert.IsTrue(map.IsTrue(bit));
			map.SetFalse(bit);
			Assert.IsFalse(map.IsTrue(bit));
			map.SetTrue(bit);
			Assert.IsTrue(map.IsTrue(bit));
		}
	}
}
