/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public sealed class FreeSlotNode : TreeInt
	{
		internal static int sizeLimit;

		internal Db4objects.Db4o.Internal.Freespace.FreeSlotNode _peer;

		internal FreeSlotNode(int a_key) : base(a_key)
		{
		}

		public override object ShallowClone()
		{
			Db4objects.Db4o.Internal.Freespace.FreeSlotNode frslot = new Db4objects.Db4o.Internal.Freespace.FreeSlotNode
				(_key);
			frslot._peer = _peer;
			return base.ShallowCloneInternal(frslot);
		}

		internal void CreatePeer(int a_key)
		{
			_peer = new Db4objects.Db4o.Internal.Freespace.FreeSlotNode(a_key);
			_peer._peer = this;
		}

		public override bool Duplicates()
		{
			return true;
		}

		public sealed override int OwnLength()
		{
			return Const4.IntLength * 2;
		}

		internal static Tree RemoveGreaterOrEqual(Db4objects.Db4o.Internal.Freespace.FreeSlotNode
			 a_in, TreeIntObject a_finder)
		{
			if (a_in == null)
			{
				return null;
			}
			int cmp = a_in._key - a_finder._key;
			if (cmp == 0)
			{
				a_finder._object = a_in;
				// the highest node in the hierarchy !!!
				return a_in.Remove();
			}
			if (cmp > 0)
			{
				a_in._preceding = RemoveGreaterOrEqual((Db4objects.Db4o.Internal.Freespace.FreeSlotNode
					)((Tree)a_in._preceding), a_finder);
				if (a_finder._object != null)
				{
					a_in._size--;
					return a_in;
				}
				a_finder._object = a_in;
				return a_in.Remove();
			}
			a_in._subsequent = RemoveGreaterOrEqual((Db4objects.Db4o.Internal.Freespace.FreeSlotNode
				)((Tree)a_in._subsequent), a_finder);
			if (a_finder._object != null)
			{
				a_in._size--;
			}
			return a_in;
		}

		public override object Read(ByteArrayBuffer buffer)
		{
			int size = buffer.ReadInt();
			int address = buffer.ReadInt();
			if (size > sizeLimit)
			{
				Db4objects.Db4o.Internal.Freespace.FreeSlotNode node = new Db4objects.Db4o.Internal.Freespace.FreeSlotNode
					(size);
				node.CreatePeer(address);
				if (Deploy.debug && Debug4.xbytes)
				{
					DebugCheckBuffer(buffer, node);
				}
				return node;
			}
			return null;
		}

		private void DebugCheckBuffer(ByteArrayBuffer buffer, Db4objects.Db4o.Internal.Freespace.FreeSlotNode
			 node)
		{
			if (!(buffer is StatefulBuffer))
			{
				return;
			}
			Transaction trans = ((StatefulBuffer)buffer).Transaction();
			if (!(trans.Container() is IoAdaptedObjectContainer))
			{
				return;
			}
			StatefulBuffer checker = trans.Container().CreateStatefulBuffer(trans, node._peer
				._key, node._key);
			checker.Read();
			for (int i = 0; i < node._key; i++)
			{
				if (checker.ReadByte() != (byte)'X')
				{
					Sharpen.Runtime.Out.WriteLine("!!! Free space corruption at:" + node._peer._key);
					break;
				}
			}
		}

		public sealed override void Write(ByteArrayBuffer a_writer)
		{
			// byte order: size, address
			a_writer.WriteInt(_key);
			a_writer.WriteInt(_peer._key);
		}

		// public static final void debug(FreeSlotNode a_node){
		// if(a_node == null){
		// return;
		// }
		// System.out.println("Address:" + a_node.i_key);
		// System.out.println("Length:" + a_node.i_peer.i_key);
		// debug((FreeSlotNode)a_node.i_preceding);
		// debug((FreeSlotNode)a_node.i_subsequent);
		// }
		public override string ToString()
		{
			return base.ToString();
			string str = "FreeSlotNode " + _key;
			if (_peer != null)
			{
				str += " peer: " + _peer._key;
			}
			return str;
		}
	}
}
