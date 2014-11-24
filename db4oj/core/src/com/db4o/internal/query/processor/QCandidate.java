/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.handlers.*;
import com.db4o.internal.handlers.array.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;
import com.db4o.reflect.*;
import com.db4o.typehandlers.*;

/**
 * Represents an actual object in the database. Forms a tree structure, indexed
 * by id. Can have dependents that are doNotInclude'd in the query result when
 * this is doNotInclude'd.
 * 
 * @exclude
 */
public class QCandidate extends QCandidateBase implements ParentCandidate {

	// db4o ID is stored in _key;

	// db4o byte stream storing the object
	ByteArrayBuffer _bytes;

	Object _member;

	// the ClassMetadata of this object
	ClassMetadata _classMetadata;

	// temporary field and member for one field during evaluation
	FieldMetadata _fieldMetadata; // null denotes null object
    
    private int _handlerVersion;

	public QCandidate(QCandidates candidates, Object member, int id) {
		super(candidates, id);
		_member = member;
	}

	public Object shallowClone() {
		QCandidate qcan = new QCandidate(_candidates, _member, _key);
        qcan.setBytes(_bytes);
		qcan._dependants = _dependants;
		qcan._include = _include;
		qcan._member = _member;
		qcan._pendingJoins = _pendingJoins;
		qcan._root = _root;
		qcan._classMetadata = _classMetadata;
		qcan._fieldMetadata = _fieldMetadata;

		return super.shallowCloneInternal(qcan);
	}

	private void checkInstanceOfCompare() {
		if (_member instanceof Compare) {
			_member = ((Compare) _member).compare();
			LocalObjectContainer stream = container();
			_classMetadata = stream.classMetadataForReflectClass(stream.reflector().forObject(_member));
			_key = stream.getID(transaction(), _member);
			if (_key == 0) {
				setBytes(null);
			} else {
				setBytes(stream.readBufferById(transaction(), _key));
			}
		}
	}
	
	@Override
	public boolean createChild(QField field, final QCandidates candidates) {
		if (!_include) {
			return false;
		}
		
		useField(field);

		if(_fieldMetadata == null || _fieldMetadata instanceof NullFieldMetadata) {
			return false;
		}
		
		TypeHandler4 handler = _fieldMetadata.getHandler();
		if (handler != null) {
		    final QueryingReadContext queryingReadContext = new QueryingReadContext(transaction(), marshallerFamily().handlerVersion(), _bytes, _key); 
			final TypeHandler4 arrayElementHandler = Handlers4.arrayElementHandler(handler, queryingReadContext);
			if (arrayElementHandler != null) {
				return createChildForDescendable(candidates, handler, queryingReadContext, arrayElementHandler);
			}

			// We may get simple types here too, if the YapField was null
			// in the higher level simple evaluation. Evaluate these
			// immediately.

			if (Handlers4.isQueryLeaf(handler)) {
				candidates._currentConstraint.visit(this);
				return true;
			}
		}
        
        _classMetadata.seekToField(transaction(), _bytes, _fieldMetadata);
        InternalCandidate candidate = readSubCandidate(candidates); 
		if (candidate == null) {
			return false;
		}

		// fast early check for ClassMetadata
		if (candidates._classMetadata != null
				&& candidates._classMetadata.isStronglyTyped()) {
			
			if (Handlers4.isUntyped(handler)){
				handler = typeHandlerFor(candidate);
			}
            if(handler == null){
                return false;
            }
		}

		addDependant(candidates.add(candidate));
		return true;
	}

