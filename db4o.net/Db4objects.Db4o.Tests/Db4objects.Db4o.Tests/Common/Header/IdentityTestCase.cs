/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Header;

namespace Db4objects.Db4o.Tests.Common.Header
{
	public class IdentityTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] arguments)
		{
			new IdentityTestCase().RunAll();
		}

		public virtual void TestIdentitySignatureIsNotNull()
		{
			Db4oDatabase identity = Db().Identity();
			Assert.IsNotNull(identity.GetSignature());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestIdentityPreserved()
		{
			Db4oDatabase ident = Db().Identity();
			Reopen();
			Db4oDatabase ident2 = Db().Identity();
			Assert.IsNotNull(ident);
			Assert.AreEqual(ident, ident2);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestGenerateIdentity()
		{
			if (IsMultiSession())
			{
				return;
			}
			byte[] oldSignature = Db().Identity().GetSignature();
			GenerateNewIdentity();
			Reopen();
			ArrayAssert.AreNotEqual(oldSignature, Db().Identity().GetSignature());
		}

		private void GenerateNewIdentity()
		{
			((LocalObjectContainer)Db()).GenerateNewIdentity();
		}
	}
}
