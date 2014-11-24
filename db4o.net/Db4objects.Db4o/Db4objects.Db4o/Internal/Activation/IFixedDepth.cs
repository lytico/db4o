/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public interface IFixedDepth
	{
		IFixedDepth AdjustDepthToBorders();
	}
}