	private boolean createChildForDescendable(final QCandidates parentCandidates, TypeHandler4 handler, final QueryingReadContext queryingReadContext, final TypeHandler4 arrayElementHandler) {
		final int offset = queryingReadContext.offset();
		boolean outerRes = true;

		// The following construct is worse than not ideal. For each constraint it completely reads the
		// underlying structure again. The structure could be kept fairly easy. TODO: Optimize!

		Iterator4 i = parentCandidates.iterateConstraints();
		while (i.moveNext()) {

			QCon qcon = (QCon) i.current();
			QField qf = qcon.getField();
			if (qf != null && !qf.name().equals(_fieldMetadata.getName())) {
				continue;
			}
			QCon tempParent = qcon.parent();
			qcon.setParent(null);

			final QCandidates candidates = new QCandidates(parentCandidates.i_trans, null, qf, false);
			candidates.addConstraint(qcon);

			qcon.setCandidates(candidates);
			
			readArrayCandidates(handler, queryingReadContext.buffer(), arrayElementHandler, candidates);
			
			queryingReadContext.seek(offset);

			final boolean isNot = qcon.isNot();
			if (isNot) {
				qcon.removeNot();
			}

			candidates.evaluate();

			final ByRef<Tree> pending = ByRef.newInstance();
			final BooleanByRef innerRes = new BooleanByRef(isNot);
			candidates.traverse(new CreateDescendChildTraversingVisitor(pending, innerRes, isNot));

			if (isNot) {
				qcon.not();
			}

			// In case we had pending subresults, we need to communicate them up to our root.
			if (pending.value != null) {
				pending.value.traverse(new Visitor4() {
					public void visit(Object a_object) {
						getRoot().evaluate((QPending) a_object);
					}
				});
			}

			if (!innerRes.value) {
				if (Debug4.queries) {
					System.out.println("  Array evaluation false. Constraint:" + qcon.id());
				}

				// Again this could be double triggering.
				// 
				// We want to clean up the "No route" at some stage.
				qcon.visit(getRoot(), qcon.evaluator().not(false));
				outerRes = false;
			}
			qcon.setParent(tempParent);
		}
		return outerRes;
	}

	private TypeHandler4 typeHandlerFor(InternalCandidate candidate) {
	    ClassMetadata classMetadata = candidate.classMetadata();
	    if (classMetadata != null) {
	    	return classMetadata.typeHandler();
	    }
	    return null;
    }

	private void readArrayCandidates(TypeHandler4 typeHandler, final ReadBuffer buffer,
        final TypeHandler4 arrayElementHandler, final QCandidates candidates) {
        if(! Handlers4.isCascading(arrayElementHandler)){
            return;
        }
        final SlotFormat slotFormat = SlotFormat.forHandlerVersion(_handlerVersion);
        slotFormat.doWithSlotIndirection(buffer, typeHandler, new Closure4() {
            public Object run() {
                
                QueryingReadContext context = null;
                
                if(Handlers4.handleAsObject(arrayElementHandler)){
                    // TODO: Code is similar to FieldMetadata.collectIDs. Try to refactor to one place.
                    int collectionID = buffer.readInt();
                    ByteArrayBuffer arrayElementBuffer = container().readBufferById(transaction(), collectionID);
                    ObjectHeader objectHeader = ObjectHeader.scrollBufferToContent(container(), arrayElementBuffer);
                    context = new QueryingReadContext(transaction(), candidates, _handlerVersion, arrayElementBuffer, collectionID);
                    objectHeader.classMetadata().collectIDs(context);
                    
                }else{
                    context = new QueryingReadContext(transaction(), candidates, _handlerVersion, buffer, 0);
                    ((CascadingTypeHandler)arrayElementHandler).collectIDs(context);
                }
                
                Tree.traverse(context.ids(), new Visitor4() {
                    public void visit(Object obj) {
                        TreeInt idNode = (TreeInt) obj;
                        candidates.add(new QCandidate(candidates, null, idNode._key));
                    }
                });
                
                Iterator4 i = context.objectsWithoutId();
                while(i.moveNext()){
                    Object obj = i.current();
                    candidates.add(new QCandidate(candidates, obj, 0));
                }
                
                return null;
            }
        
        });
    }

	ReflectClass classReflector() {
		classMetadata();
		if (_classMetadata == null) {
			return null;
		}
		return _classMetadata.classReflector();
	}
	
	@Override
	public boolean fieldIsAvailable(){
		return classReflector() != null;
	}

