/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.config.*;

public class Rename {

    public void test() {

        if (Test.run == 1) {

            if (!Test.clientServer || !Test.currentRunner.SOLO) {

                Test.deleteAllInstances(One.class);

                Test.store(new One("wasOne"));

                Test.ensureOccurrences(One.class, 1);

                Test.commit();

                // Rename messages are visible at level 1
                // Db4o.configure().messageLevel(1);

                ObjectClass oc = Db4o.configure().objectClass(One.class);

                // allways rename fields first
                oc.objectField("nameOne").rename("nameTwo");
                oc.rename(Two.class.getName());

                Test.reOpenServer();

                Test.ensureOccurrences(Two.class, 1);
                Test.ensureOccurrences(One.class, 0);
                Two two = (Two)Test.getOne(Two.class);
                Test.ensure(two.nameTwo.equals("wasOne"));
                
                //		If the messageLevel was changed above, switch back to default.
                // 		Db4o.configure().messageLevel(0);
            }

        }

    }

    public static class One {
        public String nameOne;

        public One() {

        }

        public One(String name) {
            nameOne = name;
        }
    }

    public static class Two {

        public String nameTwo;

    }

}
