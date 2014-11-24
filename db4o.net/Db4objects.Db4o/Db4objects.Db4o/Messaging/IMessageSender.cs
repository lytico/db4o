/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Messaging
{
	/// <summary>message sender for client/server messaging.</summary>
	/// <remarks>
	/// message sender for client/server messaging.
	/// <br /><br />db4o allows using the client/server TCP connection to send
	/// messages from the client to the server. Any object that can be
	/// stored to a db4o database file may be used as a message.<br /><br />
	/// For an example see Reference documentation: <br />
	/// http://developer.db4o.com/Resources/view.aspx/Reference/Client-Server/Messaging<br />
	/// http://developer.db4o.com/Resources/view.aspx/Reference/Client-Server/Remote_Code_Execution<br /><br />
	/// <b>See Also:</b><br />
	/// <see cref="Db4objects.Db4o.Config.IClientServerConfiguration.GetMessageSender()">Db4objects.Db4o.Config.IClientServerConfiguration.GetMessageSender()
	/// 	</see>
	/// ,<br />
	/// <see cref="IMessageRecipient">IMessageRecipient</see>
	/// ,<br />
	/// <see cref="Db4objects.Db4o.Config.IClientServerConfiguration.SetMessageRecipient(IMessageRecipient)
	/// 	">Db4objects.Db4o.Config.IClientServerConfiguration.SetMessageRecipient(IMessageRecipient)
	/// 	</see>
	/// </remarks>
	public interface IMessageSender
	{
		/// <summary>sends a message to the server.</summary>
		/// <remarks>sends a message to the server.</remarks>
		/// <param name="obj">the message parameter, any object may be used.</param>
		void Send(object obj);
	}
}
