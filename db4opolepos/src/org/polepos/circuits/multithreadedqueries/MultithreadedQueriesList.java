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


package org.polepos.circuits.multithreadedqueries;

import org.polepos.framework.*;


public class MultithreadedQueriesList implements CheckSummable{
    
    public static final String FIELD_NEXT = "next";
    
    public static final String FIELD_PAYLOAD = "payload";
    
    
    public MultithreadedQueriesList next;
    
    public int payload;
    
    
    public MultithreadedQueriesList() {
        
    }

    private MultithreadedQueriesList(MultithreadedQueriesList next_, int payload_) {
        next = next_;
        payload = payload_;
    }
    
    /**
     * generates a linked list with N entries with payload N
     */
    public static MultithreadedQueriesList generate(int length){
        
        int countAll = 0;
        int currentPayLoad = 1;
        int countPayLoad = 0;
        
        MultithreadedQueriesList currentList = null;
        
        while(countAll < length){
            countAll++;
            MultithreadedQueriesList nextList = new MultithreadedQueriesList(currentList, currentPayLoad);
            countPayLoad ++;
            if(countPayLoad >= currentPayLoad){
                countPayLoad = 0;
                currentPayLoad++;
            }
            currentList = nextList;
        }
        return currentList;
    }

    public long checkSum() {
        return payload;
    }

    public int getPayload() {
        return payload;
    }

}
