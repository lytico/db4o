namespace OMControlLibrary
{
	partial class QueryResult
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
			this.components = new System.ComponentModel.Container();
			this.splitContainerLeftPane = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanelResultGrid = new System.Windows.Forms.TableLayoutPanel();
			this.panelResultGridOptions = new System.Windows.Forms.Panel();
			this.panelRight = new System.Windows.Forms.Panel();
			this.labelNoOfObjects = new System.Windows.Forms.Label();
			this.lblFechedObjects = new System.Windows.Forms.Label();
			this.lblPageCount = new System.Windows.Forms.Label();
			this.btnLast = new System.Windows.Forms.Button();
			this.btnFirst = new System.Windows.Forms.Button();
			this.lblof = new System.Windows.Forms.Label();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.txtCurrentPage = new System.Windows.Forms.TextBox();
			this.btnNext = new System.Windows.Forms.Button();
			this.panelLeft = new System.Windows.Forms.Panel();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.tableLayoutPanelResult = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonSaveResult = new System.Windows.Forms.Button();
			this.toolTipPagging = new System.Windows.Forms.ToolTip(this.components);
			this.splitContainerLeftPane.Panel1.SuspendLayout();
			this.splitContainerLeftPane.Panel2.SuspendLayout();
			this.splitContainerLeftPane.SuspendLayout();
			this.tableLayoutPanelResultGrid.SuspendLayout();
			this.panelResultGridOptions.SuspendLayout();
			this.panelRight.SuspendLayout();
			this.panelLeft.SuspendLayout();
			this.tableLayoutPanelResult.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainerLeftPane
			// 
			this.splitContainerLeftPane.BackColor = System.Drawing.SystemColors.ControlDark;
			this.splitContainerLeftPane.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerLeftPane.Location = new System.Drawing.Point(0, 0);
			this.splitContainerLeftPane.Name = "splitContainerLeftPane";
			this.splitContainerLeftPane.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerLeftPane.Panel1
			// 
			this.splitContainerLeftPane.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainerLeftPane.Panel1.Controls.Add(this.tableLayoutPanelResultGrid);
			// 
			// splitContainerLeftPane.Panel2
			// 
			this.splitContainerLeftPane.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainerLeftPane.Panel2.Controls.Add(this.tableLayoutPanelResult);
			this.splitContainerLeftPane.Size = new System.Drawing.Size(1201, 809);
			this.splitContainerLeftPane.SplitterDistance = 364;
			this.splitContainerLeftPane.SplitterWidth = 2;
			this.splitContainerLeftPane.TabIndex = 2;
			// 
			// tableLayoutPanelResultGrid
			// 
			this.tableLayoutPanelResultGrid.BackColor = System.Drawing.SystemColors.Control;
			this.tableLayoutPanelResultGrid.ColumnCount = 1;
			this.tableLayoutPanelResultGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelResultGrid.Controls.Add(this.panelResultGridOptions, 0, 1);
			this.tableLayoutPanelResultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanelResultGrid.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
			this.tableLayoutPanelResultGrid.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanelResultGrid.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanelResultGrid.MinimumSize = new System.Drawing.Size(470, 0);
			this.tableLayoutPanelResultGrid.Name = "tableLayoutPanelResultGrid";
			this.tableLayoutPanelResultGrid.RowCount = 2;
			this.tableLayoutPanelResultGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelResultGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tableLayoutPanelResultGrid.Size = new System.Drawing.Size(1201, 364);
			this.tableLayoutPanelResultGrid.TabIndex = 0;
			// 
			// panelResultGridOptions
			// 
			this.panelResultGridOptions.AutoSize = true;
			this.panelResultGridOptions.Controls.Add(this.panelRight);
			this.panelResultGridOptions.Controls.Add(this.panelLeft);
			this.panelResultGridOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelResultGridOptions.Location = new System.Drawing.Point(3, 335);
			this.panelResultGridOptions.Name = "panelResultGridOptions";
			this.panelResultGridOptions.Size = new System.Drawing.Size(1195, 26);
			this.panelResultGridOptions.TabIndex = 1;
			this.panelResultGridOptions.SizeChanged += new System.EventHandler(this.panelResultGridOptions_SizeChanged);
			// 
			// panelRight
			// 
			this.panelRight.Controls.Add(this.labelNoOfObjects);
			this.panelRight.Controls.Add(this.lblFechedObjects);
			this.panelRight.Controls.Add(this.lblPageCount);
			this.panelRight.Controls.Add(this.btnLast);
			this.panelRight.Controls.Add(this.btnFirst);
			this.panelRight.Controls.Add(this.lblof);
			this.panelRight.Controls.Add(this.btnPrevious);
			this.panelRight.Controls.Add(this.txtCurrentPage);
			this.panelRight.Controls.Add(this.btnNext);
			this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelRight.Location = new System.Drawing.Point(712, 0);
			this.panelRight.Name = "panelRight";
			this.panelRight.Size = new System.Drawing.Size(483, 26);
			this.panelRight.TabIndex = 24;
			// 
			// labelNoOfObjects
			// 
			this.labelNoOfObjects.AutoSize = true;
			this.labelNoOfObjects.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelNoOfObjects.Location = new System.Drawing.Point(133, 4);
			this.labelNoOfObjects.Name = "labelNoOfObjects";
			this.labelNoOfObjects.Size = new System.Drawing.Size(13, 13);
			this.labelNoOfObjects.TabIndex = 25;
			this.labelNoOfObjects.Text = "0";
			// 
			// lblFechedObjects
			// 
			this.lblFechedObjects.AutoSize = true;
			this.lblFechedObjects.Font = new System.Drawing.Font("Tahoma", 8F);
			this.lblFechedObjects.Location = new System.Drawing.Point(3, 4);
			this.lblFechedObjects.Name = "lblFechedObjects";
			this.lblFechedObjects.Size = new System.Drawing.Size(131, 13);
			this.lblFechedObjects.TabIndex = 24;
			this.lblFechedObjects.Text = "No. of Objects Retrieved:";
			// 
			// lblPageCount
			// 
			this.lblPageCount.AutoSize = true;
			this.lblPageCount.Font = new System.Drawing.Font("Tahoma", 8F);
			this.lblPageCount.Location = new System.Drawing.Point(291, 4);
			this.lblPageCount.Name = "lblPageCount";
			this.lblPageCount.Size = new System.Drawing.Size(13, 13);
			this.lblPageCount.TabIndex = 23;
			this.lblPageCount.Text = "0";
			// 
			// btnLast
			// 
			this.btnLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
			this.btnLast.Location = new System.Drawing.Point(451, 0);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new System.Drawing.Size(31, 21);
			this.btnLast.TabIndex = 20;
			this.btnLast.Text = ">>";
			this.btnLast.UseVisualStyleBackColor = true;
			this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
			// 
			// btnFirst
			// 
			this.btnFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
			this.btnFirst.Location = new System.Drawing.Point(347, 0);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(31, 21);
			this.btnFirst.TabIndex = 17;
			this.btnFirst.Text = "<<";
			this.btnFirst.UseVisualStyleBackColor = true;
			this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
			// 
			// lblof
			// 
			this.lblof.AutoSize = true;
			this.lblof.Font = new System.Drawing.Font("Tahoma", 8F);
			this.lblof.Location = new System.Drawing.Point(269, 4);
			this.lblof.Name = "lblof";
			this.lblof.Size = new System.Drawing.Size(24, 13);
			this.lblof.TabIndex = 22;
			this.lblof.Text = "of :";
			// 
			// btnPrevious
			// 
			this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
			this.btnPrevious.Location = new System.Drawing.Point(384, 0);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(31, 21);
			this.btnPrevious.TabIndex = 18;
			this.btnPrevious.Text = "&<";
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// txtCurrentPage
			// 
			this.txtCurrentPage.Font = new System.Drawing.Font("Tahoma", 8F);
			this.txtCurrentPage.Location = new System.Drawing.Point(209, 1);
			this.txtCurrentPage.MaxLength = 6;
			this.txtCurrentPage.Name = "txtCurrentPage";
			this.txtCurrentPage.Size = new System.Drawing.Size(54, 20);
			this.txtCurrentPage.TabIndex = 21;
			this.txtCurrentPage.TextChanged += new System.EventHandler(this.txtObjectNumber_TextChanged);
			this.txtCurrentPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtObjectNumber_KeyDown);
			this.txtCurrentPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtObjectNumber_KeyPress);
			// 
			// btnNext
			// 
			this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
			this.btnNext.Location = new System.Drawing.Point(419, 0);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(31, 21);
			this.btnNext.TabIndex = 19;
			this.btnNext.Text = "&>";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// panelLeft
			// 
			this.panelLeft.Controls.Add(this.btnDelete);
			this.panelLeft.Controls.Add(this.btnSave);
			this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelLeft.Location = new System.Drawing.Point(0, 0);
			this.panelLeft.Name = "panelLeft";
			this.panelLeft.Size = new System.Drawing.Size(163, 26);
			this.panelLeft.TabIndex = 23;
			// 
			// btnDelete
			// 
			this.btnDelete.Font = new System.Drawing.Font("Tahoma", 8F);
			this.btnDelete.Location = new System.Drawing.Point(84, 0);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 22);
			this.btnDelete.TabIndex = 0;
			this.btnDelete.Text = "btnDelete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnSave
			// 
			this.btnSave.Enabled = false;
			this.btnSave.Font = new System.Drawing.Font("Tahoma", 8F);
			this.btnSave.Location = new System.Drawing.Point(3, 0);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 22);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "btnSave";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// tableLayoutPanelResult
			// 
			this.tableLayoutPanelResult.ColumnCount = 1;
			this.tableLayoutPanelResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelResult.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanelResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanelResult.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanelResult.Name = "tableLayoutPanelResult";
			this.tableLayoutPanelResult.RowCount = 2;
			this.tableLayoutPanelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanelResult.Size = new System.Drawing.Size(1201, 443);
			this.tableLayoutPanelResult.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.buttonSaveResult);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 415);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1201, 28);
			this.panel1.TabIndex = 4;
			// 
			// buttonSaveResult
			// 
			this.buttonSaveResult.Enabled = false;
			this.buttonSaveResult.Font = new System.Drawing.Font("Tahoma", 8F);
			this.buttonSaveResult.Location = new System.Drawing.Point(3, 3);
			this.buttonSaveResult.Name = "buttonSaveResult";
			this.buttonSaveResult.Size = new System.Drawing.Size(75, 22);
			this.buttonSaveResult.TabIndex = 3;
			this.buttonSaveResult.Text = "buttonSaveResult";
			this.buttonSaveResult.UseVisualStyleBackColor = true;
			this.buttonSaveResult.Click += new System.EventHandler(this.buttonSaveResult_Click);
			// 
			// QueryResult
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.CausesValidation = false;
			this.Controls.Add(this.splitContainerLeftPane);
			this.Name = "QueryResult";
			this.Size = new System.Drawing.Size(1201, 809);
			this.Load += new System.EventHandler(this.QueryResult_Load);
			this.splitContainerLeftPane.Panel1.ResumeLayout(false);
			this.splitContainerLeftPane.Panel2.ResumeLayout(false);
			this.splitContainerLeftPane.ResumeLayout(false);
			this.tableLayoutPanelResultGrid.ResumeLayout(false);
			this.tableLayoutPanelResultGrid.PerformLayout();
			this.panelResultGridOptions.ResumeLayout(false);
			this.panelRight.ResumeLayout(false);
			this.panelRight.PerformLayout();
			this.panelLeft.ResumeLayout(false);
			this.tableLayoutPanelResult.ResumeLayout(false);
			this.tableLayoutPanelResult.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainerLeftPane;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResultGrid;
		private System.Windows.Forms.Panel panelResultGridOptions;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResult;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonSaveResult;
		private System.Windows.Forms.Label lblof;
		private System.Windows.Forms.TextBox txtCurrentPage;
		private System.Windows.Forms.Button btnLast;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnFirst;
		private System.Windows.Forms.Panel panelRight;
		private System.Windows.Forms.Panel panelLeft;
		private System.Windows.Forms.Label lblPageCount;
		private System.Windows.Forms.Label lblFechedObjects;
		private System.Windows.Forms.Label labelNoOfObjects;
		internal System.Windows.Forms.ToolTip toolTipPagging;


		//private System.Windows.Forms.Panel panelProperties;



	}
}
