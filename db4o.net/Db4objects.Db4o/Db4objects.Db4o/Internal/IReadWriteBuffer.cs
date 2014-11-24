/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public interface IReadWriteBuffer : IReadBuffer, IWriteBuffer
	{
		void IncrementOffset(int numBytes);

		void IncrementIntSize();

		int Length();

		void ReadBegin(byte identifier);

		void ReadEnd();
	}
}
