/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class ChangeIdentity : AbstractDb4oTestCase, IOptOutMultiSession
	{
		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			byte[] oldSignature = Db().Identity().GetSignature();
			((LocalObjectContainer)Db()).GenerateNewIdentity();
			Reopen();
			ArrayAssert.AreNotEqual(oldSignature, Db().Identity().GetSignature());
		}
	}
}
