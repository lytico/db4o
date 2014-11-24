/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.inside.ix.*;
import com.db4o.query.*;
import com.db4o.reflect.*;
import com.db4o.types.*;

/**
 * Base class for all constraints on queries. 
 * 
 * @exclude
 */
public abstract class QCon implements Constraint, Visitor4, Unversioned {
	
	//Used for query debug only.
    static final IDGenerator idGenerator = new IDGenerator();

    // our candidate object tree
    transient QCandidates i_candidates;

    // collection of QCandidates to collect children elements and to
    // execute children. For convenience we hold them in the constraint,
    // so we can do collection and execution in two steps
    public Collection4 i_childrenCandidates;

    // all subconstraints
    public List4 _children;

    // for evaluation
    public QE i_evaluator = QE.DEFAULT;

    // ID handling for fast find of QConstraint objects in 
    // pending OR evaluations
    public int i_id;

    // ANDs and ORs on this constraint
    public Collection4 i_joins;

    // positive indicates ascending, negative indicates descending
    // value indicates ID supplied by ID generator.
    // lower IDs are applied first
    public int i_orderID = 0;

    // the parent of this constraint or null, if this is a root
    public QCon i_parent;

    // prevents circular calls on removal
    public  boolean i_removed = false;

    // our transaction to get a stream object anywhere
    transient Transaction i_trans;

    public QCon() {
        // C/S only
    }

    QCon(Transaction a_trans) {
        i_id = idGenerator.next();
        i_trans = a_trans;
    }

    QCon addConstraint(QCon a_child) {
        _children = new List4(_children, a_child);
        return a_child;
    }

    void addJoin(QConJoin a_join) {
        if (i_joins == null) {
            i_joins = new Collection4();
        }
        i_joins.add(a_join);
    }

    QCon addSharedConstraint(QField a_field, Object a_object) {
        QConObject newConstraint = new QConObject(i_trans, this, a_field, a_object);
        addConstraint(newConstraint);
        return newConstraint;
    }

    public Constraint and(Constraint andWith) {
        synchronized (streamLock()) {
            return join(andWith, true);
        }
    }

    void applyOrdering() {
        if (i_orderID != 0) {
            QCon root = getRoot();
            root.i_candidates.applyOrdering(i_candidates.i_ordered, i_orderID);
        }
    }

    boolean attach(final QQuery query, final String a_field) {
    	
    	final QCon qcon = this;
    	
    	YapClass yc = getYapClass();
    	final boolean[] foundField = { false };
    	forEachChildField(a_field, new Visitor4() {
    		public void visit(Object obj) {
    			foundField[0] = true;
    			query.addConstraint((QCon) obj);
    		}
    	});
    	
    	if (foundField[0]) {
    		return true;
    	}
    	
    	QField qf = null;
    	
    	if (yc == null || yc.holdsAnyClass()) {
    		
    		final int[] count = { 0 };
    		final YapField[] yfs = { null };
    		
    		i_trans.i_stream.i_classCollection.attachQueryNode(a_field, new Visitor4() {
    			public void visit(Object obj) {
    				yfs[0] = (YapField) ((Object[]) obj)[1];
    				count[0]++;
    			}
    		});
    		
    		if (count[0] == 0) {
    			return false;
    		}
    		
    		if (count[0] == 1) {
    			qf = yfs[0].qField(i_trans);
    		} else {
    			qf = new QField(i_trans, a_field, null, 0, 0);
    		}
    		
    	} else {
    		
    		if (yc != null) {
    			YapField yf = yc.getYapField(a_field);
    			if (yf != null) {
    				qf = yf.qField(i_trans);
    				
    			}
    		}
    		if (qf == null) {
    			qf = new QField(i_trans, a_field, null, 0, 0);
    		}
    	}
    	
    	QConPath qcp = new QConPath(i_trans, qcon, qf);
    	query.addConstraint(qcp);
    	qcon.addConstraint(qcp);
    	return true;
    }
    
    public boolean canBeIndexLeaf(){
        return false;
    }

    public boolean canLoadByIndex(){
        // virtual
        return false;
    }

    void checkLastJoinRemoved() {
        if (i_joins.size() == 0) {
            i_joins = null;
        }
    }

    void collect(QCandidates a_candidates) {
        // virtual
    }

    public Constraint contains() {
        throw notSupported();
    }

