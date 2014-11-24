/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class ObjectFieldDoesNotExistTestCase : AbstractDb4oTestCase
	{
		private static readonly string BogusFieldName = "bogusField";

		private bool _diagnosticCalled = false;

		public class Item
		{
			public string _name;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Diagnostic().AddListener(new _IDiagnosticListener_24(this));
			config.ObjectClass(typeof(ObjectFieldDoesNotExistTestCase.Item)).ObjectField(BogusFieldName
				).Indexed(true);
			config.ObjectClass(typeof(ObjectFieldDoesNotExistTestCase.Item)).ObjectField("_name"
				).Indexed(true);
		}

		private sealed class _IDiagnosticListener_24 : IDiagnosticListener
		{
			public _IDiagnosticListener_24(ObjectFieldDoesNotExistTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (d is ObjectFieldDoesNotExist)
				{
					ObjectFieldDoesNotExist message = (ObjectFieldDoesNotExist)d;
					Assert.AreEqual(ObjectFieldDoesNotExistTestCase.BogusFieldName, message._fieldName
						);
					this._enclosing._diagnosticCalled = true;
				}
			}

			private readonly ObjectFieldDoesNotExistTestCase _enclosing;
		}

		public virtual void Test()
		{
			Store(new ObjectFieldDoesNotExistTestCase.Item());
			Assert.IsTrue(_diagnosticCalled);
		}

		public static void Main(string[] args)
		{
			new ObjectFieldDoesNotExistTestCase().RunNetworking();
		}
	}
}
