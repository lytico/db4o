/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.constraints;

import com.db4o.config.*;
import com.db4o.events.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.reflect.*;
import com.db4o.reflect.core.*;

/**
 * Configures a field of a class to allow unique values only.
 * In C/S mode, add this configuration-item to the server side only.
 *
 * <b>You need to turn on indexing for the given field, otherwise the unique constrain won't work.</b>
 *
 *
 * Add this to your configuration with {@link CommonConfiguration#add(com.db4o.config.ConfigurationItem)}
 */
public class UniqueFieldValueConstraint implements ConfigurationItem {
	
	protected final Object _clazz;
	protected final String _fieldName;
	
	/**
	 * constructor to create a UniqueFieldValueConstraint. 
	 * @param clazz can be a class (Java) / Type (.NET) / instance of the class / fully qualified class name
	 * @param fieldName the name of the field that is to be unique. 
	 */
	public UniqueFieldValueConstraint(Object clazz, String fieldName) {
		_clazz = clazz;
		_fieldName = fieldName;
	}
	
	public void prepare(Configuration configuration) {
		// Nothing to do...
	}
	
	/**
	 * internal method, public for implementation reasons.
	 */
	public void apply(final InternalObjectContainer objectContainer) {
		
		if (objectContainer.isClient()) {
			throw new IllegalStateException(getClass().getName() + " should be configured on the server.");
		}
		
		EventRegistryFactory.forObjectContainer(objectContainer).committing().addListener(
				new EventListener4() {

			private FieldMetadata _fieldMetaData;
			
			private void ensureSingleOccurence(Transaction trans, ObjectInfoCollection col){
				final Iterator4 i = col.iterator();
				while(i.moveNext()){					
					final ObjectInfo objectInfo = (ObjectInfo) i.current();
		
					if (!reflectClass().isAssignableFrom(reflectorFor(trans, objectInfo.getObject())))
						continue;
					
					final Object obj = objectFor(trans, objectInfo);
					Object fieldValue = fieldMetadata().getOn(trans, obj);
					if(fieldValue == null) {
						continue;
					}
					BTreeRange range = fieldMetadata().search(trans, fieldValue);
					if(range.size() > 1){
						throw new UniqueFieldValueConstraintViolationException(classMetadata().getName(), fieldMetadata().getName()); 
					}
				}
			}

			private boolean isClassMetadataAvailable() {
				return null != classMetadata();
			}
			
			private FieldMetadata fieldMetadata() {
				if(_fieldMetaData != null){
					return _fieldMetaData;
				}
				_fieldMetaData = classMetadata().fieldMetadataForName(_fieldName);
				return _fieldMetaData;
			}
			
			private ClassMetadata classMetadata() {
				return objectContainer.classMetadataForReflectClass(reflectClass()); 
			}

			private ReflectClass reflectClass() {
				return ReflectorUtils.reflectClassFor(objectContainer.reflector(), _clazz);
			}
	
			public void onEvent(Event4 e, EventArgs args) {
				if (!isClassMetadataAvailable()) {
					return;
				}
				CommitEventArgs commitEventArgs = (CommitEventArgs) args;
				Transaction trans = (Transaction) commitEventArgs.transaction();
				ensureSingleOccurence(trans, commitEventArgs.added());
				ensureSingleOccurence(trans, commitEventArgs.updated());
			}
			
			private Object objectFor(Transaction trans, ObjectInfo info) {
				int id = (int)info.getInternalID();
			    HardObjectReference ref = HardObjectReference.peekPersisted(trans, id, 1);
			    return ref._object;
		    }
		});
		
	}

	private ReflectClass reflectorFor(Transaction trans, final Object obj) {
		return trans.container().reflector().forObject(obj);
	}
}
