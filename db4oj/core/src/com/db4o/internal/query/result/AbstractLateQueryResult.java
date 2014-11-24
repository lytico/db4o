/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query.result;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.classindex.*;
import com.db4o.reflect.*;


/**
 * @exclude
 */
public abstract class AbstractLateQueryResult extends AbstractQueryResult {
	
	protected Iterable4 _iterable;

	public AbstractLateQueryResult(Transaction transaction) {
		super(transaction);
	}
	
    public AbstractQueryResult supportSize(){
    	return toIdTree();
    }
    
    public AbstractQueryResult supportSort(){
    	return toIdList();
    }
    
    public AbstractQueryResult supportElementAccess(){
    	return toIdList();
    }
    
    protected int knownSize(){
    	return 0;
    }
    
	public IntIterator4 iterateIDs() {
		if(_iterable == null){
			throw new IllegalStateException();
		}
		return new IntIterator4Adaptor(_iterable);
	}
    
    public AbstractQueryResult toIdList(){
    	return toIdTree().toIdList();
    }

	public boolean skipClass(ClassMetadata classMetadata){
		if (classMetadata.getName() == null) {
			return true;
		}
		ReflectClass claxx = classMetadata.classReflector();
		if (stream()._handlers.ICLASS_INTERNAL.isAssignableFrom(claxx)){
			return true; 
		}
		return false;
	}
	
	protected Iterable4 classIndexesIterable(final ClassMetadataIterator classCollectionIterator) {
		return Iterators.concatMap(Iterators.iterable(classCollectionIterator), new Function4() {
			public Object apply(Object current) {
				final ClassMetadata classMetadata = (ClassMetadata)current;
				if(skipClass(classMetadata)){
					return Iterators.SKIP;
				}
				return classIndexIterable(classMetadata);
			}
		});
	}
	
	protected Iterable4 classIndexIterable(final ClassMetadata clazz) {
		return new Iterable4() {
			public Iterator4 iterator() {
				return classIndexIterator(clazz);
			}
		};
	}
	
	public Iterator4 classIndexIterator(ClassMetadata clazz) {
		return BTreeClassIndexStrategy.iterate(clazz, transaction());
	}

}
