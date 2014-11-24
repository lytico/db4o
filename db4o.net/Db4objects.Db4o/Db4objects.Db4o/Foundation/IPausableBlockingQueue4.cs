/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public interface IPausableBlockingQueue4 : IBlockingQueue4
	{
		/// <summary>
		/// <p>
		/// Pauses the queue, making calls to
		/// <see cref="IQueue4.Next()">IQueue4.Next()</see>
		/// block
		/// until
		/// <see cref="Resume()">Resume()</see>
		/// is called.
		/// </summary>
		/// <returns>whether or not this call changed the state of the queue.</returns>
		bool Pause();

		/// <summary>
		/// <p>
		/// Resumes the queue, releasing blocked calls to
		/// <see cref="IQueue4.Next()">IQueue4.Next()</see>
		/// that can reach a next queue item..
		/// </summary>
		/// <returns>whether or not this call changed the state of the queue.</returns>
		bool Resume();

		bool IsPaused();

		/// <summary>
		/// <p>
		/// Returns the next element in queue if there is one available, returns null
		/// otherwise.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Returns the next element in queue if there is one available, returns null
		/// otherwise.
		/// <p>
		/// This method will not never block, regardless of the queue being paused or
		/// no elements are available.
		/// </remarks>
		/// <returns>next element, if available and queue not paused.</returns>
		object TryNext();
	}
}
