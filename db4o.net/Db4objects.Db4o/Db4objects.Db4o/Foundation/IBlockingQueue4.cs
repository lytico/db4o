/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public interface IBlockingQueue4 : IQueue4
	{
		/// <summary>
		/// <p>
		/// Returns the next queued item or waits for it to be available for the
		/// maximum of <code>timeout</code> miliseconds.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Returns the next queued item or waits for it to be available for the
		/// maximum of <code>timeout</code> miliseconds.
		/// </remarks>
		/// <param name="timeout">maximum time to wait for the next avilable item in miliseconds
		/// 	</param>
		/// <returns>
		/// the next item or <code>null</code> if <code>timeout</code> is
		/// reached
		/// </returns>
		/// <exception cref="BlockingQueueStoppedException">
		/// if the
		/// <see cref="Stop()">Stop()</see>
		/// is called.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		object Next(long timeout);

		void Stop();

		/// <summary>
		/// <p>
		/// Removes all the available elements in the queue to the colletion passed
		/// as argument.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Removes all the available elements in the queue to the colletion passed
		/// as argument.
		/// <p>
		/// It will block until at least one element is available.
		/// </remarks>
		/// <param name="list"></param>
		/// <returns>the number of elements added to the list.</returns>
		/// <exception cref="BlockingQueueStoppedException">
		/// if the
		/// <see cref="Stop()">Stop()</see>
		/// is called.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		int DrainTo(Collection4 list);
	}
}
