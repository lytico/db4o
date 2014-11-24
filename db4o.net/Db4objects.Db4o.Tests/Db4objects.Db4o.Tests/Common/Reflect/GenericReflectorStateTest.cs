/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Reflect
{
	public class GenericReflectorStateTest : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
		}

		public virtual void TestKnownClasses()
		{
			Db().Reflector().KnownClasses();
		}
	}
}
