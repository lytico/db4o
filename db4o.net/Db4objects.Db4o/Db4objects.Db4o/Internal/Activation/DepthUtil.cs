/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Activation
{
	public sealed class DepthUtil
	{
		public static int AdjustDepthToBorders(int depth)
		{
			int depthBorder = 2;
			// TODO when can min value actually occur?
			if (depth > int.MinValue && depth < depthBorder)
			{
				return depthBorder;
			}
			return depth;
		}

		private DepthUtil()
		{
		}
	}
}
