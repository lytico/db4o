/* 
This file is part of the PolePosition database benchmark
http://www.polepos.org

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public
License along with this program; if not, write to the Free
Software Foundation, Inc., 59 Temple Place - Suite 330, Boston,
MA  02111-1307, USA. */

package org.polepos.circuits.queries;

import org.polepos.framework.*;


public class QueriesSlow extends TimedLapsCircuitBase {

    @Override
    public String description() {
        return "excecutes a variety of queries to test the efficiency of the query processor";
    }

    @Override
    public Class requiredDriver() {
        return QueriesDriver.class;
    }

    @Override
    protected void addLaps() {
    	
        add(new Lap("write", false, false));
        
        add(new Lap("queryAndBigResult"));
        add(new Lap("queryNestedAnd"));
        add(new Lap("queryTwoFieldAndNot"));

        add(new Lap("queryNotRange"));
        add(new Lap("queryOrTwoLevels"));
        add(new Lap("queryBigRangeFound"));
        add(new Lap("queryTwoLevelBigRangeFound"));

        add(new Lap("getOneFromBigRangeQuery"));
        add(new Lap("getOneFromTwoLevelBigRangeQuery"));
        add(new Lap("getOneFromOrTwoLevelsQuery"));        
    }

}
