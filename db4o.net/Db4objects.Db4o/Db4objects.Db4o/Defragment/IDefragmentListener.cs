/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Defragment;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>Listener for defragmentation process messages.</summary>
	/// <remarks>Listener for defragmentation process messages.</remarks>
	/// <seealso cref="Defragment">Defragment</seealso>
	public interface IDefragmentListener
	{
		/// <summary>
		/// This method will be called when the defragment process encounters
		/// file layout anomalies during the defragmentation process.
		/// </summary>
		/// <remarks>
		/// This method will be called when the defragment process encounters
		/// file layout anomalies during the defragmentation process.
		/// </remarks>
		/// <param name="info">The message from the defragmentation process.</param>
		void NotifyDefragmentInfo(DefragmentInfo info);
	}
}
