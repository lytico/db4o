/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Metadata;

namespace Db4objects.Db4o.Internal.Metadata
{
	/// <exclude></exclude>
	public abstract class MarshallingInfoTraverseAspectCommand : ITraverseAspectCommand
	{
		private bool _cancelled = false;

		protected readonly IMarshallingInfo _marshallingInfo;

		public MarshallingInfoTraverseAspectCommand(IMarshallingInfo marshallingInfo)
		{
			_marshallingInfo = marshallingInfo;
		}

		public int DeclaredAspectCount(ClassMetadata classMetadata)
		{
			int aspectCount = InternalDeclaredAspectCount(classMetadata);
			_marshallingInfo.DeclaredAspectCount(aspectCount);
			return aspectCount;
		}

		protected virtual int InternalDeclaredAspectCount(ClassMetadata classMetadata)
		{
			return classMetadata.ReadAspectCount(_marshallingInfo.Buffer());
		}

		public virtual bool Cancelled()
		{
			return _cancelled;
		}

		protected virtual void Cancel()
		{
			_cancelled = true;
		}

		public virtual bool Accept(ClassAspect aspect)
		{
			return true;
		}

		public virtual void ProcessAspectOnMissingClass(ClassAspect aspect, int currentSlot
			)
		{
			if (_marshallingInfo.IsNull(currentSlot))
			{
				return;
			}
			aspect.IncrementOffset(_marshallingInfo.Buffer(), (IHandlerVersionContext)_marshallingInfo
				);
		}

		public virtual void ProcessAspect(ClassAspect aspect, int currentSlot)
		{
			if (Accept(aspect))
			{
				ProcessAspect(aspect, currentSlot, _marshallingInfo.IsNull(currentSlot));
			}
			_marshallingInfo.BeginSlot();
		}

		protected abstract void ProcessAspect(ClassAspect aspect, int currentSlot, bool isNull
			);
	}
}
