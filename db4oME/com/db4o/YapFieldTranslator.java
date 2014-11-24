/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.config.*;

final class YapFieldTranslator extends YapField
{
	private final ObjectTranslator i_translator;

	YapFieldTranslator(YapClass a_yapClass, ObjectTranslator a_translator){
	    super(a_yapClass, a_translator);
		i_translator = a_translator;
		YapStream stream = a_yapClass.getStream();
		configure(stream.reflector().forClass(a_translator.storedClass()), false);
	}

	void deactivate(Transaction a_trans, Object a_onObject, int a_depth){
		if(a_depth > 0){
			cascadeActivation(a_trans, a_onObject, a_depth, false);
		}
		setOn(a_trans.i_stream, a_onObject, null);
	}

	Object getOn(Transaction a_trans, Object a_OnObject){
		try{
			return i_translator.onStore(a_trans.i_stream, a_OnObject);
		}catch(Throwable t){
			return null;
		}
	}
	
	Object getOrCreate(Transaction a_trans, Object a_OnObject) {
		return getOn(a_trans, a_OnObject);
	}

	void instantiate(YapObject a_yapObject, Object a_onObject, YapWriter a_bytes) throws CorruptionException{
		Object toSet = read(a_bytes);

		// Activation of members is necessary on purpose here.
		// Classes like Hashtable need fully activated members
		// to be able to calculate hashCode()
		
		a_bytes.getStream().activate2(a_bytes.getTransaction(), toSet, a_bytes.getInstantiationDepth());

		setOn(a_bytes.getStream(), a_onObject, toSet);
	}
	
	void refresh() {
	    // do nothing
	}
	
	private void setOn(YapStream a_stream, Object a_onObject, Object toSet){
		try{
			i_translator.onActivate(a_stream, a_onObject, toSet);
		}catch(Throwable t){}
	}
}
