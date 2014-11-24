/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Tests.Common.Qlin;

namespace Db4objects.Db4o.Tests.Common.Qlin
{
	/// <summary>
	/// Syntax and implementation of QLin were inspired by:
	/// http://www.h2database.com/html/jaqu.html
	/// </summary>
	public class BasicQLinTestCase
	{
		private IQLinable Db()
		{
			// disabled for now, we removed QLinable from the 8.0 ObjectContainer interface
			return null;
		}

		private void StoreAll(IList expected)
		{
			for (IEnumerator objIter = expected.GetEnumerator(); objIter.MoveNext(); )
			{
				object obj = objIter.Current;
			}
		}

		// store(obj);
		public virtual void TestFromSelect()
		{
			StoreAll(OccamAndZora());
			IteratorAssert.SameContent(OccamAndZora(), Db().From(typeof(BasicQLinTestCase.Cat
				)).Select());
		}

		public virtual void TestWhereFieldNameAsString()
		{
			StoreAll(OccamAndZora());
			IteratorAssert.SameContent(Occam(), Db().From(typeof(BasicQLinTestCase.Cat)).Where
				("name").Equal("Occam").Select());
		}

		public virtual void TestWherePrototypeFieldIsString()
		{
			StoreAll(OccamAndZora());
			IteratorAssert.SameContent(Occam(), Db().From(typeof(BasicQLinTestCase.Cat)).Where
				(((BasicQLinTestCase.Cat)QLinSupport.P(typeof(BasicQLinTestCase.Cat))).Name()).Equal
				("Occam").Select());
		}

		public virtual void TestWherePrototypeFieldStartsWith()
		{
			StoreAll(OccamAndZora());
			IteratorAssert.SameContent(Occam(), Db().From(typeof(BasicQLinTestCase.Cat)).Where
				(((BasicQLinTestCase.Cat)QLinSupport.P(typeof(BasicQLinTestCase.Cat))).Name()).StartsWith
				("Occ").Select());
		}

		public virtual void TestField()
		{
			StoreAll(OccamAndZora());
			IteratorAssert.SameContent(Occam(), Db().From(typeof(BasicQLinTestCase.Cat)).Where
				(QLinSupport.Field("name")).Equal("Occam").Select());
		}

		public virtual void TestWherePrototypeFieldIsPrimitiveInt()
		{
			StoreAll(OccamAndZora());
			IteratorAssert.SameContent(Occam(), Db().From(typeof(BasicQLinTestCase.Cat)).Where
				(((BasicQLinTestCase.Cat)QLinSupport.P(typeof(BasicQLinTestCase.Cat))).age).Equal
				(7).Select());
		}

		public virtual void TestWherePrototypeFieldIsSmaller()
		{
			StoreAll(OccamAndZora());
			IteratorAssert.SameContent(Zora(), Db().From(typeof(BasicQLinTestCase.Cat)).Where
				(((BasicQLinTestCase.Cat)QLinSupport.P(typeof(BasicQLinTestCase.Cat))).age).Smaller
				(7).Select());
		}

		public virtual void TestWherePrototypeFieldIsGreater()
		{
			StoreAll(OccamAndZora());
			IteratorAssert.SameContent(OccamAndZora(), Db().From(typeof(BasicQLinTestCase.Cat
				)).Where(((BasicQLinTestCase.Cat)QLinSupport.P(typeof(BasicQLinTestCase.Cat))).age
				).Greater(5).Select());
		}

		public virtual void TestLimit()
		{
			StoreAll(OccamAndZora());
			Assert.AreEqual(1, Db().From(typeof(BasicQLinTestCase.Cat)).Limit(1).Select().Count
				);
		}

		public virtual void TestPredefinedPrototype()
		{
			StoreAll(OccamAndZora());
			BasicQLinTestCase.Cat cat = ((BasicQLinTestCase.Cat)QLinSupport.Prototype(typeof(
				BasicQLinTestCase.Cat)));
			IteratorAssert.SameContent(Occam(), Db().From(typeof(BasicQLinTestCase.Cat)).Where
				(cat.Name()).StartsWith("Occ").Select());
		}

		public virtual void TestQueryingByInterface()
		{
			StoreAll(OccamAndIsetta());
			BasicQLinTestCase.Dog dog = ((BasicQLinTestCase.Dog)QLinSupport.Prototype(typeof(
				BasicQLinTestCase.Dog)));
			BasicQLinTestCase.Cat cat = ((BasicQLinTestCase.Cat)QLinSupport.Prototype(typeof(
				BasicQLinTestCase.Cat)));
			AssertQuery(Isetta(), dog, "Isetta");
			AssertQuery(Occam(), cat, "Occam");
		}

		public virtual void TestTwoLevelField()
		{
			StoreAll(OccamZoraAchatAcrobat());
		}

		public virtual void TestWhereAsNativeQuery()
		{
			StoreAll(OccamAndZora());
			BasicQLinTestCase.Cat cat = ((BasicQLinTestCase.Cat)QLinSupport.Prototype(typeof(
				BasicQLinTestCase.Cat)));
		}

