/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.classindex;

import com.db4o.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public abstract class AbstractClassIndexStrategy implements ClassIndexStrategy {

	protected final ClassMetadata _classMetadata;

	public AbstractClassIndexStrategy(ClassMetadata classMetadata) {
		_classMetadata = classMetadata;
	}

	protected int classMetadataID() {
		return _classMetadata.getID();
	}

	public int ownLength() {
		return Const4.ID_LENGTH;
	}

	protected abstract void internalAdd(Transaction trans, int id);

	public final void add(Transaction trans, int id) {
		if (DTrace.enabled) {
	        DTrace.ADD_TO_CLASS_INDEX.log(id);
	    }
		checkId(id);
		internalAdd(trans, id);
	}	

	protected abstract void internalRemove(Transaction ta, int id);

	public final void remove(Transaction ta, int id) {
	    if (DTrace.enabled){
	        DTrace.REMOVE_FROM_CLASS_INDEX.log(id);
	    }
	    checkId(id);
	    internalRemove(ta, id);
	}
	
	private void checkId(int id) {
		if (Deploy.debug) {
            if (id == 0) {
                throw new IllegalArgumentException("id can't be zero");
            }
        }
	}
}