/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class ClassAspect
	{
		protected int _handle;

		private int _disabledFromAspectCountVersion = AspectVersionContextImpl.AlwaysEnabled
			.DeclaredAspectCount();

		// used for identification when sending in C/S mode 
		public abstract Db4objects.Db4o.Internal.Marshall.AspectType AspectType();

		public abstract string GetName();

		public abstract void CascadeActivation(IActivationContext context);

		public abstract int LinkLength(IHandlerVersionContext context);

		public void IncrementOffset(IReadBuffer buffer, IHandlerVersionContext context)
		{
			buffer.Seek(buffer.Offset() + LinkLength(context));
		}

		public abstract void DefragAspect(IDefragmentContext context);

		public abstract void Marshall(MarshallingContext context, object child);

		public abstract void CollectIDs(CollectIdContext context);

		public virtual void SetHandle(int handle)
		{
			_handle = handle;
		}

		public abstract void Activate(UnmarshallingContext context);

		public abstract void Delete(DeleteContextImpl context, bool isUpdate);

		public abstract bool CanBeDisabled();

		protected virtual bool CheckEnabled(IAspectVersionContext context, IHandlerVersionContext
			 versionContext)
		{
			if (!IsEnabledOn(context))
			{
				IncrementOffset((IReadBuffer)context, versionContext);
				return false;
			}
			return true;
		}

		public virtual void DisableFromAspectCountVersion(int aspectCount)
		{
			if (!CanBeDisabled())
			{
				return;
			}
			if (aspectCount < _disabledFromAspectCountVersion)
			{
				_disabledFromAspectCountVersion = aspectCount;
			}
		}

		public bool IsEnabledOn(IAspectVersionContext context)
		{
			return _disabledFromAspectCountVersion > context.DeclaredAspectCount();
		}

		public abstract void Deactivate(IActivationContext context);

		public virtual bool IsVirtual()
		{
			return false;
		}
	}
}
