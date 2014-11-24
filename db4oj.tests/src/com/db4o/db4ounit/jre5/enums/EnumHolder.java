/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.enums;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class EnumHolder {
    // JDK1.5: typesafe enums
    private TypeCountEnum type;
    
    public EnumHolder(TypeCountEnum type) {
        this.type=type;
    }
    
    public TypeCountEnum getType() {
        return type;
    }
}
