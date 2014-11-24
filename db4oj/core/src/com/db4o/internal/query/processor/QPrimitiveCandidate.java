/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.internal.query.processor;

import com.db4o.foundation.*;
import com.db4o.internal.*;

public class QPrimitiveCandidate extends QCandidateBase {

	private Object _obj;
	
	public QPrimitiveCandidate(QCandidates candidates, Object obj) {
		super(candidates, candidates.generateCandidateId());
		_obj = obj;
	}

	@Override
	public Object getObject() {
		return _obj;
	}

	@Override
	public boolean evaluate(QConObject a_constraint, QE a_evaluator) {
		return a_evaluator.evaluate(a_constraint, this, a_constraint.translate(_obj));
	}

	@Override
	public PreparedComparison prepareComparison(ObjectContainerBase container, Object constraint) {
		ClassMetadata classMetadata = classMetadata();
		if (classMetadata == null) {
			return null;
		}
		return classMetadata.prepareComparison(container.transaction().context(), constraint);
	}

	@Override
	public ClassMetadata classMetadata() {
		return container().classMetadataForReflectClass(container().reflector().forObject(_obj));
	}

	@Override
	public boolean fieldIsAvailable(){
		return false;
	}

}
