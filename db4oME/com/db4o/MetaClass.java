/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * Class metadata to be stored to the database file
 * Don't obfuscate.
 * 
 * @exclude
 * @persistent
 */
public class MetaClass implements Internal4{
	
	/** persistent field, don't touch */
	public String name;
    
    /** persistent field, don't touch */
	public MetaField[] fields;
	
	public MetaClass(){
	}
	
	public MetaClass(String name){
		this.name = name;
	}
	
	MetaField ensureField(Transaction trans, String a_name){
		if(fields != null){
			for (int i = 0; i < fields.length; i++) {
				if(fields[i].name.equals(a_name)){
					return fields[i];
				}
            }
            MetaField[] temp = new MetaField[fields.length + 1];
            System.arraycopy(fields, 0, temp, 0, fields.length);
            fields = temp;
		}else{
			fields = new MetaField[1];
		}
		MetaField newMetaField = new MetaField(a_name);
		fields[fields.length - 1] = newMetaField;
		trans.i_stream.setInternal(trans, newMetaField, YapConst.UNSPECIFIED, false);
		trans.i_stream.setInternal(trans, this, YapConst.UNSPECIFIED, false);
		return newMetaField;
	}

}
