/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public class LegacyActivationDepthProvider : IActivationDepthProvider
	{
		public static readonly IActivationDepthProvider Instance = new LegacyActivationDepthProvider
			();

		public virtual IActivationDepth ActivationDepthFor(ClassMetadata classMetadata, ActivationMode
			 mode)
		{
			if (mode.IsPrefetch())
			{
				return new LegacyActivationDepth(1, mode);
			}
			int globalLegacyActivationDepth = ConfigImpl(classMetadata).ActivationDepth();
			Config4Class config = classMetadata.ConfigOrAncestorConfig();
			int defaultDepth = null == config ? globalLegacyActivationDepth : config.AdjustActivationDepth
				(globalLegacyActivationDepth);
			return new LegacyActivationDepth(defaultDepth, mode);
		}

		public virtual IActivationDepth ActivationDepth(int depth, ActivationMode mode)
		{
			return new LegacyActivationDepth(depth, mode);
		}

		private Config4Impl ConfigImpl(ClassMetadata classMetadata)
		{
			return classMetadata.Container().ConfigImpl;
		}
	}
}
