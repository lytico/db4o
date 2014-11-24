package com.db4o.omplus.ui.interfaces;

/**
 * An interface which lists all the functions that lead to
 * updations of other views present in the perspective.
 * The updation must only be done by the QueryResult view(class) and never by its 
 * child components directly. Child components/classes just call one
 * of the methods in this interface to ask QueryResults to upadte soem other view
 * 
 * @author prameela_nair
 *
 */
public interface IViewUpdatorForQueryResults
{
	
	//public void updatePropertiesView(Object resuleObject);
	
	//TODO: delete this...as of now for checking purpose
	//public void updatePropertiesView(Object resuleObject, String classname);

	public void updatePropertiesView(String classname);
	/**
	 * UPdate tab for new result object
	 * @param resultObj
	 */
	public void updatePropertiesView(Object resultObj);
	
}
