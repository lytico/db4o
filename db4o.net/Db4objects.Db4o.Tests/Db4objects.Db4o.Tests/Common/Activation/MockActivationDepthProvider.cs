/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Mocking;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Tests.Common.Activation
{
	/// <summary>
	/// An ActivationDepthProvider that records ActivationDepthProvider calls and
	/// delegates to another provider.
	/// </summary>
	/// <remarks>
	/// An ActivationDepthProvider that records ActivationDepthProvider calls and
	/// delegates to another provider.
	/// </remarks>
	public class MockActivationDepthProvider : MethodCallRecorder, IActivationDepthProvider
	{
		private readonly IActivationDepthProvider _delegate;

		public MockActivationDepthProvider()
		{
			_delegate = LegacyActivationDepthProvider.Instance;
		}

		public virtual IActivationDepth ActivationDepthFor(ClassMetadata classMetadata, ActivationMode
			 mode)
		{
			Record(new MethodCall("activationDepthFor", new object[] { classMetadata, mode })
				);
			return _delegate.ActivationDepthFor(classMetadata, mode);
		}

		public virtual IActivationDepth ActivationDepth(int depth, ActivationMode mode)
		{
			Record(new MethodCall("activationDepth", new object[] { depth, mode }));
			return _delegate.ActivationDepth(depth, mode);
		}
	}
}
