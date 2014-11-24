/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	/// <summary>Factory for ActivationDepth strategies.</summary>
	/// <remarks>Factory for ActivationDepth strategies.</remarks>
	public interface IActivationDepthProvider
	{
		/// <summary>Returns an ActivationDepth suitable for the specified class and activation mode.
		/// 	</summary>
		/// <remarks>Returns an ActivationDepth suitable for the specified class and activation mode.
		/// 	</remarks>
		/// <param name="classMetadata">root class that's being activated</param>
		/// <param name="mode">activation mode</param>
		/// <returns>an appropriate ActivationDepth for the class and activation mode</returns>
		IActivationDepth ActivationDepthFor(ClassMetadata classMetadata, ActivationMode mode
			);

		/// <summary>Returns an ActivationDepth that will activate at most *depth* levels.</summary>
		/// <remarks>
		/// Returns an ActivationDepth that will activate at most *depth* levels.
		/// A special case is Integer.MAX_VALUE (int.MaxValue for .net) for which a
		/// FullActivationDepth object must be returned.
		/// </remarks>
		/// <param name="depth"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		IActivationDepth ActivationDepth(int depth, ActivationMode mode);
	}
}
