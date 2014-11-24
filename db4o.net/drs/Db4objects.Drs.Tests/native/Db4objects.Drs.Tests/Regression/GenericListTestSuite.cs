/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Foundation;
using Db4oUnit.Fixtures;

namespace Db4objects.Drs.Tests.Regression
{
	class GenericListTestSuite : FixtureBasedTestSuite
	{
		public override Type[] TestUnits()
		{
			return new Type[]
				{
					typeof(GenericListTestCase)
				};
		}

		public override IFixtureProvider[] FixtureProviders()
		{	
			return new IFixtureProvider[]
				{
					new SubjectFixtureProvider(GenerateLists()),
				};
		}

		private IEnumerable GenerateLists()
		{
			IEnumerable<string> tenStrings = GenerateStrings(10);
			yield return new List<int>();
			yield return new List<string>(tenStrings);
			yield return new ArrayList();
			yield return new ArrayList(new List<string>(tenStrings));
			yield return new LinkedList<string>(tenStrings);
			yield return new LinkedList<string>();
			yield return new LinkedList<int>(Range(0, 10));
			yield return new List<Container>(GenerateContainers(tenStrings));
		}

		private IEnumerable<Container> GenerateContainers(IEnumerable<string> names)
		{
			foreach (string name in names)
			{
				yield return new Container(name, null);
			}
		}

		private static IEnumerable<int> Range(int begin, int end)
		{
			for (int i=begin; i<end; ++i)
			{
				yield return i;
			}
		}

		private static IEnumerable<string> GenerateStrings(int count)
		{
			if (count < 0) throw new ArgumentOutOfRangeException("count");
			for (int i = 0; i < count; i++)
			{
				yield return "string " + i;
			}
		}
	}
}