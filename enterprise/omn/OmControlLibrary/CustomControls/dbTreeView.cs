using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.TypeMauplation;
using OManager.BusinessLayer.FavFolder;
using OManager.BusinessLayer.UIHelper;
using OME.Logging.Common;

namespace OMControlLibrary.Common
{
	public partial class dbTreeView : TreeView
	{
		#region Member Variables

		private bool m_useInbuiltDragDrop = true;
		private readonly ImageList imageListDrag;
		private TreeNode dragNode;
		private readonly ImageList imageListTreeView;

		private Hashtable m_hashtableAssemblyNodes = new Hashtable();
		private Hashtable m_hashtableClassNodes = new Hashtable();

		private TreeNode m_PreviousTreeNode = new TreeNode();
		private TreeNode m_TreeNode = new TreeNode();
		private TreeNode treenode;

		private ContextMenuStrip m_tvViewObjectsContextMenuStrip;
		private ContextMenuStrip m_tvAddtoQueryContextMenuStrip;
		private ContextMenuStrip m_tvFavFolderContexMenu;
		private string folderName;
		//Events

		internal event EventHandler<DBContextItemClickedEventArg> OnContextMenuItemClicked;

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether drag drop in build feature is use to the tree view control. 
		/// [Default value is true].It is useful when you want to write your own drag drop event for the 
		/// tree view control. for the same value must be set to false.
		/// </summary>
		public bool UseInbuiltDragDrop
		{
			set { m_useInbuiltDragDrop = value; }
		}

		public Hashtable HashtableClassNodes
		{
			get { return m_hashtableClassNodes; }
			set { m_hashtableClassNodes = value; }
		}

		public Hashtable HashtableAssmblyNodes
		{
			get { return m_hashtableAssemblyNodes; }
			set { m_hashtableAssemblyNodes = value; }
		}

		#endregion

		#region Constructor

		public dbTreeView()
		{
			imageListDrag = new ImageList();
			imageListTreeView = new ImageList();
			AllowDrop = true;
			LabelEdit = true;
		}

		#endregion

		#region Event Handler

		/// <summary>
		/// DragEnter event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);

