package com.db4o.db4ounit.jre5.collections;

import com.db4o.collections.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public interface ArrayMap4Operation<K, V> {
    
    public void operate(ArrayMap4<K, V> map);
    
}
