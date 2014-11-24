/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Diagnostic
{
#if !CF
    /// <summary>prints Diagnostic messsages to the Console.</summary>
    /// <remarks>
    /// prints Diagnostic messsages to System.Diagnostics.Trace.
    /// Install this
    /// <see cref="Db4objects.Db4o.Diagnostic.IDiagnosticListener">Db4objects.Db4o.Diagnostic.IDiagnosticListener
    /// 	</see>
    /// with: <br />
    /// <code>commonConfig.Diagnostic.AddListener(new DiagnosticToTrace());</code><br />
    /// </remarks>
    /// <seealso cref="Db4objects.Db4o.Diagnostic.DiagnosticConfiguration">Db4objects.Db4o.Diagnostic.DiagnosticConfiguration
    /// 	</seealso>
    public class DiagnosticToTrace : Db4objects.Db4o.Diagnostic.IDiagnosticListener
    {
        /// <summary>redirects Diagnostic messages to System.Diagnostics.Trace</summary>
        /// <remarks>redirects Diagnostic messages to the Console.</remarks>
        public virtual void OnDiagnostic(Db4objects.Db4o.Diagnostic.IDiagnostic d)
        {
#if !SILVERLIGHT        	
			System.Diagnostics.Trace.WriteLine(d.ToString());
#endif
        }
    }
#endif
}
