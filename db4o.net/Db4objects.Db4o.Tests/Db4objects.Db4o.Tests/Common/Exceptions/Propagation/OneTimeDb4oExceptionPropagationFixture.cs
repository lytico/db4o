/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public class OneTimeDb4oExceptionPropagationFixture : OneTimeFatalExceptionPropagationFixtureBase
	{
		protected override Type ExceptionType()
		{
			return typeof(Db4oException);
		}

		public override void ThrowInitialException()
		{
			throw new Db4oException();
		}

		public override string Label()
		{
			return "non-fatal/oneDb4o";
		}
	}
}
