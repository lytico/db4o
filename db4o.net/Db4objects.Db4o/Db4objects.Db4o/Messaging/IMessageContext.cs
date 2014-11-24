/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.Messaging
{
	/// <summary>Additional message-related information.</summary>
	/// <remarks>Additional message-related information.</remarks>
	public interface IMessageContext
	{
		/// <summary>The container the message was dispatched to.</summary>
		/// <remarks>The container the message was dispatched to.</remarks>
		IObjectContainer Container
		{
			get;
		}

		/// <summary>The sender of the current message.</summary>
		/// <remarks>
		/// The sender of the current message.
		/// The reference can be used to send a reply to it.
		/// </remarks>
		IMessageSender Sender
		{
			get;
		}

		/// <summary>The transaction the current message has been sent with.</summary>
		/// <remarks>The transaction the current message has been sent with.</remarks>
		Db4objects.Db4o.Internal.Transaction Transaction
		{
			get;
		}
	}
}
