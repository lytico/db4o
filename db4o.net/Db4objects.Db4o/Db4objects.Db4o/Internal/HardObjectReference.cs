/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class HardObjectReference
	{
		public static readonly Db4objects.Db4o.Internal.HardObjectReference Invalid = new 
			Db4objects.Db4o.Internal.HardObjectReference(null, null);

		public readonly ObjectReference _reference;

		public readonly object _object;

		public HardObjectReference(ObjectReference @ref, object obj)
		{
			_reference = @ref;
			_object = obj;
		}

		public static Db4objects.Db4o.Internal.HardObjectReference PeekPersisted(Transaction
			 trans, int id, int depth)
		{
			object obj = trans.Container().PeekPersisted(trans, id, ActivationDepthProvider(trans
				).ActivationDepth(depth, ActivationMode.Peek), true);
			if (obj == null)
			{
				return null;
			}
			ObjectReference @ref = trans.ReferenceForId(id);
			return new Db4objects.Db4o.Internal.HardObjectReference(@ref, obj);
		}

		private static IActivationDepthProvider ActivationDepthProvider(Transaction trans
			)
		{
			return trans.Container().ActivationDepthProvider();
		}
	}
}
