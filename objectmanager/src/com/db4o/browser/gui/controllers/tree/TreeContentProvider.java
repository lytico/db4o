/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers.tree;

import org.eclipse.jface.viewers.ITreeContentProvider;
import org.eclipse.jface.viewers.Viewer;

import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIterator;
import com.db4o.objectmanager.model.nodes.IModelNode;

/**
 * TreeContentProvider.  The content provider for the object browser tree view.
 * <p>
 * This Content Provider uses GraphPosition objects to represent each
 * tree node.
 *
 * @author djo
 */
public class TreeContentProvider implements ITreeContentProvider {
    
    private IGraphIterator i;
    
    /**
     * Method children.  Return the children of the current family represented
     * as an Object[] of IGraphPosition objects.
     * 
     * @param i IGraphIterator an iterator that specifies the current family.
     * @return Object[] an array of IGraphPosition objects.
     */
    private Object[] children(IGraphIterator i) {
        Object[] result = new Object[i.numChildren()];
        int position = 0;
        while (i.hasNext()) {
            result[position] = i.getPath();
            i.next();
            ++position;
        }
        return result;
    }

    /* (non-Javadoc)
     * @see org.eclipse.jface.viewers.IContentProvider#inputChanged(org.eclipse.jface.viewers.Viewer, java.lang.Object, java.lang.Object)
     */
    public void inputChanged(Viewer viewer, Object oldInput, Object newInput) {
        i = (IGraphIterator) newInput;
    }

    /* (non-Javadoc)
     * @see org.eclipse.jface.viewers.ITreeContentProvider#getParent(java.lang.Object)
     */
    public Object getParent(Object element) {
        GraphPosition result = new GraphPosition((GraphPosition) element);
        if (result.hasParent()) {
            result.pop();
            return result;
        }
        return null;
    }

    /* (non-Javadoc)
     * @see org.eclipse.jface.viewers.IStructuredContentProvider#getElements(java.lang.Object)
     */
    public Object[] getElements(Object inputElement) {
        IGraphIterator i = (IGraphIterator) inputElement;
        i.reset();
        return children(i);
    }

    /* (non-Javadoc)
     * @see org.eclipse.jface.viewers.ITreeContentProvider#hasChildren(java.lang.Object)
     */
    public boolean hasChildren(Object element) {
        GraphPosition position = (GraphPosition) element;
        IModelNode node = position.getCurrent();
        return node != null ? node.hasChildren() : false;
    }

	/* (non-Javadoc)
	 * @see org.eclipse.jface.viewers.ITreeContentProvider#getChildren(java.lang.Object)
	 */
	public Object[] getChildren(Object parentElement) {
        i.setPath((GraphPosition) parentElement);
        i.selectNextChild();
		return children(i);
	}

	/* (non-Javadoc)
	 * @see org.eclipse.jface.viewers.IContentProvider#dispose()
	 */
	public void dispose() {
	}

}

