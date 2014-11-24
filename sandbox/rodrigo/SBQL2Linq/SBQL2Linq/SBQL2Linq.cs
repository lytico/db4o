using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SBQL2Linq
{
	class Address
	{
		public string City { get; set; }
	}

	class Emp
	{
		public string Name { get; set; }
		public Address Address { get; set; }
		public double Sal { get; set; }
		public Dept WorksIn { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}

	class Dept
	{
		private List<Emp> _employs;
		private Emp _boss;

		public string Name { get; set; }
		public double Budget { get; set; }
		public List<string> Loc { get; set; }

		public Emp Boss
		{
			get
			{
				return _boss;
			}

			set
			{
				_boss = value;
				_boss.WorksIn = this;
			}
		}

		public List<Emp> Employs
		{
			get
			{
				return _employs;
			}

			set
			{
				_employs = value;
				foreach (var e in _employs)
				{
					e.WorksIn = this;
				}
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}

	class SBQL2Linq
	{
		static void Main(string[] args)
		{
			var employees = new[]
			{
				new Emp
				{
					Name = "John Cleese",
					Sal = 2223,
					Address = new Address { City = "Bridgwater" }
				},

				new Emp
				{
					Name = "Michael Palin",
					Sal = 2221,
					Address = new Address { City = "Broomhill" }
				},

				new Emp
				{
					Name = "Eric Idle",
					Sal = 2000,
					Address = new Address { City = "Runnymede" }
				},

				new Emp
				{
					Name = "Terry Gilliam",
					Sal = 3000,
					//Address = new Address { City = "Dorset" }
				},

				new Emp
				{
					Name = "Graham Chapman",
					Sal = 2000,
					Address = new Address { City = "Dorset" }
				},
			};

			var depts = new[]
			{
				new Dept
				{
					Name = "Ministry of Silly Walks",
					Boss = employees[0],
					Budget = 500000,
					Loc = new List<String>
					{
						"Bridgwater",
						"Rome",
					},
					Employs = new List<Emp>
					{
						employees[0],
						employees[2],
						employees[3]
					}
				},

				new Dept
				{
					Name = "Parrot Store",
					Boss = employees[1],
					Budget = 100000,
					Loc = new List<string>
					{
						"Runnymede",
						"Rome",
					},
					Employs = new List<Emp>
					{
						employees[1],
						employees[4]
					}
				}
			};

			// 1. Get departments together with the average salaries of their employees:
			//
			// deptemp.((Dept as d) join avg(d.employs.Emp.sal._VALUE));
			//
			// deptemp.(Dept join avg(employs.Emp.sal._VALUE));
			//

			var query1 = from d in depts
						 select new
						 {
							 Dept = d,
							 Avg = (from e in d.Employs select e.Sal).Average()
						 };

			query1.PrettyPrint();

			//
			// 2. Get name and department name for employees earning less than 2222
			//
			// deptemp.(Emp where sal._VALUE < 2222).(name._VALUE,
			// worksIn.Dept.dname._VALUE);
			//

			var query2 = from e in employees
						 where e.Sal < 2222
						 select new
						 {
							 e.Name,
							 DeptName = e.WorksIn.Name,
						 };
			query2.PrettyPrint();

			//
			// 3. Get names of employees working for the department managed by Bert.
			//
			// deptemp.(Emp where (worksIn.Dept.boss.Emp.name._VALUE) = "Bert").
			// name._VALUE;
			//

			var query3 = from e in employees
						 where e.WorksIn.Boss.Name == "John Cleese" /* && e.Dept.Boss != e */
						 select new
						 {
							 Name = e.Name
						 };
			query3.PrettyPrint();

			//
			// 4. Get the name of Poe's boss:
			//
			// deptemp.(Emp where name._VALUE = "Poe").worksIn.Dept.boss.Emp.name._VALUE;
			//

			var query4 = from e in employees
						 where e.Name == "Eric Idle"
						 select new
						 {
							 Name = e.WorksIn.Boss.Name
						 };
			query4.PrettyPrint();

			//
			//
			// 5.Names and cities of employees working in departments managed by Bert:
			//
			// deptemp.(Dept where (boss.Emp.name._VALUE) = "Bert").employs.Emp.
			// (name._VALUE, ((address.city._VALUE) union ("No address" where not
			// exists(address))));
			//
			// deptemp.(Dept where (boss.Emp.name._VALUE) = "Bert").employs.Emp.
			// (name._VALUE as name, (((address.city._VALUE) union
			// ("No address" where not exists(address)))) as city ) as empcity;
			//

			var query5 = from e in employees
						 where e.WorksIn.Boss.Name == "John Cleese"
						 select new
						 {
							 e.Name,
							 City = (e.Address == null ? "No address" : e.Address.City)
						 };
			query5.PrettyPrint();

			//
			// 6.Get the minimal, average and maximal number of employees in departments:
			//
			// deptemp.(min(Dept.count(employs)),
			// avg(Dept.count(employs)),
			// max(Dept.count(employs)) );
			//

			var counts = from d in depts select d.Employs.Count;
			var query6 = new
			{
				Min = counts.Min(),
				Avg = counts.Average(),
				Max = counts.Max()
			};
			query6.PrettyPrint();

			//
			// 7. For each department get its name and the sum of salaries of
			// employees being not bosses:
			//
			// deptemp.(((Dept as d) join
			// ((sum(d.employs.Emp.sal._VALUE) - (d.boss.Emp.sal._VALUE)) as s )).
			// (d.dname._VALUE, s));
			//

			var query7 = from d in depts
						 select new
						 {
							 DeptName = d.Name,
							 StaffSalary = (from e in d.Employs
											where e != d.Boss
											select e.Sal).Sum()
						 };
			query7.PrettyPrint();

			//
			// 8.Is it true that each department employs an employee earning the same as
			// his/her boss?:
			//
			// deptemp. forall (Dept as d)
			// forany ((d.employs.Emp minus d.boss.Emp) as e)
			// forany (e.sal as s) (s._VALUE = d.boss.Emp.sal._VALUE);
			//
			var query8 = new
			{
				Answer = (
					from d in depts
					select (
						from e in d.Employs
						where e != d.Boss && e.Sal == d.Boss.Sal
						select e).Any()
					).All(found => found)
			};
			query8.PrettyPrint();

			//
			// 9. For each employee get the message containing his/her name and
			// the percent of the annual budget
			// of his/her department that is consumed by his/her monthly salary:
			//
			// deptemp. Emp . ("Employee " + name._VALUE + " consumes " +
			// ((sal._VALUE * 12 * 100)/(worksIn.Dept.budget._VALUE)) + "% of the " +
			// worksIn.Dept.dname._VALUE + " department budget.");
			//			
			var query9 = from e in employees
						 select new
						 {
							 Message = "Employee " + e.Name + " consumes " +
								((e.Sal * 12 * 100) / e.WorksIn.Budget) + "% of the " +
								e.WorksIn.Name + " department budget."
						 };
			query9.PrettyPrint();

			//
			// 10. Get cities hosting all departments:
			//
			// deptemp.(unique(deref(Dept.loc._VALUE)) as deptcity)
			// where forall(deptemp.Dept)(deptcity in loc._VALUE);
			//
			var query10 = from city in depts.SelectMany(d => d.Loc).Distinct()
						   where depts.All(d => d.Loc.Contains(city))
						   select new { City = city };
			query10.PrettyPrint();
		}
	}

	static class PrettyPrintExtensions
	{
		public static void PrettyPrint<T>(this IEnumerable<T> self)
		{
			if (self == null) return;

			var properties = PrintHeader<T>();

			foreach (var item in self)
			{
				PrintProperties<T>(properties, item);
			}
			Console.WriteLine();
		}

		public static void PrettyPrint<T>(this T self)
		{
			var properties = PrintHeader<T>();
			PrintProperties(properties, self);
			Console.WriteLine();
		}

		private static void PrintProperties<T>(PropertyInfo[] properties, T item)
		{
			foreach (var p in properties)
			{
				Console.Write(p.GetValue(item, null));
				Console.Write("\t");
			}
			Console.WriteLine();
		}

		private static PropertyInfo[] PrintHeader<T>()
		{
			var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var p in properties)
			{
				Console.Write(p.Name);
				Console.Write("\t");
			}
			Console.WriteLine();

			foreach (var p in properties)
			{
				Console.Write(new string('=', p.Name.Length));
				Console.Write("\t");
			}

			Console.WriteLine();
			return properties;
		}
	}
}