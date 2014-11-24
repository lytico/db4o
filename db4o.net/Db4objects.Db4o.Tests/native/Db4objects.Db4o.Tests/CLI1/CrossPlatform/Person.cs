using System;
using System.Collections.Generic;

namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	public class Person
	{
		private static IComparer<Person> _sortByYear = new SortByYearImpl();

		private string _name;
		private int _year;
		private DateTime _localReleaseDate;

		public Person(string name, int year, DateTime localReleaseDate)
		{
			_name = name;
			_year = year;
			_localReleaseDate = localReleaseDate;
		}

		public string Name
		{
			get { return _name; }
		}

		public int Year
		{
			get { return _year; }
		}

		public DateTime LocalReleaseDate
		{
			get { return _localReleaseDate; }
		}

		public override bool Equals(object obj)
		{
			Person candidate = obj as Person;
			if (candidate == null) return false;

			if (candidate.GetType() != GetType()) return false;

			// FIXME: Dates are not working correctly yet.
			//return _name == candidate.Name && _year == candidate.Year && _localReleaseDate == candidate.LocalReleaseDate;
			return _name == candidate.Name && _year == candidate.Year;
		}

		public static IComparer<Person> SortByYear
		{
			get
			{
				return _sortByYear;
			}
		}

		public override string ToString()
		{
			//FIXME: Dates not working
			return _name + "|" + _year; // +"|" + _localReleaseDate;
		}

		private sealed class SortByYearImpl : IComparer<Person>
		{
			#region IComparer<Person> Members

			public int Compare(Person lhs, Person rhs)
			{
				return lhs._year - rhs._year;
			}

			#endregion
		}
	}
}
