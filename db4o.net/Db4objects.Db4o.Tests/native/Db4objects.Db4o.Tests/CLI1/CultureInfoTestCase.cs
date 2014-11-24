/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using System.Globalization;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class CultureInfoTestCase : AbstractDb4oTestCase
	{
		public CultureInfo frfr;
		public CultureInfo sk;
		public CultureInfo invariant;

		protected override void Store()
		{
			CultureInfoTestCase test = new CultureInfoTestCase();
			test.frfr = new CultureInfo("fr-FR");
			test.sk = new CultureInfo("sk");
			test.invariant = CultureInfo.InvariantCulture;
			
			Db().Store(test);
		}
		
		public void TestRetrieveCultureInfo()
		{
			CultureInfoTestCase test = (CultureInfoTestCase) RetrieveOnlyInstance(typeof(CultureInfoTestCase));
			Assert.IsNotNull(test);
			
			Assert.IsNotNull(test.frfr);
			Assert.AreEqual("fr-FR", test.frfr.Name);
			Assert.AreEqual("fr", test.frfr.TwoLetterISOLanguageName);

			Assert.IsNotNull(test.sk);
			Assert.AreEqual("sk", test.sk.Name);
			
			Assert.AreEqual(CultureInfo.InvariantCulture, test.invariant);
		}
	}
}