		//		IteratorAssert.sameContent(occam(),
		//			db().from(Cat.class)
		//				.where(cat.name().equals("Occam"))
		//				.select());
		public virtual void TestUpdate()
		{
			StoreAll(OccamZoraAchatAcrobat());
			int newAge = 2;
			BasicQLinTestCase.Cat cat = ((BasicQLinTestCase.Cat)QLinSupport.Prototype(typeof(
				BasicQLinTestCase.Cat)));
			//		db().from(Cat.class)
			//		   .where(cat.father()).equal("Occam")
			//		   .update(cat.age(newAge));
			IObjectSet updated = Db().From(typeof(BasicQLinTestCase.Cat)).Where(cat.Name()).Equal
				("Occam").Select();
			IEnumerator i = updated.GetEnumerator();
		}

		//		while(i.hasNext()){
		//			Assert.areEqual(newAge, i.next().age());
		//		}
		public virtual void TestExecute()
		{
			StoreAll(OccamZoraAchatAcrobat());
			BasicQLinTestCase.Cat cat = ((BasicQLinTestCase.Cat)QLinSupport.Prototype(typeof(
				BasicQLinTestCase.Cat)));
		}

		//		db().from(Cat.class)
		//		  .where(cat.name()).startsWith("Zor")
		//		  .execute(cat.feed());
		private IList OccamZoraAchatAcrobat()
		{
			return Family(new BasicQLinTestCase.Cat("Occam", 7), new BasicQLinTestCase.Cat("Zora"
				, 6), new BasicQLinTestCase.Cat[] { new BasicQLinTestCase.Cat("Achat", 1), new BasicQLinTestCase.Cat
				("Acrobat", 1) });
		}

		private IList Family(BasicQLinTestCase.Cat father, BasicQLinTestCase.Cat mother, 
			BasicQLinTestCase.Cat[] children)
		{
			IList list = new ArrayList();
			list.Add(father);
			list.Add(mother);
			for (int childIndex = 0; childIndex < children.Length; ++childIndex)
			{
				BasicQLinTestCase.Cat child = children[childIndex];
				child.father = father;
				child.mother = mother;
				father.children.Add(child);
				mother.children.Add(child);
			}
			father.Spouse(mother);
			return list;
		}

		public virtual void AssertQuery(IList expected, BasicQLinTestCase.IPet pet, string
			 name)
		{
			IteratorAssert.SameContent(expected, Db().From(pet.GetType()).Where(pet.Name()).Equal
				(name).Select());
		}

		private IList OccamAndZora()
		{
			IList list = new ArrayList();
			BasicQLinTestCase.Cat occam = new BasicQLinTestCase.Cat("Occam", 7);
			BasicQLinTestCase.Cat zora = new BasicQLinTestCase.Cat("Zora", 6);
			occam.Spouse(zora);
			list.Add(occam);
			list.Add(zora);
			return list;
		}

		private IList Occam()
		{
			return SingleCat("Occam");
		}

		private IList Zora()
		{
			return SingleCat("Zora");
		}

		private IList Isetta()
		{
			return SingleDog("Isetta");
		}

		private IList OccamAndIsetta()
		{
			IList list = new ArrayList();
			list.Add(new BasicQLinTestCase.Cat("Occam"));
			list.Add(new BasicQLinTestCase.Dog("Isetta"));
			return list;
		}

		private IList SingleCat(string name)
		{
			IList list = new ArrayList();
			list.Add(new BasicQLinTestCase.Cat(name));
			return list;
		}

		private IList SingleDog(string name)
		{
			IList list = new ArrayList();
			list.Add(new BasicQLinTestCase.Dog(name));
			return list;
		}

		public class Cat : BasicQLinTestCase.IPet
		{
			public int age;

			public string name;

			public BasicQLinTestCase.Cat spouse;

			public BasicQLinTestCase.Cat father;

			public BasicQLinTestCase.Cat mother;

			public IList children = new ArrayList();

			public Cat()
			{
			}

			public Cat(string name)
			{
				this.name = name;
			}

			public Cat(string name, int age) : this(name)
			{
				this.age = age;
			}

			public virtual string Name()
			{
				return name;
			}

			public virtual void Spouse(BasicQLinTestCase.Cat spouse)
			{
				this.spouse = spouse;
				spouse.spouse = this;
			}

			public virtual BasicQLinTestCase.Cat Father()
			{
				return father;
			}

			public virtual BasicQLinTestCase.Cat Mother()
			{
				return mother;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is BasicQLinTestCase.Cat))
				{
					return false;
				}
				BasicQLinTestCase.Cat other = (BasicQLinTestCase.Cat)obj;
				if (name == null)
				{
					return other.name == null;
				}
				return name.Equals(other.name);
			}

			public virtual int Age()
			{
				return age;
			}

			public virtual void Age(int newAge)
			{
				age = newAge;
			}

			public virtual void Feed()
			{
				Sharpen.Runtime.Out.WriteLine(name + ": 'Thanks for all the fish.'");
			}
		}

		public class Dog : BasicQLinTestCase.IPet
		{
			private string _name;

			public Dog()
			{
			}

			public Dog(string name)
			{
				_name = name;
			}

			public virtual string Name()
			{
				return _name;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is BasicQLinTestCase.Dog))
				{
					return false;
				}
				BasicQLinTestCase.Dog other = (BasicQLinTestCase.Dog)obj;
				if (_name == null)
				{
					return other._name == null;
				}
				return _name.Equals(other._name);
			}
		}

		public interface IPet
		{
			string Name();
		}
	}
}
#endif // !SILVERLIGHT
