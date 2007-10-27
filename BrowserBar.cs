using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for BrowserBar.
	/// </summary>
	public class BrowserBar : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BrowserBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.SuspendLayout();
			// 
			// toolBar1
			// 
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButton1,
																						this.toolBarButton2});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(496, 42);
			this.toolBar1.TabIndex = 0;
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Text = "P";
			this.toolBarButton1.ToolTipText = "Previous";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Text = "N";
			this.toolBarButton2.ToolTipText = "Next";
			// 
			// BrowserBar
			// 
			this.Controls.Add(this.toolBar1);
			this.Name = "BrowserBar";
			this.Size = new System.Drawing.Size(496, 150);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
