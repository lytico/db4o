/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Tests.Foundation
{
	public class SignatureTestCase : ITestCase
	{
		public virtual void Test()
		{
			StatefulBuffer writer = new StatefulBuffer(null, 300);
			string stringRepresentation = SignatureGenerator.GenerateSignature();
			new LatinStringIO().Write(writer, stringRepresentation);
			Signature signature = new Signature(writer.GetWrittenBytes());
			Assert.AreEqual(stringRepresentation, signature.ToString());
		}
	}
}
