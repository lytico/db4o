/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Events
{
	/// <since>7.12</since>
	public class StringEventArgs : EventArgs
	{
		public StringEventArgs(string message)
		{
			_message = message;
		}

		public virtual string Message
		{
			get
			{
				return _message;
			}
		}

		private readonly string _message;
	}
}
