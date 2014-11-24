/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Events
{
	/// <summary>Argument for events related to cancellable actions.</summary>
	/// <remarks>Argument for events related to cancellable actions.</remarks>
	/// <seealso cref="IEventRegistry">IEventRegistry</seealso>
	public interface ICancellableEventArgs
	{
		/// <summary>Queries if the action was already cancelled by some event listener.</summary>
		/// <remarks>Queries if the action was already cancelled by some event listener.</remarks>
		bool IsCancelled
		{
			get;
		}

		/// <summary>Cancels the action related to this event.</summary>
		/// <remarks>
		/// Cancels the action related to this event.
		/// Although the related action will be cancelled all the registered
		/// listeners will still receive the event.
		/// </remarks>
		void Cancel();
	}
}
