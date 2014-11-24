/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class CallbackObjectInfoCollections
	{
		public readonly IObjectInfoCollection added;

		public readonly IObjectInfoCollection updated;

		public readonly IObjectInfoCollection deleted;

		public static readonly Db4objects.Db4o.Internal.CallbackObjectInfoCollections Emtpy
			 = Empty();

		public CallbackObjectInfoCollections(IObjectInfoCollection added_, IObjectInfoCollection
			 updated_, IObjectInfoCollection deleted_)
		{
			added = added_;
			updated = updated_;
			deleted = deleted_;
		}

		private static Db4objects.Db4o.Internal.CallbackObjectInfoCollections Empty()
		{
			return new Db4objects.Db4o.Internal.CallbackObjectInfoCollections(ObjectInfoCollectionImpl
				.Empty, ObjectInfoCollectionImpl.Empty, ObjectInfoCollectionImpl.Empty);
		}
	}
}
