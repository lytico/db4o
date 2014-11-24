/* Copyright (C) 2004 - 2006 db4objects Inc. http://www.db4o.com */

package org.polepos.drs;

import com.db4o.drs.test.*;


public class CSDb4oCar extends DrsCar {

    public CSDb4oCar () {
        super(new Db4oClientServerDrsFixture("poleposA", 4445), new Db4oDrsFixture("poleposB"));
    }
    
    @Override
    public String name() {
        return "cs-db4o";
    }

}
