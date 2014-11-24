/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	/// <summary>Controls how deep an object graph is activated.</summary>
	/// <remarks>Controls how deep an object graph is activated.</remarks>
	public interface IActivationDepth
	{
		ActivationMode Mode();

		bool RequiresActivation();

		IActivationDepth Descend(ClassMetadata metadata);
	}
}
