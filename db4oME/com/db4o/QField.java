/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.reflect.*;
import com.db4o.types.*;

/**
 * @exclude
 */
public class QField implements Visitor4, Unversioned{
	
	transient Transaction i_trans;
	public String i_name;
	transient YapField i_yapField;
	public int i_yapClassID;
	public int i_index;
	
	public QField(){
		// C/S only	
	}
	
	QField(Transaction a_trans, String name, YapField a_yapField, int a_yapClassID, int a_index){
		i_trans = a_trans;
		i_name = name;
		i_yapField = a_yapField;
		i_yapClassID = a_yapClassID;
		i_index = a_index;
		if(i_yapField != null){
		    if(! i_yapField.alive()){
		        i_yapField = null;
		    }
		}
	}
    
    boolean canHold(ReflectClass claxx){
        return i_yapField == null || i_yapField.canHold(claxx);
    }
	
	Object coerce(Object a_object){
	    ReflectClass claxx = null;
	    Reflector reflector = i_trans.reflector();
	    if(a_object != null){
	        if(a_object instanceof ReflectClass){
	            claxx = (ReflectClass)a_object;
	        }else{
	            claxx = reflector.forObject(a_object);
	        }
	    }else{
			if(Deploy.csharp){
				return a_object;
			}
	    }
        if(i_yapField == null){
            return a_object;
        }
        return i_yapField.coerce(claxx, a_object);
	}
    
	
	YapClass getYapClass(){
		if(i_yapField != null){
			return i_yapField.getFieldYapClass(i_trans.i_stream);
		}
		return null;
	}
	
	YapField getYapField(YapClass yc){
		if(i_yapField != null){
			return i_yapField;
		}
		YapField yf = yc.getYapField(i_name);
		if(yf != null){
		    yf.alive();
		}
		return yf;
	}
	
	boolean isArray(){
		return i_yapField != null && i_yapField.getHandler() instanceof YapArray;
	}
	
	boolean isClass(){
		return i_yapField == null || i_yapField.getHandler().getType() == YapConst.TYPE_CLASS;
	}
	
	boolean isSimple(){
		return i_yapField != null && i_yapField.getHandler().getType() == YapConst.TYPE_SIMPLE;
	}
	
	YapComparable prepareComparison(Object obj){
		if(i_yapField != null){
			return i_yapField.prepareComparison(obj);
		}
		if(obj == null){
			return Null.INSTANCE;
		}
		YapClass yc = i_trans.i_stream.getYapClass(i_trans.reflector().forObject(obj), true);
		YapField yf = yc.getYapField(i_name);
		if(yf != null){
			return yf.prepareComparison(obj);
		}
		return null;
	}
	
	void unmarshall(Transaction a_trans){
		if(i_yapClassID != 0){
			YapClass yc = a_trans.i_stream.getYapClass(i_yapClassID);
			i_yapField = yc.i_fields[i_index];
		}
	}
	
	public void visit(Object obj) {
		((QCandidate) obj).useField(this);
	}
}

