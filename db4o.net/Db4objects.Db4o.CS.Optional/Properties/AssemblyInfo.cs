/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("db4o - optional cs functionality")]
[assembly: AssemblyCompany("Versant Corp., Redwood City, CA, USA")]
[assembly: AssemblyProduct("db4o - database for objects")]
[assembly: AssemblyCopyright("Versant Corp. 2000 - 2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture ("")]

[assembly: ComVisible (false)]

[assembly: AssemblyVersion("8.1.269.14430")]

#if !CF && !SILVERLIGHT
[assembly: AllowPartiallyTrustedCallers]
#endif

[assembly: CLSCompliant(true)]