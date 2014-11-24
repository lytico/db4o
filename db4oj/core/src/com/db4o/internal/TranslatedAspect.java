/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.config.*;
import com.db4o.internal.marshall.*;
import com.db4o.reflect.*;
import com.db4o.typehandlers.*;


public final class TranslatedAspect extends FieldMetadata {
	private ObjectTranslator _translator;

	public TranslatedAspect(ClassMetadata containingClass, String name){
	    this(containingClass);
	    init(name);
	}
	
	public TranslatedAspect(ClassMetadata containingClass, ObjectTranslator translator) {
		this(containingClass);
		initializeTranslator(translator);
	}

	private TranslatedAspect(ClassMetadata containingClass) {
		super(containingClass);
		setAvailable();
	}

	public void initializeTranslator(ObjectTranslator translator) {
		_translator = translator;
		initializeFieldName();
		initializeFieldType();
	}
	
	public boolean alive() {
		return true;
	}

	private void initializeFieldName() {
		init(fieldNameFor(_translator));
	}

	private void initializeFieldType() {
		ObjectContainerBase stream = containingClass().container();
		
		ReflectClass storedClass = stream.reflector().forClass(translatorStoredClass(_translator));
		configure(storedClass, false);
		
		ReflectClass baseType = Handlers4.baseType(storedClass);
		stream.showInternalClasses(true);
		try {
			_fieldType = stream.produceClassMetadata(baseType);
		} finally {
			stream.showInternalClasses(false);
		}
		if (null == _fieldType) {
			throw new IllegalStateException("Cannot produce class metadata for " + baseType + "!");
		}
	}

	public static String fieldNameFor(ObjectTranslator translator) {
		return translator.getClass().getName();
	}
    
    public boolean canUseNullBitmap(){
        return false;
    }

    @Override
	public void deactivate(ActivationContext context){
		if(context.depth().requiresActivation()){
			cascadeActivation(context);
		}
		setOn(context.transaction(), context.targetObject(), null);
	}

	public Object getOn(Transaction a_trans, Object a_OnObject) {
		try {
			return _translator.onStore(a_trans.objectContainer(), a_OnObject);
		} catch(ReflectException e) {
			throw e;
		} catch (RuntimeException e) {
			throw new ReflectException(e);
		}
	}
	
	public Object getOrCreate(Transaction a_trans, Object a_OnObject) {
		return getOn(a_trans, a_OnObject);
	}

	public void activate(UnmarshallingContext context) {
	    
        Object obj = read(context);

        // Activation of members is necessary on purpose here.
        // Classes like Hashtable need fully activated members
        // to be able to calculate hashCode()
        
        if (obj != null) {
        	context.container().activate(context.transaction(), obj, context.activationDepth());
        }

        setOn(context.transaction(), context.persistentObject(), obj);
	}
	
	void refresh() {
	    // do nothing
	}
	
	private void setOn(Transaction trans, Object a_onObject, Object toSet) {
		try {
			_translator.onActivate(trans.objectContainer(), a_onObject, toSet);
		} catch (RuntimeException e) {
			throw new ReflectException(e);
		}
	}
	
	protected Object indexEntryFor(Object indexEntry) {
		return indexEntry;
	}
	
	protected Indexable4 indexHandler(ObjectContainerBase stream) {
		return (Indexable4)getHandler();
	}
	
	public boolean equals(Object obj) {
        if(obj == this){
            return true;
        }
        if(obj == null || obj.getClass() != getClass()){
            return false;
        }
        TranslatedAspect other = (TranslatedAspect) obj;
        return _translator.equals(other._translator);
	}
	
	public int hashCode() {
	    return _translator.hashCode();
	}
	
    public AspectType aspectType() {
        return AspectType.TRANSLATOR;
    }

	public boolean isObjectConstructor() {
		return _translator instanceof ObjectConstructor;
    }

	public Object construct(ObjectReferenceContext context) {
		ContextState contextState = context.saveState();
		boolean fieldHasValue = containingClass().seekToField(context, this);
        try {
            return ((ObjectConstructor)_translator).onInstantiate(
            			context.container(),
            			fieldHasValue ? read(context) : null);                      
        } finally {
            context.restoreState(contextState);
        }
    }

}
