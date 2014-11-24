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


package org.polepos.circuits.fragmentation;


public class SilverstoneItem {
    
    
    private static final int MAXIMUM_WORKLOAD_LENGTH = 100;
    
    private static final String WORKLOAD_TEMPLATE;
    
    private static final String[] workloads = new String[100]; 
    
    
    static{
        StringBuffer sb = new StringBuffer();
        for (int i = 0; i < MAXIMUM_WORKLOAD_LENGTH; i++) {
            sb.append("L");
        }
        WORKLOAD_TEMPLATE = sb.toString();
        for (int i = 0; i < workloads.length; i++) {
			workloads[i] = WORKLOAD_TEMPLATE.substring(0, i); 
		}
    }
    
    public int _index;
    
    public String _workload;
    
    public SilverstoneItem(){
        
    }
    
    public SilverstoneItem(int i) {
        _index = i;
        int loadLength = i % 100;
        _workload = workloads[loadLength];
    }
    
    public int getIndex(){
        return _index;
    }

}
