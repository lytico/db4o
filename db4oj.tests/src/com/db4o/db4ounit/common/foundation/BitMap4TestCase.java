/* Copyright (C) 2bitbit4 - 2bitbit6  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class BitMap4TestCase implements TestCase {
    
    public void test() {
        
        byte[] buffer = new byte[100];
        
        for (int i = 0; i < 17; i++) {
            BitMap4 map = new BitMap4(i);            
            map.writeTo(buffer, 11);
            
            BitMap4 reReadMap = new BitMap4(buffer,11, i);
            
            for (int j = 0; j < i; j++) {
                tBit(map, j);
                tBit(reReadMap, j);
            }
        }
        
    }
    
    private void tBit(BitMap4 map, int bit) {
        map.setTrue(bit);
        Assert.isTrue(map.isTrue(bit));
        map.setFalse(bit);
        Assert.isFalse(map.isTrue(bit));
        map.setTrue(bit);
        Assert.isTrue(map.isTrue(bit));
        
    }

}
