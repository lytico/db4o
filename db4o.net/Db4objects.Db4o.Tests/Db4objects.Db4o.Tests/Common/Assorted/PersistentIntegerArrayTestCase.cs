/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	/// <exclude></exclude>
	public class PersistentIntegerArrayTestCase : AbstractDb4oTestCase, IOptOutMultiSession
		, IOptOutDefragSolo
	{
		public static void Main(string[] arguments)
		{
			new PersistentIntegerArrayTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			int[] original = new int[] { 10, 99, 77 };
			PersistentIntegerArray arr = new PersistentIntegerArray(SlotChangeFactory.IdSystem
				, null, original);
			arr.Write(SystemTrans());
			int id = arr.GetID();
			Reopen();
			arr = new PersistentIntegerArray(SlotChangeFactory.IdSystem, null, id);
			arr.Read(SystemTrans());
			int[] copy = arr.Array();
			ArrayAssert.AreEqual(original, copy);
		}
	}
}
