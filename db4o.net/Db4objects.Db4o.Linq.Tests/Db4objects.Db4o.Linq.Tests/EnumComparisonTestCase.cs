/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;

namespace Db4objects.Db4o.Linq.Tests
{
    public class EnumComparisonTestCase : AbstractDb4oLinqTestCase
    {
        #region Test Subject

        public enum Sex
        {
            Undefined,
            Male,
            Female,
        }

        public enum Style : ulong
        {
            Undefined,
            Fashion,
            Old,
            Traditional,
        }
        
        public class Person
        {
            public Person(string name, Sex sex, Style style)
            {
                _name = name;
                _sex = sex;
                _style = style;

            	StyleField = _style;
            }

            public string Name
            {
                get { return _name; }
            }

            public Sex Sex
            {
                get { return _sex; }
            }

            public Style Style
            {
                get { return _style; }
            }

            public override string ToString()
            {
                return _name + " / " + _sex + " / " + _style;
            }

            public override bool Equals(object obj)
            {
                var other = obj as Person;
                if (other == null) return false;

                if (other.GetType() != GetType()) return false;

                return _name == other.Name && _sex == other.Sex && _style == other.Style;
            }

            public override int GetHashCode()
            {
                return _name.GetHashCode() ^ _sex.GetHashCode() ^ _style.GetHashCode();
            }

        	public Style StyleField;

            public string _name;
            public Sex _sex;
            public Style _style;
        }

        private static Person[] Persons = new[] 
                                            {
                                                new Person("Gi", Sex.Female, Style.Fashion),
                                                new Person("Adriano", Sex.Male, Style.Traditional),
                                                new Person("Carol", Sex.Female, Style.Fashion),
                                                new Person("Tânia", Sex.Female, Style.Traditional),
                                                new Person("Zé", Sex.Male, Style.Old),
                                                new Person("Gi", Sex.Male, Style.Fashion),
                                                new Person("Adriano", Sex.Male, Style.Traditional),
                                            };


        #endregion

        protected override void Store()
        {
            foreach (Person p in Persons)
            {
                Store(p);
            }
        }

        #region Tests

        public void TestSex()
        {
            AssertSex(Sex.Male);
            AssertSex(Sex.Female);
        }

        public void TestBackingEnumOnUlong()
        {
            AssertStyle(Style.Fashion);
            AssertStyle(Style.Old);
            AssertStyle(Style.Traditional);
        }

        public void TestMixedAndConditions()
        {
            AssertStyleAndName(Style.Fashion, "Gi");
            AssertStyleAndName(Style.Traditional, "Adriano");
            AssertStyleAndName(Style.Fashion, "Carol");
            AssertStyleAndName(Style.Fashion, "Adriano");
        }

        public void _TestMixedOrConditions()
        {
            AssertStyleOrName(Style.Fashion, "Adriano");
        }

		public void TestDirectFieldAccess()
		{
			AssertQuery("(Person(StyleField == Fashion))",
				delegate
				{
					var actual = from Person p in Db()
								 where p.StyleField == Style.Fashion
								 select p;

					AssertSet(MatchingPersons(p => p.StyleField == Style.Fashion), actual);
				});
			
		}

        #endregion

        #region Helper Methods

        private void AssertStyleOrName(Style style, string name)
        {
            string expectedQuery = String.Format("(Person((_style == {0}) or (_name == '{1}')))", style, name);

            AssertQuery(expectedQuery,
                delegate
                {
                    var actual = from Person p in Db()
                                 where p.Style == style || p.Name == name
                                 select p;

                    AssertSet(MatchingPersons(p => p.Style == style || p.Name == name), actual);
                });
        }

        private void AssertStyleAndName(Style style, string name)
        {
            string expectedQuery = String.Format("(Person((_style == {0}) and (_name == '{1}')))", style, name);

            AssertQuery(expectedQuery,
                delegate
                {
                    var actual = from Person p in Db()
                                 where p.Style == style && p.Name == name
                                 select p;

                    AssertSet(MatchingPersons(p => p.Style == style && p.Name == name), actual);
                });
        }

        private void AssertStyle(Style style)
        {
            string expectedQuery = String.Format("(Person(_style == {0}))", style);

            AssertQuery(expectedQuery,
                delegate
                {
                    var actual = from Person p in Db()
                                 where p.Style == style
                                 select p;

                    AssertSet(MatchingPersons(p => p.Style == style), actual);
                });
        }

        private void AssertSex(Sex sex)
        {
            string expectedQuery = String.Format("(Person(_sex == {0}))", sex);

            AssertQuery(expectedQuery,
                delegate
                {
                    var actual = from Person p in Db()
                                 where p.Sex == sex
                                 select p;

                    AssertSet(MatchingPersons(p => p.Sex == sex), actual);
                });
        }

        private static IEnumerable<Person> MatchingPersons(Func<Person, bool> predicate)
        {
            foreach (Person candidate in Persons)
            {
                if (predicate(candidate)) yield return candidate;
            }
        }

        #endregion
    }
}
