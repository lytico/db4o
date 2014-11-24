using System;
using System.Collections.Generic;
using System.Text;

namespace OMControlLibrary.Common
{
	#region public class Constants
	/// <summary>
	/// This class is used for declaring all the constants.
	/// </summary>
	public class Constants
	{
		public const string JAPANESE_CULTURE = "ja-JP";
		public const string ENGLISH_CULTURE = "en-US";

		public const string RESOURCE_NAME = "OMControlLibrary.Resources.Resource";
		public const string RESOURCE_NAME_JP = "OMControlLibrary.Resources.Resource.jp";
		public const string DB4OICON = "OMControlLibrary.Images.db4o.ico";

		#region Global Constants
		public const string VIEW_CLASSPOPERTY = "ClassProperty";
		public const string VIEW_QUERYBUILDER = "QueryBuilder";
		public const string VIEW_ATTRIBUTES = "Attributes";
		public const string VIEW_DBPROPERTIES = "DBProperties";
		public const string VIEW_OBJECTPROPERTIES = "ObjectProperties";
		public const string GUID_FORMATTER_STRING = "GUID_FORMATTER_STRING";


		#endregion

		#region Windows Related Constants

		//GUIDS
		public const string GUID_OBJECTBROWSER = "{286981DF-4D5D-4D66-9CAF-C21793F6836E}";
		public const string GUID_QUERYBUILDER = "{426E8D27-3D33-4fc8-B3E9-9883AADC679F}";
		public const string GUID_PROPERTIES = "{997e97ed-a58a-4ac2-aa89-c95068ae3750}";

		//Class Names
		public const string CLASS_NAME_LOGIN = "OMControlLibrary.Login";
		public const string CLASS_NAME_OBJECTBROWSER = "OMControlLibrary.ObjectBrowser";
		public const string CLASS_NAME_QUERYBUILDER = "OMControlLibrary.QueryBuilder";
		public const string CLASS_NAME_PROPERTIES = "OMControlLibrary.PropertiesTab";

		//Captions
		public const string DB4O_BROWSER_CAPTION = "DB4O_BROWSER_CAPTION";
		public const string QUERY_BUILDER_CAPTION = "QUERY_BUILDER_CAPTION";
		public const string LOGIN_WINDOW_LOCAL_CAPTION = "LOGIN_WINDOW_LOCAL_CAPTION";
		public const string LOGIN_WINDOW_REMOTE_CAPTION = "LOGIN_WINDOW_REMOTE_CAPTION";
		public const string LOGIN_OPEN_FILE_DIALOG_CAPTION = "LOGIN_OPEN_FILE_DIALOG_CAPTION";
		public const string LOGIN_CAPTION_LOCAL = "LOGIN_CAPTION_LOCAL";
		public const string LOGIN_CAPTION_OPEN = "LOGIN_CAPTION_OPEN";
		public const string LOGIN_CAPTION_CONNECT = "LOGIN_CAPTION_CONNECT";

		public const string QUERYBUILDER = "Query Builder";
		public const string LOGIN = "Connection Info";
		public const string DB4OBROWSER = "db4o Browser";
		public const string DB4OPROPERTIES = "DataBase Properties";
		public const string PROPERTIES = "Properties";

		//Toolbar Captions
		public const string TOOLBAR_DISCONNECT = "Disconnect";




		#endregion

		#region Toolbar Button Text

		public const string TOOLBAR_MODE_REDUCED = "TOOLBAR_MODE_REDUCED";
		public const string TOOLBAR_MODE_FULL = "TOOLBAR_MODE_FULL";
		public const string TOOLBAR_LOGIN_CAPTION = "TOOLBAR_LOGIN_CAPTION";
		public const string TOOLBAR_LOGOUT_CAPTION = "TOOLBAR_LOGOUT_CAPTION";

		#endregion

		#region Constant for Properties ToolWindow

		public const string PROPERTIES_TAB_DATABASE_CAPTION = "PROPERTIES_TAB_DATABASE_CAPTION";
		public const string PROPERTIES_TAB_CAPTION = "PROPERTIES_TAB_CAPTION";
		#endregion

