/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.query.*;
import com.db4o.reflect.*;

/**
 * Represents an actual object in the database. Forms a tree structure, indexed
 * by id. Can have dependents that are doNotInclude'd in the query result when
 * this is doNotInclude'd.
 * 
 * @exclude
 */
public class QCandidate extends TreeInt implements Candidate, Orderable {

	// db4o ID is stored in i_key;

	// db4o byte stream storing the object
	YapReader _bytes;

	final QCandidates _candidates;

	// Dependant candidates
	private List4 _dependants;

	// whether to include in the result set
	// may use id for optimisation ???
	boolean _include = true;

	private Object _member;

	// Comparable
	Orderable _order;

	// Possible pending joins on children
	Tree _pendingJoins;

	// The evaluation root to compare all ORs
	private QCandidate _root;

	// the YapClass of this object
	YapClass _yapClass;

	// temporary yapField and member for one field during evaluation
	YapField _yapField; // null denotes null object

	private QCandidate(QCandidates qcandidates) {
		super(0);
		_candidates = qcandidates;
	}

	private QCandidate() {
		this(null);
		// dummy constructor to get "this" out of declaration for C#
	}

	QCandidate(QCandidates candidates, Object obj, int id, boolean include) {
		super(id);
		if (DTrace.enabled) {
			DTrace.CREATE_CANDIDATE.log(id);
		}
		_candidates = candidates;
		_order = this;
		_member = obj;
		_include = include;
	}

	public Object shallowClone() {
		QCandidate qcan = new QCandidate(_candidates);
		qcan._bytes = _bytes;
		qcan._dependants = _dependants;
		qcan._include = _include;
		qcan._member = _member;
		qcan._order = _order;
		qcan._pendingJoins = _pendingJoins;
		qcan._root = _root;
		qcan._yapClass = _yapClass;
		qcan._yapField = _yapField;

		return super.shallowCloneInternal(qcan);
	}

	void addDependant(QCandidate a_candidate) {
		_dependants = new List4(_dependants, a_candidate);
	}

	private void checkInstanceOfCompare() {
		if (_member instanceof Compare) {
			_member = ((Compare) _member).compare();
			YapFile stream = getStream();
			_yapClass = stream.getYapClass(stream.reflector()
					.forObject(_member), false);
			_key = (int) stream.getID(_member);
			_bytes = stream.readReaderByID(getTransaction(), _key);
		}
	}

	// /
	// / <Tree Code>
	// /

	public int compare(Tree a_to) {
		return _order.compareTo(((QCandidate) a_to)._order);
	}

	public int compareTo(Object a_object) {
		return _key - ((TreeInt) a_object)._key;
	}

	boolean createChild(final QCandidates a_candidates) {
		if (!_include) {
			return false;
		}

		QCandidate candidate = null;

		if (_yapField != null) {
			TypeHandler4 handler = _yapField.getHandler();
			if (handler != null) {

				final YapReader[] arrayBytes = { _bytes };
				final TypeHandler4 arrayWrapper = handler.readArrayWrapper(
						getTransaction(), arrayBytes);

				if (arrayWrapper != null) {

					final int offset = arrayBytes[0]._offset;
					boolean outerRes = true;

					// The following construct is worse than not ideal.
					// For each constraint it completely reads the
					// underlying structure again. The structure could b
					// kept fairly easy. TODO: Optimize!

					Iterator4 i = a_candidates.iterateConstraints();
					while (i.hasNext()) {

						QCon qcon = (QCon) i.next();
						QField qf = qcon.getField();
						if (qf == null || qf.i_name.equals(_yapField.getName())) {

							QCon tempParent = qcon.i_parent;
							qcon.setParent(null);

							final QCandidates candidates = new QCandidates(
									a_candidates.i_trans, null, qf);
							candidates.addConstraint(qcon);

							qcon.setCandidates(candidates);
							arrayWrapper.readCandidates(arrayBytes[0],
									candidates);
							arrayBytes[0]._offset = offset;

							final boolean isNot = qcon.isNot();
							if (isNot) {
								qcon.removeNot();
							}

							candidates.evaluate();

							final Tree[] pending = new Tree[1];
							final boolean[] innerRes = { isNot };
							candidates.traverse(new Visitor4() {
								public void visit(Object obj) {

									QCandidate cand = (QCandidate) obj;

									if (cand.include()) {
										innerRes[0] = !isNot;
									}

									// Collect all pending subresults.

									if (cand._pendingJoins != null) {
										cand._pendingJoins
												.traverse(new Visitor4() {
													public void visit(
															Object a_object) {
														QPending newPending = (QPending) a_object;

														// We need to change
														// the
														// constraint here, so
														// our
														// pending collector
														// uses
														// the right
														// comparator.
														newPending
																.changeConstraint();
														QPending oldPending = (QPending) Tree
																.find(
																		pending[0],
																		newPending);
														if (oldPending != null) {

															// We only keep one
															// pending result
															// for
															// all array
															// elements.
															// and memorize,
															// whether we had a
															// true or a false
															// result.
															// or both.

															if (oldPending._result != newPending._result) {
																oldPending._result = QPending.BOTH;
															}

														} else {
															pending[0] = Tree
																	.add(
																			pending[0],
																			newPending);
														}
													}
												});
									}
								}
							});

							if (isNot) {
								qcon.not();
							}

							// In case we had pending subresults, we
							// need to communicate
							// them up to our root.
							if (pending[0] != null) {
								pending[0].traverse(new Visitor4() {
									public void visit(Object a_object) {
										getRoot().evaluate((QPending) a_object);
									}
								});
							}

							if (!innerRes[0]) {

								if (Deploy.debugQueries) {
									System.out
											.println("  Array evaluation false. Constraint:"
													+ qcon.i_id);
								}

								// Again this could be double triggering.
								// 
								// We want to clean up the "No route"
								// at some stage.

								qcon.visit(getRoot(), qcon.i_evaluator
										.not(false));

								outerRes = false;
							}

							qcon.setParent(tempParent);

						}
					}

					return outerRes;
				}

				// We may get simple types here too, if the YapField was null
				// in the higher level simple evaluation. Evaluate these
				// immediately.

				if (handler.getType() == YapConst.TYPE_SIMPLE) {
					a_candidates.i_currentConstraint.visit(this);
					return true;
				}
			}
		}

		if (candidate == null) {
			candidate = readSubCandidate(a_candidates);
			if (candidate == null) {
				return false;
			}
		}

		// fast early check for YapClass
		if (a_candidates.i_yapClass != null
				&& a_candidates.i_yapClass.isStrongTyped()) {
			if (_yapField != null) {
				TypeHandler4 handler = _yapField.getHandler();
				if (handler != null
						&& (handler.getType() == YapConst.TYPE_CLASS)) {
					YapClass yc = (YapClass) handler;
					if (yc instanceof YapClassAny) {
						yc = candidate.readYapClass();
					}
					if (!yc.canHold(a_candidates.i_yapClass.classReflector())) {
						return false;
					}
				}
			}
		}

		addDependant(a_candidates.addByIdentity(candidate));
		return true;
	}

