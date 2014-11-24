/*
 * This file is part of com.db4o.browser.
 *
 * com.db4o.browser is free software; you can redistribute it and/or modify
 * it under the terms of version 2 of the GNU General Public License
 * as published by the Free Software Foundation.
 *
 * com.db4o.browser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with com.swtworkbench.ed; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
package com.db4o.objectmanager.model.nodes;

import java.io.PrintStream;

import com.db4o.objectmanager.model.IDatabase;
import com.db4o.objectmanager.model.nodes.partition.PartitionFieldNodeFactory;
import com.db4o.reflect.ReflectClass;

/**
 * Class ClassNode.
 * 
 * @author djo
 */
public class ClassNode implements IModelNode {

	private final ReflectClass _class;
    private final IDatabase _database;
	private static final int THRESHOLD = 100;
    private long[] _ids;

	/**
	 * @param contents
	 * @param database
	 */
	public ClassNode(ReflectClass contents, IDatabase database) {
		_class = contents;
        _database = database;
    }

	public ClassNode(ReflectClass contents, IDatabase database,int[][] treeSpec,int start,int end) {
		_class = contents;
        _database = database;
    }
    
    public IDatabase getDatabase() {
        return _database;
    }

    /* (non-Javadoc)
     * @see com.db4o.objectmanager.model.nodes.IModelNode#mayHaveChildren()
     */
    public boolean hasChildren() {
        return true;
    }
    
    /* (non-Javadoc)
     * @see com.db4o.objectmanager.model.nodes.IModelNode#children()
     */
    public IModelNode[] children() {
        if (_ids == null)
            _ids=_database.instanceIds(_class);
        return PartitionFieldNodeFactory.create(_ids,0,_ids.length,_database);
    }
	
	/* (non-Javadoc)
	 * @see com.db4o.objectmanager.model.nodes.IModelNode#getName()
	 */
	public String getName() {
		return "";
	}
	
	/* (non-Javadoc)
	 * @see com.db4o.objectmanager.model.nodes.IModelNode#getValueString()
	 */
	public String getValueString() {
		return getText();
	}

	/* (non-Javadoc)
	 * @see com.db4o.objectmanager.model.nodes.IModelNode#getText()
	 */
	public String getText() {
		return _class.getName();
	}

    public ReflectClass getReflectClass() {
        return _class;
    }

    public void setShowType(boolean showType) {
        // Nothing needed here
    }

    public boolean isEditable() {
        return false;
    }

    public Object getEditValue() {
        return null;
    }

	/* (non-Javadoc)
	 * @see com.db4o.objectmanager.model.nodes.IModelNode#getId()
	 */
	public long getId() {
		return -1;
	}

	// ClassNodes should be invisible in the XML output...
	
	public void printXmlReferenceNode(PrintStream out) {
	}

	public void printXmlStart(PrintStream out) {
	}

	public void printXmlEnd(PrintStream out) {
	}

	public void printXmlValueNode(PrintStream out) {
	}
	
	public boolean shouldIndent() {
		return false;
	}

	/**
	 * Computes tree level start indices per level (into level below or
	 * into the id list, if leaf level).
	 * 
	 * Example: (29,3) should result in
	 * {{0,3,6,9,12,15,18,21,24,27},{0,3,6,8},{0,2}}
	 */
//    private static int[][] computeTreeSpec(int numItems, int threshold) {
//        java.util.List structure=new ArrayList();
//        int curnum=numItems;
//        
//		int[] lastlevel=null;
//        while(curnum>threshold) {
//            int numbuckets=(int)Math.ceil((float)curnum/threshold);
//            int minbucketsize=curnum/numbuckets;
//            int numexceeding=curnum%numbuckets;
//            int[] curlevel=new int[numbuckets];
//            int startidx=0;
//			curlevel[0]=0;
//            for (int bucketidx = 1; bucketidx < curlevel.length; bucketidx++) {
//                int curfillsize=minbucketsize;
//                if(bucketidx <= numexceeding) {
//                    curfillsize++;
//                }
//				startidx+=curfillsize;
//				curlevel[bucketidx] = startidx;
//            }
//            structure.add(curlevel);
//			lastlevel=curlevel;
//            curnum=numbuckets;
//        }
//        return (int[][])structure.toArray(new int[structure.size()][]);    
//    }
}
