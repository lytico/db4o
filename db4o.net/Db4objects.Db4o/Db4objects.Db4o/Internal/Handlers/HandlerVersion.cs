/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class HandlerVersion
	{
		public readonly int _number;

		public static readonly Db4objects.Db4o.Internal.Handlers.HandlerVersion Invalid = 
			new Db4objects.Db4o.Internal.Handlers.HandlerVersion(-1);

		public HandlerVersion(int number)
		{
			_number = number;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			return ((Db4objects.Db4o.Internal.Handlers.HandlerVersion)obj)._number == _number;
		}
	}
}
