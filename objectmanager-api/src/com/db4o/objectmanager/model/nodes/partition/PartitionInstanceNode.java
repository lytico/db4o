/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model.nodes.partition;

import java.io.PrintStream;

import com.db4o.objectmanager.model.IDatabase;
import com.db4o.objectmanager.model.nodes.IModelNode;

public class PartitionInstanceNode implements IModelNode {

    private IDatabase _database;
    private long[] _sourceIds;
    private int _startIdx;
    private int _endIdx;

    public PartitionInstanceNode(IDatabase database, long[] sourceIds, int startIdx, int endIdx) {
        _database=database;
        _sourceIds=sourceIds;
        _startIdx=startIdx;
        _endIdx=endIdx;
    }
    
    public IDatabase getDatabase() {
        return _database;
    }

    public boolean hasChildren() {
        return true;
    }

    public IModelNode[] children() {
        return PartitionFieldNodeFactory.create(_sourceIds, _startIdx, _endIdx, _database);
    }

    public String getText() {
        return "More: ["+_startIdx+".."+_endIdx+"]";
    }

    public String getName() {
        return "More: ["+_startIdx+".."+_endIdx+"]";
    }

    public String getValueString() {
        return "";
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

	public long getId() {
		return -1;
	}

	// Partitions shouldn't show up in XML exports...
	
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

}
