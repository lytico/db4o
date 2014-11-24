/* Copyright (C) 2004 - 2006 db4objects Inc. http://www.db4o.com */

package org.polepos.drs;

import com.db4o.drs.test.*;

public class CSCSCar extends DrsCar {
	
    public CSCSCar () {
        super(new Db4oClientServerDrsFixture("poleposA", 4449), new Db4oClientServerDrsFixture("poleposB", 4445));
    }
    
    @Override
    public String name() {
        return "cs-cs";
    }


}