	void doNotInclude() {
		_include = false;
		if (_dependants != null) {
			Iterator4 i = new Iterator4Impl(_dependants);
			_dependants = null;
			while (i.hasNext()) {
				((QCandidate) i.next()).doNotInclude();
			}
		}
	}

	public boolean duplicates() {
		return _order.hasDuplicates();
	}

	boolean evaluate(final QConObject a_constraint, final QE a_evaluator) {
		if (a_evaluator.identity()) {
			return a_evaluator.evaluate(a_constraint, this, null);
		}
		if (_member == null) {
			_member = value();
		}
		return a_evaluator.evaluate(a_constraint, this, a_constraint
				.translate(_member));
	}

	boolean evaluate(QPending a_pending) {

		if (Deploy.debugQueries) {
			System.out.println("Pending arrived Join: " + a_pending._join.i_id
					+ " Constraint:" + a_pending._constraint.i_id + " res:"
					+ a_pending._result);
		}

		QPending oldPending = (QPending) Tree.find(_pendingJoins, a_pending);

		if (oldPending == null) {
			a_pending.changeConstraint();
			_pendingJoins = Tree.add(_pendingJoins, a_pending);
			return true;
		} else {
			_pendingJoins = _pendingJoins.removeNode(oldPending);
			oldPending._join.evaluatePending(this, oldPending, a_pending,
					a_pending._result);
			return false;
		}

	}

	ReflectClass classReflector() {
		readYapClass();
		if (_yapClass == null) {
			return null;
		}
		return _yapClass.classReflector();
	}

	// / ***<Candidate interface code>***

	public ObjectContainer objectContainer() {
		return getStream();
	}

	public Object getObject() {
		Object obj = value(true);
		if (obj instanceof YapReader) {
			/* CHANGED (pr) */
			YapReader reader = (YapReader) obj;
			int offset = reader._offset;
			obj = reader.toString(getTransaction());
			reader._offset = offset;
		}
		return obj;
	}

	QCandidate getRoot() {
		return _root == null ? this : _root;
	}

	private YapFile getStream() {
		return getTransaction().i_file;
	}

	private Transaction getTransaction() {
		return _candidates.i_trans;
	}

	public boolean hasDuplicates() {

		// Subcandidates are evaluated along with their constraints
		// in one big QCandidates object. The tree can have duplicates
		// so evaluation can be cascaded up to different roots.

		return _root != null;
	}

	public void hintOrder(int a_order, boolean a_major) {
		_order = new Order();
		_order.hintOrder(a_order, a_major);
	}

	public boolean include() {
		return _include;
	}

	/**
	 * For external interface use only. Call doNotInclude() internally so
	 * dependancies can be checked.
	 */
	public void include(boolean flag) {
		// TODO:
		// Internal and external flag may need to be handled seperately.
		_include = flag;
	}

