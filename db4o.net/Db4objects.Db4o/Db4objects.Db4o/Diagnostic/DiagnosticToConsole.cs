/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>prints Diagnostic messsages to the Console.</summary>
	/// <remarks>
	/// prints Diagnostic messages to the Console.
	/// Install this
	/// <see cref="Db4objects.Db4o.Diagnostic.IDiagnosticListener">Db4objects.Db4o.Diagnostic.IDiagnosticListener
	/// </see>
	/// with: <br/>
	/// <code>commonConfig.Diagnostic.AddListener(new DiagnosticToConsole());</code><br/>
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Diagnostic.IDiagnosticConfiguration">Db4objects.Db4o.Diagnostic.IDiagnosticConfiguration
	/// </seealso>
	public class DiagnosticToConsole : IDiagnosticListener
	{
		/// <summary>redirects Diagnostic messages to the Console.</summary>
		/// <remarks>redirects Diagnostic messages to the Console.</remarks>
		public virtual void OnDiagnostic(IDiagnostic d)
		{
			Sharpen.Runtime.Out.WriteLine(d.ToString());
		}
	}
}