		#region Constants in Login Tool Window
		internal const string AXHOST_GUID = "59EE46BA-677D-4d20-BF10-8D8067CB8B33";

		internal const string LOGIN_FILE_TEXT = "LOGIN_FILE_TEXT";
		internal const string LOGIN_HOST_TEXT = "LOGIN_HOST_TEXT";
		internal const string LOGIN_PORT_TEXT = "LOGIN_PORT_TEXT";
		internal const string LOGIN_USERNAME_TEXT = "LOGIN_USERNAME_TEXT";
		internal const string LOGIN_PASSWORD_TEXT = "LOGIN_PASSWORD_TEXT";
		internal const string LOGIN_RECENTCONNECTION_TEXT = "LOGIN_RECENTCONNECTION_TEXT";
		internal const string LOGIN_NEWCONNECTION_TEXT = "LOGIN_NEWCONNECTION_TEXT";
		internal const string LOGIN_TYPE_TEXT = "LOGIN_TYPE_TEXT";

		internal const string ERROR_MSG_CONNECTION = "ERROR_MSG_CONNECTION";

		#endregion

		#region Constant for Object Browser Toolwindow

		public const int MAX_IMAGE_WIDTH = 256;
		public const string OBJBROWSER_FIND_TEXT = "OBJBROWSER_FIND_TEXT";
		public const string TOOLTIP_OBJECTBROWSER_CLEARSEARCH = "TOOLTIP_OBJECTBROWSER_CLEARSEARCH";
		public const string TOOLTIP_OBJECTBROWSER_ASSEMBLY_VIEW = "TOOLTIP_OBJECTBROWSER_ASSEMBLY_VIEW";
		public const string TOOLTIP_OBJECTBROWSER_CLASS_VIEW = "TOOLTIP_OBJECTBROWSER_CLASS_VIEW";

		public const string TOOLTIP_OBJECTBROWSER_FILTER = "TOOLTIP_OBJECTBROWSER_FILTER";
		public const string TOOLTIP_OBJECTBROWSER_FILTER_PREV = "TOOLTIP_OBJECTBROWSER_FILTER_PREV";
		public const string TOOLTIP_OBJECTBROWSER_FILTER_NEXT = "TOOLTIP_OBJECTBROWSER_FILTER_NEXT";

		#endregion

		#region Constants for TreeView

		public const string DUMMY_NODE_TEXT = "dummy";

		#endregion

		#region Datagrid Constants

		//Common Constants
		public const int INVALID_INDEX_VALUE = -1;

		//Class Property Grid Column Header Text
		public const string CLASS_PROPERTY_FIELD_NAME = "CLASS_PROPERTY_FIELD_NAME";
		public const string CLASS_PROPERTY_DATA_TYPE = "CLASS_PROPERTY_DATA_TYPE";
		public const string CLASS_PROPERTY_ISINDEXED = "CLASS_PROPERTY_ISINDEXED";
		public const string CLASS_PROPERTY_ISPUBLIC = "CLASS_PROPERTY_ISPUBLIC";

		//Class Properties Grid Column Name
		public const string PROPERTY_FIELD_NAME = "PROPERTY_FIELD_NAME";
		public const string PROPERTY_DATA_TYPE = "PROPERTY_DATA_TYPE";
		public const string PROPERTY_ISINDEXED = "PROPERTY_ISINDEXED";
		public const string PROPERTY_ISPUBLIC = "PROPERTY_ISPUBLIC";

		//Database Properties Grid Column Name
		public const string DB_PROPERTY_SIZE = "DB_PROPERTY_SIZE";
		public const string DB_PROPERTY_CLASSES = "DB_PROPERTY_CLASSES";
		public const string DB_PROPERTY_FREESPACE = "DB_PROPERTY_FREESPACE";

		//Objects Properties Grid Column Name
		public const string OBJECT_PROPERTY_UUID = "OBJECT_PROPERTY_UUID";
		public const string OBJECT_PROPERTY_LOCALID = "OBJECT_PROPERTY_LOCALID";
		public const string OBJECT_PROPERTY_DEPTH = "OBJECT_PROPERTY_DEPTH";
		public const string OBJECT_PROPERTY_VERSION = "OBJECT_PROPERTY_VERSION";

