namespace OMControlLibrary
{
	partial class QueryBuilder
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
            this.splitContainerQueryBuilder = new System.Windows.Forms.SplitContainer();
            this.splitContainerRecentQueryAndQueyBuilder = new System.Windows.Forms.SplitContainer();
            this.panelRecentQueries = new System.Windows.Forms.Panel();
            this.comboboxRecentQueries = new ToolTipComboBox();
            this.labelRecentQueries = new System.Windows.Forms.Label();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.panelQueryGrid = new System.Windows.Forms.Panel();
            this.panelAddQueryGroup = new System.Windows.Forms.Panel();
            this.buttonRunQuery = new System.Windows.Forms.Button();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.buttonAddQueryGroup = new System.Windows.Forms.Button();
            this.labelBuildQuery = new System.Windows.Forms.Label();
            this.splitContainerQueryBuilder.Panel1.SuspendLayout();
            this.splitContainerQueryBuilder.SuspendLayout();
            this.splitContainerRecentQueryAndQueyBuilder.Panel1.SuspendLayout();
            this.splitContainerRecentQueryAndQueyBuilder.Panel2.SuspendLayout();
            this.splitContainerRecentQueryAndQueyBuilder.SuspendLayout();
            this.panelRecentQueries.SuspendLayout();
            this.tableLayoutPanelTop.SuspendLayout();
            this.panelAddQueryGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerQueryBuilder
            // 
            this.splitContainerQueryBuilder.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerQueryBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerQueryBuilder.Location = new System.Drawing.Point(0, 0);
            this.splitContainerQueryBuilder.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainerQueryBuilder.Name = "splitContainerQueryBuilder";
            this.splitContainerQueryBuilder.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerQueryBuilder.Panel1
            // 
            this.splitContainerQueryBuilder.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerQueryBuilder.Panel1.Controls.Add(this.splitContainerRecentQueryAndQueyBuilder);
            this.splitContainerQueryBuilder.Panel1MinSize = 200;
            // 
            // splitContainerQueryBuilder.Panel2
            // 
            this.splitContainerQueryBuilder.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerQueryBuilder.Panel2.Margin = new System.Windows.Forms.Padding(1);
            this.splitContainerQueryBuilder.Panel2.Padding = new System.Windows.Forms.Padding(1);
            this.splitContainerQueryBuilder.Size = new System.Drawing.Size(655, 812);
            this.splitContainerQueryBuilder.SplitterDistance = 596;
            this.splitContainerQueryBuilder.SplitterWidth = 2;
            this.splitContainerQueryBuilder.TabIndex = 2;
            // 
            // splitContainerRecentQueryAndQueyBuilder
            // 
            this.splitContainerRecentQueryAndQueyBuilder.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerRecentQueryAndQueyBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRecentQueryAndQueyBuilder.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerRecentQueryAndQueyBuilder.IsSplitterFixed = true;
            this.splitContainerRecentQueryAndQueyBuilder.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRecentQueryAndQueyBuilder.Name = "splitContainerRecentQueryAndQueyBuilder";
            this.splitContainerRecentQueryAndQueyBuilder.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRecentQueryAndQueyBuilder.Panel1
            // 
            this.splitContainerRecentQueryAndQueyBuilder.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerRecentQueryAndQueyBuilder.Panel1.Controls.Add(this.panelRecentQueries);
            // 
            // splitContainerRecentQueryAndQueyBuilder.Panel2
            // 
            this.splitContainerRecentQueryAndQueyBuilder.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerRecentQueryAndQueyBuilder.Panel2.Controls.Add(this.tableLayoutPanelTop);
            this.splitContainerRecentQueryAndQueyBuilder.Size = new System.Drawing.Size(655, 596);
            this.splitContainerRecentQueryAndQueyBuilder.SplitterDistance = 40;
            this.splitContainerRecentQueryAndQueyBuilder.SplitterWidth = 2;
            this.splitContainerRecentQueryAndQueyBuilder.TabIndex = 3;
            // 
            // panelRecentQueries
            // 
            this.panelRecentQueries.Controls.Add(this.comboboxRecentQueries);
            this.panelRecentQueries.Controls.Add(this.labelRecentQueries);
            this.panelRecentQueries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRecentQueries.Location = new System.Drawing.Point(0, 0);
            this.panelRecentQueries.Margin = new System.Windows.Forms.Padding(0);
            this.panelRecentQueries.Name = "panelRecentQueries";
            this.panelRecentQueries.Size = new System.Drawing.Size(655, 40);
            this.panelRecentQueries.TabIndex = 6;
            // 
            // comboboxRecentQueries
            // 
            this.comboboxRecentQueries.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.comboboxRecentQueries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboboxRecentQueries.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboboxRecentQueries.Font = new System.Drawing.Font("Tahoma", 8F);
            this.comboboxRecentQueries.FormattingEnabled = true;
            this.comboboxRecentQueries.Location = new System.Drawing.Point(0, 19);
            this.comboboxRecentQueries.MaxDropDownItems = 5;
            this.comboboxRecentQueries.Name = "comboboxRecentQueries";
            this.comboboxRecentQueries.Size = new System.Drawing.Size(655, 21);
            this.comboboxRecentQueries.TabIndex = 1;
            this.comboboxRecentQueries.SelectedIndexChanged += new System.EventHandler(this.comboboxRecentQueries_SelectedIndexChanged);
            this.comboboxRecentQueries.Click += new System.EventHandler(this.comboboxRecentQueries_Click);
            // 
            // labelRecentQueries
            // 
            this.labelRecentQueries.AutoSize = true;
            this.labelRecentQueries.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRecentQueries.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelRecentQueries.Location = new System.Drawing.Point(0, 0);
            this.labelRecentQueries.Name = "labelRecentQueries";
            this.labelRecentQueries.Size = new System.Drawing.Size(103, 13);
            this.labelRecentQueries.TabIndex = 0;
            this.labelRecentQueries.Text = "Recent Queries 1";
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.AutoSize = true;
            this.tableLayoutPanelTop.ColumnCount = 1;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTop.Controls.Add(this.panelQueryGrid, 0, 1);
            this.tableLayoutPanelTop.Controls.Add(this.panelAddQueryGroup, 0, 2);
            this.tableLayoutPanelTop.Controls.Add(this.labelBuildQuery, 0, 0);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTop.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 3;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(655, 554);
            this.tableLayoutPanelTop.TabIndex = 3;
            // 
            // panelQueryGrid
            // 
            this.panelQueryGrid.AllowDrop = true;
            this.panelQueryGrid.AutoScroll = true;
            this.panelQueryGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelQueryGrid.Location = new System.Drawing.Point(0, 20);
            this.panelQueryGrid.Margin = new System.Windows.Forms.Padding(0);
            this.panelQueryGrid.Name = "panelQueryGrid";
            this.panelQueryGrid.Size = new System.Drawing.Size(655, 506);
            this.panelQueryGrid.TabIndex = 6;
            // 
            // panelAddQueryGroup
            // 
            this.panelAddQueryGroup.Controls.Add(this.buttonRunQuery);
            this.panelAddQueryGroup.Controls.Add(this.buttonClearAll);
            this.panelAddQueryGroup.Controls.Add(this.buttonAddQueryGroup);
            this.panelAddQueryGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAddQueryGroup.Location = new System.Drawing.Point(0, 526);
            this.panelAddQueryGroup.Margin = new System.Windows.Forms.Padding(0);
            this.panelAddQueryGroup.Name = "panelAddQueryGroup";
            this.panelAddQueryGroup.Size = new System.Drawing.Size(655, 28);
            this.panelAddQueryGroup.TabIndex = 7;
            // 
            // buttonRunQuery
            // 
            this.buttonRunQuery.Enabled = false;
            this.buttonRunQuery.Font = new System.Drawing.Font("Tahoma", 8F);
            this.buttonRunQuery.Location = new System.Drawing.Point(173, 2);
            this.buttonRunQuery.Name = "buttonRunQuery";
            this.buttonRunQuery.Padding = new System.Windows.Forms.Padding(1);
            this.buttonRunQuery.Size = new System.Drawing.Size(85, 24);
            this.buttonRunQuery.TabIndex = 10;
            this.buttonRunQuery.Text = "&Run Query";
            this.buttonRunQuery.UseVisualStyleBackColor = true;
            this.buttonRunQuery.Click += new System.EventHandler(this.buttonRunQuery_Click);
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.BackColor = System.Drawing.SystemColors.Control;
            this.buttonClearAll.Font = new System.Drawing.Font("Tahoma", 8F);
            this.buttonClearAll.Location = new System.Drawing.Point(88, 2);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(85, 24);
            this.buttonClearAll.TabIndex = 1;
            this.buttonClearAll.Text = "&Clear All";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // buttonAddQueryGroup
            // 
            this.buttonAddQueryGroup.BackColor = System.Drawing.SystemColors.Control;
            this.buttonAddQueryGroup.Font = new System.Drawing.Font("Tahoma", 8F);
            this.buttonAddQueryGroup.Location = new System.Drawing.Point(3, 2);
            this.buttonAddQueryGroup.Name = "buttonAddQueryGroup";
            this.buttonAddQueryGroup.Size = new System.Drawing.Size(85, 24);
            this.buttonAddQueryGroup.TabIndex = 0;
            this.buttonAddQueryGroup.Text = "&Add Group";
            this.buttonAddQueryGroup.UseVisualStyleBackColor = true;
            this.buttonAddQueryGroup.Click += new System.EventHandler(this.buttonAddQueryGroup_Click);
            // 
            // labelBuildQuery
            // 
            this.labelBuildQuery.BackColor = System.Drawing.SystemColors.Control;
            this.labelBuildQuery.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelBuildQuery.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelBuildQuery.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelBuildQuery.Location = new System.Drawing.Point(0, 0);
            this.labelBuildQuery.Margin = new System.Windows.Forms.Padding(0);
            this.labelBuildQuery.Name = "labelBuildQuery";
            this.labelBuildQuery.Size = new System.Drawing.Size(655, 20);
            this.labelBuildQuery.TabIndex = 8;
            this.labelBuildQuery.Text = "Build Query";
            this.labelBuildQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QueryBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.splitContainerQueryBuilder);
            this.Name = "QueryBuilder";
            this.Size = new System.Drawing.Size(655, 812);
            this.Load += new System.EventHandler(this.QueryBuilder_Load);
            this.Resize += new System.EventHandler(this.QueryBuilder_Resize);
            this.splitContainerQueryBuilder.Panel1.ResumeLayout(false);
            this.splitContainerQueryBuilder.ResumeLayout(false);
            this.splitContainerRecentQueryAndQueyBuilder.Panel1.ResumeLayout(false);
            this.splitContainerRecentQueryAndQueyBuilder.Panel2.ResumeLayout(false);
            this.splitContainerRecentQueryAndQueyBuilder.Panel2.PerformLayout();
            this.splitContainerRecentQueryAndQueyBuilder.ResumeLayout(false);
            this.panelRecentQueries.ResumeLayout(false);
            this.panelRecentQueries.PerformLayout();
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.panelAddQueryGroup.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainerQueryBuilder;
		private System.Windows.Forms.SplitContainer splitContainerRecentQueryAndQueyBuilder;
		private System.Windows.Forms.Panel panelRecentQueries;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTop;
		private System.Windows.Forms.Panel panelQueryGrid;
		private System.Windows.Forms.Panel panelAddQueryGroup;
		private System.Windows.Forms.Button buttonClearAll;
		private System.Windows.Forms.Button buttonAddQueryGroup;
		private System.Windows.Forms.Label labelBuildQuery;
		private System.Windows.Forms.Label labelRecentQueries;
		private ToolTipComboBox comboboxRecentQueries;
		private System.Windows.Forms.Button buttonRunQuery;
	}
}