    void createCandidates(Collection4 a_candidateCollection) {
        Iterator4 j = a_candidateCollection.iterator();
        while (j.hasNext()) {
            QCandidates candidates = (QCandidates) j.next();
            if (candidates.tryAddConstraint(this)) {
                i_candidates = candidates;
                return;
            }
        }
        i_candidates = new QCandidates(i_trans, getYapClass(), getField());
        i_candidates.addConstraint(this);
        a_candidateCollection.add(i_candidates);
    }

    void doNotInclude(QCandidate a_root) {
        if(DTrace.enabled){
            DTrace.DONOTINCLUDE.log(i_id);
        }
        if (Deploy.debugQueries) {
            System.out.println("QCon.doNotInclude " + i_id /*+ " " + getYapClass()*/
            );
        }
        if (i_parent != null) {
            i_parent.visit1(a_root, this, false);
        } else {
            a_root.doNotInclude();
        }
    }

    public Constraint equal() {
        throw notSupported();
    }

    boolean evaluate(QCandidate a_candidate) {
        throw YapConst.virtualException();
    }

    void evaluateChildren() {
        Iterator4 i = i_childrenCandidates.iterator();
        while (i.hasNext()) {
            ((QCandidates) i.next()).evaluate();
        }
    }

    void evaluateCollectChildren() {
        if(DTrace.enabled){
            DTrace.COLLECT_CHILDREN.log(i_id);
        }
        Iterator4 i = i_childrenCandidates.iterator();
        while (i.hasNext()) {
            ((QCandidates) i.next()).collect(i_candidates);
        }
    }

    void evaluateCreateChildrenCandidates() {
        i_childrenCandidates = new Collection4();
    	Iterator4 i = iterateChildren();
    	while(i.hasNext()){
			((QCon)i.next()).createCandidates(i_childrenCandidates);
    	}
    }

    void evaluateEvaluations() {
        Iterator4 i = iterateChildren();
		while(i.hasNext()){
			((QCon)i.next()).evaluateEvaluationsExec(i_candidates, true);
		}
    }

    void evaluateEvaluationsExec(QCandidates a_candidates, boolean rereadObject) {
        // virtual
    }

    void evaluateSelf() {
        i_candidates.filter(this);
    }

    void evaluateSimpleChildren() {
    	
    	// TODO: sort the constraints for YapFields first,
    	// so we stay with the same YapField
    	
    	if(_children == null) {
    		return;
    	}
    	
        Iterator4 i = iterateChildren();
        while(i.hasNext()){
    		QCon qcon = (QCon)i.next();
    		i_candidates.setCurrentConstraint(qcon);
    		qcon.setCandidates(i_candidates);
    		qcon.evaluateSimpleExec(i_candidates);
    		qcon.applyOrdering();
    	}
    	i_candidates.setCurrentConstraint(null);
    }

    void evaluateSimpleExec(QCandidates a_candidates) {
        // virtual
    }

    void exchangeConstraint(QCon a_exchange, QCon a_with) {
        List4 previous = null;
        List4 current = _children;
        while (current != null) {
            if (current._element == a_exchange) {
                if (previous == null) {
                    _children = current._next;
                } else {
                    previous._next = current._next;
                }
            }
            previous = current;
            current = current._next;
        }
        
        _children = new List4(_children, a_with);
    }

    void forEachChildField(final String name, final Visitor4 visitor) {
    	Iterator4 i = iterateChildren();
    	while(i.hasNext()){
    		Object obj = i.next();
    		if (obj instanceof QConObject) {
    			if (((QConObject) obj).i_field.i_name.equals(name)) {
    				visitor.visit(obj);
    			}
    		}
    	}
    }

    QField getField() {
        return null;
    }

    public Object getObject() {
        throw notSupported();
    }

    QCon getRoot() {
        if (i_parent != null) {
            return i_parent.getRoot();
        }
        return this;
    }

    QCon getTopLevelJoin() {
        if(! hasJoins()){
            return this;
        }
        Iterator4 i = iterateJoins();
        if (i_joins.size() == 1) {
            return ((QCon) i.next()).getTopLevelJoin();
        }
        Collection4 col = new Collection4();
        while (i.hasNext()) {
            col.ensure(((QCon) i.next()).getTopLevelJoin());
        }
        i = col.iterator();
        QCon qcon = (QCon) i.next();
        if (col.size() == 1) {
            return qcon;
        }
        while (i.hasNext()) {
            qcon = (QCon) qcon.and((Constraint) i.next());
        }
        return qcon;
    }

