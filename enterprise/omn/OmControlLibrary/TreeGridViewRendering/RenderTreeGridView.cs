using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.DataPopulation;
using OManager.BusinessLayer.Common;
using OManager.BusinessLayer.UIHelper;
using OMControlLibrary.Common;
using OME.AdvancedDataGridView;
using OME.Logging.Common;
namespace OMControlLibrary.TreeGridViewRendering
{
	
	public class RenderTreeGridView
	{
		private ImageList m_imageListTreeGrid;
		private TreeGridView treegrid;
		public TreeGridView RenderTreeGridViewDetails(long id, string classname)
		{
			InitializeImageList();
			treegrid = InitializeTreeGridView();
			bool readOnly=OMEInteraction.GetCurrentConnParams().ConnectionReadOnly;
			ProxyTreeGridRenderer item = AssemblyInspectorObject.DataPopulation.GetTreeGridViewDetails(readOnly,id, classname);
			TreeGridNode rootNode = new TreeGridNode();
			treegrid.Nodes.Add(rootNode);
			PopulateNode(rootNode, item);
			rootNode.Expand();
			rootNode.ImageIndex = 0;
			List<ProxyTreeGridRenderer> proxyList = AssemblyInspectorObject.DataPopulation.TransverseTreeGridViewDetails(readOnly,id,classname);
            foreach (ProxyTreeGridRenderer item1 in proxyList)
			{
				PopulateTreeGridNode(rootNode, item1);
			}

			return treegrid;
		}

		private void PopulateNode(TreeGridNode rootNode, ProxyTreeGridRenderer item)
		{
		    rootNode.Cells[0].Value = item.DisplayFieldName;
            rootNode.Cells[0].Tag  = item.QualifiedName ;
			rootNode.Cells[1].Value = item.FieldValue;
            rootNode.Cells[1].Tag = item.FieldName ;
			rootNode.Cells[2].Value = item.FieldType;
			rootNode.Cells[2].Tag = item.ObjectType;
			rootNode.Tag = item.ObjectId;
			rootNode.Cells[1].ReadOnly = item.ReadOnlyStatus;
		}


	

		public void AddNodesToTreeview(TreeGridNode node, bool activate)
		{
			List<ProxyTreeGridRenderer> proxyList =
				AssemblyInspectorObject.DataPopulation.ExpandTreeGidNode
					(OMEInteraction.GetCurrentConnParams().ConnectionReadOnly,
					 node.Tag, activate);
			if (proxyList == null)
				return;

			foreach (ProxyTreeGridRenderer item1 in proxyList)
			{
				PopulateTreeGridNode(node, item1);
			}
		}

		private void PopulateTreeGridNode(TreeGridNode rootNode, ProxyTreeGridRenderer NodeDetails)
		{
			TreeGridNode node = new TreeGridNode();
			rootNode.Nodes.Add(node);
			PopulateNode(node, NodeDetails);
			node.ImageIndex = 0;
			node.Collapse();
			if (NodeDetails.HasSubNode || NodeDetails.ObjectId != 0)
			{
				TreeGridNode treenodeDummyChildNode = new TreeGridNode();
				node.Nodes.Add(treenodeDummyChildNode);
				treenodeDummyChildNode.Cells[0].Value = BusinessConstants.DB4OBJECTS_DUMMY;
				if (NodeDetails.HasSubNode && NodeDetails.ObjectId == 0)
					node.Tag = NodeDetails.SubObject;
			}
		}

        private void InitializeImageList()
        {
            try
            {
                m_imageListTreeGrid = new ImageList();
                m_imageListTreeGrid.Images.Add(dbImages.TreeViewClass); //0 Class
                m_imageListTreeGrid.Images.Add(dbImages.TreeViewCollection); //1 Collections 
                m_imageListTreeGrid.Images.Add(dbImages.TreeViewPrimitive); //2 Primitive
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
        }

        private TreeGridView InitializeTreeGridView()
        {
            try
            {
                //TreeGridView Initialization
                TreeGridView treeGridView = new TreeGridView();
                treeGridView.Size = new Size(530, 442);
                treeGridView.Location = new Point(2, 2);
                treeGridView.Name = BusinessConstants.DB4OBJECTS_TREEGRIDVIEW;
                treeGridView.RowHeadersVisible = false;
                treeGridView.ShowLines = true;
                treeGridView.Dock = DockStyle.Fill;
                treeGridView.Visible = true;
                treeGridView.AllowDrop = true;

                //Column Intialization

                //Field Column
                TreeGridColumn m_fieldColumn = new TreeGridColumn();
                m_fieldColumn.DefaultNodeImage = null;
                m_fieldColumn.FillWeight = 386.9562F;
                m_fieldColumn.HeaderText = BusinessConstants.DB4OBJECTS_FIELD;
                m_fieldColumn.Name = BusinessConstants.DB4OBJECTS_FIELDCOLOUMN;
                m_fieldColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                m_fieldColumn.ReadOnly = true;
                m_fieldColumn.Width = 170;

                //Value Column
                TreeGridViewDateTimePickerColumn m_valueColumn = new TreeGridViewDateTimePickerColumn();
                m_valueColumn.FillWeight = 50F;
                m_valueColumn.HeaderText = BusinessConstants.DB4OBJECTS_VALUEFORGRID;
                m_valueColumn.Name = BusinessConstants.DB4OBJECTS_VALUECOLUMN;
                m_valueColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                m_valueColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //	m_valueColumn.ReadOnly = AssemblyInspectorObject.Connection.GetCurrentRecentConnectionReadOnlyStatus(); 

                //Type Column
                DataGridViewTextBoxColumn m_typeColumn = new DataGridViewTextBoxColumn();
                m_typeColumn.FillWeight = 50F;
                m_typeColumn.HeaderText = BusinessConstants.DB4OBJECTS_TYPE;
                m_typeColumn.Name = BusinessConstants.DB4OBJECTS_TYPECOLUMN;
                m_typeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                m_typeColumn.ReadOnly = true;
                m_typeColumn.Width = 150;

                treeGridView.Columns.AddRange(new DataGridViewColumn[] { m_fieldColumn, m_valueColumn, m_typeColumn });

                treeGridView.ImageList = m_imageListTreeGrid;
                treeGridView.ScrollBars = ScrollBars.Both;

                return treeGridView;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }

            return null;
        }
	}
}
