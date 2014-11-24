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


/**
 * Class ITreeNode. An interface for tree nodes in the containership tree.
 * 
 * @author djo
 */
public interface IModelNode {
    
    /**
     * @return the database implementation
     */
    public IDatabase getDatabase();
    
	/**
	 * @return if this node has children
	 */
	public boolean hasChildren();

	/**
	 * @return an array of child nodes (no generics now please)
	 */
	public IModelNode[] children();

	/**
	 * @return the text string to display
	 */
	public String getText();
	
	/**
	 * Return the field name
	 * 
	 * @return
	 */
	public String getName();
	
	/**
	 * Return the field value as a string
	 * 
	 * @return
	 */
	public String getValueString();

    /**
     * Return the name of the node's type
     * @param showType If the data type of the node should be displayed
     */
    public void setShowType(boolean showType);
    
    /**
     * @return true if this node is editable
     */
    public boolean isEditable();

    /**
     * @return the object to edit if isEditable() is true; null otherwise.
     */
    public Object getEditValue();
    
    /**
     * @return the OID for this node or -1 if there is none
     */
    public long getId();

    
    // XML export support methods
    
	/**
	 * Print a reference node for this object
	 * 
	 * @param out The stream on which to print the output
	 */
	public void printXmlReferenceNode(PrintStream out);

	/**
	 * Print an XML start tag for this node
	 * 
	 * @param out The stream on which to print the start tag
	 */
	public void printXmlStart(PrintStream out);

	/**
	 * Print an XML end tag for this node
	 * 
	 * @param out The stream on which to print the end tag
	 */
	public void printXmlEnd(PrintStream out);

	/**
	 * Print an XML line containing the value of this node
	 * @param out
	 */
	public void printXmlValueNode(PrintStream out);

	/**
	 * @return true if the XML code should indent for children of this node; false otherwise
	 */
	public boolean shouldIndent();
	
	
}

