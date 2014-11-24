/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.controllers;

import org.eclipse.swt.events.SelectionAdapter;
import org.eclipse.swt.events.SelectionEvent;

import com.db4o.browser.gui.views.ISelectionSource;
import com.db4o.objectmanager.model.GraphPosition;
import com.db4o.objectmanager.model.IGraphIterator;
import com.db4o.objectmanager.model.IGraphIteratorSelectionListener;

/**
 * NavigationController.   Manages the forward and back buttons in the UI.
 *
 * @author djo
 */
public class NavigationController implements IBrowserController {
	
	private IGraphIterator model = null;
	
	private final int STACK_LIMIT = 50;
	private GraphPosition[] undoRedoStack = new GraphPosition[STACK_LIMIT];
	private int stackPosition;
	private int stackMax;

	private ISelectionSource leftButton;
	private ISelectionSource rightButton;
	
	/**
	 * Construct a NavigationController on a specific UI element
	 * 
	 * @param ui The DbBrowserPane to which to attach.
	 */
	public NavigationController(ISelectionSource leftButton, ISelectionSource rightButton) {
		this.leftButton = leftButton;
		this.rightButton = rightButton;
		leftButton.addSelectionListener(new SelectionAdapter() {
			public void widgetSelected(SelectionEvent e) {
				safeRun(undo);
			}
		});
		rightButton.addSelectionListener(new SelectionAdapter() {
			public void widgetSelected(SelectionEvent e) {
				safeRun(redo);
			}
		});
		resetUndoRedoStack();
		enableButtons();
	}
	
	/**
	 * Enable or disable the left and right buttons based on the undo/redo state
	 */
	private void enableButtons() {
		if (stackPosition > 0) {
			leftButton.setEnabled(true);
		} else {
			leftButton.setEnabled(false);
		}
		if (stackPosition < stackMax) {
			rightButton.setEnabled(true);
		} else {
			rightButton.setEnabled(false);
		}
	}

	/* (non-Javadoc)
	 * @see com.db4o.browser.gui.controllers.IBrowserController#setInput(com.db4o.objectmanager.model.IGraphIterator, com.db4o.objectmanager.model.GraphPosition)
	 */
	public void setInput(IGraphIterator input, GraphPosition selection) {
		if (model != null) {
			model.removeSelectionChangedListener(selectionChangedListener);
		}
		
		model = input;
		model.addSelectionChangedListener(selectionChangedListener);
		resetUndoRedoStack();
	}
	
	/**
	 * Set the UndoRedoStack to be empty;
	 */
	public void resetUndoRedoStack() {
		stackPosition=-1;
		stackMax=-1;
		for (int i = 0; i < undoRedoStack.length; i++) {
			undoRedoStack[i] = null;		// Encourage the GC...
		}
        enableButtons();
	}
	
	/**
	 * Returns if no additional items can be added to the stack without
	 * the stack scrolling.
	 * 
	 * @return true if the stack must scroll in order for an item to be added.
	 */
	private boolean isFull() {
		return stackMax >= STACK_LIMIT;
	}
	
	/**
	 * If the stack is full, we must scroll an element off the bottom.
	 */
	private void scrollStack() {
		if (!isFull())
			return;
		
		for (int i = 0; i < undoRedoStack.length-1; i++) {
			undoRedoStack[i] = undoRedoStack[i+1];
		}
		--stackPosition;
		--stackMax;
	}

	// Prevent recursive callbacks...
	private int navigating = 0;
	private void safeRun(Runnable r) {
		if (navigating > 0)
			return;
		++navigating;
		try {
			r.run();
		} finally {
			--navigating;
		}
	}
	
	private Runnable add = new Runnable() {
		public void run() {
			GraphPosition element = model.getPath();
			scrollStack();
			++stackPosition;
			stackMax = stackPosition;		// Discard the redo stack
			undoRedoStack[stackPosition] = element;
			enableButtons();
		}
	};
	
	private Runnable undo = new Runnable() {
		public void run() {
            if (model.isPathSelectionChangable()) {
    			if (stackPosition >= 0) {
    				--stackPosition;
    				model.setSelectedPath(undoRedoStack[stackPosition]);
    			}
    			enableButtons();
            }
		}
	};
	
	private Runnable redo = new Runnable() {
		public void run() {
            if (model.isPathSelectionChangable()) {
    			if (stackPosition < stackMax) {
    				++stackPosition;
    				model.setSelectedPath(undoRedoStack[stackPosition]);
    			}
    			enableButtons();
            }
		}
	};

	private IGraphIteratorSelectionListener selectionChangedListener = new IGraphIteratorSelectionListener() {
        public boolean canSelectionChange() {
            // From our point of view it's always legal to change the selection...
            return true;
        }
		public void selectionChanged() {
			safeRun(add);
		}
	};

}
