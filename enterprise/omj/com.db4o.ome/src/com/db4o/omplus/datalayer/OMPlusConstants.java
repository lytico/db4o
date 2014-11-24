package com.db4o.omplus.datalayer;

public class OMPlusConstants 
{
	//public static final String PLUGIN_ID = "OMPlus";
	//public static final String ECLIPSE_PLUGIN_DATA_FOLDE_NAME = "OME";
	public static final String PLUGIN_ID = "com.db4o.ome";
	public static final String ECLIPSE_PLUGIN_DATA_FOLDE_NAME = "com.db4o.ome";
	
	/**
	 * View Id's -These should map to the Id's defined in plugin.xml for views
	 */
	public static final String CLASS_VIEWER_ID = "com.db4o.omplus.ui.ClassViewer";
	public static final String QUERY_BUILDER_ID = "com.db4o.omplus.ui.QueryBuilder";
	public static final String PROPERTY_VIEWER_ID = "com.db4o.omplus.ui.PropertyViewer";
	public static final String QUERY_RESULTS_ID = "com.db4o.omplus.ui.QueryResults";
	public static final String CLASS_VIEW_RESULTS_FOLDER_ID = "com.db4o.omplus.ui.folderClassViewQueryResults";
	
	public static final String OME_PERSPECTIVE_ID = "com.db4o.omplus.ui.OMPlusPerspective";

	public static final String WEB_SERVICES_LOGIN_ID = "com.db4o.omplus.ui.actions.WebServiceLoginAction";
	//The editor Id used by eclipse. Got this while debugging.
	//TODO: Should ideally retrieve this thru' some known mechanism that eclipse has given
	public static String BROWSER_EDITOR_ID = "org.eclipse.ui.browser.editor";
	
	/**
	 * Id's for menu buttons - These should map to the Id's defined 
	 * in plugin.xml for actions(menus/toolbars)
	 */
	public static final String EXTREME_CONNECT_PLUGIN_ID = "com.db4o.omplus.ui.actions.browsers.ExtremeConnectId";
	public static final String SUPPORT_CASES_PLUGIN_ID = "com.db4o.omplus.ui.actions.browsers.SupportCasesId";
	public static final String HELP_PLUGIN_ID = "com.db4o.omplus.ui.actions.browsers.HelpId";
	
	//Browser names
	//TODO: change this names to reflect the correct one
	public static final String EXTREME_CONNECT_BROWSER_NAME = "Xtreme Connect";
	public static final String SUPPORT_CASES_BROWSER_NAME = "Support Cases";
	public static final String HELP_BROWSER_NAME = "Help";	
	public static final String DB4O_HOMEPAGE_BROWSER_NAME = "db4o Homepage";
	public static final String DB4O_DEVELOPER_COMMUNITY_BROWSER_NAME = "db4o Developer Community";
	public static final String DB4O_DOWNLOADS_BROWSER_NAME = "db4o Downloads";
	public static final String WEB_SERVICE_CANCEL_BROWSER_NAME = "Contact Sales";
	public static final String FORGOT_PASSWORD_BROWSER_NAME = "Forgot Password";
	
	//Browser Id's
	public static final String EXTREME_CONNECT_BROWSER_ID = "XtremeConnectBrowserId";
	public static final String SUPPORT_CASES_BROWSER_ID = "SupportCasesBrowserId";
	public static final String HELP_BROWSER_ID = "HelpBrowserId";
	public static final String DB4O_HOMEPAGE_BROWSER_ID = "db4oHomepageBrowserId";
	public static final String DB4O_DEVELOPER_COMMUNITY_BROWSER_ID = "db4oDeveloperCommunityBrowserId";
	public static final String DB4O_DOWNLOADS_BROWSER_ID = "db4oDownloadsBrowserId";
	public static final String WEB_SERVICE_CANCEL_BROWSER_ID = "WebServiceCancelBrowserId";
	public static final String FORGOT_PASSWORD_BROWSER_ID = "ForgotPasswordBrowserId";
	
	// Web Service status labels
	public static final String FULL_MODE = "Logged In: OME Full mode";
	public static final String REDUCED_MODE = "Logged In: OME Reduced mode";
	public static final String STATUS_LOGGEDOUT = "Logged Out: OME Reduced mode";
	
	//Browser URL's
	public static final String EXTREME_CONNECT_URL = "https://customer.db4o.com/Peer/Default.aspx";
	public static final String SUPPORT_CASES_URL = "https://customer.db4o.com/Support/Default.aspx";
	//public static final String HELP_BROWSER_STRING = "FAQ/FAQ.htm";
	public static final String DB4O_HOMEPAGE_URL = "http://db4o.com/";
	public static final String DB4O_DEVELOPER_COMMUNITY_URL = "http://developer.db4o.com/";
	public static final String DB4O_DOWNLOADS_URL = "http://developer.db4o.com/files/default.aspx";
	
	
	public static final String HELP_BROWSER_LOCATION = "FAQ/FAQ.htm";
	public static final String WEB_SERVICE_CANCEL_BROWSER_LOCATION = "ContactSales/ContactSales.htm";
	/*//public static final String PATH_SEPARATOR = System.getProperty("file.separator");
	public static final String HELP_BROWSER_STRING = "file:///"+System.getProperty("user.dir")+
														PATH_SEPARATOR+"plugins"+
														PATH_SEPARATOR+ ECLIPSE_PLUGIN_DATA_FOLDE_NAME+
														PATH_SEPARATOR + "FAQ/FAQ.htm";
	//public static final String WEB_SERVICE_CANCEL_BROWSER_STRING = "ContactSales/ContactSales.htm";
	public static final String WEB_SERVICE_CANCEL_BROWSER_STRING = "file:///"+
															System.getProperty("user.dir")+
															PATH_SEPARATOR+"plugins"+
															PATH_SEPARATOR+ ECLIPSE_PLUGIN_DATA_FOLDE_NAME+
															PATH_SEPARATOR + "ContactSales/ContactSales.htm";*/
	public static final String FORGOT_PASSWORD_URL = "http://www.db4o.com/users/retrievePassword.aspx";
	
