/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>base class for Diagnostic messages</summary>
	public abstract class DiagnosticBase : IDiagnostic
	{
		/// <summary>returns the reason for the message</summary>
		public abstract object Reason();

		/// <summary>returns the potential problem that triggered the message</summary>
		public abstract string Problem();

		/// <summary>suggests a possible solution for the possible problem</summary>
		public abstract string Solution();

		public override string ToString()
		{
			return ":: db4o " + Db4oVersion.Name + " Diagnostics ::\n  " + Reason() + " :: " 
				+ Problem() + "\n    " + Solution();
		}
	}
}
