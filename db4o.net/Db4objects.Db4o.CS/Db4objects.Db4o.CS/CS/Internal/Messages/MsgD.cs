/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <summary>Messages with Data for Client/Server Communication</summary>
	public class MsgD : Msg
	{
		internal StatefulBuffer _payLoad;

		internal MsgD() : base()
		{
		}

		internal MsgD(string aName) : base(aName)
		{
		}

		public override ByteArrayBuffer GetByteLoad()
		{
			return _payLoad;
		}

		public sealed override StatefulBuffer PayLoad()
		{
			return _payLoad;
		}

		public virtual void PayLoad(StatefulBuffer writer)
		{
			_payLoad = writer;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForByte(Transaction trans
			, byte b)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD msg = GetWriterForLength(trans, 1);
			msg._payLoad.WriteByte(b);
			return msg;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForBuffer(Transaction trans
			, ByteArrayBuffer buffer)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD writer = GetWriterForLength(trans, buffer
				.Length());
			writer.WriteBytes(buffer._buffer);
			return writer;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForLength(Transaction trans
			, int length)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = (Db4objects.Db4o.CS.Internal.Messages.MsgD
				)PublicClone();
			message.SetTransaction(trans);
			message._payLoad = new StatefulBuffer(trans, length + Const4.MessageLength);
			message.WriteInt(_msgID);
			message.WriteInt(length);
			if (trans.ParentTransaction() == null)
			{
				message._payLoad.WriteByte(Const4.SystemTrans);
			}
			else
			{
				message._payLoad.WriteByte(Const4.UserTrans);
			}
			return message;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriter(Transaction trans)
		{
			return GetWriterForLength(trans, 0);
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForInts(Transaction trans
			, int[] ints)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = GetWriterForLength(trans, Const4
				.IntLength * ints.Length);
			for (int i = 0; i < ints.Length; i++)
			{
				message.WriteInt(ints[i]);
			}
			return message;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForIntArray(Transaction
			 a_trans, int[] ints, int length)
		{
			return GetWriterForIntSequence(a_trans, length, IntIterators.ForInts(ints, length
				));
		}

		public virtual Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForIntSequence(
			Transaction trans, int length, IEnumerator iterator)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = GetWriterForLength(trans, Const4
				.IntLength * (length + 1));
			message.WriteInt(length);
			while (iterator.MoveNext())
			{
				message.WriteInt(((int)iterator.Current));
			}
			return message;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForInt(Transaction a_trans
			, int id)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = GetWriterForLength(a_trans, Const4
				.IntLength);
			message.WriteInt(id);
			return message;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForIntString(Transaction
			 a_trans, int anInt, string str)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = GetWriterForLength(a_trans, Const4
				.stringIO.Length(str) + Const4.IntLength * 2);
			message.WriteInt(anInt);
			message.WriteString(str);
			return message;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForLong(Transaction a_trans
			, long a_long)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = GetWriterForLength(a_trans, Const4
				.LongLength);
			message.WriteLong(a_long);
			return message;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForLongs(Transaction trans
			, long[] longs)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = GetWriterForLength(trans, Const4
				.LongLength * longs.Length);
			for (int i = 0; i < longs.Length; i++)
			{
				message.WriteLong(longs[i]);
			}
			return message;
		}

		public virtual Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForSingleObject
			(Transaction trans, object obj)
		{
			SerializedGraph serialized = Serializer.Marshall(trans.Container(), obj);
			Db4objects.Db4o.CS.Internal.Messages.MsgD msg = GetWriterForLength(trans, serialized
				.MarshalledLength());
			serialized.Write(msg._payLoad);
			return msg;
		}

		public Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriterForString(Transaction a_trans
			, string str)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = GetWriterForLength(a_trans, Const4
				.stringIO.Length(str) + Const4.IntLength);
			message.WriteString(str);
			return message;
		}

		public virtual Db4objects.Db4o.CS.Internal.Messages.MsgD GetWriter(StatefulBuffer
			 bytes)
		{
			Db4objects.Db4o.CS.Internal.Messages.MsgD message = GetWriterForLength(bytes.Transaction
				(), bytes.Length());
			message._payLoad.Append(bytes._buffer);
			return message;
		}

		public virtual byte[] ReadBytes()
		{
			return _payLoad.ReadBytes(ReadInt());
		}

		public int ReadInt()
		{
			return _payLoad.ReadInt();
		}

		public long ReadLong()
		{
			return _payLoad.ReadLong();
		}

		public bool ReadBoolean()
		{
			return _payLoad.ReadByte() != 0;
		}

		public virtual object ReadObjectFromPayLoad()
		{
			return Serializer.Unmarshall(Container(), _payLoad);
		}

		internal sealed override Msg ReadPayLoad(IMessageDispatcher messageDispatcher, Transaction
			 a_trans, Socket4Adapter sock, ByteArrayBuffer reader)
		{
			int length = reader.ReadInt();
			a_trans = CheckParentTransaction(a_trans, reader);
			Db4objects.Db4o.CS.Internal.Messages.MsgD command = (Db4objects.Db4o.CS.Internal.Messages.MsgD
				)PublicClone();
			command.SetTransaction(a_trans);
			command.SetMessageDispatcher(messageDispatcher);
			command._payLoad = ReadMessageBuffer(a_trans, sock, length);
			return command;
		}

		public string ReadString()
		{
			int length = ReadInt();
			return Const4.stringIO.Read(_payLoad, length);
		}

		public virtual object ReadSingleObject()
		{
			return Serializer.Unmarshall(Container(), SerializedGraph.Read(_payLoad));
		}

		public void WriteBytes(byte[] aBytes)
		{
			_payLoad.Append(aBytes);
		}

		public void WriteInt(int aInt)
		{
			_payLoad.WriteInt(aInt);
		}

		public void WriteLong(long l)
		{
			_payLoad.WriteLong(l);
		}

		public void WriteString(string aStr)
		{
			_payLoad.WriteInt(aStr.Length);
			Const4.stringIO.Write(_payLoad, aStr);
		}
	}
}
