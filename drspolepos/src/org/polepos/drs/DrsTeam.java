/* Copyright (C) 2006  db4objects Inc.  http://www.db4o.com */

package org.polepos.drs;

import org.polepos.framework.*;

import com.db4o.*;

/**
 * @exclude
 */
public class DrsTeam extends Team{
    
    public DrsTeam() {
        
        // FIXME: This should probably be done by a Db4oDrsFixture
        
        Db4o.configure().generateUUIDs(Integer.MAX_VALUE);
        Db4o.configure().generateVersionNumbers(Integer.MAX_VALUE);
    }

    @Override
    public void configure(int[] options) {
        
    }

    @Override
    public String name() {
        return "dRS";
    }

    @Override
    public String description() {
        return "db4o Replication System";
    }

    @Override
    public Car[] cars() {
        return new Car[]{
        		new Db4oDb4oCar(), 
        		new Db4oCSCar(),
        		new CSDb4oCar(),
        		new CSCSCar()};
    }

    @Override
    public Driver[] drivers() {
        return new Driver[]{
            new KyalamiDrs()
        };
    }

    @Override
    public String website() {
        return "http://developer.db4o.com";
    }

}