	ReflectClass memberClass() {
		return transaction().reflector().forObject(_member);
	}

	
	private void read() {
		if (_include) {
			if (_bytes == null) {
				if (_key > 0) {
					if (DTrace.enabled) {
						DTrace.CANDIDATE_READ.log(_key);
					}
                    setBytes(container().readBufferById(transaction(), _key));
					if (_bytes == null) {
						include(false);
					}
				} else {
				    include(false);
				}
			}
		}
	}
	
	private int currentOffSet(){
	    return _bytes._offset;
	}

	private InternalCandidate readSubCandidate(QCandidates candidateCollection) {
		read();
		if (_bytes == null || _fieldMetadata == null) {
		    return null;
		}
		final int offset = currentOffSet();
        QueryingReadContext context = newQueryingReadContext();
        TypeHandler4 handler = HandlerRegistry.correctHandlerVersion(context, _fieldMetadata.getHandler());
        InternalCandidate subCandidate = candidateCollection.readSubCandidate(context, handler);
		seek(offset);
		if (subCandidate != null) {
			subCandidate.root(getRoot());
			return subCandidate;
		}
		return null;
	}
	
	private void seek(int offset){
	    _bytes._offset = offset;
	}

    private QueryingReadContext newQueryingReadContext() {
        return new QueryingReadContext(transaction(), _handlerVersion, _bytes, _key);
    }

	private void readThis(boolean a_activate) {
		read();

		final ObjectContainerBase container = transaction().container();
		
		_member = container.tryGetByID(transaction(), _key);
			
		if (_member != null && (a_activate || _member instanceof Compare)) {
			container.activate(transaction(), _member);
			checkInstanceOfCompare();
		}
	}

	@Override
	public ClassMetadata classMetadata() {
		if (_classMetadata != null) {
			return _classMetadata;
		}
		read();
		if (_bytes == null) {
			return null;
		}
	    seek(0);
        ObjectContainerBase stream = container();
        ObjectHeader objectHeader = new ObjectHeader(stream, _bytes);
		_classMetadata = objectHeader.classMetadata();
        
		if (_classMetadata != null) {
			if (stream._handlers.ICLASS_COMPARE.isAssignableFrom(_classMetadata.classReflector())) {
				readThis(false);
			}
		}
		return _classMetadata;
	}

