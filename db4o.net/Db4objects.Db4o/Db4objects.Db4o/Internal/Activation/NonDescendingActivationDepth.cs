/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	/// <summary>Transparent Activation strategy.</summary>
	/// <remarks>Transparent Activation strategy.</remarks>
	public class NonDescendingActivationDepth : ActivationDepthImpl
	{
		public NonDescendingActivationDepth(ActivationMode mode) : base(mode)
		{
		}

		public override IActivationDepth Descend(ClassMetadata metadata)
		{
			return this;
		}

		public override bool RequiresActivation()
		{
			return false;
		}
	}
}
