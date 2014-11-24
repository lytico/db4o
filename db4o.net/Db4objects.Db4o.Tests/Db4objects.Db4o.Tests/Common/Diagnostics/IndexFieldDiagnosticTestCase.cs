/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Diagnostics;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class IndexFieldDiagnosticTestCase : AbstractDb4oTestCase
	{
		private bool _diagnosticsCalled;

		public class Car
		{
			public string model;

			public IList history;

			public Car(string model) : this(model, new ArrayList())
			{
			}

			public Car(string model, IList history)
			{
				this.model = model;
				this.history = history;
			}

			public virtual string GetModel()
			{
				return model;
			}

			public virtual IList GetHistory()
			{
				return history;
			}

			public override string ToString()
			{
				return model;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			IndexFieldDiagnosticTestCase.Car car = new IndexFieldDiagnosticTestCase.Car("BMW"
				);
			Store(car);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Diagnostic().AddListener(new _IDiagnosticListener_51(this));
		}

		private sealed class _IDiagnosticListener_51 : IDiagnosticListener
		{
			public _IDiagnosticListener_51(IndexFieldDiagnosticTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (d is LoadedFromClassIndex)
				{
					this._enclosing._diagnosticsCalled = true;
				}
			}

			private readonly IndexFieldDiagnosticTestCase _enclosing;
		}

		public virtual void TestNonIndexedFieldQuery()
		{
			IQuery query = NewQuery(typeof(IndexFieldDiagnosticTestCase.Car));
			query.Descend("model").Constrain("BMW");
			query.Execute();
			Assert.IsTrue(_diagnosticsCalled);
		}

		public virtual void TestClassQuery()
		{
			Db().Query(typeof(IndexFieldDiagnosticTestCase.Car));
			Assert.IsFalse(_diagnosticsCalled);
		}
	}
}
