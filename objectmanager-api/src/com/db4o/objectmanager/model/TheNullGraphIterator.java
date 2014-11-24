package com.db4o.objectmanager.model;

import com.db4o.objectmanager.model.nodes.IModelNode;

public class TheNullGraphIterator implements IGraphIterator {
	
	private static IGraphIterator nullIterator = null;
	private static GraphPosition nullGraphPosition = new GraphPosition();

	public static IGraphIterator getDefault() {
		if (nullIterator == null) {
			nullIterator = new TheNullGraphIterator();
		}
		return nullIterator;
	}

	public GraphPosition getPath() {
		return nullGraphPosition;
	}

	public void setPath(GraphPosition path) {
	}

	public boolean isPathSelectionChangable() {
		return false;
	}

	public boolean setSelectedPath(GraphPosition path) {
		return false;
	}

	public boolean hasParent() {
		return false;
	}

	public boolean nextHasChildren() {
		return false;
	}

	public boolean previousHasChildren() {
		return false;
	}

	public void selectParent() {
	}

	public IModelNode selectNextChild() {
        return null;
    }

	public void selectPreviousChild() {
	}

	public void reset() {
	}

	public int numChildren() {
		return 0;
	}

	public void addSelectionChangedListener(IGraphIteratorSelectionListener selectionListener) {
	}

	public void removeSelectionChangedListener(IGraphIteratorSelectionListener selectionListener) {
	}

	public boolean hasNext() {
		return false;
	}

	public Object next() {
		return null;
	}

	public boolean hasPrevious() {
		return false;
	}

	public Object previous() {
		return null;
	}

	public int nextIndex() {
		return 0;
	}

	public int previousIndex() {
		return 0;
	}

	public void remove() {
	}

	public void set(Object arg0) {
	}

	public void add(Object arg0) {
	}
	
}
