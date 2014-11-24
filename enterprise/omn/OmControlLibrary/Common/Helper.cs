using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.TypeMauplation;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.Login;
using EnvDTE;
using OManager.BusinessLayer.UIHelper;
using sforce;
using Microsoft.VisualStudio.CommandBars;
using System.Data;
using OME.Crypto;
using OME.Logging.Common;
using OME.Logging.Tracing;
using stdole;

namespace OMControlLibrary.Common
{
	public class Helper
	{
		#region Member Variable

		
		private static string m_className;
		private static string m_baseClass;
		private static List<OMQuery> m_listOMQueries;
		private static Hashtable m_OMResultedQuery = new Hashtable();
		private static int m_tabIndex;

		
		private static Window queryResultToolWindow;
	
		private static List<Hashtable> m_hashList;
		private static Hashtable m_hashClassGUID;
		private static Hashtable m_hashTableBaseClass = new Hashtable();
		public static AccountManagementService serviceProxy;
		public static CommandBarButton m_statusLabel;
		static bool _checkIfLoginWindowIsVisible;
		#endregion

		#region Constant

		private const string GENERIC_TEXT = "(G) ";
		private const string RECENT_QUERY_QUERY_COLUMN = "Query";
		private const string RECENT_QUERY_OMQUERY_COLUMN = "OMQuery";
		
	
		#endregion

		#region Static Properties
		
		

		public static Hashtable HashTableBaseClass
		{
			get { return m_hashTableBaseClass; }
			set { m_hashTableBaseClass = value; }
		}

		public static Hashtable OMResultedQuery
		{
			get { return m_OMResultedQuery; }
			set { m_OMResultedQuery = value; }
		}

		public static Hashtable HashClassGUID
		{
			get { return m_hashClassGUID; }
			set { m_hashClassGUID = value; }
		}

		public static List<Hashtable> HashList
		{
			get { return m_hashList; }
			set { m_hashList = value; }
		}

		public static int Tab_index
		{
			get { return m_tabIndex; }
			set { m_tabIndex = value; }
		}

		public static Window QueryResultToolWindow
		{
			get { return queryResultToolWindow; }
			set { queryResultToolWindow = value; }
		}

		public static bool CheckIfLoginWindowIsVisible
		{
			get
			{
				return _checkIfLoginWindowIsVisible;
			}
			set
			{
				_checkIfLoginWindowIsVisible = value;
			}

		}
		
		/// <summary>
		/// Get/Set the selected class
		/// </summary>
		public static string ClassName
		{
			get
			{
				if (m_className != null)
				{
					if (m_className.Contains(GENERIC_TEXT))
						m_className = m_className.Replace(GENERIC_TEXT, string.Empty);
				}
				return m_className;
			}
			set { m_className = value; }
		}

		/// <summary>
		/// Get/Set the base class name for selected node. 
		/// </summary>
		public static string BaseClass
		{
			get
			{
				if (m_baseClass != null)
				{
					if (m_baseClass.Contains(GENERIC_TEXT))
						m_baseClass = m_baseClass.Replace(GENERIC_TEXT, string.Empty);
				}
				return m_baseClass;
			}
			set
			{
			   
                m_baseClass = value;
			}
		}

		public static List<OMQuery> ListOMQueries
		{
			get
			{
                ConnectionDetails currConnectionDetails = OMEInteraction.GetRefreshedCurrentRecentConnection();  
				if (currConnectionDetails != null)
				{
					m_listOMQueries = currConnectionDetails.QueryList;
				}
				return m_listOMQueries;
				
			}
			set { m_listOMQueries = value; }
		}

		#endregion

		public static string GetResourceString(string key)
		{
			try
			{
				return ApplicationManager.ResourceManager.GetString(key);
			}
			catch (ArgumentNullException objargEx)
			{
				objargEx.ToString();
			}
			return string.Empty;
		}

		public static StdPicture GetResourceImage(string key)
		{
			try
			{
				using (Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(key))
				{
					return (StdPicture)PictureHost .IPictureDisp(Image.FromStream(imageStream));
				}
			}
			catch
			{
				return null;
			}
		}

