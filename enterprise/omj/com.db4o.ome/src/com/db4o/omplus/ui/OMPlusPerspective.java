package com.db4o.omplus.ui;

import org.eclipse.ui.IFolderLayout;
import org.eclipse.ui.IPageLayout;
import org.eclipse.ui.IPerspectiveFactory;


import com.db4o.omplus.datalayer.OMPlusConstants;

public class OMPlusPerspective implements IPerspectiveFactory
{
	public static final String ID = OMPlusConstants.OME_PERSPECTIVE_ID;
		
	public void createInitialLayout(IPageLayout layout) 
	{
		String editorAreaId = IPageLayout.ID_EDITOR_AREA;
		
		// before fix for OMJ - 21
	/*	IFolderLayout folderLayout = layout.createFolder(OMPlusConstants.CLASS_VIEW_RESULTS_FOLDER_ID,IPageLayout.LEFT, 0.50f,  editorAreaId);
		folderLayout.addView(OMPlusConstants.CLASS_VIEWER_ID);
		folderLayout.addView(OMPlusConstants.QUERY_RESULTS_ID);*/
		layout.addView(OMPlusConstants.CLASS_VIEWER_ID,IPageLayout.LEFT, 0.50f,  editorAreaId);
		
		// before fix for OMJ - 21
//		layout.addView(OMPlusConstants.QUERY_BUILDER_ID, IPageLayout.RIGHT , 0.5f,  editorAreaId);
		
		IFolderLayout folderLayout = layout.createFolder(OMPlusConstants.CLASS_VIEW_RESULTS_FOLDER_ID,IPageLayout.RIGHT, 0.5f,  editorAreaId);
		folderLayout.addView(OMPlusConstants.QUERY_BUILDER_ID);
		folderLayout.addView(OMPlusConstants.QUERY_RESULTS_ID);
		
		layout.addView(OMPlusConstants.PROPERTY_VIEWER_ID, IPageLayout.BOTTOM, 0.75f, OMPlusConstants.CLASS_VIEWER_ID);
		
		layout.addActionSet("OMPlus.actionSet");
				
		layout.setFixed(false);
		layout.setEditorAreaVisible(false);
		
	}
}
