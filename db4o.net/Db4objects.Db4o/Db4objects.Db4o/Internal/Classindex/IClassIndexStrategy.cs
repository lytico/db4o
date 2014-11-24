/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Classindex
{
	/// <exclude></exclude>
	public interface IClassIndexStrategy
	{
		void Initialize(ObjectContainerBase stream);

		void Read(ObjectContainerBase stream, int indexID);

		int Write(Transaction transaction);

		void Add(Transaction transaction, int id);

		void Remove(Transaction transaction, int id);

		int EntryCount(Transaction transaction);

		int OwnLength();

		void Purge();

		/// <summary>Traverses all index entries (java.lang.Integer references).</summary>
		/// <remarks>Traverses all index entries (java.lang.Integer references).</remarks>
		void TraverseIds(Transaction transaction, IVisitor4 command);

		void DontDelete(Transaction transaction, int id);

		IEnumerator AllSlotIDs(Transaction trans);

		// FIXME: Why is this never called?
		void DefragReference(ClassMetadata classMetadata, DefragmentContextImpl context, 
			int classIndexID);

		int Id();

		// FIXME: Why is this never called?
		void DefragIndex(DefragmentContextImpl context);

		IIntVisitable IdVisitable(Transaction trans);
	}
}