		#region Listing Helper Methods

		public static string GetClassGUID(string className)
		{
			if (className.Contains(GENERIC_TEXT))
				className = className.Replace(GENERIC_TEXT, string.Empty);

			string classGUID = Guid.NewGuid().ToString("B");
		    try
			{
				if (m_hashClassGUID == null)
				{
					m_hashClassGUID = new Hashtable();
				}

				bool isPresent = m_hashClassGUID.ContainsKey(className);
				if (isPresent)
				{
					classGUID = (string)m_hashClassGUID[className];
				}
				else
				{
					m_hashClassGUID.Add(className, classGUID);
				}
			}
			catch (Exception)
			{
				classGUID = null;
			}
			return classGUID;
		}

		public static void PopulateRecentQueryComboBox(ComboBox comboboxRecentQueries)
		{
		    try
			{
				DataTable recentQueriesDatatable = new DataTable();
				recentQueriesDatatable.Columns.Add(RECENT_QUERY_QUERY_COLUMN, typeof(string));
				recentQueriesDatatable.Columns.Add(RECENT_QUERY_OMQUERY_COLUMN, typeof(OMQuery));

				comboboxRecentQueries.DataSource = null;
				comboboxRecentQueries.Items.Clear();

				DataRow newRow = recentQueriesDatatable.NewRow();
				newRow[0] = GetResourceString(Constants.COMBOBOX_DEFAULT_TEXT);
				newRow[1] = null;

				recentQueriesDatatable.Rows.Add(newRow);
				long timeForRecentQueriesCreation = OMEInteraction.GetTimeforRecentQueriesCreation();
				long timeforDbCreation = AssemblyInspectorObject.Connection.DbCreationTime(); 
                if (timeForRecentQueriesCreation != 0)
                {
                    if (timeForRecentQueriesCreation > timeforDbCreation)
                    {
                        foreach (OMQuery qry in ListOMQueries)
                        {
                            if (qry != null)
                            {
                                newRow = recentQueriesDatatable.NewRow();
                                newRow[0] = qry.QueryString;
                                newRow[1] = qry;

                                recentQueriesDatatable.Rows.Add(newRow);
                            }
                        }
                    }
                    else
                    {
						OMEInteraction.RemoveRecentQueries();
                        ListOMQueries.Clear();
                    }
                }

			    comboboxRecentQueries.DisplayMember = RECENT_QUERY_QUERY_COLUMN;
				comboboxRecentQueries.ValueMember = RECENT_QUERY_OMQUERY_COLUMN;

				comboboxRecentQueries.DataSource = recentQueriesDatatable;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		

		public static bool OnTreeViewAfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode treenode = e.Node;
			if (treenode.Nodes.Count >= 1 && treenode.Nodes[0].Name == Constants.DUMMY_NODE_TEXT)
			{
				treenode.Nodes[Constants.DUMMY_NODE_TEXT].Remove();
				return true;
			}
			
			return false;
		}

		

	    public static string FormulateParentName(TreeNode tempTreeNode, IDictionaryEnumerator eNum)
		{
			string parentName;
			if (tempTreeNode.Parent == null || tempTreeNode.Parent.Tag.ToString() == "Fav Folder")
			{
				parentName = tempTreeNode.Text.Split(',')[0];
			    string[] str = parentName.Split('.');
				parentName = str[str.Length -1] + '.' + eNum.Key;
			}
			else
			{
				parentName = FormulateCompleteClassPath(tempTreeNode) + eNum.Key;

			}
			return parentName;
		}
		public static string FormulateCompleteClassPath(TreeNode treeNode)
		{
			StringBuilder fullpath = new StringBuilder(string.Empty);
			List<string> stringParent = new List<string>();
			try
			{
				while (treeNode.Parent != null && treeNode.Parent.Tag != null
					&& treeNode.Parent.Tag.ToString() != "Fav Folder" && treeNode.Parent.Tag.ToString() != "Assembly View")
				{

					if (!string.IsNullOrEmpty(treeNode.Text))
						stringParent.Add(treeNode.Text);

					treeNode = treeNode.Parent;

				}
			    string[] breakName = (treeNode.Text.Split(',')[0]).Split('.');
                stringParent.Add(breakName[breakName.Length-1]);

				for (int i = stringParent.Count; i > 0; i--)
				{
					string parent = stringParent[i - 1] + ".";
					fullpath.Append(parent);
				}
			}
			catch (Exception ex)
			{
				LoggingHelper.ShowMessage(ex);
			}
			return fullpath.ToString();

		}

		public static string FindRootNode(TreeNode node)
		{
			try
			{
				if (node != null)
				{
					while (node.Parent != null && node.Parent.Tag != null && node.Parent.Tag.ToString() != "Fav Folder" && node.Parent.Tag.ToString() != "Assembly View")
					{
						node = node.Parent;
					}

					return node.Text;
				}
			}
			catch (Exception ex)
			{
				LoggingHelper.ShowMessage(ex);
			}
			return string.Empty;
		}
       

		public static bool CheckUniqueNessAttributes(string fullpath, dbDataGridView datagridAttributeList)
		{
			try
			{
				if (datagridAttributeList.Rows.Count > 0)
				{
					for (int i = 0; i < datagridAttributeList.Rows.Count; i++)
					{
						if (fullpath.Equals(datagridAttributeList.Rows[i].Cells[0].Value.ToString()))
							return false;
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return true;
		}

		#endregion

		#region Public  Methods

		public static bool IsNumeric(string strToCheck)
		{
			Regex objAlphaNumericPattern = new Regex(Constants.VALIDATION_REGX_NUMERIC);
			return objAlphaNumericPattern.IsMatch(strToCheck);
		}

		public static void ClearAllCachedAttributes()
		{
			try
			{
				BaseClass = string.Empty;

				ClassName = string.Empty;
				if (HashClassGUID != null)
					HashClassGUID.Clear();

				if (HashList != null)
					HashList.Clear();

				if (HashTableBaseClass != null)
					HashTableBaseClass.Clear();
				if (OMResultedQuery != null)
					OMResultedQuery.Clear();

				QueryResultToolWindow = null;
				Tab_index = 0;
				ListofModifiedObjects.Instance.Clear();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

		}

		#endregion

		#region WebService Related Methods

		public static byte[] EncryptPass(string pass)
		{
			try
			{
				CryptoDES objCryptoDES = new CryptoDES();
				objCryptoDES.Initialize();
				string encryptSTR = objCryptoDES.DESSelfEncrypt(pass);
				byte[] contents = StrToByteArray(encryptSTR);
				return contents;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
				return null;
			}

		}
		private static byte[] StrToByteArray(string str)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			return encoding.GetBytes(str);
		}

		private static string ByteArrayToStr(byte[] array)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			return encoding.GetString(array);
		}

		public static string DecryptPass(byte[] array)
		{
			CryptoDES objCryptoDES = new CryptoDES();
			objCryptoDES.Initialize();
			string info = ByteArrayToStr(array);
			info = objCryptoDES.DESSelfDecrypt(info);
			return info;
		}

		
		#endregion

		internal static string GetFullPath(TreeNode treenode)
		{
			StringBuilder fullpath = new StringBuilder(string.Empty);
			TreeNode treenodeParent;
			List<string> stringParent = new List<string>();
            string parentName = string.Empty; 

			try
			{
				OMETrace.WriteFunctionStart();

				treenodeParent = treenode.Parent;
				while (treenodeParent != null && treenodeParent.Tag.ToString() != "Fav Folder" && treenodeParent.Tag.ToString() != "Assembly View")
				{
					if (treenodeParent.Name.LastIndexOf(",") != -1)
					{
						char[] splitChar = { ',' };
						//Set the base class name for selected field
						BaseClass = treenodeParent.Name;

						//get parent name from node text
						parentName = treenodeParent.Name.Split(splitChar)[0];


					    int classIndex = parentName.LastIndexOf('.');

						//get the parent name of selected node
						parentName = parentName.Substring(classIndex + 1, parentName.Length - classIndex - 1);
					}
                   else
                    {
                        BaseClass = treenodeParent.Name;
                        int classIndex = treenodeParent.Name.LastIndexOf('.');

                        //get the parent name of selected node
                        parentName = treenodeParent.Name.Substring(classIndex + 1, treenodeParent.Name.Length - classIndex - 1);
                    }

				    if (!string.IsNullOrEmpty(parentName))
						stringParent.Add(parentName);

					if (treenodeParent.Parent != null)
					{
						treenodeParent = treenodeParent.Parent;
					}
					else
						break;
				}

				//Prepare fullpath of the selected node
				for (int i = stringParent.Count; i > 0; i--)
				{
					string parent = stringParent[i - 1] + ".";
					fullpath.Append(parent);
				}

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
			return fullpath.Append(treenode.Name).ToString();
		}

        public static string GetCaption(string evaluateString)
        {
            string caption = string.Empty;


            int index = evaluateString.LastIndexOf(Constants.CONST_COMMA_CHAR);
            if (index > -1)
            {
                string strClassName = evaluateString.Remove(0, index);

                string str = evaluateString.Remove(index);

                index = str.LastIndexOf(Constants.CONST_DOT_CHAR);
                caption = str.Remove(0, index + 1) + strClassName;
            }
            else
            {
                caption = evaluateString;
            }
            return caption;
        }
		public static void SetPicture(Assembly assembly, _CommandBarButton button, string resource, string masked)
		{
			using (Stream imageStream = assembly.GetManifestResourceStream(resource))
			{
				button.Picture = (StdPicture)PictureHost.IPictureDisp(Image.FromStream(imageStream));
#if !NET_4_0
				using (Stream imageStreamMask = assembly.GetManifestResourceStream(masked))
				{
                    button.Mask = (StdPicture)PictureHost.IPictureDisp(Image.FromStream(imageStreamMask));
				}
#endif
			}
		}
        public static void SetTabPicture(Window window, string iconResource)
        {
            window.SetTabPicture(Helper.GetResourceImage(iconResource));
        }
        public delegate void delPassData(long[] objectid);
		public static void CreateQueryResultToolwindow(long[] objectid)
		{
			
            QueryResultToolWindow = ViewBase.CreateToolWindow(Constants.QUERYRESULT, BaseClass, GetClassGUID(BaseClass));
			QueryResultToolWindow.IsFloating = false;
			QueryResultToolWindow.Linkable = false;
			if (QueryResultToolWindow.AutoHides)
			{
				QueryResultToolWindow.AutoHides = false;
			}
			QueryResultToolWindow.Visible = true;

			QueryResult queryResult = QueryResultToolWindow.Object as QueryResult;
			delPassData del = queryResult.Setobjectid;
			del(objectid);



		}

        public static void AddElementToAttributeGrid(dbDataGridView dbDataGridAttributes, string className, string fullpath)
        {
            dbDataGridAttributes.Rows.Add(1);
            int index = dbDataGridAttributes.Rows.Count - 1;
            dbDataGridAttributes.Rows[index].Cells[0].Value = fullpath;
            dbDataGridAttributes.Rows[index].Cells[0].Tag = className;
            dbDataGridAttributes.ClearSelection();
            dbDataGridAttributes.Rows[index].Cells[0].Selected = true;
            ProxyType type = AssemblyInspectorObject.DataType.ResolveType(className);
            string newclassName = type != null ? type.FullName : className;
            dbDataGridAttributes.Rows[index].Tag = newclassName;
        }

		public static void SaveDataIfRequired()
        {
           
            try
            {
                if (HashClassGUID != null)
                {
                    foreach (DictionaryEntry entry in HashClassGUID)
                    {
                        string winCaption = entry.Key.ToString();


                        string caption = GetCaption(winCaption);

                        dbDataGridView dataGridView = ListofModifiedObjects.Instance[winCaption] as dbDataGridView;
                        if (dataGridView != null)
                        {
                            ListofModifiedObjects.SaveBeforeWindowHiding(caption, dataGridView);
                            ListofModifiedObjects.Instance.Remove(winCaption);
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
        }
	}
   
}
