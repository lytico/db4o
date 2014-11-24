/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Metadata;

namespace Db4objects.Db4o.Internal.Metadata
{
	/// <exclude></exclude>
	public abstract class TraverseFieldCommand : ITraverseAspectCommand
	{
		public virtual bool Cancelled()
		{
			return false;
		}

		public virtual int DeclaredAspectCount(ClassMetadata classMetadata)
		{
			return classMetadata.DeclaredAspectCount();
		}

		public virtual void ProcessAspect(ClassAspect aspect, int currentSlot)
		{
			if (aspect is FieldMetadata)
			{
				Process((FieldMetadata)aspect);
			}
		}

		public virtual void ProcessAspectOnMissingClass(ClassAspect aspect, int currentSlot
			)
		{
		}

		// do nothing
		protected abstract void Process(FieldMetadata field);
	}
}
