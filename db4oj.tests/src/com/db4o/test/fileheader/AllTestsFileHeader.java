/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.fileheader;

import com.db4o.db4ounit.common.assorted.*;
import com.db4o.test.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTestsFileHeader extends AllTestsJdk1_2 {

    public static void main(String[] args) {
        runSolo(new FileHeaderTestSuite());
        new SimplestPossibleTestCase().runNetworking();
    }

}
