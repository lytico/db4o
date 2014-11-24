/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	/// <summary>
	/// Activates a fixed depth of the object graph regardless of
	/// any existing activation depth configuration settings.
	/// </summary>
	/// <remarks>
	/// Activates a fixed depth of the object graph regardless of
	/// any existing activation depth configuration settings.
	/// </remarks>
	public class FixedActivationDepth : ActivationDepthImpl
	{
		private readonly int _depth;

		public FixedActivationDepth(int depth, ActivationMode mode) : base(mode)
		{
			_depth = depth;
		}

		public FixedActivationDepth(int depth) : this(depth, ActivationMode.Activate)
		{
		}

		public override bool RequiresActivation()
		{
			return _depth > 0;
		}

		public override IActivationDepth Descend(ClassMetadata metadata)
		{
			if (_depth < 1)
			{
				return this;
			}
			return new Db4objects.Db4o.Internal.Activation.FixedActivationDepth(_depth - 1, _mode
				);
		}

		// TODO code duplication in fixed activation/update depth
		public virtual Db4objects.Db4o.Internal.Activation.FixedActivationDepth AdjustDepthToBorders
			()
		{
			return new Db4objects.Db4o.Internal.Activation.FixedActivationDepth(DepthUtil.AdjustDepthToBorders
				(_depth));
		}
	}
}