		//Query Builder Grid Column Header Text
		public const string QUERY_GRID_FIELD = "QUERY_GRID_FIELD";
		public const string QUERY_GRID_CONDITION = "QUERY_GRID_CONDITION";
		public const string QUERY_GRID_VALUE = "QUERY_GRID_VALUE";
		public const string QUERY_GRID_OPERATOR = "QUERY_GRID_OPERATOR";

		//Attribute Grid Column Header Text
		public const string ATTRIB_TEXT = "ATTRIB_TEXT";


		//Query Builder Grid Column Name 
		public const string QUERY_GRID_DELETEROW = "QUERY_GRID_DELETEROW";
		public const string QUERY_GRID_CALSSNAME_HIDDEN = "CLASSNAME_HIDDEN";
		public const string QUERY_GRID_FIELDTYPE_HIDDEN = "FIELDTYPE_HIDDEN";
        public const string QUERY_GRID_FIELDTYPE_DISPLAY_HIDDEN = "FIELDTYPE_DISPLAY_HIDDEN";

		//Query Result Grid Column Name 
		public const string QUERY_GRID_ISEDITED_HIDDEN = "ISEDITED_HIDDEN";

		public const string GRID_SHOW_ALL_COLUMN = "SHOW_ALL_COLUMN";

		internal const string DBDATAGRIDVIEW_TEXTBOX_COLUMN =
			"System.Windows.Forms.DataGridViewTextBoxColumn";
		internal const string DBDATAGRIDVIEW_CHECKBOX_COLUMN =
			 "System.Windows.Forms.DataGridViewCheckBoxColumn";
		internal const string DBDATAGRIDVIEW_COMBOBOX_COLUMN =
			 "System.Windows.Forms.DataGridViewComboBoxColumn";


		//DataGrid Context Menu
		internal const string CONTEXT_MENU_DATAGRID_DELETE = "DeleteRow";


		//Invalid Trace Messages
		internal const string TRACEMESSAGE_INVALIDROW_INDEX = "Invalid row index found in datagridview ";

		#endregion

		#region Vaidation Constants

		internal static string VALIDATION_REGX_ALPHANUMERIC = "[^a-zA-Z0-9_.\\b]";
		internal static string VALIDATION_REGX_NUMERIC = "[0-9\\b]";

		#endregion

		#region UI Literals Constants

		public const string QUERY_GROUP_CAPTION = "QUERY_GROUP_CAPTION";



		#endregion

		#region Query Result
		internal const string QUERY_RESULT_SHOW_HIDE_QUERY_BUILDER = "QUERY_RESULT_SHOW_HIDE_QUERY_BUILDER";
		internal const string QUERY_RESULT_PROPERTIES_PANE_TEXT = "QUERY_RESULT_PROPERTIES_PANE_TEXT";

		internal const string BUTTON_SAVE_CAPTION = "BUTTON_SAVE_CAPTION";
		internal const string BUTTON_DELETE_CAPTION = "BUTTON_DELETE_CAPTION";
		internal const string CONTEXT_MENU_CAPTION = "CONTEXT_MENU_CLEAR";
		internal const string BUTTON_UPDATE_QUERY_CAPTION = "BUTTON_UPDATE_QUERY_CAPTION";
		#endregion

		#region Query Builder
		internal const string LABEL_RECENT_QUERIES_CAPTION = "LABEL_RECENT_QUERIES";
		internal const string LABEL_QUERY_BUILDER_CAPTION = "LABEL_QUERY_BUILDER";
		internal const string BUTTON_ADD_GROUP_CAPTION = "BUTTON_ADD_GROUP_CAPTION";
		internal const string BUTTON_CLEAR_AAL_CAPTION = "BUTTON_CLEAR_AAL_CAPTION";
		internal const string COMBOBOX_DEFAULT_TEXT = "COMBOBOX_DEFAULT_TEXT";
		internal const string LABEL_OBJECTS_NO = "LABEL_OBJECTS_NO";
		internal const string LABEL_OF = "LABEL_OF";
		internal const string TOOLTIP_PAGE_FIRST = "TOOLTIP_PAGE_FIRST";
		internal const string TOOLTIP_PAGE_PREV = "TOOLTIP_PAGE_PREV";
		internal const string TOOLTIP_PAGE_NEXT = "TOOLTIP_PAGE_NEXT";
		internal const string TOOLTIP_PAGE_LAST = "TOOLTIP_PAGE_LAST";

