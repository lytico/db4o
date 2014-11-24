/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using System.Windows.Forms;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of WebBrowserViewControl.
	/// </summary>
	public class WebBrowserViewControl : UserControl
	{
		private System.ComponentModel.IContainer components;
		private WebBrowser _webBrowser;
		private ToolBar _toolbar;
		private ToolBarButton _buttonBack;
		private ImageList _toolbarImages;
		private ToolBarButton _buttonForward;
		public WebBrowserViewControl()
		{
			InitializeComponent();
		}
		
		public WebBrowser WebBrowser
		{
			get
			{
				return _webBrowser;
			}
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebBrowserViewControl));
            this._buttonForward = new System.Windows.Forms.ToolBarButton();
            this._toolbarImages = new System.Windows.Forms.ImageList(this.components);
            this._buttonBack = new System.Windows.Forms.ToolBarButton();
            this._toolbar = new System.Windows.Forms.ToolBar();
            this._webBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // _buttonForward
            // 
            this._buttonForward.ImageIndex = 1;
            this._buttonForward.Name = "_buttonForward";
            this._buttonForward.ToolTipText = "Forward";
            // 
            // _toolbarImages
            // 
            this._toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_toolbarImages.ImageStream")));
            this._toolbarImages.TransparentColor = System.Drawing.Color.White;
            this._toolbarImages.Images.SetKeyName(0, "");
            this._toolbarImages.Images.SetKeyName(1, "");
            // 
            // _buttonBack
            // 
            this._buttonBack.ImageIndex = 0;
            this._buttonBack.Name = "_buttonBack";
            this._buttonBack.ToolTipText = "Back";
            // 
            // _toolbar
            // 
            this._toolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this._toolbar.AutoSize = false;
            this._toolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this._buttonBack,
            this._buttonForward});
            this._toolbar.ButtonSize = new System.Drawing.Size(18, 18);
            this._toolbar.Divider = false;
            this._toolbar.DropDownArrows = true;
            this._toolbar.ImageList = this._toolbarImages;
            this._toolbar.Location = new System.Drawing.Point(0, 0);
            this._toolbar.Name = "_toolbar";
            this._toolbar.ShowToolTips = true;
            this._toolbar.Size = new System.Drawing.Size(544, 24);
            this._toolbar.TabIndex = 0;
            this._toolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this._toolbar_ButtonClick);
            // 
            // _webBrowser
            // 
            this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._webBrowser.Location = new System.Drawing.Point(0, 24);
            this._webBrowser.Name = "_webBrowser";
            this._webBrowser.Size = new System.Drawing.Size(544, 392);
            this._webBrowser.TabIndex = 1;
            // 
            // WebBrowserViewControl
            // 
            this.Controls.Add(this._webBrowser);
            this.Controls.Add(this._toolbar);
            this.Name = "WebBrowserViewControl";
            this.Size = new System.Drawing.Size(544, 416);
            this.ResumeLayout(false);

		}

		private void OnCanGoForwardChanged(object sender, EventArgs e)
		{
			_buttonForward.Enabled = _webBrowser.CanGoForward;
		}

		private void OnCanGoBackChanged(object sender, EventArgs e)
		{
			_buttonBack.Enabled = _webBrowser.CanGoBack;
		}

		#endregion

		void OnToolbarButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (_buttonBack == e.Button)
			{
				_webBrowser.GoBack();
			}
			else if (_buttonForward == e.Button)
			{
				_webBrowser.GoForward();
			}
		}

        private void _toolbar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            OnToolbarButtonClick(sender, e);
        }
		
	}
}
