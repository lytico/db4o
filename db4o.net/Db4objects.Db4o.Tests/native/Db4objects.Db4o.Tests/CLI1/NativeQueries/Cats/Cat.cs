/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI1.NativeQueries.Cats
{
	public class Cat : Animal
	{
		public string _firstName;

		public string _lastName;

		public int _age;

		public Cat _father;

		public Cat _mother;

		public string GetFirstName()
		{
			return _firstName;
		}

		public int GetAge()
		{
			return _age;
		}

		public string GetFullName()
		{
			return _firstName + " " + _lastName;
		}

		public Cat GetFather()
		{
			return _father;
		}
	}
}
