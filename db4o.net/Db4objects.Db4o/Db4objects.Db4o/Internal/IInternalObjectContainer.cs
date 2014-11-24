/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Callbacks;
using Db4objects.Db4o.Internal.Events;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public partial interface IInternalObjectContainer : IExtObjectContainer
	{
		void Callbacks(ICallbacks cb);

		ICallbacks Callbacks();

		ObjectContainerBase Container
		{
			get;
		}

		Db4objects.Db4o.Internal.Transaction Transaction
		{
			get;
		}

		NativeQueryHandler GetNativeQueryHandler();

		ClassMetadata ClassMetadataForReflectClass(IReflectClass reflectClass);

		ClassMetadata ClassMetadataForName(string name);

		ClassMetadata ClassMetadataForID(int id);

		HandlerRegistry Handlers
		{
			get;
		}

		Config4Impl ConfigImpl
		{
			get;
		}

		object SyncExec(IClosure4 block);

		int InstanceCount(ClassMetadata clazz, Db4objects.Db4o.Internal.Transaction trans
			);

		bool IsClient
		{
			get;
		}

		void StoreAll(Db4objects.Db4o.Internal.Transaction trans, IEnumerator objects);

		IUpdateDepthProvider UpdateDepthProvider();

		EventRegistryImpl NewEventRegistry();

		bool InCallback();
	}
}
