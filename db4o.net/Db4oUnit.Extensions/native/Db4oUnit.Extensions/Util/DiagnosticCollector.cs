/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using System.Text;
using Db4objects.Db4o.Diagnostic;

namespace Db4oUnit.Extensions.Util
{
	public class DiagnosticCollector<T> : IDiagnosticListener
	{
		public void OnDiagnostic(IDiagnostic d)
		{
			if (typeof(T) == d.GetType())
			{
				_diagnostics.Add(d);
			}
		}

		public IList<IDiagnostic> Diagnostics
		{
			get { return _diagnostics; }
		}

		public bool Empty
		{
			get { return _diagnostics.Count == 0; }
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (IDiagnostic diagnostic in _diagnostics)
			{
				sb.Append(diagnostic + "\r\n");
			}

			return sb.ToString();
		}

		private readonly IList<IDiagnostic> _diagnostics = new List<IDiagnostic>();
	}
}
