/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.Diagnostics;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class DiagnosticsTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Diagnostic().AddListener(new _IDiagnosticListener_15());
		}

		private sealed class _IDiagnosticListener_15 : IDiagnosticListener
		{
			public _IDiagnosticListener_15()
			{
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (!(d is NotTransparentActivationEnabled))
				{
					Assert.Fail("no diagnostic message expected but was " + d);
				}
			}
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		public virtual void TestNoDiagnosticsForInternalClasses()
		{
			Store(new DiagnosticsTestCase.Item("one"));
			RetrieveOnlyInstance(typeof(DiagnosticsTestCase.Item));
		}
	}
}
