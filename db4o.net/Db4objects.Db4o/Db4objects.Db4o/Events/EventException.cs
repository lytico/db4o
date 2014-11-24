/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Events
{
	/// <summary>
	/// db4o-specific exception.<br/><br/>
	/// Exception thrown during event dispatching if a client
	/// provided event handler throws.<br/><br/>
	/// The exception thrown by the client can be retrieved by
	/// calling EventException.InnerException.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br/><br/>
	/// Exception thrown during event dispatching if a client
	/// provided event handler throws.<br/><br/>
	/// The exception thrown by the client can be retrieved by
	/// calling EventException.InnerException.
	/// </remarks>
	[System.Serializable]
	public class EventException : Db4oRecoverableException
	{
		public EventException(Exception exc) : base(exc)
		{
		}
	}
}
