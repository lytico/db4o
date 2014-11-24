/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.classindex.*;
import com.db4o.internal.diagnostic.*;
import com.db4o.internal.fieldindex.*;
import com.db4o.internal.handlers.*;
import com.db4o.internal.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public final class QCandidates implements /*Visitor4, */ FieldFilterable {

    // Transaction necessary as reference to stream
    public final LocalTransaction i_trans;


    public QueryResultCandidates _result;

    // collection of all constraints
    private List4 _constraints;

    // possible class information
    ClassMetadata _classMetadata;

    // possible field information
    private QField _field;

    // current executing constraint, only set where needed
    QCon _currentConstraint;

    private IDGenerator _idGenerator;
    
    private boolean _loadedFromClassIndex;
    
    private boolean _loadedFromClassFieldIndex;
    
    private boolean _isTopLevel;
    
    QCandidates(LocalTransaction a_trans, ClassMetadata a_classMetadata, QField a_field, boolean isTopLevel) {
    	_result = new QueryResultCandidates(this);
    	_isTopLevel = isTopLevel;
    	i_trans = a_trans;
    	_classMetadata = a_classMetadata;
    	_field = a_field;
   
    	if (a_field == null
    			|| a_field._fieldMetadata == null
				|| !(a_field._fieldMetadata.getHandler() instanceof StandardReferenceTypeHandler)
    	) {
    		return;
    	}

    	ClassMetadata yc = ((StandardReferenceTypeHandler) a_field._fieldMetadata.getHandler()).classMetadata();
    	if (_classMetadata == null) {
    		_classMetadata = yc;
    	} else {
    		yc = _classMetadata.getHigherOrCommonHierarchy(yc);
    		if (yc != null) {
    			_classMetadata = yc;
    		}
    	}
    	
    }
    
    public boolean isTopLevel(){
    	return _isTopLevel;
    }

    public InternalCandidate add(InternalCandidate candidate) {
        if(Debug4.queries){
            String msg = "Candidate added ID: " + candidate.id();
            InternalCandidate root = candidate.getRoot();
            if(root != null){
            	msg += " root: " + root.id();
            }
			System.out.println(msg);
        }
        _result.add(candidate);
        
        if(((QCandidateBase)candidate)._size == 0){
        	
        	// This means that the candidate was already present
        	// and QCandidate does not allow duplicates.
        	
        	// In this case QCandidate#isDuplicateOf will have
        	// placed the existing QCandidate in the i_root
        	// variable of the new candidate. We return it here: 
        	
        	return candidate.getRoot();
        
        }
        return candidate;
    }

    void addConstraint(QCon a_constraint) {
        _constraints = new List4(_constraints, a_constraint);
    }
    
    public InternalCandidate readSubCandidate(QueryingReadContext context, TypeHandler4 handler){
        ObjectID objectID = ObjectID.NOT_POSSIBLE;
        try {
            int offset = context.offset();
            if(handler instanceof ReadsObjectIds){
                objectID = ((ReadsObjectIds)handler).readObjectID(context);
            }
            if(objectID.isValid()){
                return new QCandidate(this, null, objectID._id);
            }
            if(objectID == ObjectID.NOT_POSSIBLE){
                context.seek(offset);
                Object obj = context.read(handler);
                if(obj != null){
                	int id = context.container().getID(context.transaction(), obj);
                	if(id == 0) {
                		return new QPrimitiveCandidate(this, obj);
                	}
					QCandidate candidate =  new QCandidate(this, obj, id);
                	candidate.classMetadata(context.container().classMetadataForObject(obj));
                    return candidate;
                }
            }
            
        } catch (Exception e) {
            
            // FIXME: Catchall
            
        }
        return null;
    }
    
	void collect(final QCandidates a_candidates) {
		Iterator4 i = iterateConstraints();
		while(i.moveNext()){
			QCon qCon = (QCon)i.current();
			setCurrentConstraint(qCon);
			qCon.collect(a_candidates);
		}
		setCurrentConstraint(null);
    }

    void execute() {
        if(DTrace.enabled){
            DTrace.QUERY_PROCESS.log();
        }
        final FieldIndexProcessorResult result = processFieldIndexes();
        if(result.foundIndex()){
        	_result.fieldIndexProcessorResult(result);
        }else{
        	loadFromClassIndex();
        }
        evaluate();
    }
    
    public Iterator4 executeSnapshot(Collection4 executionPath){
    	IntIterator4 indexIterator = new IntIterator4Adaptor(iterateIndex(processFieldIndexes()));
    	Tree idRoot = TreeInt.addAll(null, indexIterator);
    	Iterator4 snapshotIterator = new TreeKeyIterator(idRoot);
    	Iterator4 singleObjectQueryIterator  = singleObjectSodaProcessor(snapshotIterator);
		return mapIdsToExecutionPath(singleObjectQueryIterator, executionPath);
    }
    
    private Iterator4 singleObjectSodaProcessor(Iterator4 indexIterator){
    	return Iterators.map(indexIterator, new Function4() {
			public Object apply(Object current) {
				int id = ((Integer)current).intValue();
				QCandidateBase candidate = new QCandidate(QCandidates.this, null, id); 
				_result.singleCandidate(candidate); 
				evaluate();
				if(! candidate.include()){
					return Iterators.SKIP;
				}
				return current;
			}
		});
    }
    
    public Iterator4 executeLazy(Collection4 executionPath){
    	Iterator4 indexIterator = iterateIndex(processFieldIndexes());
    	Iterator4 singleObjectQueryIterator  = singleObjectSodaProcessor(indexIterator);
		return mapIdsToExecutionPath(singleObjectQueryIterator, executionPath);
    }
    
    private Iterator4 iterateIndex (FieldIndexProcessorResult result ){
    	if(result.noMatch()){
    		return Iterators.EMPTY_ITERATOR;
    	}
    	if(result.foundIndex()){
    		return result.iterateIDs();
    	}
    	if(!_classMetadata.hasClassIndex()) {
    		return Iterators.EMPTY_ITERATOR;
    	}
    	return BTreeClassIndexStrategy.iterate(_classMetadata, i_trans);
    }

	private Iterator4 mapIdsToExecutionPath(Iterator4 singleObjectQueryIterator, Collection4 executionPath) {
		
		if(executionPath == null){
			return singleObjectQueryIterator;
		}
		
		Iterator4 res = singleObjectQueryIterator;
		
		Iterator4 executionPathIterator = executionPath.iterator();
		while(executionPathIterator.moveNext()){
			
			final String fieldName = (String) executionPathIterator.current();
			
			res = Iterators.concat(Iterators.map(res, new Function4() {
				
				public Object apply(Object current) {
					int id = ((Integer)current).intValue();
                    CollectIdContext context = CollectIdContext.forID(i_trans, id);
                    if(context == null){
                    	return Iterators.SKIP;
                    }
                    context.classMetadata().collectIDs(context, fieldName);
					return new TreeKeyIterator(context.ids());
				}
			}));
			
		}
		return res;
	}
    
	public ObjectContainerBase stream() {
		return i_trans.container();
	}

	public int classIndexEntryCount() {
		return _classMetadata.indexEntryCount(i_trans);
	}

	private FieldIndexProcessorResult processFieldIndexes() {
		if(_constraints == null){
			return FieldIndexProcessorResult.NO_INDEX_FOUND;
		}
		return new FieldIndexProcessor(this).run();
	}

    void evaluate() {
    	if (_constraints == null) {
    		return;
    	}
    	
    	forEachConstraint(new Procedure4() {
			public void apply(Object arg) {
	            QCon qCon = (QCon)arg;
	            qCon.setCandidates(QCandidates.this);
	    		qCon.evaluateSelf();
			}
		});
    	
    	forEachConstraint(new Procedure4() {
			public void apply(Object arg) {
	            ((QCon)arg).evaluateSimpleChildren();
			}
		});
    	
    	forEachConstraint(new Procedure4() {
			public void apply(Object arg) {
	            ((QCon)arg).evaluateEvaluations();
			}
		});

    	forEachConstraint(new Procedure4() {
			public void apply(Object arg) {
	            ((QCon)arg).evaluateCreateChildrenCandidates();
			}
		});
    	
    	forEachConstraint(new Procedure4() {
			public void apply(Object arg) {
	            ((QCon)arg).evaluateCollectChildren();
			}
		});

    	forEachConstraint(new Procedure4() {
			public void apply(Object arg) {
	            ((QCon)arg).evaluateChildren();
			}
		});

    }
    
    private void forEachConstraint(Procedure4 proc){
    	Iterator4 i = iterateConstraints();
    	while(i.moveNext()){
    		QCon constraint = (QCon)i.current();
    		if(! constraint.processedByIndex()){
    			proc.apply(constraint);
    		} 
    	}
    }

    boolean isEmpty() {
        final boolean[] ret = new boolean[] { true };
        traverse(new Visitor4() {
            public void visit(Object obj) {
                if (((InternalCandidate) obj).include()) {
                    ret[0] = false;
                }
            }
        });
        return ret[0];
    }

    boolean filter(Visitor4 visitor) {
    	return _result.filter(visitor);
    }

    boolean filter(QField field, FieldFilterable filterable) {
    	return _result.filter(field, filterable);
    }

    int generateCandidateId(){
        if(_idGenerator == null){
            _idGenerator = new IDGenerator();
        }
        return - _idGenerator.next();
    }
    
    public Iterator4 iterateConstraints(){
        if(_constraints == null){
            return Iterators.EMPTY_ITERATOR;
        }
        return new Iterator4Impl(_constraints);
    }

    void loadFromClassIndex() {
    	if (!isEmpty()) {
    		return;
    	}
    	
    	_result.loadFromClassIndex(_classMetadata.index());
    	
        DiagnosticProcessor dp = i_trans.container()._handlers.diagnosticProcessor();
        if (dp.enabled() && !isClassOnlyQuery()){
            dp.loadedFromClassIndex(_classMetadata);
        }
        
        _loadedFromClassIndex = true;
        
    }

	void setCurrentConstraint(QCon a_constraint) {
        _currentConstraint = a_constraint;
    }

    void traverse(Visitor4 visitor) {
    	_result.traverse(visitor);
    }
    
    void traverseIds(IntVisitor visitor) {
    	_result.traverseIds(visitor);
    }


    // FIXME: This method should go completely.
    //        We changed the code to create the QCandidates graph in two steps:
    //        (1) call fitsIntoExistingConstraintHierarchy to determine whether
    //            or not we need more QCandidates objects
    //        (2) add all constraints
    //        This method tries to do both in one, which results in missing
    //        constraints. Not all are added to all QCandiates.
    //        Right methodology is in 
    //        QQueryBase#createCandidateCollection
    //        and
    //        QQueryBase#createQCandidatesList
    boolean tryAddConstraint(QCon a_constraint) {

        if (_field != null) {
            QField qf = a_constraint.getField();
            if (qf != null) {
                if (_field.name()!=null&&!_field.name().equals(qf.name())) {
                    return false;
                }
            }
        }

        if (_classMetadata == null || a_constraint.isNullConstraint()) {
            addConstraint(a_constraint);
            return true;
        }
        ClassMetadata yc = a_constraint.getYapClass();
        if (yc != null) {
            yc = _classMetadata.getHigherOrCommonHierarchy(yc);
            if (yc != null) {
                _classMetadata = yc;
                addConstraint(a_constraint);
                return true;
            }
        }
        addConstraint(a_constraint);
        return false;
    }

//    public void visit(Object a_tree) {
//    	final QCandidate parent = (QCandidate) a_tree;
//    	if (parent.createChild(this)) {
//    		return;
//    	}
//    	
//    	// No object found.
//    	// All children constraints are necessarily false.
//    	// Check immediately.
//		Iterator4 i = iterateConstraints();
//		while(i.moveNext()){
//			((QCon)i.current()).visitOnNull(parent.getRoot());
//		}
//    		
//    }


	@Override
	public void filter(QField field, ParentCandidate parent) {
    	if (parent.createChild(field, this)) {
    		return;
    	}
    	
    	// No object found.
    	// All children constraints are necessarily false.
    	// Check immediately.
		Iterator4 i = iterateConstraints();
		while(i.moveNext()){
			((QCon)i.current()).visitOnNull(parent.getRoot());
		}
	}

    public String toString() {
    	final StringBuffer sb = new StringBuffer();
    	_result.traverse(new Visitor4() {
			public void visit(Object obj) {
				QCandidateBase candidate = (QCandidateBase) obj;
				sb.append(" ");
				sb.append(candidate._key);
			}
		});
    	return sb.toString();
    }
	
	public final Transaction transaction(){
	    return i_trans;
	}
	
	public boolean wasLoadedFromClassIndex(){
		return _loadedFromClassIndex;
	}
	
	public boolean wasLoadedFromClassFieldIndex(){
		return _loadedFromClassFieldIndex;
	}
	
	public void wasLoadedFromClassFieldIndex(boolean flag){
		_loadedFromClassFieldIndex = flag;
	}

	public boolean fitsIntoExistingConstraintHierarchy(QCon constraint) {
        if (_field != null) {
            QField qf = constraint.getField();
            if (qf != null) {
                if (_field.name()!=null&&!_field.name().equals(qf.name())) {
                    return false;
                }
            }
        }

        if (_classMetadata == null || constraint.isNullConstraint()) {
            return true;
        }
        ClassMetadata classMetadata = constraint.getYapClass();
        if (classMetadata == null) {
        	return false;
        }
        classMetadata = _classMetadata.getHigherOrCommonHierarchy(classMetadata);
        if (classMetadata == null) {
        	return false;
        }
        _classMetadata = classMetadata;
        return true;
	}
	
	private boolean isClassOnlyQuery() {
		if(_constraints._next != null) {
			return false;
		}
		if(!(_constraints._element instanceof QConClass)) {
			return false;
		}
		return !((QCon)_constraints._element).hasChildren();
	}
}
