/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.inside.freespace;

import com.db4o.*;

/**
 * @exclude
 */
public final class FreeSlotNode extends TreeInt {
	public static int sizeLimit;

	FreeSlotNode _peer;

	FreeSlotNode(int a_key) {
		super(a_key);
	}

	public Object shallowClone() {
		FreeSlotNode frslot = new FreeSlotNode(_key);
		frslot._peer = _peer;
		return super.shallowCloneInternal(frslot);
	}

	final void createPeer(int a_key) {
		_peer = new FreeSlotNode(a_key);
		_peer._peer = this;
	}

	public boolean duplicates() {
		return true;
	}

	public final int ownLength() {
		return YapConst.YAPINT_LENGTH * 2;
	}

	final static Tree removeGreaterOrEqual(FreeSlotNode a_in,
			TreeIntObject a_finder) {
		if (a_in == null) {
			return null;
		}
		int cmp = a_in._key - a_finder._key;
		if (cmp == 0) {
			a_finder._object = a_in; // the highest node in the hierarchy !!!
			return a_in.remove();
		} else {
			if (cmp > 0) {
				a_in._preceding = removeGreaterOrEqual(
						(FreeSlotNode) a_in._preceding, a_finder);
				if (a_finder._object != null) {
					a_in._size--;
					return a_in;
				}
				a_finder._object = a_in;
				return a_in.remove();
			} else {
				a_in._subsequent = removeGreaterOrEqual(
						(FreeSlotNode) a_in._subsequent, a_finder);
				if (a_finder._object != null) {
					a_in._size--;
				}
				return a_in;
			}
		}
	}

	public Object read(YapReader a_reader) {
		int size = a_reader.readInt();
		int address = a_reader.readInt();
		if (size > sizeLimit) {
			FreeSlotNode node = new FreeSlotNode(size);
			node.createPeer(address);
			if (Deploy.debug) {
				if (a_reader instanceof YapWriter) {
					Transaction trans = ((YapWriter) a_reader).getTransaction();
					if (trans.i_stream instanceof YapRandomAccessFile) {
						YapWriter checker = trans.i_stream.getWriter(trans,
								node._peer._key, node._key);
						checker.read();
						for (int i = 0; i < node._key; i++) {
							if (checker.readByte() != (byte) 'X') {
								System.out
										.println("!!! Free space corruption at:"
												+ node._peer._key);
								break;
							}
						}
					}
				}
			}
			return node;
		}
		return null;
	}

	public final void write(YapReader a_writer) {
		// byte order: size, address
		a_writer.writeInt(_key);
		a_writer.writeInt(_peer._key);
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

	public String toString() {
		if (!Debug.freespace) {
			return super.toString();

		}
		String str = "FreeSlotNode " + _key;
		if (_peer != null) {
			str += " peer: " + _peer._key;
		}
		return str;
	}
}
