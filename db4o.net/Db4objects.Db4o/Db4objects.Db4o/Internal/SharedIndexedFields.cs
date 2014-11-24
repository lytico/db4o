/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class SharedIndexedFields
	{
		public readonly VersionFieldMetadata _version = new VersionFieldMetadata();

		public readonly UUIDFieldMetadata _uUID = new UUIDFieldMetadata();

		public readonly CommitTimestampFieldMetadata _commitTimestamp = new CommitTimestampFieldMetadata
			();
	}
}
