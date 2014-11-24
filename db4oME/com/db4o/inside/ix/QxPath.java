/* Copyright (C) 2005   db4objects Inc.   http://www.db4o.com */
package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;

/**
 * Query Index Path
 */
class QxPath extends TreeInt {

	private final QxProcessor _processor;

	private QCon _constraint;

	final QxPath _parent;

	private IxTraverser[] _indexTraversers;

	private NIxPaths[] _ixPaths;

	private Tree _nCandidates;

	private Tree _candidates;

	private final int _depth;

	QxPath(QxProcessor processor, QxPath parent, QCon constraint, int depth) {
		super(0);
		_processor = processor;
		_parent = parent;
		_constraint = constraint;
		_depth = depth;
	}

	public Object shallowClone() {
		QxPath qpath = new QxPath(_processor, _parent, _constraint, _depth);
		qpath._indexTraversers = _indexTraversers;
		qpath._ixPaths = _ixPaths;
		qpath._nCandidates=_nCandidates;
		qpath._candidates=_candidates;
		
		return super.shallowCloneInternal(qpath);
	}

	void buildPaths() {

		int id = _constraint.identityID();
		if (id > 0) {
			processChildCandidates(new TreeInt(id));
			return;
		}

		boolean isLeaf = true;
		Iterator4 i = _constraint.iterateChildren();
		while (i.hasNext()) {
			isLeaf = false;
			QCon childConstraint = (QCon) i.next();
			if (childConstraint.canLoadByIndex()) {
				new QxPath(_processor, this, childConstraint, _depth + 1)
						.buildPaths();
			}
		}
		if (!isLeaf) {
			return;
		}
		if (!_constraint.canLoadByIndex()) {
			return;
		}

		if (!_constraint.canBeIndexLeaf()) {
			return;
		}

		_indexTraversers = new IxTraverser[] { new IxTraverser() };

		_key = ((QConObject) _constraint).findBoundsQuery(_indexTraversers[0]);

		if (_key < 0) {
			return;
		}

		if (Debug.useNIxPaths) {

			if (_key > 0) {

				_ixPaths = new NIxPaths[] { _indexTraversers[0].convert() };

				// FIXME: remove NIxPath redundancies
				// indexPaths.removeRedundancies();

				expectNixCount(_ixPaths[0], _key);
			}
		}

		_processor.addPath(this);
	}

	private void expectNixCount(NIxPaths ixPaths, int count) {
		if (Debug.ixTrees) {
			int cnt = ixPaths.count();
			if (count != cnt) {
				System.err.println("Different Index candidate count");
				System.err.println("" + count + ", " + cnt);
				// new RuntimeException().printStackTrace();
			}
		}
	}

	void load() {
		loadFromIndexTraversers();
		loadFromNixPaths();
		if (_parent == null) {
			return;
		}
		if (Debug.useNIxPaths) {
			if (_processor.exceedsLimit(Tree.size(_nCandidates), _depth)) {
				return;
			}
		} else {
			if (_processor.exceedsLimit(Tree.size(_candidates), _depth)) {
				return;
			}
		}

		QxPath parentPath = new QxPath(_processor, _parent._parent,
				_parent._constraint, _depth - 1);
		if (Debug.useNIxPaths) {
			parentPath.processChildCandidates(_nCandidates);
		} else {
			parentPath.processChildCandidates(_candidates);
		}
	}

	private void loadFromIndexTraversers() {
		if (_indexTraversers == null) {
			return;
		}
		for (int i = 0; i < _indexTraversers.length; i++) {
			_indexTraversers[i].visitAll(new Visitor4() {
				public void visit(Object a_object) {
					int id = ((Integer) a_object).intValue();
					if (_candidates == null) {
						_candidates = new TreeInt(id);
					} else {
						_candidates = _candidates.add(new TreeInt(id));
					}
				}
			});
		}
	}

	private void loadFromNixPaths() {
		if (!Debug.useNIxPaths) {
			return;
		}
		if (_ixPaths == null) {
			return;
		}
		for (int i = 0; i < _ixPaths.length; i++) {
			if (_ixPaths[i] != null) {
				_ixPaths[i].traverse(new Visitor4() {
					public void visit(Object a_object) {
						int id = ((Integer) a_object).intValue();
						if (_nCandidates == null) {
							_nCandidates = new TreeInt(id);
						} else {
							_nCandidates = _nCandidates.add(new TreeInt(id));
						}
					}
				});
			}
		}
		compareLoadedNixPaths();
	}

	private void compareLoadedNixPaths() {

		if (!Debug.ixTrees) {
			return;
		}

		if (Tree.size(_candidates) != Tree.size(_nCandidates)) {
			System.err.println("Different index tree size");
			System.err.println("" + Tree.size(_candidates) + ", "
					+ Tree.size(_nCandidates));
			// new RuntimeException().printStackTrace();
			return;
		}

		Tree.traverse(_nCandidates, new Visitor4() {
			public void visit(Object a_object) {
				if (_candidates.find((Tree) a_object) == null) {
					System.err.println("Element not in old tree");
					System.err.println(a_object);
					// new RuntimeException().printStackTrace();
				}
			}
		});
	}

	void processChildCandidates(Tree candidates) {

		if (candidates == null) {
			_processor.addPath(this);
			return;
		}

		if (_parent == null) {
			_candidates = candidates;
			_nCandidates = candidates;
			_processor.addPath(this);
			return;
		}

		_indexTraversers = new IxTraverser[candidates.size()];

		if (Debug.useNIxPaths) {
			_ixPaths = new NIxPaths[candidates.size()];
		}

		final int[] ix = new int[] { 0 };
		final boolean[] err = new boolean[] { false };
		candidates.traverse(new Visitor4() {
			public void visit(Object a_object) {

				int idx = ix[0]++;

				_indexTraversers[idx] = new IxTraverser();
				int count = _indexTraversers[idx].findBoundsQuery(_constraint,
						new Integer(((TreeInt) a_object)._key));
				if (count >= 0) {
					_key += count;
				} else {
					err[0] = true;
				}

				if (Debug.useNIxPaths) {

					if (count > 0) {
						_ixPaths[idx] = _indexTraversers[idx].convert();

						expectNixCount(_ixPaths[idx], count);

					}

				}

			}
		});
		if (err[0]) {
			return;
		}
		_processor.addPath(this);
	}

	public boolean isTopLevelComplete() {

		if (_parent == null) {

			// FIXME: and if all joins are evaluated

			return true;

		}
		return false;
	}

	boolean onSameFieldAs(QxPath other) {
		return _constraint.onSameFieldAs(other._constraint);
	}

	Tree toQCandidates(QCandidates candidates) {
		if (Debug.useNIxPaths) {
			return TreeInt.toQCandidate((TreeInt) _nCandidates, candidates);
		}
		return TreeInt.toQCandidate((TreeInt) _candidates, candidates);
	}

	void mergeForSameField(QxPath other) {
		if (other._ixPaths == null) {
			return;
		}
		int oldCount = _ixPaths[0].count();
		for (int i = 0; i < other._ixPaths.length; i++) {
			if (other._ixPaths[i] != null) {
				other._ixPaths[i]._paths.traverse(new Visitor4() {
					public void visit(Object a_object) {
						_ixPaths[0].add((NIxPath) a_object);
					}
				});
			}
		}
		int newCount = _ixPaths[0].count();
		_key += newCount - oldCount;
	}

}
