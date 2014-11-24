namespace OMControlLibrary
{
	partial class ObjectBrowser : ViewBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectBrowser));
			this.tableLayoutPanelObjectTreeView = new System.Windows.Forms.TableLayoutPanel();
			this.dbtreeviewObject = new OMControlLibrary.Common.dbTreeView();
			this.panelSearch = new System.Windows.Forms.Panel();
			this.toolStripFav = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonFolder = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPrevious = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonFlatView = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonAssemblyView = new System.Windows.Forms.ToolStripButton();
			this.toolStripSearch = new System.Windows.Forms.ToolStrip();
			this.toolStripComboBoxFilter = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripButtonFilter = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
			this.tableLayoutPanelObjectTreeView.SuspendLayout();
			this.panelSearch.SuspendLayout();
			this.toolStripFav.SuspendLayout();
			this.toolStripSearch.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanelObjectTreeView
			// 
			this.tableLayoutPanelObjectTreeView.AutoSize = true;
			this.tableLayoutPanelObjectTreeView.ColumnCount = 1;
			this.tableLayoutPanelObjectTreeView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelObjectTreeView.Controls.Add(this.dbtreeviewObject, 0, 1);
			this.tableLayoutPanelObjectTreeView.Controls.Add(this.panelSearch, 0, 0);
			this.tableLayoutPanelObjectTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanelObjectTreeView.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanelObjectTreeView.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanelObjectTreeView.Name = "tableLayoutPanelObjectTreeView";
			this.tableLayoutPanelObjectTreeView.RowCount = 2;
			this.tableLayoutPanelObjectTreeView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
			this.tableLayoutPanelObjectTreeView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelObjectTreeView.Size = new System.Drawing.Size(1240, 778);
			this.tableLayoutPanelObjectTreeView.TabIndex = 2;
			// 
			// dbtreeviewObject
			// 
			this.dbtreeviewObject.AllowDrop = true;
			this.dbtreeviewObject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dbtreeviewObject.Font = new System.Drawing.Font("Tahoma", 8F);
			this.dbtreeviewObject.FullRowSelect = true;
			this.dbtreeviewObject.HashtableAssmblyNodes = ((System.Collections.Hashtable)(resources.GetObject("dbtreeviewObject.HashtableAssmblyNodes")));
			this.dbtreeviewObject.HashtableClassNodes = ((System.Collections.Hashtable)(resources.GetObject("dbtreeviewObject.HashtableClassNodes")));
			this.dbtreeviewObject.LabelEdit = true;
			this.dbtreeviewObject.Location = new System.Drawing.Point(3, 59);
			this.dbtreeviewObject.Name = "dbtreeviewObject";
			this.dbtreeviewObject.Size = new System.Drawing.Size(1234, 716);
			this.dbtreeviewObject.TabIndex = 8;
			this.dbtreeviewObject.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.dbtreeviewObject_AfterCollapse);
			this.dbtreeviewObject.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.dbtreeviewObject_AfterSelect);
			this.dbtreeviewObject.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.dbtreeviewObject_AfterExpand);
			// 
			// panelSearch
			// 
			this.panelSearch.Controls.Add(this.toolStripFav);
			this.panelSearch.Controls.Add(this.toolStripSearch);
			this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSearch.Location = new System.Drawing.Point(3, 3);
			this.panelSearch.Name = "panelSearch";
			this.panelSearch.Size = new System.Drawing.Size(1234, 50);
			this.panelSearch.TabIndex = 7;
			// 
			// toolStripFav
			// 
			this.toolStripFav.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripFav.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFolder,
            this.toolStripButtonPrevious,
            this.toolStripButtonNext,
            this.toolStripSeparator2,
            this.toolStripButtonFlatView,
            this.toolStripButtonAssemblyView});
			this.toolStripFav.Location = new System.Drawing.Point(0, 0);
			this.toolStripFav.Name = "toolStripFav";
			this.toolStripFav.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripFav.Size = new System.Drawing.Size(1234, 25);
			this.toolStripFav.TabIndex = 7;
			this.toolStripFav.Text = "toolStrip1";
			// 
			// toolStripButtonFolder
			// 
			this.toolStripButtonFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonFolder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFolder.Image")));
			this.toolStripButtonFolder.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripButtonFolder.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonFolder.Name = "toolStripButtonFolder";
			this.toolStripButtonFolder.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonFolder.Text = "toolStripButton1";
			this.toolStripButtonFolder.ToolTipText = "Favourite New Folder";
			this.toolStripButtonFolder.Click += new System.EventHandler(this.toolStripButtonFolder_Click);
			// 
			// toolStripButtonPrevious
			// 
			this.toolStripButtonPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPrevious.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrevious.Image")));
			this.toolStripButtonPrevious.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripButtonPrevious.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonPrevious.Name = "toolStripButtonPrevious";
			this.toolStripButtonPrevious.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPrevious.Text = "toolStripButton2";
			this.toolStripButtonPrevious.ToolTipText = "Back";
			this.toolStripButtonPrevious.Click += new System.EventHandler(this.toolStripButtonPrevious_Click);
			// 
			// toolStripButtonNext
			// 
			this.toolStripButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonNext.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNext.Image")));
			this.toolStripButtonNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonNext.Name = "toolStripButtonNext";
			this.toolStripButtonNext.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonNext.Text = "toolStripButton3";
			this.toolStripButtonNext.ToolTipText = "Forward";
			this.toolStripButtonNext.Click += new System.EventHandler(this.toolStripButtonNext_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonFlatView
			// 
			this.toolStripButtonFlatView.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripButtonFlatView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonFlatView.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFlatView.Image")));
			this.toolStripButtonFlatView.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripButtonFlatView.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonFlatView.Name = "toolStripButtonFlatView";
			this.toolStripButtonFlatView.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonFlatView.Text = "toolStripButton1";
			this.toolStripButtonFlatView.Click += new System.EventHandler(this.toolStripButtonFlatView_Click);
			// 
			// toolStripButtonAssemblyView
			// 
			this.toolStripButtonAssemblyView.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripButtonAssemblyView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonAssemblyView.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAssemblyView.Image")));
			this.toolStripButtonAssemblyView.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripButtonAssemblyView.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonAssemblyView.Name = "toolStripButtonAssemblyView";
			this.toolStripButtonAssemblyView.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonAssemblyView.Text = "toolStripButton2";
			this.toolStripButtonAssemblyView.Click += new System.EventHandler(this.toolStripButtonAssemblyView_Click);
			// 
			// toolStripSearch
			// 
			this.toolStripSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStripSearch.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxFilter,
            this.toolStripButtonClear,
            this.toolStripButtonFilter});
			this.toolStripSearch.Location = new System.Drawing.Point(0, 25);
			this.toolStripSearch.Name = "toolStripSearch";
			this.toolStripSearch.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripSearch.Size = new System.Drawing.Size(1234, 25);
			this.toolStripSearch.TabIndex = 6;
			// 
			// toolStripComboBoxFilter
			// 
			this.toolStripComboBoxFilter.Name = "toolStripComboBoxFilter";
			this.toolStripComboBoxFilter.Size = new System.Drawing.Size(180, 25);
			this.toolStripComboBoxFilter.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxFilter_SelectedIndexChanged);
			this.toolStripComboBoxFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripComboBoxFilter_KeyDown);
			this.toolStripComboBoxFilter.TextChanged += new System.EventHandler(this.toolStripComboBoxFilter_TextChanged);
			this.toolStripComboBoxFilter.Click += new System.EventHandler(this.toolStripComboBoxFilter_Click);
			this.toolStripComboBoxFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripComboBoxFilter_KeyPress);
			// 
			// toolStripButtonFilter
			// 
			this.toolStripButtonFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripButtonFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonFilter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFilter.Image")));
			this.toolStripButtonFilter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripButtonFilter.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonFilter.Name = "toolStripButtonFilter";
			this.toolStripButtonFilter.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonFilter.Text = "toolStripButton1";
			this.toolStripButtonFilter.Click += new System.EventHandler(this.toolStripButtonFilter_Click);
			// 
			// toolStripButtonClear
			// 
			this.toolStripButtonClear.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonClear.Enabled = false;
			this.toolStripButtonClear.Font = new System.Drawing.Font("Tahoma", 8F);
			this.toolStripButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClear.Image")));
			this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonClear.Name = "toolStripButtonClear";
			this.toolStripButtonClear.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonClear.ToolTipText = "Clear";
			this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
			// 
			// ObjectBrowser
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.MenuBar;
			this.Controls.Add(this.tableLayoutPanelObjectTreeView);
			this.Name = "ObjectBrowser";
			this.Size = new System.Drawing.Size(1240, 778);
			this.Load += new System.EventHandler(this.ObjectBrowser_Load);
			this.tableLayoutPanelObjectTreeView.ResumeLayout(false);
			this.panelSearch.ResumeLayout(false);
			this.panelSearch.PerformLayout();
			this.toolStripFav.ResumeLayout(false);
			this.toolStripFav.PerformLayout();
			this.toolStripSearch.ResumeLayout(false);
			this.toolStripSearch.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelObjectTreeView;
		private System.Windows.Forms.Panel panelSearch;
		private System.Windows.Forms.ToolStrip toolStripSearch;
		private System.Windows.Forms.ToolStripButton toolStripButtonClear;
		private OMControlLibrary.Common.dbTreeView dbtreeviewObject;

		internal OMControlLibrary.Common.dbTreeView DbtreeviewObject
		{
			get { return dbtreeviewObject; }
			set { dbtreeviewObject = value; }
		}
		private System.Windows.Forms.ToolStripComboBox toolStripComboBoxFilter;
		private System.Windows.Forms.ToolStripButton toolStripButtonFilter;
		private System.Windows.Forms.ToolStrip toolStripFav;
		private System.Windows.Forms.ToolStripButton toolStripButtonFolder;
		private System.Windows.Forms.ToolStripButton toolStripButtonPrevious;
		private System.Windows.Forms.ToolStripButton toolStripButtonNext;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButtonFlatView;
		private System.Windows.Forms.ToolStripButton toolStripButtonAssemblyView;

		internal System.Windows.Forms.ToolStripButton ToolStripButtonAssemblyView
		{
			get { return toolStripButtonAssemblyView; }
			set { toolStripButtonAssemblyView = value; }
		}

	}
}