	//Browser tool tips
	public static final String EXTREME_CONNECT_TOOLTIP = "Xtreme Connect";
	public static final String SUPPORT_CASES_TOOLTIP = "Support Cases";
	public static final String HELP_BROWSER_TOOLTIP = "Help";	
	public static final String DB4O_HOMEPAGE_TOOLTIP = "db4o Homepage";
	public static final String DB4O_DEVELOPER_COMMUNITY_TOOLTIP = "db4o Developer Community";
	public static final String DB4O_DOWNLOADS_TOOLTIP = "db4o Downloads";
	public static final String WEB_SERVICE_CANCEL_BROWSER_TOOLTIP = "Contact Sales";
	public static final String FORGOT_PASSWORD_BROWSER_TOOLTIP = "Forgot Password";
	
	
	public final static String CLASSPATH = "java.class.path";
	public final static String PLUGIN_FLD = "plugins";
	public final static char BACKSLASH = '/';
	
	public final static String REGEX = ":";
	
	
	/**
	 * Types of object that can be queried
	 */
	
	public static final int PRIMITIVE = 1;
	public static final int COLLECTION = 2;
	public static final int COMPLEX = 3;
	
	
	/**
	 * Icon Id's
	 */
	public static final int COLUMN_VISIBLE_ICON = 0;
	public static final int PRIMITIVE_ICON = 1;
	public static final int COLLECTION_ICON = 2;
	public static final int COMPLEX_ICON = 3;
	public static final int FLAT_VIEW_ICON = 4;
	public static final int HIERARCHICAL_VIEW_ICON = 5;
	public static final int DB4O_LOGO_IMAGE = 6;
	public static final int CLOSE_GROUP_ICON = 7;
	public static final int BROWSE_ICON = 8;
	public static final int CLEAR_FILTER_ICON = 9;
	public static final int SEARCH_NEXT_ICON = 10;
	public static final int SEARCH_PREV_ICON = 11;
	public static final int DB4O_WIND_ICON = 12;
	

	public static final int PACKAGE_NODE = 0;
	public static final int CLASS_NODE = 1;
	public static final int CLASS_FIELD_NODE = 2;
	public static final char DOT_OPERATOR = '.';
	
	public static final String DATE_FORMAT = "dd/MM/yyyy";
	
	//Note: Regular expression pattern string to validate dates in the format "d/m/y" from 1/1/1600 - 31/12/9999.
	//Checks for valid days for a given month and the days are validated for the given month and year. 
	//Validates Leap years for all 4 digits years from 1600-9999, and all 2...
	public static final String DATE_REG_EX = "^(?:(31)(\\D)(0?[13578]|1[02])\\2|(29|30)(\\D)(0?[13-9]|1[0-2])\\5|(0?[1-9]|1\\d|2[0-8])"+
													"(\\D)(0?[1-9]|1[0-2])\\8)((?:1[6-9]|[2-9]\\d)?\\d{2})$|^(29)(\\D)(0?2)\\12((?:1[6-9]|[2-9]"+
													"d)?(?:0[48]|[2468][048]|[13579][26])|(?:16|[2468][048]|[3579][26])00)$";

	
	public static final int MAX_OBJS_PAGE = 50;

	public static final String NULL_VALUE = "null";
	
	//NOTE: these names have been picked from the reponse recieved by web service. If they change the structure
	//Modify this too
	/**
	 * Web Service Feature names
	 */	
	public static final String WEB_SERVICE_QUERY_BUILDER = "QueryBuilder";
	public static final String WEB_SERVICE_SUPPORT = "Support";
	public static final String WEB_SERVICE_PAIRING = "Pairing";
	
	public static String[] booleanArrayItems = {"True", "False"};
	
	public static final String YES = "Yes";
	public static final String NO = "No";
	
	/**
	 * General title to be used across teh application for messageboxes, dialog boxes
	 */
	public static final String DIALOG_BOX_TITLE = "ObjectManager Enterprise";
	
	/**
	 * int representing what button was clicked in teh dialog box.
	 * 
	 * NOTE: Add more variables if more buttons added
	 */
	public static final int DIALOG_OK_CLICKED = 0;
	public static final int DIALOG_CANCEL_CLICKED = 1;
	
	public static final String OME_COMMAND_CATEGORY_ID = "com.db4o.omplus.ui.commands";
	public static final String OME_CONNECT_COMMAND_ID = "com.db4o.omplus.ui.commands.connectCommand";
	
	
}
