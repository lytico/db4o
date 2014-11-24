/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Activation
{
	/// <exclude></exclude>
	public class ActivationContext4 : IActivationContext
	{
		private readonly Db4objects.Db4o.Internal.Transaction _transaction;

		private readonly object _targetObject;

		private readonly IActivationDepth _depth;

		public ActivationContext4(Db4objects.Db4o.Internal.Transaction transaction, object
			 obj, IActivationDepth depth)
		{
			if (null == obj)
			{
				throw new ArgumentNullException();
			}
			_transaction = transaction;
			_targetObject = obj;
			_depth = depth;
		}

		public virtual void CascadeActivationToTarget()
		{
			IActivationContext context = ClassMetadata().DescendOnCascadingActivation() ? Descend
				() : this;
			CascadeActivation(context);
		}

		public virtual void CascadeActivationToChild(object obj)
		{
			if (obj == null)
			{
				return;
			}
			IActivationContext cascadingContext = ForObject(obj);
			Db4objects.Db4o.Internal.ClassMetadata classMetadata = cascadingContext.ClassMetadata
				();
			if (classMetadata == null || !classMetadata.HasIdentity())
			{
				return;
			}
			CascadeActivation(cascadingContext.Descend());
		}

		private void CascadeActivation(IActivationContext context)
		{
			IActivationDepth depth = context.Depth();
			if (!depth.RequiresActivation())
			{
				return;
			}
			if (depth.Mode().IsDeactivate())
			{
				Container().StillToDeactivate(_transaction, context.TargetObject(), depth, false);
			}
			else
			{
				// FIXME: [TA] do we really need to check for isValueType here?
				Db4objects.Db4o.Internal.ClassMetadata classMetadata = context.ClassMetadata();
				if (classMetadata.IsStruct())
				{
					classMetadata.CascadeActivation(context);
				}
				else
				{
					Container().StillToActivate(context);
				}
			}
		}

		public virtual ObjectContainerBase Container()
		{
			return _transaction.Container();
		}

		public virtual object TargetObject()
		{
			return _targetObject;
		}

		public virtual Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return Container().ClassMetadataForObject(_targetObject);
		}

		public virtual IActivationDepth Depth()
		{
			return _depth;
		}

		public virtual IObjectContainer ObjectContainer()
		{
			return Container();
		}

		public virtual Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _transaction;
		}

		public virtual IActivationContext ForObject(object newTargetObject)
		{
			return new Db4objects.Db4o.Internal.Activation.ActivationContext4(Transaction(), 
				newTargetObject, Depth());
		}

		public virtual IActivationContext Descend()
		{
			return new Db4objects.Db4o.Internal.Activation.ActivationContext4(Transaction(), 
				TargetObject(), Depth().Descend(ClassMetadata()));
		}
	}
}
