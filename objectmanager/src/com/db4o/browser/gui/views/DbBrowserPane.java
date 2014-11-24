/*
 * Created on Jan 14, 2005
 */
package com.db4o.browser.gui.views;

import java.util.Map;

import org.eclipse.swt.graphics.Rectangle;
import org.eclipse.swt.layout.FillLayout;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Text;
import org.eclipse.swt.widgets.Tree;
import org.eclipse.ve.sweet.reflect.DuckType;

import com.swtworkbench.community.xswt.XSWT;

/**
 * Class DbBrowserPane.  A SWT control that can be used as the basis for a
 * database browser.  This implementation defines the UI in an layout.xswt
 * file.
 * 
 * @author djo
 */
public class DbBrowserPane extends Composite {
	
	/**
     * Construct a DbBrowserPane object.
     * 
     * The standard SWT constructor.  Note that although this class extends
     * Composite, it does not make sense to set a layout manager on it.
     * 
	 * @param parent The SWT parent control
	 * @param style SWT style bits.  Accepts the same style bits as Composite
	 */
	public DbBrowserPane(Composite parent, int style) {
		super(parent, style);
        parent.setLayout(new FillLayout());
        setLayout(new GridLayout());
        contents = createContents();
	}
    
    protected Map createContents() {
        Map contents;
        Rectangle displayBounds = getDisplay().getBounds();
        if (displayBounds.width > 480 && displayBounds.height > 480) {
            contents = XSWT.createl(this, "layout-desktop.xswt", getClass());
        } else {
            contents = XSWT.createl(this, "layout.xswt", getClass());
        }
        return contents;
    }

    protected Map contents = null;
    
    /**
     * Returns an ITextProperty object representing the Path Label.  If a 
     * control named "PathLabel" is present, this is returned, albeit
     * DuckTyped to an ITextProperty.  Otherwise, the Shell is returned,
     * DuckTyped to an ITextProperty.
     * 
     * @return an ITextProperty object where the path label may be set.
     */
    public ITextProperty getPathLabel() {
		Object pathLabel = contents.get("PathLabel");
		if (pathLabel != null) {
			return (ITextProperty) DuckType.implement(ITextProperty.class, pathLabel);
		}
		return (ITextProperty) DuckType.implement(ITextProperty.class, getShell());
    }
    
    /**
     * Returns the Search text box
     * 
     * @return the search text box
     */
    public Text getSearch() {
        return (Text) contents.get("Search");
    }
    
    /**
     * Returns the button next to the search text box
     * 
     * @return the clear search button
     */
    public Button getSearchButton() {
        Button button;
        return (Button) contents.get("ClearSearchButton");
    }
    
    /**
     * Method getObjectTree.  Returns the object tree.
     * 
     * @return Tree the object tree.
     */
    public Tree getObjectTree() {
        return (Tree) contents.get("ObjectTree");
    }
    
    /**
     * Method GetFieldArea.  Returns the area where the field names and
     * values will be displayed.
     * 
     * @return Composite the field display area
     */
    public Composite getFieldArea() {
        return (Composite) contents.get("FieldArea");
    }
	
	/**
	 * Method GetLeftButton.  Returns the "back" button.
	 * 
	 * @return Button the "back" navigation button
	 */
	public ISelectionSource getLeftButton() {
		return (ISelectionSource) DuckType.implement(ISelectionSource.class, contents.get("LeftButton"));
	}
	
	/**
	 * Method GetRightButton.  Returns the "forward" button.
	 * 
	 * @return Button the "forward" navigation button
	 */
	public ISelectionSource getRightButton() {
		return (ISelectionSource) DuckType.implement(ISelectionSource.class, contents.get("RightButton"));
	}
    
    /**
     * Method getQueryButton.  Returns the query button.
     * 
     * @return Button the Query... button
     */
    public Button getQueryButton() {
        return (Button) contents.get("QueryButton");
    }
    
    /**
     * @return the Cancel button
     */
    public Button getCancelButton() {
    	return (Button) contents.get("CancelButton");
    }
    
    /**
     * @return the Save button
     */
    public Button getSaveButton() {
    	return (Button) contents.get("SaveButton");
    }

    /**
     * @return the Delete button
     */
    public Button getDeleteButton() {
    	return (Button) contents.get("DeleteButton");
    }

}


