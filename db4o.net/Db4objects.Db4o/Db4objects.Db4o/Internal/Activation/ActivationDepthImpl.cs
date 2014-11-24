/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public abstract class ActivationDepthImpl : IActivationDepth
	{
		protected readonly ActivationMode _mode;

		protected ActivationDepthImpl(ActivationMode mode)
		{
			_mode = mode;
		}

		public virtual ActivationMode Mode()
		{
			return _mode;
		}

		public abstract IActivationDepth Descend(ClassMetadata arg1);

		public abstract bool RequiresActivation();
	}
}