	void isDuplicateOf(Tree a_tree) {
		_size = 0;
		_root = (QCandidate) a_tree;
	}

	private ReflectClass memberClass() {
		return getTransaction().reflector().forObject(_member);
	}

	YapComparable prepareComparison(YapStream a_stream, Object a_constraint) {
		if (_yapField != null) {
			return _yapField.prepareComparison(a_constraint);
		}
		if (_yapClass == null) {

			YapClass yc = null;
			if (_bytes != null) {
				yc = a_stream.getYapClass(a_stream.reflector().forObject(
						a_constraint), true);
			} else {
				if (_member != null) {
					yc = a_stream.getYapClass(a_stream.reflector().forObject(
							_member), false);
				}
			}
			if (yc != null) {
				if (_member != null && _member.getClass().isArray()) {
					TypeHandler4 ydt = (TypeHandler4) yc
							.prepareComparison(a_constraint);
					if (a_stream.reflector().array().isNDimensional(
							memberClass())) {
						YapArrayN yan = new YapArrayN(a_stream, ydt, false);
						return yan;
					} else {
						YapArray ya = new YapArray(a_stream, ydt, false);
						return ya;
					}
				} else {
					return yc.prepareComparison(a_constraint);
				}
			}
			return null;
		} else {
			return _yapClass.prepareComparison(a_constraint);
		}
	}

	private void read() {
		if (_include) {
			if (_bytes == null) {
				if (_key > 0) {
					if (DTrace.enabled) {
						DTrace.CANDIDATE_READ.log(_key);
					}
					_bytes = getStream().readReaderByID(getTransaction(), _key);
					if (_bytes == null) {
						_include = false;
					}
				} else {
					_include = false;
				}
			}
		}
	}

	private QCandidate readSubCandidate(QCandidates candidateCollection) {
		int id = 0;
		read();
		if (_bytes != null) {
			final int offset = _bytes._offset;

			try {
				id = _bytes.readInt();
			} catch (Exception e) {
				return null;
			}
			_bytes._offset = offset;

			if (id != 0) {
				QCandidate candidate = new QCandidate(candidateCollection,
						null, id, true);
				candidate._root = getRoot();
				return candidate;
			}
		}
		return null;
	}

	private void readThis(boolean a_activate) {
		read();

		Transaction trans = getTransaction();
		if (trans != null) {

			_member = trans.i_stream.getByID1(trans, _key);

			if (_member != null && (a_activate || _member instanceof Compare)) {
				trans.i_stream.activate1(trans, _member);
				checkInstanceOfCompare();
			}
		}
	}

	YapClass readYapClass() {
		if (_yapClass == null) {
			read();
			if (_bytes != null) {

				_bytes._offset = 0;
				if (Deploy.debug) {
					_bytes.readBegin(0, YapConst.YAPOBJECT);
				}
				YapStream stream = getStream();
				_yapClass = stream.getYapClass(_bytes.readInt());
				if (_yapClass != null) {
					if (stream.i_handlers.ICLASS_COMPARE
							.isAssignableFrom(_yapClass.classReflector())) {
						readThis(false);
					}
				}
			}
		}
		return _yapClass;
	}

	public String toString() {
		if (!Debug4.prettyToStrings) {
			return super.toString();
		}
		String str = "QCandidate ";
		if (_yapClass != null) {
			str += "\n   YapClass " + _yapClass.getName();
		}
		if (_yapField != null) {
			str += "\n   YapField " + _yapField.getName();
		}
		if (_member != null) {
			str += "\n   Member " + _member.toString();
		}
		if (_root != null) {
			str += "\n  rooted by:\n";
			str += _root.toString();
		} else {
			str += "\n  ROOT";
		}
		return str;
	}

	void useField(QField a_field) {
		read();
		if (_bytes == null) {
			_yapField = null;
		} else {
			readYapClass();
			_member = null;
			if (a_field == null) {
				_yapField = null;
			} else {
				if (_yapClass == null) {
					_yapField = null;
				} else {
					_yapField = a_field.getYapField(_yapClass);
					if (_yapField == null
							| !_yapClass.findOffset(_bytes, _yapField)) {
						if (_yapClass.holdsAnyClass()) {
							_yapField = null;
						} else {
							_yapField = new YapFieldNull();
						}
					}
				}
			}
		}
	}

	Object value() {
		return value(false);
	}

	// TODO: This is only used for Evaluations. Handling may need
	// to be different for collections also.
	Object value(boolean a_activate) {
		if (_member == null) {
			if (_yapField == null) {
				readThis(a_activate);
			} else {
				int offset = _bytes._offset;
				try {
					_member = _yapField.readQuery(getTransaction(), _bytes);
				} catch (CorruptionException ce) {
					_member = null;
				}
				_bytes._offset = offset;
				checkInstanceOfCompare();
			}
		}
		return _member;
	}
}
