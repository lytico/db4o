/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * Base class for balanced trees.
 * 
 * @exclude
 */
public class TreeInt extends Tree implements ReadWriteable {

	public int _key;

	public TreeInt(int a_key) {
		this._key = a_key;
	}

	public int compare(Tree a_to) {
		return _key - ((TreeInt) a_to)._key;
	}

	Tree deepClone() {
		return new TreeInt(_key);
	}

	public boolean duplicates() {
		return false;
	}

	public static final TreeInt find(Tree a_in, int a_key) {
		if (a_in == null) {
			return null;
		}
		return ((TreeInt) a_in).find(a_key);
	}

	final TreeInt find(int a_key) {
		int cmp = _key - a_key;
		if (cmp < 0) {
			if (_subsequent != null) {
				return ((TreeInt) _subsequent).find(a_key);
			}
		} else {
			if (cmp > 0) {
				if (_preceding != null) {
					return ((TreeInt) _preceding).find(a_key);
				}
			} else {
				return this;
			}
		}
		return null;
	}

	public Object read(YapReader a_bytes) {
		return new TreeInt(a_bytes.readInt());
	}

	public void write(YapReader a_writer) {
		a_writer.writeInt(_key);
	}

	public int ownLength() {
		return YapConst.YAPINT_LENGTH;
	}

	boolean variableLength() {
		return false;
	}

	QCandidate toQCandidate(QCandidates candidates) {
		QCandidate qc = new QCandidate(candidates, null, _key, true);
		qc._preceding = toQCandidate((TreeInt) _preceding, candidates);
		qc._subsequent = toQCandidate((TreeInt) _subsequent, candidates);
		qc._size = _size;
		return qc;
	}

	public static QCandidate toQCandidate(TreeInt tree, QCandidates candidates) {
		if (tree == null) {
			return null;
		}
		return tree.toQCandidate(candidates);
	}

	public String toString() {
		return "" + _key;
	}

	protected Tree shallowCloneInternal(Tree tree) {
		TreeInt treeint=(TreeInt)super.shallowCloneInternal(tree);
		treeint._key=_key;
		return treeint;
	}

	public Object shallowClone() {
		TreeInt treeint= new TreeInt(_key);
		return shallowCloneInternal(treeint);
	}

}
