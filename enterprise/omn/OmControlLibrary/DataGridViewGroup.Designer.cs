namespace OMControlLibrary
{
	partial class DataGridViewGroup
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.panelTop = new System.Windows.Forms.Panel();
			this.labelRemove = new System.Windows.Forms.Label();
			this.comboBoxOperator = new System.Windows.Forms.ComboBox();
			this.labelQueryGroup = new System.Windows.Forms.Label();
			this.dbDataGridView = new OMControlLibrary.Common.dbDataGridView();
			this.panelTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dbDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// panelTop
			// 
			this.panelTop.BackColor = System.Drawing.SystemColors.Control;
			this.panelTop.Controls.Add(this.labelRemove);
			this.panelTop.Controls.Add(this.comboBoxOperator);
			this.panelTop.Controls.Add(this.labelQueryGroup);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTop.Location = new System.Drawing.Point(0, 0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(646, 23);
			this.panelTop.TabIndex = 0;
			// 
			// labelRemove
			// 
			this.labelRemove.BackColor = System.Drawing.Color.Transparent;
			this.labelRemove.Font = new System.Drawing.Font("Wingdings 2", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.labelRemove.Location = new System.Drawing.Point(621, 3);
			this.labelRemove.Name = "labelRemove";
			this.labelRemove.Size = new System.Drawing.Size(16, 16);
			this.labelRemove.TabIndex = 3;
			this.labelRemove.Text = "Ï";
			this.labelRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelRemove.Visible = false;
			this.labelRemove.MouseLeave += new System.EventHandler(this.labelRemove_MouseLeave);
			this.labelRemove.Click += new System.EventHandler(this.labelRemove_Click);
			this.labelRemove.MouseHover += new System.EventHandler(this.labelRemove_MouseHover);
			// 
			// comboBoxOperator
			// 
			this.comboBoxOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxOperator.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboBoxOperator.FormattingEnabled = true;
			this.comboBoxOperator.Location = new System.Drawing.Point(496, 1);
			this.comboBoxOperator.Name = "comboBoxOperator";
			this.comboBoxOperator.Size = new System.Drawing.Size(121, 21);
			this.comboBoxOperator.TabIndex = 1;
			this.comboBoxOperator.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperator_SelectedIndexChanged);
			// 
			// labelQueryGroup
			// 
			this.labelQueryGroup.AutoSize = true;
			this.labelQueryGroup.BackColor = System.Drawing.Color.Transparent;
			this.labelQueryGroup.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
			this.labelQueryGroup.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelQueryGroup.Location = new System.Drawing.Point(3, 4);
			this.labelQueryGroup.Name = "labelQueryGroup";
			this.labelQueryGroup.Size = new System.Drawing.Size(102, 13);
			this.labelQueryGroup.TabIndex = 0;
			this.labelQueryGroup.Text = "labelQueryGroup";
			this.labelQueryGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// dbDataGridView
			// 
			this.dbDataGridView.AllowDrop = true;
			this.dbDataGridView.AllowUserToAddRows = false;
			this.dbDataGridView.AllowUserToDeleteRows = false;
			this.dbDataGridView.AllowUserToResizeRows = false;
			this.dbDataGridView.BackgroundColor = System.Drawing.Color.White;
			this.dbDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dbDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dbDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
			this.dbDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
			this.dbDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.dbDataGridView.EnableHeadersVisualStyles = false;
			this.dbDataGridView.Location = new System.Drawing.Point(0, 23);
			this.dbDataGridView.MultiSelect = false;
			this.dbDataGridView.Name = "dbDataGridView";
			this.dbDataGridView.RowHeadersVisible = false;
			this.dbDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dbDataGridView.Size = new System.Drawing.Size(646, 106);
			this.dbDataGridView.TabIndex = 1;
			this.dbDataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dbDataGridView_MouseDown);
			this.dbDataGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dbDataGridView_CellBeginEdit);
			this.dbDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dbDataGridView_KeyDown);
			this.dbDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dbDataGridView_CellClick);
			this.dbDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dbDataGridView_RowsAdded);
			this.dbDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dbDataGridView_CellEndEdit);
			this.dbDataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.dbDataGridView_DragEnter);
			this.dbDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dbDataGridView_DataError);
			this.dbDataGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dbDataGridView_EditingControlShowing);
			this.dbDataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dbDataGridView_RowsRemoved);
			// 
			// DataGridViewGroup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.dbDataGridView);
			this.Controls.Add(this.panelTop);
			this.Name = "DataGridViewGroup";
			this.Size = new System.Drawing.Size(646, 130);
			this.Load += new System.EventHandler(this.DataGridViewGroup_Load);
			this.panelTop.ResumeLayout(false);
			this.panelTop.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dbDataGridView)).EndInit();
			this.ResumeLayout(false);

		}


		#endregion

		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Label labelQueryGroup;


		private System.Windows.Forms.ComboBox comboBoxOperator;
		private OMControlLibrary.Common.dbDataGridView dbDataGridView;
		private System.Windows.Forms.Label labelRemove;
	}
}
