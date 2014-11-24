/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal
{
	public sealed class CallBackMode
	{
		public static readonly Db4objects.Db4o.Internal.CallBackMode All = new Db4objects.Db4o.Internal.CallBackMode
			("ALL");

		public static readonly Db4objects.Db4o.Internal.CallBackMode DeleteOnly = new Db4objects.Db4o.Internal.CallBackMode
			("DELETE_ONLY");

		public static readonly Db4objects.Db4o.Internal.CallBackMode None = new Db4objects.Db4o.Internal.CallBackMode
			("NONE");

		private string _desc;

		private CallBackMode(string desc)
		{
			_desc = desc;
		}

		public override string ToString()
		{
			return _desc;
		}
	}
}
