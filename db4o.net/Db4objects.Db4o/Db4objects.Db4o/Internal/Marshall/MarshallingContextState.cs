/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class MarshallingContextState
	{
		internal readonly MarshallingBuffer _buffer;

		public MarshallingContextState(MarshallingBuffer buffer)
		{
			_buffer = buffer;
		}
	}
}
