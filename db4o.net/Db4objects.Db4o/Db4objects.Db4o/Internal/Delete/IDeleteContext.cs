/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Delete
{
	/// <exclude></exclude>
	public interface IDeleteContext : IContext, IReadBuffer, IHandlerVersionContext
	{
		bool CascadeDelete();

		int CascadeDeleteDepth();

		void Delete(ITypeHandler4 handler);

		void DeleteObject();

		bool IsLegacyHandlerVersion();

		void DefragmentRecommended();

		Slot ReadSlot();

		int ObjectId();
	}
}