	public String toString() {
		String str = "QCandidate id: " + _key; 
		if (_classMetadata != null) {
			str += "\n   YapClass " + _classMetadata.getName();
		}
		if (_fieldMetadata != null) {
			str += "\n   YapField " + _fieldMetadata.getName();
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

	public void useField(QField a_field) {
		read();
		if (_bytes == null) {
			_fieldMetadata = null;
            return;
		} 
		classMetadata();
		_member = null;
		if (a_field == null) {
			_fieldMetadata = null;
            return;
		} 
		if (_classMetadata == null) {
			_fieldMetadata = null;
            return;
		} 
		_fieldMetadata = fieldMetadataFrom(a_field, _classMetadata);
		if(_fieldMetadata == null){
		    fieldNotFound();
		    return;
		}
        
		HandlerVersion handlerVersion = _classMetadata.seekToField(transaction(), _bytes, _fieldMetadata);
        
		if (handlerVersion == HandlerVersion.INVALID ) {
		    fieldNotFound();
		    return;
		}
		
		_handlerVersion = handlerVersion._number;
	}
	
	private FieldMetadata fieldMetadataFrom(QField qField, ClassMetadata type) {
		final FieldMetadata existingField = qField.getFieldMetadata();
		if(existingField != null){
			return existingField;
		}
		FieldMetadata field = type.fieldMetadataForName(qField.name());
		if(field != null){
		    field.alive();
		}
		return field;
	}
		

	private void fieldNotFound(){
        if (_classMetadata.holdsAnyClass()) {
            // retry finding the field on reading the value 
            _fieldMetadata = null;
        } else {
            // we can't get a value for the field, comparisons should definitely run against null
            _fieldMetadata = new NullFieldMetadata();
        }
        _handlerVersion = HandlerRegistry.HANDLER_VERSION;  
	}
	

	Object value() {
		return value(false);
	}

	// TODO: This is only used for Evaluations. Handling may need
	// to be different for collections also.
	Object value(boolean a_activate) {
		if (_member == null) {
			if (_fieldMetadata == null) {
				readThis(a_activate);
			} else {
				int offset = currentOffSet();
				_member = _fieldMetadata.read(newQueryingReadContext());
				seek(offset);
				checkInstanceOfCompare();
			}
		}
		return _member;
	}
    
    void setBytes(ByteArrayBuffer bytes){
        _bytes = bytes;
    }
    
    private MarshallerFamily marshallerFamily(){
        return MarshallerFamily.version(_handlerVersion);
    }
    
    public void classMetadata(ClassMetadata classMetadata) {
		_classMetadata = classMetadata;
	}
	
	@Override
	public boolean evaluate(final QConObject a_constraint, final QE a_evaluator) {
		if (a_evaluator.identity()) {
			return a_evaluator.evaluate(a_constraint, this, null);
		}
		if (_member == null) {
			_member = value();
		}
		return a_evaluator.evaluate(a_constraint, this, a_constraint
				.translate(_member));
	}

	public Object getObject() {
		Object obj = value(true);
		if (obj instanceof ByteArrayBuffer) {
			ByteArrayBuffer reader = (ByteArrayBuffer) obj;
			int offset = reader._offset;
	        obj = StringHandler.readString(transaction().context(), reader); 
			reader._offset = offset;
		}
		return obj;
	}

	@Override
	public PreparedComparison prepareComparison(ObjectContainerBase container, Object constraint) {
	    Context context = container.transaction().context();
	    
		if (_fieldMetadata != null) {
			return _fieldMetadata.prepareComparison(context, constraint);
		}
		if (_classMetadata != null) {
			return _classMetadata.prepareComparison(context, constraint);
		}
		Reflector reflector = container.reflector();
		ClassMetadata classMetadata = null;
		if (_bytes != null) {
			classMetadata = container.produceClassMetadata(reflector.forObject(constraint));
		} else {
			if (_member != null) {
				classMetadata = container.classMetadataForReflectClass(reflector.forObject(_member));
			}
		}
		if (classMetadata != null) {
			if (_member != null && _member.getClass().isArray()) {
				TypeHandler4 arrayElementTypehandler = classMetadata.typeHandler(); 
				if (reflector.array().isNDimensional(memberClass())) {
					MultidimensionalArrayHandler mah = 
						new MultidimensionalArrayHandler(arrayElementTypehandler, false);
					return mah.prepareComparison(context, _member);
				} 
				ArrayHandler ya = new ArrayHandler(arrayElementTypehandler, false);
				return ya.prepareComparison(context, _member);
			} 
			return classMetadata.prepareComparison(context, constraint);
		}
		return null;
	}

	static final class CreateDescendChildTraversingVisitor implements Visitor4 {
		private final ByRef<Tree> _pending;
		private final BooleanByRef _innerRes;
		private final boolean _isNot;

		CreateDescendChildTraversingVisitor(ByRef<Tree> pending, BooleanByRef innerRes, boolean isNot) {
			_pending = pending;
			_innerRes = innerRes;
			_isNot = isNot;
		}

		public void visit(Object obj) {
			InternalCandidate cand = (InternalCandidate) obj;

			if (cand.include()) {
				_innerRes.value = !_isNot;
			}

			// Collect all pending subresults.

			if (cand.pendingJoins() == null) {
				return;
			}

			cand.pendingJoins().traverse(new Visitor4() {
				public void visit(Object a_object) {
					QPending newPending = ((QPending) a_object).internalClonePayload();

					// We need to change the constraint here, so our pending collector
					// uses the right comparator.
					newPending.changeConstraint();
					QPending oldPending = (QPending) Tree.find(_pending.value, newPending);
					if (oldPending != null) {
						// We only keep one pending result for all array elements and memorize,
						// whether we had a true or a false result or both.
						if (oldPending._result != newPending._result) {
							oldPending._result = QPending.BOTH;
						}
					} 
					else {
						_pending.value = Tree.add(_pending.value, newPending);
					}
				}
			});
		}
	}

}
