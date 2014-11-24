/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Untypedhierarchy;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Untypedhierarchy
{
	/// <summary>
	/// epaul:
	/// Shows a bug.
	/// </summary>
	/// <remarks>
	/// epaul:
	/// Shows a bug.
	/// carlrosenberger:
	/// Fixed!
	/// The error was due to the the behaviour of STCompare.java.
	/// It compared the syntetic fields in inner classes also.
	/// I changed the behaviour to neglect all fields that
	/// contain a "$".
	/// </remarks>
	/// <author><a href="mailto:Paul-Ebermann@gmx.de">Paul Ebermann</a></author>
	/// <version>0.1</version>
	public class STInnerClassesTestCase : SodaBaseTestCase
	{
		public class Parent
		{
			public object child;

			public Parent(STInnerClassesTestCase _enclosing, object o)
			{
				this._enclosing = _enclosing;
				// Generierter package-Name
				this.child = o;
			}

			public override string ToString()
			{
				return "Parent[" + this.child + "]";
			}

			public Parent(STInnerClassesTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly STInnerClassesTestCase _enclosing;
		}

		public class Child
		{
			public object childFirst;

			public Child(STInnerClassesTestCase _enclosing, object o)
			{
				this._enclosing = _enclosing;
				this.childFirst = o;
			}

			public override string ToString()
			{
				return "Child[" + this.childFirst + "]";
			}

			public Child(STInnerClassesTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly STInnerClassesTestCase _enclosing;
		}

		public override object[] CreateData()
		{
			return new object[] { new STInnerClassesTestCase.Parent(this, new STInnerClassesTestCase.Child
				(this, "Example")), new STInnerClassesTestCase.Parent(this, new STInnerClassesTestCase.Child
				(this, "no Example")) };
		}

		/// <summary>Only</summary>
		public virtual void TestNothing()
		{
			IQuery q = NewQuery();
			q.Descend("child");
			SodaTestUtil.Expect(q, _array);
		}
		//SodaTest.log(q);
		// STSomeClasses
	}
}
