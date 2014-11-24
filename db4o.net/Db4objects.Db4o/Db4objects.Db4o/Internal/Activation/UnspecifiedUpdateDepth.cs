/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public abstract class UnspecifiedUpdateDepth : IUpdateDepth
	{
		protected UnspecifiedUpdateDepth()
		{
		}

		public virtual bool SufficientDepth()
		{
			return true;
		}

		public virtual bool Negative()
		{
			return true;
		}

		public override string ToString()
		{
			return GetType().FullName;
		}

		public virtual IUpdateDepth Adjust(ClassMetadata clazz)
		{
			FixedUpdateDepth depth = (FixedUpdateDepth)ForDepth(clazz.UpdateDepthFromConfig()
				).Descend();
			return depth;
		}

		public virtual IUpdateDepth AdjustUpdateDepthForCascade(bool isCollection)
		{
			throw new InvalidOperationException();
		}

		public virtual IUpdateDepth Descend()
		{
			throw new InvalidOperationException();
		}

		protected abstract FixedUpdateDepth ForDepth(int depth);

		public abstract bool CanSkip(ObjectReference arg1);
	}
}
