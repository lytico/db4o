/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class TreeIntObject extends TreeInt {

	public Object _object;

	public TreeIntObject(int a_key) {
		super(a_key);
	}

	public TreeIntObject(int a_key, Object a_object) {
		super(a_key);
		_object = a_object;
	}

	public Object shallowClone() {
		return shallowCloneInternal(new TreeIntObject(_key));
	}

	protected Tree shallowCloneInternal(Tree tree) {
		TreeIntObject tio = (TreeIntObject) super.shallowCloneInternal(tree);
		tio._object = _object;
		return tio;
	}

	public Object read(YapReader a_bytes) {
		int key = a_bytes.readInt();
		Object obj = null;
		if (_object instanceof Tree) {
			obj = new TreeReader(a_bytes, (Tree) _object).read();
		} else {
			obj = ((Readable) _object).read(a_bytes);
		}
		return new TreeIntObject(key, obj);
	}

	public void write(YapReader a_writer) {
		a_writer.writeInt(_key);
		if (_object == null) {
			a_writer.writeInt(0);
		} else {
			if (_object instanceof Tree) {
				Tree.write(a_writer, (Tree) _object);
			} else {
				((ReadWriteable) _object).write(a_writer);
			}
		}
	}

	public int ownLength() {
		if (_object == null) {
			return YapConst.YAPINT_LENGTH * 2;
		} else {
			return YapConst.YAPINT_LENGTH + ((Readable) _object).byteCount();
		}
	}

	boolean variableLength() {
		return true;
	}

}
