/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;

namespace Db4oUnit
{
	[System.Serializable]
	public partial class AssertionException : TestException
	{
		private const long serialVersionUID = 900088031151055525L;

		public AssertionException(string message) : base(message, null)
		{
		}

		public AssertionException(string message, Exception cause) : base(message, cause)
		{
		}
	}
}
