/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	internal class FieldMetadataState
	{
		private readonly string _info;

		private FieldMetadataState(string info)
		{
			_info = info;
		}

		internal static readonly Db4objects.Db4o.Internal.FieldMetadataState NotLoaded = 
			new Db4objects.Db4o.Internal.FieldMetadataState("not loaded");

		internal static readonly Db4objects.Db4o.Internal.FieldMetadataState Unavailable = 
			new Db4objects.Db4o.Internal.FieldMetadataState("unavailable");

		internal static readonly Db4objects.Db4o.Internal.FieldMetadataState Available = 
			new Db4objects.Db4o.Internal.FieldMetadataState("available");

		internal static readonly Db4objects.Db4o.Internal.FieldMetadataState Updating = new 
			Db4objects.Db4o.Internal.FieldMetadataState("updating");

		public override string ToString()
		{
			return _info;
		}
	}
}
