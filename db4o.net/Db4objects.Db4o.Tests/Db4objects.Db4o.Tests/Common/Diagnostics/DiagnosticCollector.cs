/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class DiagnosticCollector : IDiagnosticListener
	{
		private IList _diagnostics = new ArrayList();

		public virtual void OnDiagnostic(IDiagnostic d)
		{
			_diagnostics.Add(d);
		}

		public virtual IList Diagnostics()
		{
			return _diagnostics;
		}
	}
}
