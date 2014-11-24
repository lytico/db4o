package com.db4o.cs.internal.objectexchange;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.marshall.*;

public class StandardReferenceCollector implements ReferenceCollector {

	private Transaction _transaction;
	
	public StandardReferenceCollector(Transaction transaction) {
	    _transaction = transaction;
    }

	public Iterator4<Integer> referencesFrom(int id) {
	    final CollectIdContext context = CollectIdContext.forID(_transaction, id);
	    final ClassMetadata classMetadata = context.classMetadata();
	    if (null == classMetadata) {
	    	// most probably ClassMetadata reading
	    	return Iterators.EMPTY_ITERATOR;
	    }
	    if (!classMetadata.hasIdentity()) {
	    	throw new IllegalStateException(classMetadata.toString());
	    }
	    if (!Handlers4.isCascading(classMetadata.typeHandler())) {
	    	return Iterators.EMPTY_ITERATOR;
	    }
		classMetadata.collectIDs(context);
		return new TreeKeyIterator(context.ids());
    }

}
