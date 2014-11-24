/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>Implements one step in the defragmenting process.</summary>
	/// <remarks>Implements one step in the defragmenting process.</remarks>
	/// <exclude></exclude>
	internal interface IPassCommand
	{
		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void ProcessObjectSlot(DefragmentServicesImpl context, ClassMetadata classMetadata
			, int id);

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void ProcessClass(DefragmentServicesImpl context, ClassMetadata classMetadata, int
			 id, int classIndexID);

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void ProcessClassCollection(DefragmentServicesImpl context);

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void ProcessBTree(DefragmentServicesImpl context, BTree btree);

		void Flush(DefragmentServicesImpl context);
	}
}