    YapClass getYapClass() {
        return null;
    }

    public Constraint greater() {
        throw notSupported();
    }
    
    public boolean hasChildren(){
        return _children != null;
    }
    
    public boolean hasOrJoins(){
        Collection4 lookedAt = new Collection4();
        return hasOrJoins(lookedAt);
    }
    
    boolean hasOrJoins(Collection4 lookedAt){
        if(lookedAt.containsByIdentity(this)){
            return false;
        }
        lookedAt.add(this);
        if(i_joins == null){
            return false;
        }
        Iterator4 i = iterateJoins();
        while(i.hasNext()){
            QConJoin join = (QConJoin)i.next();
            if(! join.i_and){
                return true;
            }
            if(join.hasOrJoins(lookedAt)){
                return true;
            }
        }
        return false;
    }
    
    public boolean hasJoins(){
        if(i_joins == null){
            return false;
        }
        return i_joins.size() > 0;
    }

    boolean hasObjectInParentPath(Object obj) {
        if (i_parent != null) {
            return i_parent.hasObjectInParentPath(obj);
        }
        return false;
    }

    public Constraint identity() {
        throw notSupported();
    }

    public int identityID() {
        return 0;
    }
    
    public IxTree indexRoot(){
        throw YapConst.virtualException();
    }

    boolean isNot() {
        return i_evaluator instanceof QENot;
    }

    boolean isNullConstraint() {
        return false;
    }
    
    Iterator4 iterateJoins(){
        if(i_joins == null){
            return Iterator4Impl.EMPTY;
        }
        return i_joins.iterator();
    }
    
    public Iterator4 iterateChildren(){
        if(_children == null){
            return Iterator4Impl.EMPTY;
        }
        return new Iterator4Impl(_children);
    }
    
    Constraint join(Constraint a_with, boolean a_and) {
        if (!(a_with instanceof QCon) /*|| a_with == this*/
            ) {

            // TODO: one of our STOr test cases somehow carries 
            // the same constraint twice. This may be a result
            // of a funny AND. Check!

            return null;
        }
        if (a_with == this) {
            return this;
        }
        return join1((QCon) a_with, a_and);
    }

    Constraint join1(QCon a_with, boolean a_and) {

        if (a_with instanceof QConstraints) {
            int j = 0;
            Collection4 joinHooks = new Collection4();
            Constraint[] constraints = ((QConstraints) a_with).toArray();
            for (j = 0; j < constraints.length; j++) {
                joinHooks.ensure(((QCon) constraints[j]).joinHook());
            }
            Constraint[] joins = new Constraint[joinHooks.size()];
            j = 0;
            Iterator4 i = joinHooks.iterator();
            while (i.hasNext()) {
                joins[j++] = join((Constraint) i.next(), a_and);
            }
            return new QConstraints(i_trans, joins);
        }

        QCon myHook = joinHook();
        QCon otherHook = a_with.joinHook();
        if (myHook == otherHook) {
            // You might like to check out, what happens, if you
            // remove this line. It seems to open a bug in an
            // StOr testcase.
            return myHook;
        }

        QConJoin cj = new QConJoin(i_trans, myHook, otherHook, a_and);
        myHook.addJoin(cj);
        otherHook.addJoin(cj);
        return cj;
    }

    QCon joinHook() {
        return getTopLevelJoin();
    }

    public Constraint like() {
        throw notSupported();
    }

    public Constraint startsWith(boolean caseSensitive) {
        throw notSupported();
    }

    public Constraint endsWith(boolean caseSensitive) {
        throw notSupported();
    }

    void log(String indent) {
        if (Deploy.debugQueries) {

            final String childIndent = "   " + indent;
            String name = getClass().getName();
            int pos = J2MEUtil.lastIndexOf(name,".") + 1;
            name = name.substring(pos);
            System.out.println(indent + name + " " + logObject() + "   " + i_id);
            // System.out.println(indent + "JOINS");
            if (hasJoins()) {
                Iterator4 i = iterateJoins();
                while (i.hasNext()) {
                    QCon join = (QCon) i.next();
                    // joins += join.i_id + " ";
                    join.log(childIndent);
                }
            }
            //		System.out.println(joins);
            //		System.out.println(indent + getClass().getName() + " " + i_id + " " + i_debugField + " " + joins );
            // System.out.println(indent + "CONSTRAINTS");
            
			if(_children != null){
				Iterator4 i = new Iterator4Impl(_children);
				while(i.hasNext()){
					((QCon)i.next()).log(childIndent);
				}
			}
        }
    }

