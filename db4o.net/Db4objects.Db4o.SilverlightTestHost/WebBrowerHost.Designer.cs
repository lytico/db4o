namespace Db4objects.Db4o.SilverlightTestHost
{
	partial class WebBrowerHost
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.silverlightTestHost = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// silverlightTestHost
			// 
			this.silverlightTestHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.silverlightTestHost.Location = new System.Drawing.Point(0, 0);
			this.silverlightTestHost.MinimumSize = new System.Drawing.Size(20, 20);
			this.silverlightTestHost.Name = "silverlightTestHost";
			this.silverlightTestHost.Size = new System.Drawing.Size(575, 525);
			this.silverlightTestHost.TabIndex = 0;
			// 
			// WebBrowerHost
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(575, 525);
			this.Controls.Add(this.silverlightTestHost);
			this.Name = "WebBrowerHost";
			this.Text = "WebBrowerHost";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser silverlightTestHost;
	}
}