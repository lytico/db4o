/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	public interface IParentCandidate : IInternalCandidate
	{
		bool CreateChild(QField field, QCandidates candidates);

		void UseField(QField field);
	}
}