			try
			{
				DragHelper.ImageList_DragEnter(Handle, e.X - Left,
				                               e.Y - Top);

				e.Effect = DragDropEffects.Move;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// DrageOver event
		/// </summary>
		
		protected override void OnDragOver(DragEventArgs e)
		{
			try
			{
				base.OnDragOver(e);

				Point formP = PointToClient(new Point(e.X, e.Y));
				DragHelper.ImageList_DragMove(formP.X, formP.Y);

				e.Effect = DragDropEffects.Move;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			try
			{
				base.OnDragDrop(e);
				e.Effect = DragDropEffects.Move;
				Point pos = PointToClient(new Point(e.X, e.Y));
				TreeNode parentTreeNode = GetNodeAt(pos);
				if (parentTreeNode != null)
				{
					TreeNode dragNode = new TreeNode(Helper.FindRootNode(SelectedNode));
					dragNode.Tag = dragNode.Text;
					dragNode.Name = dragNode.Text;
					dragNode.ImageIndex =
						dragNode.SelectedImageIndex = 1;
					if (dragNode.Tag != null && dragNode.Tag.ToString() != "Fav Folder")
					{
						//dragNode.Tag = this.SelectedNode.Text;
						AddDummyChildNode(dragNode);

						bool checkSameNode = false;
						for (int i = 0; i < parentTreeNode.Nodes.Count; i++)
						{
							TreeNode tNode = parentTreeNode.Nodes[i];
							if (tNode.Text == dragNode.Text)
							{
								checkSameNode = true;
								break;
							}
						}
						if (checkSameNode == false && parentTreeNode.Tag != null && parentTreeNode.Tag.ToString() == "Fav Folder")
						{
							parentTreeNode.Nodes.Add(dragNode);
							FavouriteFolder Fav = new FavouriteFolder(null, parentTreeNode.Text);
							Fav.FolderName = parentTreeNode.Text;
							if (parentTreeNode.Nodes.Count > 0)
							{
								List<string> lststr = new List<string>();
								foreach (TreeNode tnode in parentTreeNode.Nodes)
								{
									lststr.Add(tnode.Text);
								}
								Fav.ListClass = lststr;
							}

							OMEInteraction.SaveFavourite( Fav);
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		protected override void OnAfterExpand(TreeViewEventArgs e)
		{
			base.OnAfterExpand(e);
			if (e.Node.Parent == null && e.Node.Tag != null && e.Node.Tag.ToString() == "Fav Folder")
			{
				e.Node.ImageIndex =
					e.Node.SelectedImageIndex = 4;
			}
		}


		protected override void OnAfterCollapse(TreeViewEventArgs e)
		{
			base.OnAfterCollapse(e);
			if (e.Node.Parent == null && e.Node.Tag != null && e.Node.Tag.ToString() == "Fav Folder")
			{
				e.Node.ImageIndex =
					e.Node.SelectedImageIndex = 5;
			}
		}


		/// <summary>
		/// ItemDrag Event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnItemDrag(ItemDragEventArgs e)
		{
            try
            {
               
                TreeNode tNode = ((TreeNode) e.Item);
                tNode.TreeView.SelectedNode = tNode;
                ((TreeNode) e.Item).TreeView.SelectedNode = tNode;
                if (tNode.Tag != null && tNode.Tag.ToString() == "Fav Folder" || tNode.Tag.ToString() == "Assembly View")
                {
                    DoDragDrop(e.Item, DragDropEffects.None);
                    return;
                }

				ProxyType  type = AssemblyInspectorObject.DataType.ResolveType(tNode.Tag.ToString());
                if (tNode.Nodes.Count == 0)
                {
                    if (!(type.IsEditable && (type.IsPrimitive || type.IsNullable)  )) 
                        
                    {
                        DoDragDrop(e.Item, DragDropEffects.None);
                        return;
                    }
                }

               
                    //// Get drag node and select it
                    dragNode = (TreeNode) e.Item;
                    SelectedNode = dragNode;

                    // Reset image list used for drag image
                    imageListDrag.Images.Clear();

                    //Check for the max image width 
                    int imageWidth = dragNode.Bounds.Width + Indent;
                    if (imageWidth > Constants.MAX_IMAGE_WIDTH)
                        imageWidth = Constants.MAX_IMAGE_WIDTH;

                    imageListDrag.ImageSize = new Size(imageWidth, dragNode.Bounds.Height);

                    // Create new bitmap
                    // This bitmap will contain the tree node image to be dragged


                    Bitmap bmp = new Bitmap(imageWidth, dragNode.Bounds.Height);

                    //// Get graphics from bitmap
                    Graphics gfx = Graphics.FromImage(bmp);

                    //// Draw node icon into the bitmap
                    gfx.DrawImage(ImageList.Images[dragNode.ImageIndex], 0, 0);

                    //// Draw node label into bitmap
                    gfx.DrawString(dragNode.Text,
                                   Font,
                                   new SolidBrush(ForeColor),
                                   Indent, 1.0f);

                    //// Add bitmap to imagelist
                    imageListDrag.Images.Add(bmp);

                    //// Get mouse position in client coordinates
                    Point p = PointToClient(MousePosition);

                    //// Compute delta between mouse position and node bounds
                    int dx = p.X + Indent - dragNode.Bounds.Left;
                    int dy = p.Y - dragNode.Bounds.Top;

                    //// Begin dragging image
                    if (DragHelper.ImageList_BeginDrag(imageListDrag.Handle, 0, dx, dy))
                    {
                        // Begin dragging
                        DoDragDrop(bmp, DragDropEffects.Move);
                        // End dragging image
                        DragHelper.ImageList_EndDrag();
                    }

                    DoDragDrop(e.Item, DragDropEffects.Copy);
                    base.OnItemDrag(e);
                
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			try
			{
				treenode = GetNodeAt(e.X, e.Y);
				if (treenode != null)
				{
					if (treenode.Tag != null && treenode.Tag.ToString() != "Assembly View")
					{
						base.OnMouseDown(e);
						if (treenode != null && e.Button == MouseButtons.Right)
						{
							SelectedNode = treenode;
							ContextMenuStrip = null;
                            if (treenode.Tag != null && treenode.Tag.ToString() != "Fav Folder")
                            {
                                if (m_hashtableClassNodes.Contains(treenode.Name) ||
                                    m_hashtableAssemblyNodes.Contains(treenode.Name) )
                                {
                                    ContextMenuStrip = m_tvViewObjectsContextMenuStrip;
                                }
                                else
                                {
                                    string typeOfObject = string.Empty;
                                    if (treenode.Tag != null)
                                    {
                                    	ProxyType  fieldType =AssemblyInspectorObject.DataType.ResolveType(treenode.Tag.ToString());
                                        if (fieldType.IsEditable)
                                        {
                                           
                                            ContextMenuStrip = m_tvAddtoQueryContextMenuStrip;
                                        }
                                    }
                                }
                            }
						    if (treenode.Tag != null && treenode.Tag.ToString() == "Fav Folder")
							{
								//queryBuilder.GetAllQueryGroups();
								BuildContextMenu(null, false, false);
								ContextMenuStrip = m_tvFavFolderContexMenu;
							}
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}


		protected override void OnKeyDown(KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Delete)
				{
					FavouriteFolder Fav = null;
					if (SelectedNode.Tag != null && SelectedNode.Tag.ToString() == "Fav Folder")
					{
						Fav = new FavouriteFolder(null, SelectedNode.Text);
						OMEInteraction.UpdateFavourite( Fav);
						Nodes.Remove(SelectedNode);
					}
					else if (SelectedNode.Parent != null && SelectedNode.Parent.Tag != null &&
					         SelectedNode.Parent.Tag.ToString() == "Fav Folder")
					{
						TreeNode tNode = SelectedNode;
						TreeNode parentNode = SelectedNode.Parent;


						if (parentNode.Nodes.Count > 0)
						{
							List<string> lststr = new List<string>();
							foreach (TreeNode tempNode in parentNode.Nodes)
							{
								lststr.Add(tempNode.Text);
							}
							Fav = new FavouriteFolder(lststr, tNode.Parent.Text);
						}
						OMEInteraction.SaveFavourite( Fav);
						SelectedNode.Remove();
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		protected void MainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				ToolStripItem tsItem = e.ClickedItem;

				if (tsItem.Tag != null)
				{
					DBContextItemClickedEventArg arg = new DBContextItemClickedEventArg(SelectedNode, e.ClickedItem.Tag);
					arg.Item = e.ClickedItem;

					if (OnContextMenuItemClicked != null)
						OnContextMenuItemClicked(sender, arg);

					if (ContextMenuStrip != null)
						ContextMenuStrip.Dispose();
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		protected void SubMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				DBContextItemClickedEventArg arg = new DBContextItemClickedEventArg(SelectedNode, e.ClickedItem.Tag);

				arg.Item = e.ClickedItem;
				if (OnContextMenuItemClicked != null)
					OnContextMenuItemClicked(sender, arg);

				ContextMenuStrip.Dispose();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Helper Methods

		public void SetTreeViewImages()
		{
			try
			{
				imageListTreeView.Images.Add(dbImages.TreeViewAssembly); //0 Assembly
				imageListTreeView.Images.Add(dbImages.TreeViewClass); //1 Classes
				imageListTreeView.Images.Add(dbImages.TreeViewPrimitive); //2 Primitive
				imageListTreeView.Images.Add(dbImages.TreeViewCollection); //3 Primitive
				imageListTreeView.Images.Add(dbImages.openFolder); // 4 open Folder
				imageListTreeView.Images.Add(dbImages.closedFolder); // 5 closed Folder

				ImageList = imageListTreeView;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		public void AddFavouritFolderFromDatabase()
		{
            try
            {
				long TimeforFavCreation =
				   OMEInteraction.GetTimeforFavCreation();
            	long TimeforDbCreation = AssemblyInspectorObject.Connection.DbCreationTime();
                if (TimeforFavCreation != 0)
                {
                if (TimeforFavCreation > TimeforDbCreation  )
                {
                	List<FavouriteFolder> lstfavFolder =OMEInteraction.GetFavourites();

                       

                    if (lstfavFolder != null)
                    {
                        foreach (FavouriteFolder fav in lstfavFolder)
                        {
                            if (fav != null)
                            {
                                TreeNode ParentFolder = new TreeNode(fav.FolderName);
                                ParentFolder.Name = fav.FolderName;
                                ParentFolder.Tag = "Fav Folder";
                                if (fav.ListClass != null)
                                {
                                    foreach (string str in fav.ListClass)
                                    {
                                        TreeNode child = new TreeNode(str);
                                        string strValue;
                                        int index = str.LastIndexOf(',');
                                        strValue = index == -1 ? str : str.Substring(0, index);
                                        child.Tag = strValue;
                                        child.Name = str;
                                        child.ImageIndex = child.SelectedImageIndex = 1;
                                        ParentFolder.Nodes.Add(child);

                                        AddDummyChildNode(child);
                                    }
                                }
                                ParentFolder.ImageIndex = ParentFolder.SelectedImageIndex = 5;
                                Nodes.Add(ParentFolder);
                            }
                        }
                    }
                }
                else

                {
                    OMEInteraction.RemoveFavFolder();
                }
                    }
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }
		}

		public void AddTreeNode(Hashtable classes, TreeNode treenodeparent)
		{
			try
			{
				if (classes == null)
				{
					return;
				}

				TreeNode treeNodeNew = null;

				if (treenodeparent == null)
					m_hashtableClassNodes.Clear();

				BeginUpdate();

				foreach (DictionaryEntry entry in classes)
				{
					Application.DoEvents();

					string nodevalue = entry.Key.ToString();
					string nodetype = entry.Value.ToString();

					if (!string.IsNullOrEmpty(nodevalue))
						treeNodeNew = new TreeNode(nodevalue);

					treeNodeNew.Name = nodevalue;
					treeNodeNew.Tag = nodetype;

					if (treenodeparent == null)
					{
						treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = 1;

						if (!HashtableClassNodes.ContainsKey(treeNodeNew.Name))
							HashtableClassNodes.Add(treeNodeNew.Name, treeNodeNew);

						Nodes.Add(treeNodeNew);
						AddDummyChildNode(treeNodeNew);
						continue;
					}
					ProxyType fieldType = AssemblyInspectorObject.DataType.ResolveType(nodetype);
                    treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = SetImageIndex(fieldType);

					treenodeparent.Nodes.Add(treeNodeNew);

                    if (!fieldType.IsEditable && !fieldType.FullName.EndsWith("mscorlib"))
					{
						if (AssemblyInspectorObject.Connection.GetFieldCount(ClassNameFor(treeNodeNew)) > 0)
							
						{
                            if (!fieldType.IsCollection  || !fieldType.IsArray )
							{
								AddDummyChildNode(treeNodeNew);
								treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = 1; //Classes;
							}
						}
						else
						{
                            if (fieldType.IsCollection || fieldType.IsArray)
							{
								treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = 3; //Classes;
							}
						}
					}
				}
			}

			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
			finally
			{
				EndUpdate();
			}
		}

		private static string ClassNameFor(TreeNode node)
		{
			return IsClass(node) ? node.Name : node.Tag.ToString();
		}

		private static bool IsClass(TreeNode node)
		{
			return node.Name.LastIndexOf(',') > 0;
		}

		private static int SetImageIndex(ProxyType   type)
		{
			int imageIndex = 0;

			try
			{
                if (type.IsEditable &&(type.IsPrimitive || type.IsNullable))	

				{
					imageIndex = 2;
				}
                else if (type.IsArray || type.IsCollection || (type.IsEditable && !type.IsPrimitive))
				{
					imageIndex = 3;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return imageIndex;
		}

		/// <summary>
		/// This functions adds the dummy node under the specified node
		/// </summary>
		/// <param name="treenodeParent">Node 
		/// under which the dummy node is to added</param>
		private static void AddDummyChildNode(TreeNode treenodeParent)
		{
			try
			{
				//Code to calculate number of child removed,
				//instead, when the group is expanded, childs are retrieved.
				if (treenodeParent != null)
				{
					TreeNode treenodeDummyChildNode = new TreeNode(Constants.DUMMY_NODE_TEXT);

					treenodeDummyChildNode.Name = Constants.DUMMY_NODE_TEXT;
					treenodeParent.Nodes.Add(treenodeDummyChildNode);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		public void PopulateAssemblyTreeView(Hashtable classes)
		{
			try
			{
				BeginUpdate();

				Nodes.Clear();
				HashtableAssmblyNodes.Clear();

				foreach (DictionaryEntry entry in classes)
				{
					string nodevalue = entry.Key.ToString();
					if (string.IsNullOrEmpty(nodevalue))
						return;

					TreeNode node = NewTreeNode(nodevalue, "Assembly View", 0);
					AddAssemblyClassesNodesTo(node, (List<string>) entry.Value);

					if (!m_hashtableAssemblyNodes.ContainsKey(node.Name))
						m_hashtableAssemblyNodes.Add(node.Name, node);

					Nodes.Add(node);
					node.Expand();
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
			finally
			{
				EndUpdate();
			}
		}

		private void AddAssemblyClassesNodesTo(TreeNode parent, IEnumerable<string> classes)
		{
			foreach (string className in classes)
			{
				TreeNode classNode = NewTreeNode(className, className, 1);
				if (!HashtableAssmblyNodes.ContainsKey(className))
					HashtableAssmblyNodes.Add(className, classNode);
                
				parent.Nodes.Add(classNode);
				AddDummyChildNode(classNode);
			}
		}

		private static TreeNode NewTreeNode(string name, string tag, int imageIndex)
		{
			TreeNode node = new TreeNode(name, imageIndex, imageIndex);
			node.Name = name;
			node.Tag = tag;

			return node;
		}

		public void AddFavoriteFolder()
		{
			TreeNode treeNodeNew = new TreeNode();
			List<string> favFolderList = new List<string>();
			int count = 0;
			try
			{
				for (int i = 0; i < Nodes.Count; i++)
				{
					TreeNode tNode = Nodes[i];
					if (tNode.Tag != null && tNode.Tag.ToString() == "Fav Folder")
					{
						count++;
						favFolderList.Add(tNode.Text);
					}
				}
				if (count > 0)
				{
					 
					bool checkUnique;
					treeNodeNew.Text = "New Folder " + count;
					while (true)
					{
						checkUnique = CheckUniqueFolderName(treeNodeNew, favFolderList, count);
						if (checkUnique == false)
						{
							count++;
							treeNodeNew.Text = "New Folder " + count;
						}
						else
						{
							break;
						}
					}
				}
				else
				{
					treeNodeNew.Text = "New Folder";
				}
				treeNodeNew.Tag = "Fav Folder";
				treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = 5;

				Nodes.Insert(0, treeNodeNew);
				SelectedNode = treeNodeNew;
				LabelEdit = true;
				SelectedNode.BeginEdit();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}


		private static bool CheckUniqueFolderName(TreeNode treeNodeNew, List<string> favFolderList, int count)
		{
			try
			{
				foreach (string str in favFolderList)
				{
					if (str == treeNodeNew.Text)
					{
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

		protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
		{
			try
			{
				base.OnBeforeLabelEdit(e);
				folderName = SelectedNode.Text;
				if (e.Node.Tag != null && e.Node.Tag.ToString() != "Fav Folder")
				{
					LabelEdit = false;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
		{
			try
			{
				base.OnAfterLabelEdit(e);
				if (!string.IsNullOrEmpty(e.Label))
				{
					bool checkSameNode = false;
					for (int i = 0; i < Nodes.Count; i++)
					{
						TreeNode tNode = Nodes[i];
						if (tNode.Text == e.Label)
						{
							checkSameNode = true;
							break;
						}
					}
					if (checkSameNode == false)
					{
						e.Node.Text = e.Label;
					}
					else
					{
						MessageBox.Show("The Foldername already exist, Please use some other name.",
						                Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.OK, MessageBoxIcon.Error);
						e.CancelEdit = true;
					    return;
					}
				}
				else if (e.Label == string.Empty)
				{
					MessageBox.Show("The Foldername cannot be empty.", Helper.GetResourceString(Constants.PRODUCT_CAPTION),
					                MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.CancelEdit = true;
				    return;
				}
				if (e.Node.Text != folderName)
				{
				    FavouriteFolder fav= OMEInteraction.GetFolderfromDatabaseByFoldername(folderName);
                    FavouriteFolder newFav = new FavouriteFolder(null, e.Node.Text);
                    if (fav != null)
                        OMEInteraction.RenameFolderInDatabase(fav, newFav);
                    else
                        OMEInteraction.SaveFavourite(newFav);
				}
				else
				{
					FavouriteFolder newFav = new FavouriteFolder(null, e.Node.Text);
					OMEInteraction.SaveFavourite( newFav);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}


		public void FindTreeNodesClasses(Hashtable list, TreeNode treenodeparent, string strToFind)
		{
			try
			{
				IDictionaryEnumerator enumerator = list.GetEnumerator();

				TreeNode treeNodeNew = null;

				if (Nodes.Count > 0)
					Nodes.Clear();

				m_hashtableClassNodes.Clear();
				BeginUpdate();

				while (enumerator.MoveNext())
				{
					string nodevalue = string.Empty;
					string nodetype = string.Empty;

					nodevalue = enumerator.Key.ToString();
					nodetype = enumerator.Value.ToString();

					if (nodevalue.ToLower().Contains(strToFind))
					{
						if (!string.IsNullOrEmpty(nodevalue))
							treeNodeNew = new TreeNode(nodevalue);

						treeNodeNew.Name = nodevalue;
						treeNodeNew.Tag = nodetype;

						if (treenodeparent == null)
						{
							treeNodeNew.ImageIndex =
								treeNodeNew.SelectedImageIndex = 1; //Classes

							if (!m_hashtableClassNodes.ContainsKey(treeNodeNew.Name))
							{
								m_hashtableClassNodes.Add(treeNodeNew.Name, treeNodeNew);
							}
							Nodes.Add(treeNodeNew);

							AddDummyChildNode(treeNodeNew);
							continue;
						}

						ProxyType fieldType = AssemblyInspectorObject.DataType.ResolveType(nodetype);
                        treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = SetImageIndex(fieldType);

					    treenodeparent.Nodes.Add(treeNodeNew);

                        if (!fieldType.IsEditable )
						{
							string param = string.Empty;

							if (treeNodeNew.Name.IndexOf(",") == -1)
								param = treeNodeNew.Tag.ToString();
							else
								param = treeNodeNew.Name;

							string className = string.Empty;

							if (treenodeparent.Name.LastIndexOf(',') > 0)
							{
								className = treenodeparent.Name;
							}
							else
							{
								className = treenodeparent.Tag.ToString();
							}
                            bool collection = AssemblyInspectorObject.DataType.CheckIfCollection(className, nodevalue);

                           
							if (AssemblyInspectorObject.Connection.GetFieldCount(param) > 0)
							{
								if (!collection)
								{
									AddDummyChildNode(treeNodeNew);
									treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = 1; //Classes;
								}
							}
							else
							{
                                if (collection )
								{
									treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = 3; //Classes;
								}
							}
						}
					}
				}
			}

			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
			finally
			{
				//this.Sort();
				EndUpdate();
			}
		}

		public void FindTreeNodesAssemblyView(Hashtable list, string strToFind)
		{
			TreeNode treeNodeNew = null;
		    bool CheckEntries = false;
			string nodevalue = string.Empty;
			string nodetype = string.Empty;

			try
			{
				IDictionaryEnumerator enumerator =
					list.GetEnumerator();

				BeginUpdate();
				Nodes.Clear();
				m_hashtableAssemblyNodes.Clear();

				while (enumerator.MoveNext())
				{
					
					List<string> classes = (List<string>) enumerator.Value;

				 
					if (classes.Count > 0)
					{
						for (int i = 0; i < classes.Count; i++)
						{
							TreeNode newClassesTreeNodes = new TreeNode(classes[i]);
							newClassesTreeNodes.Name = classes[i];
							if (newClassesTreeNodes.Name.ToLower().Contains(strToFind))
							{
                                if (CheckEntries == false)
                                {
                                    nodevalue = enumerator.Key.ToString();
                                   

                                    if (!string.IsNullOrEmpty(nodevalue))
                                        treeNodeNew = new TreeNode(nodevalue);
                                    else
                                        return;
                                    treeNodeNew.Name = nodevalue;
                                    treeNodeNew.Tag = "Assembly View";
                                    CheckEntries = true;
                                }
							    newClassesTreeNodes.Tag = classes[i];
								newClassesTreeNodes.ImageIndex =
									newClassesTreeNodes.SelectedImageIndex = 1; //Classes

								if (!m_hashtableAssemblyNodes.ContainsKey(newClassesTreeNodes.Name))
									m_hashtableAssemblyNodes.Add(newClassesTreeNodes.Name, newClassesTreeNodes);

								treeNodeNew.Nodes.Add(newClassesTreeNodes);
								AddDummyChildNode(newClassesTreeNodes);
							}
						}
					}

				    if (treeNodeNew != null && CheckEntries ==true)
				    {
				        treeNodeNew.ImageIndex = treeNodeNew.SelectedImageIndex = 0; //Assembly
				        Nodes.Add(treeNodeNew);
				        treeNodeNew.Expand();
				    }
				    CheckEntries = false;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
			finally
			{
				EndUpdate();
			}
		}


		public void FindNSelectNode(TreeNode Node, string className, dbTreeView dbtree)
		{
			try
			{
			    string strClassName = string.Empty;
				//string classNameWithoutAssembly = className;
                int index = className.LastIndexOf(',');
                if (index > -1)
                {
                    strClassName = className.Remove(index);
                }
                else
                {

                    strClassName = className;
                }
			    //bool foundNode = false;
				if (Node != null && Node.Tag != null)
				{
					if (Node.Tag.Equals(className) || Node.Tag.Equals(strClassName))
					{
						dbtree.SelectedNode = Node;
						return;
					}
					else
					{
						if (Node.Parent != null
						    && Node.Parent.Tag != null)
						{
							if (Node.Nodes.Count >= 1)
							{
								for (int i = 0; i < Node.Nodes.Count; i++)
								{
									if (Node.Nodes[i].Nodes.Count > 0)
									{
										if (Node.Nodes[i].Tag.Equals(className) || Node.Nodes[i].Tag.Equals(strClassName))
										{
											dbtree.SelectedNode = Node.Nodes[i];
											//foundNode = true;
											return;
										}
										else
										{
											FindNSelectNode(Node.Nodes[i], className, dbtree);
										}
									}
								}
							}
						}
						if (Node.Tag.ToString() != "Assembly View")
							Node = Node.NextNode;
						else
						{
							if (Node.Nodes != null && Node.Nodes.Count > 0)
								Node = Node.Nodes[0];
						}
						FindNSelectNode(Node, className, dbtree);
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Heightlights the selected node.
		/// </summary>
		public void UpdateTreeNodeSelection(TreeNode selectedNode, bool isAssemblyView)
		{
			TreeNode parentNode;
			Hashtable m_htnodes;
			try
			{
				if (selectedNode == null)
					return;

				//Check whether tree node with given tag exists.
				m_htnodes = isAssemblyView ? HashtableAssmblyNodes : HashtableClassNodes;

				parentNode = selectedNode.Parent;
				string selectedNodeName = selectedNode.Text;
				while (parentNode != null && parentNode.Tag != null
				       && parentNode.Tag.ToString() != "Assembly View" && parentNode.Tag.ToString() != "Fav Folder")
				{
					selectedNodeName = parentNode.Name;
					parentNode = parentNode.Parent;
				}

				UnHighlightAllFavFolders();
				if (parentNode != null && parentNode.Tag != null && parentNode.Tag.ToString() != "Assembly View")
				{
					HighlightFavFolder(selectedNodeName, parentNode);
				}
				else
				{
					if (m_htnodes.Contains(selectedNodeName))
					{
						//get the treenode to be selected.
						m_TreeNode = m_htnodes[selectedNodeName] as TreeNode;
						if (m_TreeNode != null)
						{
							m_TreeNode.Text.Trim();
							SelectedNode = selectedNode.Name.LastIndexOf(",") != -1 ? m_TreeNode : selectedNode;

							m_TreeNode.Text += "            ";
							Font fontTree = new Font(Font.Name, Font.Size, FontStyle.Bold);
							m_TreeNode.NodeFont = fontTree;
							m_TreeNode.Text = m_TreeNode.Text.Trim();
						}
						//Check if previously selected treenode is not null.
						if (m_PreviousTreeNode != null && m_PreviousTreeNode != m_TreeNode)
						{
							//Set previously selected treenode as regular font.
							Font preFont = new Font(Font.Name, Font.Size, FontStyle.Regular);
							m_PreviousTreeNode.NodeFont = preFont;
							m_PreviousTreeNode = m_TreeNode;
							m_PreviousTreeNode.Text = m_PreviousTreeNode.Text.Trim();
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void UnHighlightAllFavFolders()
		{
			try
			{
				foreach (TreeNode node in Nodes)
				{
					if (node.Tag != null && node.Tag.ToString() == "Fav Folder")
					{
						foreach (TreeNode tNode in node.Nodes)
						{
							Font preFont = new Font(Font.Name, Font.Size, FontStyle.Regular);
							tNode.NodeFont = preFont;
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void HighlightFavFolder(string selectedNodeName, TreeNode parentNode)
		{
			try
			{
				if (parentNode.Tag != null && parentNode.Tag.ToString() == "Fav Folder")
				{
					string FavFol = parentNode.Text;
					foreach (TreeNode node in Nodes)
					{
						if (node.Text == FavFol)
						{
							foreach (TreeNode tNode in node.Nodes)
							{
								if (tNode.Text == selectedNodeName)
								{
									Font fontTree = new Font(Font.Name, Font.Size, FontStyle.Bold);
									tNode.NodeFont = fontTree;
									tNode.Text = tNode.Text.Trim();
								}
								else
								{
									Font preFont = new Font(Font.Name, Font.Size, FontStyle.Regular);
									tNode.NodeFont = preFont;
								}
							}
						}
						else
						{
							Font preFont = new Font(Font.Name, Font.Size, FontStyle.Regular);
							node.NodeFont = preFont;
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		public void BuildContextMenu(List<string> contextmenulist, bool FavChild, bool showDeleteClass)
		{
			string menuName;

			ToolStripMenuItem objSubMenu;
			ToolStripMenuItem objMainMenu;
			ToolStripMenuItem objMainMenu1;
			try
			{
				if (FavChild == false)
				{
					m_tvFavFolderContexMenu = new ContextMenuStrip();

					m_tvFavFolderContexMenu.Name = Name;
					objMainMenu = new ToolStripMenuItem("Rename");
					objMainMenu.Name = "Rename";
					objMainMenu.Tag = "Rename";
					m_tvFavFolderContexMenu.Items.Add(objMainMenu);

					objMainMenu1 = new ToolStripMenuItem("Delete Folder");
					objMainMenu1.Name = "Delete Folder";
					objMainMenu1.Tag = "Delete Folder";
					m_tvFavFolderContexMenu.Items.Add(objMainMenu1);
					m_tvFavFolderContexMenu.ItemClicked += MainMenu_ItemClicked;
					m_tvFavFolderContexMenu.Opening += ContextMenuStrip_Opening;
				}
				else
				{
					m_tvViewObjectsContextMenuStrip = new ContextMenuStrip();
					m_tvAddtoQueryContextMenuStrip = new ContextMenuStrip();
					m_tvViewObjectsContextMenuStrip.Name = Name;

					if (contextmenulist != null)
					{
						menuName = Constants.CONTEXT_MENU_ADD_TO_ATTRIBUTE;
						objMainMenu = new ToolStripMenuItem(Helper.GetResourceString(Constants.CONTEXT_MENU_ADD_TO_ATTRIBUTE));
						objMainMenu.Name = menuName;
						objMainMenu.Tag = menuName;

						m_tvAddtoQueryContextMenuStrip.Items.Add(objMainMenu);

						objMainMenu = new ToolStripMenuItem(Helper.GetResourceString(Constants.CONTEXT_MENU_ADD_TO_QUERY));
						objMainMenu.Name = Constants.CONTEXT_MENU_ADD_TO_QUERY;
						objMainMenu.Tag = null;

						for (int i = 0; i < contextmenulist.Count; i++)
						{
							objSubMenu = new ToolStripMenuItem();
							objSubMenu.Text = contextmenulist[i];
							objSubMenu.Name = Constants.CONTEXT_MENU_EXPRESSION_GROUP + i;
							objSubMenu.Tag = Constants.CONTEXT_MENU_EXPRESSION_GROUP;
							objMainMenu.DropDownItems.Add(objSubMenu);
						}
						m_tvAddtoQueryContextMenuStrip.Items.Add(objMainMenu);


						objMainMenu.DropDownItemClicked += SubMenu_DropDownItemClicked;
						m_tvAddtoQueryContextMenuStrip.ItemClicked += MainMenu_ItemClicked;
						m_tvAddtoQueryContextMenuStrip.Opening += ContextMenuStrip_Opening;
					}
					else
					{
						objMainMenu = new ToolStripMenuItem(Helper.GetResourceString(Constants.CONTEXT_MENU_SHOW_ALL_OBJECTS));
						objMainMenu.Name = Constants.CONTEXT_MENU_SHOW_ALL_OBJECTS;
						objMainMenu.Tag = Constants.CONTEXT_MENU_SHOW_ALL_OBJECTS;
						m_tvViewObjectsContextMenuStrip.Items.Add(objMainMenu);
						if (showDeleteClass)
						{
							objMainMenu1 = new ToolStripMenuItem("Delete Class");
							objMainMenu1.Name = "Delete Class";
							objMainMenu1.Tag = "Delete Class";
							m_tvViewObjectsContextMenuStrip.Items.Add(objMainMenu1);
						}
						m_tvViewObjectsContextMenuStrip.ItemClicked += MainMenu_ItemClicked;
						m_tvViewObjectsContextMenuStrip.Opening += ContextMenuStrip_Opening;
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				if (treenode == null || treenode.Tag.ToString() == "Assembly View")
				{
					e.Cancel = true;
					return;
				}

				if (treenode.Tag != null)
				{
					string selectedNodeName = string.Empty;

					if (SelectedNode != null)
					{
                        if (SelectedNode.Name.LastIndexOf(",") == -1 && SelectedNode.Name.LastIndexOf(".") == -1)
						{
							TreeNode parentNode = SelectedNode.Parent;

							while (parentNode != null && parentNode.Tag != null && parentNode.Tag.ToString() != "Fav Folder" &&
							       parentNode.Tag.ToString() != "Assembly View")
							{
								selectedNodeName = parentNode.Name;
								parentNode = parentNode.Parent;
							}

							if (SelectedNode.Tag.ToString() != "Fav Folder")
							{
								if (Helper.HashTableBaseClass.Count > 0 && !Helper.HashTableBaseClass.Contains(selectedNodeName))
									e.Cancel = true;
							}
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion
	}


	/// <summary>
	/// DBContextItemClickedEventArg : For handling the contextmenu click
	/// </summary>
	public class DBContextItemClickedEventArg : EventArgs
	{
		private object m_data;
		private object m_tag;
		private object m_item;

		public object Item
		{
			get { return m_item; }
			set { m_item = value; }
		}

		public object Tag
		{
			get { return m_tag; }
			set { m_tag = value; }
		}
		public object Data
		{
			get { return m_data; }
			set { m_data = value; }
		}

		public DBContextItemClickedEventArg(object data, object tag)
		{
			m_data = data;
			m_tag = tag;
		}

		public DBContextItemClickedEventArg()
		{
		}
	}

	/// <summary>
	/// Class provides the effect of draging a item of treeview
	/// </summary>
	public class DragHelper
	{
		[DllImport("comctl32.dll")]
		public static extern bool InitCommonControls();

		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImageList_BeginDrag(IntPtr himlTrack, int
		                                                                	iTrack, int dxHotspot, int dyHotspot);

		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImageList_DragMove(int x, int y);

		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern void ImageList_EndDrag();

		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);

		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImageList_DragLeave(IntPtr hwndLock);

		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImageList_DragShowNolock(bool fShow);

		static DragHelper()
		{
			InitCommonControls();
		}
	}
}