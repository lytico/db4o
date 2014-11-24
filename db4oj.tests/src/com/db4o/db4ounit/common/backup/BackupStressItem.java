/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.backup;


public class BackupStressItem {
    
    public String _name;
    
    public int _iteration;
    
    public BackupStressItem(String name, int iteration) {
        _name = name;
        _iteration = iteration;
    }
    
}
