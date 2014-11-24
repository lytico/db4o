/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Reflect;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class DrsFixture
	{
		public readonly IDrsProviderFixture a;

		public readonly IDrsProviderFixture b;

		public readonly IReflector reflector;

		public DrsFixture(IDrsProviderFixture fixtureA, IDrsProviderFixture fixtureB) : this
			(fixtureA, fixtureB, null)
		{
		}

		public DrsFixture(IDrsProviderFixture fixtureA, IDrsProviderFixture fixtureB, IReflector
			 reflector)
		{
			if (null == fixtureA)
			{
				throw new ArgumentException("fixtureA");
			}
			if (null == fixtureB)
			{
				throw new ArgumentException("fixtureB");
			}
			a = fixtureA;
			b = fixtureB;
			this.reflector = reflector;
		}
	}
}
