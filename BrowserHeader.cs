using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for BrowserHeader.
	/// </summary>
	public class BrowserHeader : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnForw;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.GroupBox groupBox1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public event EventHandler GoButtonPressed;
		public event EventHandler BackButtonPressed;
		public event EventHandler ForwardButtonPressed;

		/// <summary>
		/// Gets or sets the address in the address bar.
		/// </summary>
		public string Address
		{
			get
			{
				return txtAddress.Text;
			}
			set
			{
				txtAddress.Text = value;
			}
		}


		public BrowserHeader()
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
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.btnBack = new System.Windows.Forms.Button();
			this.btnForw = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtAddress
			// 
			this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAddress.Location = new System.Drawing.Point(51, 10);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(582, 20);
			this.txtAddress.TabIndex = 1;
			this.txtAddress.Text = "";
			this.txtAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAddress_KeyDown);
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnGo.Location = new System.Drawing.Point(637, 8);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(32, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go!";
			this.btnGo.Click += new System.EventHandler(this.Go_Click);
			// 
			// btnBack
			// 
			this.btnBack.Location = new System.Drawing.Point(3, 8);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(23, 23);
			this.btnBack.TabIndex = 3;
			this.btnBack.Text = "<";
			this.btnBack.Click += new System.EventHandler(this.Back_Clicked);
			// 
			// btnForw
			// 
			this.btnForw.Location = new System.Drawing.Point(27, 8);
			this.btnForw.Name = "btnForw";
			this.btnForw.Size = new System.Drawing.Size(23, 23);
			this.btnForw.TabIndex = 4;
			this.btnForw.Text = ">";
			this.btnForw.Click += new System.EventHandler(this.Forward_Clicked);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.btnBack);
			this.groupBox1.Controls.Add(this.txtAddress);
			this.groupBox1.Controls.Add(this.btnForw);
			this.groupBox1.Controls.Add(this.btnGo);
			this.groupBox1.Location = new System.Drawing.Point(0, -5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(672, 34);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			// 
			// BrowserHeader
			// 
			this.Controls.Add(this.groupBox1);
			this.Name = "BrowserHeader";
			this.Size = new System.Drawing.Size(672, 31);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Enables or disables the Back button.
		/// </summary>
		/// <param name="enable"></param>
		public void EnableBack(bool enable)
		{
			btnBack.Enabled = enable;
		}

		/// <summary>
		/// Enables or disables the Forward button.
		/// </summary>
		/// <param name="enable"></param>
		public void EnableForward(bool enable)
		{
			btnForw.Enabled = enable;
		}

		private void Go_Click(object sender, System.EventArgs e)
		{
			if (GoButtonPressed != null)
			{
				GoButtonPressed(sender, e);
			}
		}

		private void Back_Clicked(object sender, System.EventArgs e)
		{
			if (BackButtonPressed != null)
			{
				BackButtonPressed(sender, e);
			}
		}

		private void Forward_Clicked(object sender, System.EventArgs e)
		{
			if (ForwardButtonPressed != null)
			{
				ForwardButtonPressed(sender, e);
			}
		}

		private void txtAddress_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (GoButtonPressed != null)
				{
					GoButtonPressed(sender, e);
				}
				btnGo.Select();
			}
		}
	}
}
