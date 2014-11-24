package com.db4o.objectmanager.model.nodes;

import java.io.PrintStream;

import com.db4o.objectmanager.model.IDatabase;

public class NullNode implements IModelNode {
	private IDatabase database;

    public NullNode(IDatabase database) {
        this.database = database;
	}
    
    public IDatabase getDatabase() {
        return database;
    }
	
	public boolean hasChildren() {
		return false;
	}

	public IModelNode[] children() {
		return new IModelNode[0];
	}

	public String getText() {
		return "null";
	}

	public String getName() {
		return "null";
	}

	public String getValueString() {
		return "null";
	}

    public void setShowType(boolean showType) {
        // Nothing needed here
    }

    public boolean isEditable() {
        return true;
    }

    public Object getEditValue() {
        return "";
    }

	public long getId() {
		return -1;
	}

	public void printXmlReferenceNode(PrintStream out) {
	}

	public void printXmlStart(PrintStream out) {
	}

	public void printXmlEnd(PrintStream out) {
	}

	public void printXmlValueNode(PrintStream out) {
		out.print("<null/>");
	}

	public boolean shouldIndent() {
		return false;
	}


}
