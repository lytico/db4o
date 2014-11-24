/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>listens to Diagnostic messages.</summary>
	/// <remarks>
	/// listens to Diagnostic messages.
	/// <br/><br/>Create a class that implements this listener interface and add
	/// the listener by calling <code>commonConfig.Diagnostic.AddListener()</code>.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Diagnostic.IDiagnosticConfiguration">Db4objects.Db4o.Diagnostic.IDiagnosticConfiguration
	/// </seealso>
	public interface IDiagnosticListener
	{
		/// <summary>this method will be called with Diagnostic messages.</summary>
		/// <remarks>this method will be called with Diagnostic messages.</remarks>
		void OnDiagnostic(IDiagnostic d);
	}
}
