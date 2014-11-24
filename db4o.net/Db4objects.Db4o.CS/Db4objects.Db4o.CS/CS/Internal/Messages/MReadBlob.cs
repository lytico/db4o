/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Sharpen.IO;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MReadBlob : MsgBlob, IServerSideMessage
	{
		/// <exception cref="System.IO.IOException"></exception>
		public override void ProcessClient(Socket4Adapter sock)
		{
			Msg message = Msg.ReadMessage(MessageDispatcher(), Transaction(), sock);
			if (message.Equals(Msg.Length))
			{
				try
				{
					_currentByte = 0;
					_length = message.PayLoad().ReadInt();
					_blob.GetStatusFrom(this);
					_blob.SetStatus(Status.Processing);
					Copy(sock, this._blob.GetClientOutputStream(), _length, true);
					message = Msg.ReadMessage(MessageDispatcher(), Transaction(), sock);
					if (message.Equals(Msg.Ok))
					{
						this._blob.SetStatus(Status.Completed);
					}
					else
					{
						this._blob.SetStatus(Status.Error);
					}
				}
				catch (Exception)
				{
				}
			}
			else
			{
				if (message.Equals(Msg.Error))
				{
					this._blob.SetStatus(Status.Error);
				}
			}
		}

		public virtual void ProcessAtServer()
		{
			try
			{
				BlobImpl blobImpl = this.ServerGetBlobImpl();
				if (blobImpl != null)
				{
					blobImpl.SetTrans(Transaction());
					Sharpen.IO.File file = blobImpl.ServerFile(null, false);
					int length = (int)file.Length();
					Socket4Adapter sock = ServerMessageDispatcher().Socket();
					Msg.Length.GetWriterForInt(Transaction(), length).Write(sock);
					FileInputStream fin = new FileInputStream(file);
					Copy(fin, sock, false);
					sock.Flush();
					Msg.Ok.Write(sock);
				}
			}
			catch (Exception)
			{
				Write(Msg.Error);
			}
		}
	}
}
