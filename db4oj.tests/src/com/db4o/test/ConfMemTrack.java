/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;


/**
 * Tests WeakReferences.
 * Derive AllTests from this configuration and watch
 * the memory consumption in the task manager while
 * this test runs.
 */
    
public class ConfMemTrack extends AllTestsConfAll{
    
    Class[] TESTS =
    new Class[] {
			  MemTrack.class
    };
    
    
    int RUNS = 1000;
    
    boolean CLIENT_SERVER = false;
}
