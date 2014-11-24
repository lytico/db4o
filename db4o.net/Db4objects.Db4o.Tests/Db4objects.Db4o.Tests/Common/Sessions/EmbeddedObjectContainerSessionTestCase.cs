/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Sessions;

namespace Db4objects.Db4o.Tests.Common.Sessions
{
	public class EmbeddedObjectContainerSessionTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override bool Equals(object obj)
			{
				if (GetType() != obj.GetType())
				{
					return false;
				}
				EmbeddedObjectContainerSessionTestCase.Item other = (EmbeddedObjectContainerSessionTestCase.Item
					)obj;
				if (_name == null)
				{
					if (other._name != null)
					{
						return false;
					}
				}
				else
				{
					if (!_name.Equals(other._name))
					{
						return false;
					}
				}
				return true;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			EmbeddedObjectContainerSessionTestCase.Item item = new EmbeddedObjectContainerSessionTestCase.Item
				("one");
			Store(item);
		}

		public virtual void TestIsolationAgainstMainObjectContainer()
		{
			AssertIsolation(Db(), OpenSession());
		}

		public virtual void TestIsolationBetweenSessions()
		{
			AssertIsolation(OpenSession(), OpenSession());
		}

		private IObjectContainer OpenSession()
		{
			return Db().Ext().OpenSession();
		}

		private void AssertIsolation(IObjectContainer session1, IObjectContainer session2
			)
		{
			EmbeddedObjectContainerSessionTestCase.Item originalItem = RetrieveItem(session1);
			EmbeddedObjectContainerSessionTestCase.Item sessionItem = RetrieveItem(session2);
			Assert.AreNotSame(originalItem, sessionItem);
			Assert.AreEqual(originalItem, sessionItem);
		}

		private EmbeddedObjectContainerSessionTestCase.Item RetrieveItem(IObjectContainer
			 session)
		{
			IQuery query = session.Query();
			query.Constrain(typeof(EmbeddedObjectContainerSessionTestCase.Item));
			IObjectSet objectSet = query.Execute();
			EmbeddedObjectContainerSessionTestCase.Item sessionItem = ((EmbeddedObjectContainerSessionTestCase.Item
				)objectSet.Next());
			return sessionItem;
		}
	}
}
