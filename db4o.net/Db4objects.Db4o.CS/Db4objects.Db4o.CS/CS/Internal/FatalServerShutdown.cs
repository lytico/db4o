/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.CS.Internal
{
	/// <summary>
	/// This class exists to work around a decaf conversion problem
	/// when the code was directly in ServerMessageDispatcherImp.
	/// </summary>
	/// <remarks>
	/// This class exists to work around a decaf conversion problem
	/// when the code was directly in ServerMessageDispatcherImp.
	/// </remarks>
	/// <exclude></exclude>
	internal class FatalServerShutdown
	{
		internal FatalServerShutdown(ObjectServerImpl server, Exception origExc)
		{
			try
			{
				server.Close(ShutdownMode.Fatal(origExc));
			}
			catch (Exception throwable)
			{
				throw new CompositeDb4oException(new Exception[] { origExc, throwable });
			}
		}
	}
}
