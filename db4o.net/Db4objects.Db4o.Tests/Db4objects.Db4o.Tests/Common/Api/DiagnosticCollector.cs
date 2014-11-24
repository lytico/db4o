/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Tests.Common.Api
{
	internal sealed class DiagnosticCollector : IDiagnosticListener
	{
		internal ArrayList _diagnostics = new ArrayList();

		public void OnDiagnostic(IDiagnostic d)
		{
			_diagnostics.Add(d);
		}

		public void Verify(object[] expected)
		{
			ArrayAssert.AreEqual(expected, Sharpen.Collections.ToArray(_diagnostics));
		}
	}
}
