/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class CascadeToArray : AbstractDb4oTestCase
	{
		public class Atom
		{
			public CascadeToArray.Atom child;

			public string name;

			public Atom()
			{
			}

			public Atom(CascadeToArray.Atom child)
			{
				this.child = child;
			}

			public Atom(string name)
			{
				this.name = name;
			}

			public Atom(CascadeToArray.Atom child, string name) : this(child)
			{
				this.name = name;
			}
		}

		public object[] objects;

		protected override void Configure(IConfiguration conf)
		{
			conf.ObjectClass(this).CascadeOnUpdate(true);
			conf.ObjectClass(this).CascadeOnDelete(true);
		}

		protected override void Store()
		{
			CascadeToArray cta = new CascadeToArray();
			cta.objects = new object[] { new CascadeToArray.Atom("stored1"), new CascadeToArray.Atom
				(new CascadeToArray.Atom("storedChild1"), "stored2") };
			Db().Store(cta);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Foreach(GetType(), new _IVisitor4_52(this));
			// This one should NOT cascade
			Reopen();
			Foreach(GetType(), new _IVisitor4_69());
			// Cascade-On-Delete Test: We only want one Atom to remain.
			Db().Commit();
			Reopen();
			IObjectSet os = NewQuery(GetType()).Execute();
			while (os.HasNext())
			{
				Db().Delete(os.Next());
			}
			Assert.AreEqual(1, CountOccurences(typeof(CascadeToArray.Atom)));
		}

		private sealed class _IVisitor4_52 : IVisitor4
		{
			public _IVisitor4_52(CascadeToArray _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object obj)
			{
				CascadeToArray cta = (CascadeToArray)obj;
				for (int i = 0; i < cta.objects.Length; i++)
				{
					CascadeToArray.Atom atom = (CascadeToArray.Atom)cta.objects[i];
					atom.name = "updated";
					if (atom.child != null)
					{
						atom.child.name = "updated";
					}
				}
				this._enclosing.Db().Store(cta);
			}

			private readonly CascadeToArray _enclosing;
		}

		private sealed class _IVisitor4_69 : IVisitor4
		{
			public _IVisitor4_69()
			{
			}

			public void Visit(object obj)
			{
				CascadeToArray cta = (CascadeToArray)obj;
				for (int i = 0; i < cta.objects.Length; i++)
				{
					CascadeToArray.Atom atom = (CascadeToArray.Atom)cta.objects[i];
					Assert.AreEqual("updated", atom.name);
					if (atom.child != null)
					{
						Assert.AreNotEqual("updated", atom.child.name);
					}
				}
			}
		}

		public static void Main(string[] arguments)
		{
			new CascadeToArray().RunSolo();
		}
	}
}
