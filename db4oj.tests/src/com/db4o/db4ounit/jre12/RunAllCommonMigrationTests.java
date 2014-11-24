package com.db4o.db4ounit.jre12;

import com.db4o.db4ounit.common.migration.*;

@decaf.Ignore(decaf.Platform.JDK11)
public class RunAllCommonMigrationTests {
    public static void main(String[] args) {
        System.exit(new AllCommonTests().runSolo());
    }
}
