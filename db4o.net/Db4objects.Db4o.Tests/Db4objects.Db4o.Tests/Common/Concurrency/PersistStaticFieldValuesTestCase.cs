/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class PersistStaticFieldValuesTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new PersistStaticFieldValuesTestCase().RunConcurrency();
		}

		public static readonly PersistStaticFieldValuesTestCase.PsfvHelper One = new PersistStaticFieldValuesTestCase.PsfvHelper
			();

		public static readonly PersistStaticFieldValuesTestCase.PsfvHelper Two = new PersistStaticFieldValuesTestCase.PsfvHelper
			();

		public static readonly PersistStaticFieldValuesTestCase.PsfvHelper Three = new PersistStaticFieldValuesTestCase.PsfvHelper
			();

		public PersistStaticFieldValuesTestCase.PsfvHelper one;

		public PersistStaticFieldValuesTestCase.PsfvHelper two;

		public PersistStaticFieldValuesTestCase.PsfvHelper three;

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(PersistStaticFieldValuesTestCase)).PersistStaticFieldValues
				();
		}

		protected override void Store()
		{
			PersistStaticFieldValuesTestCase psfv = new PersistStaticFieldValuesTestCase();
			psfv.one = One;
			psfv.two = Two;
			psfv.three = Three;
			Store(psfv);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			PersistStaticFieldValuesTestCase psfv = (PersistStaticFieldValuesTestCase)((PersistStaticFieldValuesTestCase
				)RetrieveOnlyInstance(oc, typeof(PersistStaticFieldValuesTestCase)));
			Assert.AreSame(One, psfv.one);
			Assert.AreSame(Two, psfv.two);
			Assert.AreSame(Three, psfv.three);
		}

		public class PsfvHelper
		{
		}
	}
}
#endif // !SILVERLIGHT
