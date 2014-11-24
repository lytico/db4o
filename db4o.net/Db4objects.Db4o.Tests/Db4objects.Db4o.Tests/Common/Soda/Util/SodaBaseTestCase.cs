/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Util
{
	public abstract class SodaBaseTestCase : AbstractDb4oTestCase
	{
		[System.NonSerialized]
		protected object[] _array;

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupBeforeStore()
		{
			_array = CreateData();
		}

		protected override void Store()
		{
			object[] data = CreateData();
			for (int idx = 0; idx < data.Length; idx++)
			{
				Db().Store(data[idx]);
			}
		}

		public abstract object[] CreateData();

		protected virtual void Expect(IQuery query, int[] indices)
		{
			SodaTestUtil.Expect(query, CollectCandidates(indices), false);
		}

		protected virtual void ExpectOrdered(IQuery query, int[] indices)
		{
			SodaTestUtil.ExpectOrdered(query, CollectCandidates(indices));
		}

		private object[] CollectCandidates(int[] indices)
		{
			object[] data = new object[indices.Length];
			for (int idx = 0; idx < indices.Length; idx++)
			{
				data[idx] = _array[indices[idx]];
			}
			return data;
		}
	}
}
