/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public interface IInternalReadContext : IReadContext, IHandlerVersionContext
	{
		IReadBuffer Buffer(IReadBuffer buffer);

		IReadBuffer Buffer();

		ObjectContainerBase Container();

		int Offset();

		object Read(ITypeHandler4 handler);

		object ReadAtCurrentSeekPosition(ITypeHandler4 handler);

		IReadWriteBuffer ReadIndirectedBuffer();

		void Seek(int offset);

		int HandlerVersion();

		void NotifyNullReferenceSkipped();
	}
}
