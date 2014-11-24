/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude>TODO: remove this class or make it private to ClassMetadataRepository</exclude>
	public class ClassMetadataIterator : MappingIterator
	{
		private readonly ClassMetadataRepository i_collection;

		internal ClassMetadataIterator(ClassMetadataRepository a_collection, IEnumerator 
			iterator) : base(iterator)
		{
			i_collection = a_collection;
		}

		public virtual ClassMetadata CurrentClass()
		{
			return (ClassMetadata)Current;
		}

		protected override object Map(object current)
		{
			return i_collection.ReadClassMetadata((ClassMetadata)current, null);
		}
	}
}
