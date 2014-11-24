/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Ext
{
	/// <summary>Static constants to describe the status of objects.</summary>
	/// <remarks>Static constants to describe the status of objects.</remarks>
	public class Status
	{
		public const double Unused = -1.0;

		public const double Available = -2.0;

		public const double Queued = -3.0;

		public const double Completed = -4.0;

		public const double Processing = -5.0;

		public const double Error = -99.0;
	}
}
