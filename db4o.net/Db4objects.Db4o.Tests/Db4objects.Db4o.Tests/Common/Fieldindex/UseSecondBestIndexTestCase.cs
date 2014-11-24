/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class UseSecondBestIndexTestCase : AbstractDb4oTestCase
	{
		internal bool loadedFromClassIndex;

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(UseSecondBestIndexTestCase.Parent)).ObjectField("id").Indexed
				(true);
			config.ObjectClass(typeof(UseSecondBestIndexTestCase.Child)).ObjectField("id").Indexed
				(true);
			config.Diagnostic().AddListener(new _IDiagnosticListener_17(this));
		}

		private sealed class _IDiagnosticListener_17 : IDiagnosticListener
		{
			public _IDiagnosticListener_17(UseSecondBestIndexTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (d is LoadedFromClassIndex)
				{
					this._enclosing.loadedFromClassIndex = true;
				}
			}

			private readonly UseSecondBestIndexTestCase _enclosing;
		}

		public class Parent
		{
			public UseSecondBestIndexTestCase.Child child;

			public int id;

			public Parent(int id)
			{
				this.id = id;
			}
		}

		public class Child
		{
			public int id;

			public Child(int id)
			{
				this.id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new UseSecondBestIndexTestCase.Parent(42));
			UseSecondBestIndexTestCase.Parent parent2 = new UseSecondBestIndexTestCase.Parent
				(42);
			parent2.child = new UseSecondBestIndexTestCase.Child(42);
			Store(parent2);
		}

		public virtual void TestUsingIndex()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(UseSecondBestIndexTestCase.Parent));
			q.Descend("id").Constrain(42);
			q.Descend("child").Descend("id").Constrain(42);
			q.Execute();
			Assert.IsFalse(loadedFromClassIndex);
		}
	}
}
