/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;


/**
 * @exclude
 */
public class TreeIntObject<T> extends TreeInt {

	public T _object;

	public TreeIntObject(int a_key) {
		super(a_key);
	}

	public TreeIntObject(int a_key, T a_object) {
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
    
    public Object getObject() {
        return _object;
    }
    
    public void setObject(T obj) {
        _object = obj;
    }

	public Object read(ByteArrayBuffer a_bytes) {
		int key = a_bytes.readInt();
		Object obj = null;
		if (_object instanceof TreeInt) {
			obj = new TreeReader(a_bytes, (Readable) _object).read();
		} else {
			obj = ((Readable) _object).read(a_bytes);
		}
		return new TreeIntObject(key, obj);
	}

	public void write(ByteArrayBuffer a_writer) {
		a_writer.writeInt(_key);
		if (_object == null) {
			a_writer.writeInt(0);
		} else {
			if (_object instanceof TreeInt) {
				TreeInt.write(a_writer, (TreeInt) _object);
			} else {
				((ReadWriteable) _object).write(a_writer);
			}
		}
	}

	public int ownLength() {
		if (_object == null) {
			return Const4.INT_LENGTH * 2;
		} 
		return Const4.INT_LENGTH + ((Readable) _object).marshalledLength();
	}

	boolean variableLength() {
		return true;
	}

	public static <T> TreeIntObject<T> add(TreeIntObject<T> tree, int key, T value) {
		return Tree.add(tree, new TreeIntObject<T>(key, value));
	}
}
