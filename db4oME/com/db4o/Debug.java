/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;

/**
 * @exclude
 */
public abstract class Debug extends Debug4 {
    

    public static final boolean useOldClassIndex = true;
    
    public static final boolean useBTrees = false;
    
    public static final boolean useNIxPaths = true;
    
    public static final boolean ixTrees = false;
    
    public static final boolean freespace = Deploy.debug ? true :false;
    
    public static final boolean xbytes = Debug.freespace ? true : false;
    
    public static final boolean freespaceChecker = false;
    
    public static final boolean checkSychronization = false;
    
    public static final boolean atHome = false;

    public static final boolean indexAllFields = true;
    
    public static final boolean configureAllClasses = indexAllFields;
    public static final boolean configureAllFields = indexAllFields;
    
    public static final boolean weakReferences = true;

    public static final boolean arrayTypes = true;

    public static final boolean verbose = false;

    public static final boolean fakeServer = false;
    static final boolean messages = false;

    public static final boolean nio = true;
    
    static final boolean lockFile = true;

    static final boolean longTimeOuts = false;

    static YapFile serverStream;
    static Queue4 clientMessageQueue;
    static Lock4 clientMessageQueueLock;
    
    public static void expect(boolean cond){
        if(! cond){
            throw new RuntimeException("Should never happen");
        }
    }
    
    public static void ensureLock(Object obj) {
        if (atHome) {
            try {
                obj.wait(1);
            } catch (IllegalMonitorStateException imse) {
                System.err.println("No Lock Alarm.");
                imse.printStackTrace();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public static boolean exceedsMaximumBlockSize(int a_length) {
        if (a_length > YapConst.MAXIMUM_BLOCK_SIZE) {
            if (atHome) {
                System.err.println("Maximum block size  exceeded!!!");
                new Exception().printStackTrace();
            }
            return true;
        }
        return false;
    }
    
    public static boolean exceedsMaximumArrayEntries(int a_entries, boolean a_primitive){
        if (a_entries > (a_primitive ? YapConst.MAXIMUM_ARRAY_ENTRIES_PRIMITIVE : YapConst.MAXIMUM_ARRAY_ENTRIES)) {
            if (atHome) {
                System.err.println("Maximum array elements exceeded!!!");
                new Exception().printStackTrace();
            }
            return true;
        }
        return false;
    }

}
