/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;

namespace Db4oUnit
{
	[System.Serializable]
	public class TearDownFailureException : TestException
	{
		private const long serialVersionUID = -5998743679496701084L;

		public TearDownFailureException(Exception cause) : base(cause)
		{
		}
	}
}
