/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */


using System.Collections;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
	public enum CsEnumState
	{
		None,
		Open,
		Running,
		Closed
	}

	/// <summary>
	/// enums
	/// </summary>
	public class CsEnum : AbstractDb4oTestCase
	{
		public CsEnumState _state;

		public CsEnum()
		{
		}

		public CsEnum(CsEnumState state)
		{
			_state = state;
		}

		public CsEnumState State
		{
			get
			{
				return _state;
			}

			set
			{
				_state = value;
			}
		}

		override protected void Store()
		{
			Store(new CsEnum(CsEnumState.Open));
			Store(new CsEnum(CsEnumState.Closed));
			Store(new CsEnum(CsEnumState.Running));
		}

		public void TestValueConstrain()
		{
			IQuery q = NewQuery(typeof(CsEnum));
			IObjectSet os = q.Execute();
			Assert.AreEqual(3, os.Count);

			TstQueryByEnum(CsEnumState.Open);
			TstQueryByEnum(CsEnumState.Closed);
		}

		public void TestOrConstrain()
		{
			IQuery q = NewQuery(typeof(CsEnum));
			q.Descend("_state").Constrain(CsEnumState.Open).Or(
				q.Descend("_state").Constrain(CsEnumState.Running));
			
			EnsureObjectSet(q.Execute(), CsEnumState.Open, CsEnumState.Running);
		}

		public void TestQBE()
		{
			TstQBE(3, CsEnumState.None); // None is the zero/uninitialized value
			TstQBE(1, CsEnumState.Closed);
			TstQBE(1, CsEnumState.Open);
			TstQBE(1, CsEnumState.Running);
		}

		private void TstQBE(int expectedCount, CsEnumState value)
		{
			IObjectSet os = Db().QueryByExample(new CsEnum(value));
			Assert.AreEqual(expectedCount, os.Count);
		}

		private void EnsureObjectSet(IObjectSet os, params CsEnumState[] expected)
		{
			Assert.AreEqual(expected.Length, os.Count);
			ArrayList l = new ArrayList();
			while (os.HasNext())
			{
				l.Add(((CsEnum)os.Next()).State);
			}
			
			foreach (CsEnumState e in expected)
			{	
				Assert.IsTrue(l.Contains(e));
				l.Remove(e);
			}
		}

		void TstQueryByEnum(CsEnumState template)
		{
			IQuery q = NewQuery(typeof(CsEnum));
			q.Descend("_state").Constrain(template);

			IObjectSet os = q.Execute();
			Assert.AreEqual(1, os.Count);
			Assert.AreEqual(template, ((CsEnum)os.Next()).State);
		}
	}
}
