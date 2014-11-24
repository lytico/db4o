/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>provides methods to configure the behaviour of db4o
	/// diagnostics.</summary>
	/// <remarks>
	/// provides methods to configure the behaviour of db4o diagnostics.
	/// <br/>
	/// <br/>
	/// Diagnostic system can be enabled on a running db4o database to
	/// notify a user about possible problems or misconfigurations.
	/// Diagnostic listeners can be be added and removed with calls to this
	/// interface. To install the most basic listener call:
	/// <br/>
	/// <code>commonConfig.Diagnostic.AddListener(new DiagnosticToConsole());</code>
	/// </remarks>
	/// <seealso cref="IConfiguration.Diagnostic">IConfiguration.Diagnostic
	/// </seealso>
	/// <seealso cref="IDiagnosticListener">IDiagnosticListener
	/// </seealso>
	public interface IDiagnosticConfiguration
	{
		/// <summary>adds a DiagnosticListener to listen to Diagnostic messages.</summary>
		/// <remarks>adds a DiagnosticListener to listen to Diagnostic messages.</remarks>
		void AddListener(IDiagnosticListener listener);

		/// <summary>removes all DiagnosticListeners.</summary>
		/// <remarks>removes all DiagnosticListeners.</remarks>
		void RemoveAllListeners();
	}
}
