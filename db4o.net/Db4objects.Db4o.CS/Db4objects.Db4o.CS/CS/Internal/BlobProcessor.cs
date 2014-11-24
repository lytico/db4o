/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	internal class BlobProcessor : IRunnable
	{
		private ClientObjectContainer stream;

		private IQueue4 queue = new NonblockingQueue();

		private bool terminated = false;

		internal BlobProcessor(ClientObjectContainer aStream)
		{
			stream = aStream;
		}

		internal virtual void Add(MsgBlob msg)
		{
			lock (queue)
			{
				queue.Add(msg);
			}
		}

		internal virtual bool IsTerminated()
		{
			lock (this)
			{
				return terminated;
			}
		}

		public virtual void Run()
		{
			try
			{
				Socket4Adapter socket = stream.CreateParallelSocket();
				MsgBlob msg = null;
				// no blobLock synchronisation here, since our first msg is valid
				lock (queue)
				{
					msg = (MsgBlob)queue.Next();
				}
				while (msg != null)
				{
					msg.Write(socket);
					msg.ProcessClient(socket);
					lock (stream._blobLock)
					{
						lock (queue)
						{
							msg = (MsgBlob)queue.Next();
						}
						if (msg == null)
						{
							terminated = true;
							Msg.CloseSocket.Write(socket);
							try
							{
								socket.Close();
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}
	}
}
