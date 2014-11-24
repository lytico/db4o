/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Joins.Typed;

namespace Db4objects.Db4o.Tests.Common.Soda.Joins.Typed
{
	public class JointEqualsIdentityTestCase : AbstractDb4oTestCase
	{
		public class TestSubject
		{
			public string _name;

			public JointEqualsIdentityTestCase.TestSubject _child;

			public TestSubject(string name, JointEqualsIdentityTestCase.TestSubject child)
			{
				_name = name;
				_child = child;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			JointEqualsIdentityTestCase.TestSubject subjectA = new JointEqualsIdentityTestCase.TestSubject
				("A", null);
			JointEqualsIdentityTestCase.TestSubject subjectB = new JointEqualsIdentityTestCase.TestSubject
				("B", subjectA);
			JointEqualsIdentityTestCase.TestSubject subjectC = new JointEqualsIdentityTestCase.TestSubject
				("C", subjectA);
			Store(subjectA);
			Store(subjectB);
			Store(subjectC);
		}

		public virtual void TestJointEqualsIdentity()
		{
			JointEqualsIdentityTestCase.TestSubject child = RetrieveChild();
			IQuery query = NewQuery(typeof(JointEqualsIdentityTestCase.TestSubject));
			IConstraint constraint = query.Descend("_name").Constrain("B").Equal();
			constraint.And(query.Descend("_child").Constrain(child).Identity());
			Assert.AreEqual(1, query.Execute().Count);
		}

		private JointEqualsIdentityTestCase.TestSubject RetrieveChild()
		{
			IQuery query = NewQuery(typeof(JointEqualsIdentityTestCase.TestSubject));
			query.Descend("_child").Constrain(null);
			return (JointEqualsIdentityTestCase.TestSubject)query.Execute().Next();
		}
	}
}
