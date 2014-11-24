/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;


final class YBoolean extends YapJavaClass
{

    static final int LENGTH = 1 + YapConst.ADDED_LENGTH;
	
	private static final byte TRUE = (byte) 'T';
	private static final byte FALSE = (byte) 'F';
	private static final byte NULL = (byte) 'N';
	
	private static final Boolean i_primitive = new Boolean(false);
	
    public YBoolean(YapStream stream) {
        super(stream);
    }
    
	public int getID(){
		return 4;
	}
	
	public Object defaultValue(){
		return i_primitive;
	}
	
	public int linkLength(){
		return LENGTH;
	}
	
	protected Class primitiveJavaClass(){
		return Boolean.class;
	}
	
	Object primitiveNull(){
		return i_primitive;
	}

	Object read1(YapReader a_bytes){
		if (Deploy.debug){
			a_bytes.readBegin(YapConst.YAPBOOLEAN);
		}
		byte ret = a_bytes.readByte();
		if (Deploy.debug){
			a_bytes.readEnd();
		}
		
		if(ret == TRUE){
			return new Boolean(true);
		}
		if(ret == FALSE){
			return new Boolean(false);
		}
		
		return null;
	}
	
	public void write(Object a_object, YapReader a_bytes){
		if(Deploy.debug){
			a_bytes.writeBegin(YapConst.YAPBOOLEAN);
		}
		byte set;
		if (a_object == null){
			set = NULL;
		} else {
			if(((Boolean)a_object).booleanValue()){
				set = TRUE;
			}else{
				set = FALSE;
			}
		}
		a_bytes.append(set);
		if(Deploy.debug){
			a_bytes.writeEnd();
		}
	}

	
	// Comparison_______________________
	
	private boolean i_compareTo;
	
	private boolean val(Object obj){
		return ((Boolean)obj).booleanValue();
	}
	
	void prepareComparison1(Object obj){
		i_compareTo = val(obj);
	}
    
    public Object current1(){
        return new Boolean(i_compareTo);
    }
	
	boolean isEqual1(Object obj){
		return obj instanceof Boolean && val(obj) == i_compareTo;
	}
	
	boolean isGreater1(Object obj){
	    if(i_compareTo){
	        return false;
	    }
		return obj instanceof Boolean && val(obj);
	}
	
	boolean isSmaller1(Object obj){
	    if(! i_compareTo){
	        return false;
	    }
	    return obj instanceof Boolean && ! val(obj);
	}
}
