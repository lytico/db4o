/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model;

import com.db4o.objectmanager.model.nodes.IModelNode;

import java.util.ListIterator;



/**
 * IGraphIterator.  A Visitor object that can traverse an object graph.
 *
 * @author djo
 */
public interface IGraphIterator extends ListIterator {
	
    /**
     * Method getPath.
     * 
     * Returns the path traversed to get to the current object.
     * 
	 * @return GraphPosition the path to the current object. 
	 */
	public GraphPosition getPath();
    
    /**
	 * Method setPath. Sets the current position in the graph using the
	 * specified GraphPosition.
	 * <p>
	 * Note that no checking is done to make sure that the GraphPosition object
	 * being used to reset the position was created using this IGraphIterator.
	 * If a different IGraphIterator created the GraphPosition object than is the
	 * receiver of this message, the results are undefined.
	 * 
	 * @param path The GraphPosition to use when setting the current position.
	 */
    public void setPath(GraphPosition path);
    
    /**
     * Method isPathSelectionChangable.  returns true if the selectedPath
     * can be changed.  This will ask all selection listeners if the
     * selection can be changed.  If no listener vetos the path selection,
     * the path selection is considered to be changeable.
     * 
     * @return true if SelectedPath can be changed, false otherwise.
     */
    public boolean isPathSelectionChangable();
	
	/**
	 * Method setSelectedPath.  Calls isPathSelectionChangable().  If this
     * returns true, calls setPath(path), then notifies all selection 
     * listeners that the selected path has changed.
     * 
	 * @param path The GraphPosition to use when setting the current position.
     * @return true if setting the path was successful, false otherwise.
	 */
	public boolean setSelectedPath(GraphPosition path);
    
    /**
     * Method hasParent.  Indicates if the current element has a parent.
     * 
     * Returns true if the current element has a parent (is not a root node
     * in this object graph).
     * 
     * @return true if the current object has a parent; false otherwise.
     */
    public boolean hasParent();
    
    /**
     * Method nextHasChildren.  Indicates if the next element could have
     * children.
     * 
     * Returns true if the next element could have at least one child.
     * 
     * @return true if there is a next element and the next element could have 
     * at least one child; false otherwise.
     */
    public boolean nextHasChildren();
    
    /**
     * Method previousMayHaveChildren.  Indicates if the previous element could have
     * children.
     * 
     * Returns true if the previous element could have at least one child.
     * 
     * @return true if there is a and the previous element could have at least 
     * one child; false otherwise.
     */
    public boolean previousHasChildren();
    
    /**
     * Method selectParent().  Traverses up the object graph to the parent
     * of the current node collection.
     */
    public void selectParent();
    
    /**
     * Method selectNextChild().  Makes the next child in the object graph 
     * the new parent, and selects its children to iterate over.
     */
    public IModelNode selectNextChild();
    
    /**
	 * Method selectPreviousChild(). Makes the previous child in the object
	 * graph the new parent, and selects its children to iterate over.
	 */
    public void selectPreviousChild();
    
    /**
     * Method reset.  Reset the graph visitor to the top-level set of nodes.
     */
    public void reset();
    
    /**
     * Method numChildren.  Returns the number of next child elements.
     * @return
     */
    public int numChildren();

	/**
	 * Method addSelectionChangedListener.  Adds a selection changed listener
	 * to the collection of listeners that will be notified when the 
	 * IGraphController's selection changes.
	 * 
	 * @param selectionListener The selectionListener to add.
	 */
	public void addSelectionChangedListener(IGraphIteratorSelectionListener selectionListener);

	/**
	 * Method removeSelectionChangedListener.  Removes the specified selection
	 * changed listener from the collection of listeners that will be notified
	 * when the IGraphController's selection changes.
	 * 
	 * @param selectionListener The selectionListener to remove.
	 */
	public void removeSelectionChangedListener(IGraphIteratorSelectionListener selectionListener);
}
