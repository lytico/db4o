/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	/// <summary>Activates the full object graph.</summary>
	/// <remarks>Activates the full object graph.</remarks>
	public class FullActivationDepth : ActivationDepthImpl
	{
		public FullActivationDepth(ActivationMode mode) : base(mode)
		{
		}

		public FullActivationDepth() : this(ActivationMode.Activate)
		{
		}

		public override IActivationDepth Descend(ClassMetadata metadata)
		{
			return this;
		}

		public override bool RequiresActivation()
		{
			return true;
		}
	}
}
