/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Internal.Activation
{
	public class TPFixedUpdateDepth : FixedUpdateDepth
	{
		private IModifiedObjectQuery _query;

		public TPFixedUpdateDepth(int depth, IModifiedObjectQuery query) : base(depth)
		{
			_query = query;
		}

		public override bool CanSkip(ObjectReference @ref)
		{
			ClassMetadata clazz = @ref.ClassMetadata();
			return clazz.Reflector().ForClass(typeof(IActivatable)).IsAssignableFrom(clazz.ClassReflector
				()) && !_query.IsModified(@ref.GetObject());
		}

		protected override FixedUpdateDepth ForDepth(int depth)
		{
			return new Db4objects.Db4o.Internal.Activation.TPFixedUpdateDepth(depth, _query);
		}
	}
}
