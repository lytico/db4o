/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.IO
{
	/// <summary>Strategy for file/byte array growth.</summary>
	/// <remarks>Strategy for file/byte array growth.</remarks>
	public interface IGrowthStrategy
	{
		/// <summary>
		/// returns the incremented size after the growth
		/// strategy has been applied
		/// </summary>
		/// <param name="curSize">the original size</param>
		/// <returns>
		/// the new size, after the growth strategy has been
		/// applied, must be bigger than curSize
		/// </returns>
		long NewSize(long curSize, long requiredSize);
	}
}
