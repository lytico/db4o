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

package org.polepos.teams.db4o;

import org.polepos.circuits.sepangmulti.*;
import org.polepos.circuits.trees.*;

import com.db4o.config.*;

/**
 * @author Herkules
 */
public class TreesMultiDb4o extends Db4oDriver implements TreesMultiDriver{
	

	@Override
	public void configure(Configuration config) {
		
	}
	
	@Override
	public void circuitCompleted() {
		treeRootIDs = null;
	}
    
    long[] treeRootIDs;
    
	public void write(){
        begin();
        treeRootIDs = new long[setup().getObjectCount()];
        for (int treeIdx = 0; treeIdx < setup().getObjectCount(); treeIdx++) {
        	Tree tree = Tree.createTree(setup().getDepth());
        	store(tree);
        	treeRootIDs[treeIdx] = db().getID(tree);
        	commit();
		}
	}

	public void read(){
        for (int treeIdx = 0; treeIdx < setup().getObjectCount(); treeIdx++) {
	        Tree tree = readAndActivate(treeIdx);
	        Tree.traverse(tree, new TreeVisitor() {
	            public void visit(Tree tree) {
	                addToCheckSum(tree.getDepth());
	            }
	        });
        }
	}

    public void delete() {
        begin();
        for (int treeIdx = 0; treeIdx < setup().getObjectCount(); treeIdx++) {
	        Tree tree = readAndActivate(treeIdx);
	        Tree.traverse(tree, new TreeVisitor() {
	            public void visit(Tree tree) {
	                db().delete(tree);
	            }
	        });
	        commit();
        }
    }
    
    private Tree readAndActivate(int treeIdx){
        Tree tree = (Tree)db().getByID(treeRootIDs[treeIdx]);
        db().activate(tree, Integer.MAX_VALUE);
        return tree;
    }
}