    String logObject() {
        return "";
    }

    void marshall() {
        Iterator4 i = iterateChildren();
		while(i.hasNext()){
			((QCon)i.next()).marshall();
		}
    }

    public Constraint not() {
        synchronized (streamLock()) {
            if (!(i_evaluator instanceof QENot)) {
                i_evaluator = new QENot(i_evaluator);
            }
            return this;
        }
    }

    private RuntimeException notSupported() {
        return new RuntimeException("Not supported.");
    }
    
    public boolean onSameFieldAs(QCon other){
        return false;
    }

    public Constraint or(Constraint orWith) {
        synchronized (streamLock()) {
            return join(orWith, false);
        }
    }

    boolean remove() {
        if (!i_removed) {
            i_removed = true;
            removeChildrenJoins();
            return true;
        }
        return false;
    }

    void removeChildrenJoins() {
        if (! hasJoins()) {
        	return;
        }
        Iterator4 i = iterateJoins();
        while(i.hasNext()){
        	QConJoin qcj = (QConJoin) i.next();
        	if (qcj.removeForParent(this)) {
        		i_joins.remove(qcj);
        	}
        }
        checkLastJoinRemoved();
    }

    void removeJoin(QConJoin a_join) {
        i_joins.remove(a_join);
        checkLastJoinRemoved();
    }

    void removeNot() {
        if (isNot()) {
            i_evaluator = ((QENot) i_evaluator).i_evaluator;
        }
    }

    public void setCandidates(QCandidates a_candidates) {
        i_candidates = a_candidates;
    }

    void setOrdering(int a_ordering) {
        i_orderID = a_ordering;
    }

    void setParent(QCon a_newParent) {
        i_parent = a_newParent;
    }

    QCon shareParent(Object a_object, boolean[] removeExisting) {
        // virtual
        return null;
    }

    QConClass shareParentForClass(ReflectClass a_class, boolean[] removeExisting) {
        // virtual
        return null;
    }

    public Constraint smaller() {
        throw notSupported();
    }

    protected Object streamLock() {
        return i_trans.i_stream.i_lock;
    }

    boolean supportsOrdering() {
        return false;
    }

    void unmarshall(final Transaction a_trans) {
    	if (i_trans != null) {
    		return;
    	}
    	i_trans = a_trans;
    	unmarshallParent(a_trans);
    	unmarshallJoins(a_trans);
        unmarshallChildren(a_trans);
    }

	private void unmarshallParent(final Transaction a_trans) {
		if (i_parent != null) {
    		i_parent.unmarshall(a_trans);
    	}
	}

	private void unmarshallChildren(final Transaction a_trans) {
		Iterator4 i = iterateChildren();
        while(i.hasNext()){
            ((QCon)i.next()).unmarshall(a_trans);
        }
	}

	private void unmarshallJoins(final Transaction a_trans) {
		if (hasJoins()) {
    		Iterator4 i = iterateJoins();
    		while (i.hasNext()) {
    			((QCon) i.next()).unmarshall(a_trans);
    		}
    	}
	}

    public void visit(Object obj) {
        QCandidate qc = (QCandidate) obj;
        visit1(qc.getRoot(), this, evaluate(qc));
    }

    void visit(QCandidate a_root, boolean res) {
        visit1(a_root, this, i_evaluator.not(res));
    }

    void visit1(QCandidate a_root, QCon a_reason, boolean res) {

        // The a_reason parameter makes it eays to distinguish
        // between calls from above (a_reason == this) and below.

        if (hasJoins()) {
            // this should probably be on the Join
            Iterator4 i = iterateJoins();
            while (i.hasNext()) {
                a_root.evaluate(new QPending((QConJoin) i.next(), this, res));
            }
        } else {
            if (!res) {
                doNotInclude(a_root);
            }
        }
    }

    final void visitOnNull(final QCandidate a_root) {

        // TODO: It may be more efficient to rule out 
        // all possible keepOnNull issues when starting
        // evaluation.

        if (Deploy.debugQueries) {
            System.out.println("QCon.visitOnNull " + i_id);
        }
        
		Iterator4 i = iterateChildren();
		while(i.hasNext()){
			((QCon)i.next()).visitOnNull(a_root);
		}

        if (visitSelfOnNull()) {
            visit(a_root, isNullConstraint());
        }

    }

    boolean visitSelfOnNull() {
        return true;
    }

}
