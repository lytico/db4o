/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class InvalidUUIDTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new InvalidUUIDTestCase().RunConcurrency();
		}

		public string name;

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(this.GetType()).GenerateUUIDs(true);
		}

		protected override void Store()
		{
			name = "theOne";
			Store(this);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc(IExtObjectContainer oc)
		{
			IObjectSet os = oc.Query(typeof(InvalidUUIDTestCase));
			if (os.Count == 0)
			{
				// already deleted by other threads
				return;
			}
			Assert.AreEqual(1, os.Count);
			InvalidUUIDTestCase iu = (InvalidUUIDTestCase)os.Next();
			Db4oUUID myUuid = oc.GetObjectInfo(iu).GetUUID();
			Assert.IsNotNull(myUuid);
			byte[] mySignature = myUuid.GetSignaturePart();
			long myLong = myUuid.GetLongPart();
			long unknownLong = long.MaxValue - 100;
			byte[] unknownSignature = new byte[] { 1, 2, 4, 99, 33, 22 };
			Db4oUUID unknownLongPart = new Db4oUUID(unknownLong, mySignature);
			Db4oUUID unknownSignaturePart = new Db4oUUID(myLong, unknownSignature);
			Db4oUUID unknownBoth = new Db4oUUID(unknownLong, unknownSignature);
			Assert.IsNull(oc.GetByUUID(unknownLongPart));
			Assert.IsNull(oc.GetByUUID(unknownSignaturePart));
			Assert.IsNull(oc.GetByUUID(unknownBoth));
			Assert.IsNull(oc.GetByUUID(unknownLongPart));
			Thread.Sleep(500);
			oc.Delete(iu);
			oc.Commit();
			Assert.IsNull(oc.GetByUUID(myUuid));
		}
	}
}
#endif // !SILVERLIGHT