		#endregion

		#region WebServices Constants
		internal const string OBJECTMANAGER_USER_PERMISSION_QUERYBUILDER = "QueryBuilder";
		internal const string OBJECTMANAGER_USER_PERMISSION_SUPPORT = "Support";
		internal const string OBJECTMANAGER_USER_PERMISSION_PAIRING = "Pairing";
		#endregion

		//ObjectBrowser
		public const string OBJECTMANAGER_CONTACT_US_FILE_PATH = @"/ContactSales/ContactSales.htm";

		//DataGridViewGroup 

		internal const string HOT_KEY_CHAR = "&";


		//Messages
		internal const string VALIDATION_MESSAGE_VALUE_NOT_SPECIFIED = "VALIDATION_MESSAGE_VALUE_NOT_SPECIFIED";
		internal const string CONFIRMATION_MSG_REMOVE_QUERY_GROUP = "CONFIRMATION_MSG_REMOVE_QUERY_GROUP";
		internal const string OBJECTMANAGER_MSG_RESULT_NOT_FOUND = "OBJECTMANAGER_MSG_RESULT_NOT_FOUND";
		internal const string VALIDATION_MSG_INVALIDE_VALUE = "VALIDATION_MSG_INVALIDE_VALUE";
		internal const string VALIDATION_DEFAULT_GROUP_IS_EMPTY = "VALIDATION_DEFAULT_GROUP_IS_EMPTY";
		internal const string VALIDATION_MSG_INVALID_CREDENTIALS = "VALIDATION_MSG_INVALID_CREDENTIALS";
		internal const string VALIDATION_MSG_SELECT_DATABASE = "VALIDATION_MSG_SELECT_DATABASE";
		internal const string VALIDATION_MSG_ENTER_HOST = "VALIDATION_MSG_ENTER_HOST";
		internal const string VALIDATION_MSG_ENTER_PORT = "VALIDATION_MSG_ENTER_PORT";
		internal const string VALIDATION_MSG_ENTER_USERNAME = "VALIDATION_MSG_ENTER_USERNAME";
		internal const string VALIDATION_MSG_ENTER_PASSWORD = "VALIDATION_MSG_ENTER_PASSWORD";
		internal const string VALIDATION_MSG_SELECT_REMOTE_CONNECTION = "VALIDATION_MSG_SELECT_REMOTE_CONNECTION";
		internal const string VALIDATION_MSG_PORT_RANG = "VALIDATION_MSG_PORT_RANG";
		internal const string VALIDATION_MSG_MANDATORY_FIELDS = "VALIDATION_MSG_MANDATORY_FIELDS";

		internal const string PROGRESS_MESSAGE_CHECKING_CREDENTIALS = "PROGRESS_MESSAGE_CHECKING_CREDENTIALS";

		public const string CONFIRMATION_MSG_LOGOUT = "CONFIRMATION_MSG_LOGOUT";
		public const string PRODUCT_CAPTION = "PRODUCT_CAPTION";
		//TreeView
		internal const string CONTEXT_MENU_ADD_TO_QUERY = "CONTEXT_MENU_ADD_TO_QUERY";
		internal const string CONTEXT_MENU_ADD_TO_ATTRIBUTE = "CONTEXT_MENU_ADD_TO_ATTRIBUTE";
		internal const string CONTEXT_MENU_SHOW_ALL_OBJECTS = "CONTEXT_MENU_SHOW_ALL_OBJECTS";
		internal const string CONTEXT_MENU_EXPRESSION_GROUP = "EXPRESSION_GROUP_";

        //
        public const char CONST_DOT_CHAR = '.';

        public const string CONST_COMMA_STRING = ",";
        public const char CONST_COMMA_CHAR = ',';
	    public const string QUERYRESULT = "OMControlLibrary.QueryResult";
	}
	#endregion
}
