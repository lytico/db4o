/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <summary>Messages for Client/Server Communication</summary>
	public abstract class Msg : System.ICloneable, IMessage
	{
		internal static int _messageIdGenerator = 1;

		private static Db4objects.Db4o.CS.Internal.Messages.Msg[] _messages = new Db4objects.Db4o.CS.Internal.Messages.Msg
			[75];

		internal int _msgID;

		internal string _name;

		private Db4objects.Db4o.Internal.Transaction _trans;

		private IMessageDispatcher _messageDispatcher;

		public static readonly MRuntimeException RuntimeException = new MRuntimeException
			();

		public static readonly MClassID ClassId = new MClassID();

		public static readonly MClassMetadataIdForName ClassMetadataIdForName = new MClassMetadataIdForName
			();

		public static readonly MClassNameForID ClassNameForId = new MClassNameForID();

		public static readonly MClose Close = new MClose();

		public static readonly MCloseSocket CloseSocket = new MCloseSocket();

		public static readonly MCommit Commit = new MCommit();

		public static readonly MCommittedCallBackRegistry CommittedCallbackRegister = new 
			MCommittedCallBackRegistry();

		public static readonly MCommittedInfo CommittedInfo = new MCommittedInfo();

		public static readonly MCommitSystemTransaction CommitSystemtrans = new MCommitSystemTransaction
			();

		public static readonly MCreateClass CreateClass = new MCreateClass();

		public static readonly MClassMeta ClassMeta = new MClassMeta();

		public static readonly MVersion CurrentVersion = new MVersion();

		public static readonly MDelete Delete = new MDelete();

		public static readonly MError Error = new MError();

		public static readonly MFailed Failed = new MFailed();

		public static readonly MGetAll GetAll = new MGetAll();

		public static readonly MGetClasses GetClasses = new MGetClasses();

		public static readonly MGetInternalIDs GetInternalIds = new MGetInternalIDs();

		public static readonly MGetThreadID GetThreadId = new MGetThreadID();

		public static readonly MIDList IdList = new MIDList();

		public static readonly MIdentity Identity = new MIdentity();

		public static readonly MIsAlive IsAlive = new MIsAlive();

		public static readonly MLength Length = new MLength();

		public static readonly MLogin Login = new MLogin();

		public static readonly MLoginOK LoginOk = new MLoginOK();

		public static readonly MNull Null = new MNull();

		public static readonly MObjectByUuid ObjectByUuid = new MObjectByUuid();

		public static readonly MsgObject ObjectToClient = new MsgObject();

		public static readonly MObjectSetFetch ObjectsetFetch = new MObjectSetFetch();

		public static readonly MObjectSetFinalized ObjectsetFinalized = new MObjectSetFinalized
			();

		public static readonly MObjectSetGetId ObjectsetGetId = new MObjectSetGetId();

		public static readonly MObjectSetIndexOf ObjectsetIndexof = new MObjectSetIndexOf
			();

		public static readonly MObjectSetReset ObjectsetReset = new MObjectSetReset();

		public static readonly MObjectSetSize ObjectsetSize = new MObjectSetSize();

		public static readonly MOK Ok = new MOK();

		public static readonly MPing Ping = new MPing();

		public static readonly MPong Pong = new MPong();

		public static readonly MPrefetchIDs PrefetchIds = new MPrefetchIDs();

		public static readonly MProcessDeletes ProcessDeletes = new MProcessDeletes();

		public static readonly MQueryExecute QueryExecute = new MQueryExecute();

		public static readonly MQueryResult QueryResult = new MQueryResult();

		public static readonly MRaiseCommitTimestamp RaiseCommitTimestamp = new MRaiseCommitTimestamp
			();

		public static readonly MReadBlob ReadBlob = new MReadBlob();

		public static readonly MReadBytes ReadBytes = new MReadBytes();

		public static readonly MReadSlot ReadSlot = new MReadSlot();

		public static readonly MReadMultipleObjects ReadMultipleObjects = new MReadMultipleObjects
			();

		public static readonly MReadObject ReadObject = new MReadObject();

		public static readonly MReadReaderById ReadReaderById = new MReadReaderById();

		public static readonly MReleaseSemaphore ReleaseSemaphore = new MReleaseSemaphore
			();

		public static readonly MRollback Rollback = new MRollback();

		public static readonly MSetSemaphore SetSemaphore = new MSetSemaphore();

		public static readonly MSuccess Success = new MSuccess();

		public static readonly MSwitchToFile SwitchToFile = new MSwitchToFile();

		public static readonly MSwitchToMainFile SwitchToMainFile = new MSwitchToMainFile
			();

		public static readonly MTaDelete TaDelete = new MTaDelete();

		public static readonly MTaIsDeleted TaIsDeleted = new MTaIsDeleted();

		public static readonly MUserMessage UserMessage = new MUserMessage();

		public static readonly MUseTransaction UseTransaction = new MUseTransaction();

		public static readonly MWriteBlob WriteBlob = new MWriteBlob();

		public static readonly MWriteNew WriteNew = new MWriteNew();

		public static readonly MWriteUpdate WriteUpdate = new MWriteUpdate();

		public static readonly MWriteBatchedMessages WriteBatchedMessages = new MWriteBatchedMessages
			();

		public static readonly MsgBlob DeleteBlobFile = new MDeleteBlobFile();

		public static readonly MInstanceCount InstanceCount = new MInstanceCount();

		public static readonly MRequestExceptionWithResponse RequestExceptionWithResponse
			 = new MRequestExceptionWithResponse();

		public static readonly MRequestExceptionWithoutResponse RequestExceptionWithoutResponse
			 = new MRequestExceptionWithoutResponse();

		public static readonly MCommitReplication CommitReplication = new MCommitReplication
			();

		public static readonly MGenerateTransactionTimestamp GenerateTransactionTimestamp
			 = new MGenerateTransactionTimestamp();

		public static readonly MVersionForId VersionForId = new MVersionForId();

		public static readonly MUseDefaultTransactionTimestamp UseDefaultTransactionTimestamp
			 = new MUseDefaultTransactionTimestamp();

		internal Msg()
		{
			_msgID = _messageIdGenerator++;
			_messages[_msgID] = this;
		}

		internal Msg(string aName) : this()
		{
			_name = aName;
		}

		public static Db4objects.Db4o.CS.Internal.Messages.Msg GetMessage(int id)
		{
			return _messages[id];
		}

		public Db4objects.Db4o.CS.Internal.Messages.Msg PublicClone()
		{
			return (Db4objects.Db4o.CS.Internal.Messages.Msg)MemberwiseClone();
		}

		public sealed override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null || obj.GetType() != this.GetType())
			{
				return false;
			}
			return _msgID == ((Db4objects.Db4o.CS.Internal.Messages.Msg)obj)._msgID;
		}

		public override int GetHashCode()
		{
			return _msgID;
		}

		/// <summary>
		/// dummy method to allow clean override handling
		/// without casting
		/// </summary>
		public virtual ByteArrayBuffer GetByteLoad()
		{
			return null;
		}

		internal string GetName()
		{
			if (_name == null)
			{
				return GetType().FullName;
			}
			return _name;
		}

		protected virtual LocalTransaction ServerTransaction()
		{
			return (LocalTransaction)_trans;
		}

		protected virtual Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _trans;
		}

		protected virtual LocalObjectContainer LocalContainer()
		{
			return (LocalObjectContainer)Container();
		}

		protected virtual ObjectContainerBase Container()
		{
			return Transaction().Container();
		}

		protected virtual object ContainerLock()
		{
			return Container().Lock();
		}

		protected virtual Config4Impl Config()
		{
			return Container().Config();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		protected static StatefulBuffer ReadMessageBuffer(Db4objects.Db4o.Internal.Transaction
			 trans, Socket4Adapter socket)
		{
			return ReadMessageBuffer(trans, socket, Const4.MessageLength);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		protected static StatefulBuffer ReadMessageBuffer(Db4objects.Db4o.Internal.Transaction
			 trans, Socket4Adapter socket, int length)
		{
			StatefulBuffer buffer = new StatefulBuffer(trans, length);
			int offset = 0;
			while (length > 0)
			{
				int read = socket.Read(buffer._buffer, offset, length);
				if (read < 0)
				{
					throw new Db4oIOException();
				}
				offset += read;
				length -= read;
			}
			return buffer;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public static Db4objects.Db4o.CS.Internal.Messages.Msg ReadMessage(IMessageDispatcher
			 messageDispatcher, Db4objects.Db4o.Internal.Transaction trans, Socket4Adapter socket
			)
		{
			StatefulBuffer reader = ReadMessageBuffer(trans, socket);
			Db4objects.Db4o.CS.Internal.Messages.Msg message = _messages[reader.ReadInt()].ReadPayLoad
				(messageDispatcher, trans, socket, reader);
			return message;
		}

		/// <param name="socket"></param>
		internal virtual Db4objects.Db4o.CS.Internal.Messages.Msg ReadPayLoad(IMessageDispatcher
			 messageDispatcher, Db4objects.Db4o.Internal.Transaction a_trans, Socket4Adapter
			 socket, ByteArrayBuffer reader)
		{
			Db4objects.Db4o.CS.Internal.Messages.Msg msg = PublicClone();
			msg.SetMessageDispatcher(messageDispatcher);
			msg.SetTransaction(CheckParentTransaction(a_trans, reader));
			return msg;
		}

		protected Db4objects.Db4o.Internal.Transaction CheckParentTransaction(Db4objects.Db4o.Internal.Transaction
			 a_trans, ByteArrayBuffer reader)
		{
			if (reader.ReadByte() == Const4.SystemTrans && a_trans.ParentTransaction() != null)
			{
				return a_trans.ParentTransaction();
			}
			return a_trans;
		}

		public void SetTransaction(Db4objects.Db4o.Internal.Transaction aTrans)
		{
			_trans = aTrans;
		}

		public sealed override string ToString()
		{
			return GetName();
		}

		public virtual void Write(Db4objects.Db4o.CS.Internal.Messages.Msg msg)
		{
			_messageDispatcher.Write(msg);
		}

		public virtual void WriteException(Exception e)
		{
			Write(RuntimeException.GetWriterForSingleObject(Transaction(), e));
		}

		public virtual Db4objects.Db4o.CS.Internal.Messages.Msg RespondInt(int response)
		{
			return IdList.GetWriterForInt(Transaction(), response);
		}

		public virtual bool Write(Socket4Adapter sock)
		{
			if (null == sock)
			{
				throw new ArgumentNullException();
			}
			lock (sock)
			{
				try
				{
					sock.Write(PayLoad()._buffer);
					sock.Flush();
					return true;
				}
				catch (Exception)
				{
					// TODO: .NET convert SocketException to Db4oIOException
					// and let Db4oIOException bubble up.
					//e.printStackTrace();
					return false;
				}
			}
		}

		public virtual StatefulBuffer PayLoad()
		{
			StatefulBuffer writer = new StatefulBuffer(Transaction(), Const4.MessageLength);
			writer.WriteInt(_msgID);
			return writer;
		}

		public virtual IMessageDispatcher MessageDispatcher()
		{
			return _messageDispatcher;
		}

		public virtual IServerMessageDispatcher ServerMessageDispatcher()
		{
			if (_messageDispatcher is IServerMessageDispatcher)
			{
				return (IServerMessageDispatcher)_messageDispatcher;
			}
			throw new InvalidOperationException();
		}

		public virtual IClientMessageDispatcher ClientMessageDispatcher()
		{
			if (_messageDispatcher is IClientMessageDispatcher)
			{
				return (IClientMessageDispatcher)_messageDispatcher;
			}
			throw new InvalidOperationException();
		}

		public virtual void SetMessageDispatcher(IMessageDispatcher messageDispatcher)
		{
			_messageDispatcher = messageDispatcher;
		}

		public virtual void LogMsg(int msgCode, string msg)
		{
			Container().LogMsg(msgCode, msg);
		}

		/// <summary>to be overridden by implementors of MessageWithResponse</summary>
		public virtual void PostProcessAtServer()
		{
		}

		// do nothing by default
		protected virtual Db4objects.Db4o.Internal.Transaction SystemTransaction()
		{
			return Container().SystemTransaction();
		}

		object System.ICloneable.Clone()
		{
			return MemberwiseClone();
		}
	}
}
