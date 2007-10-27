using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for NewFolderForm.
	/// </summary>
	public class NewFolderForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblFolderName;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtFolderName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewFolderForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblFolderName = new System.Windows.Forms.Label();
			this.txtFolderName = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblFolderName
			// 
			this.lblFolderName.Location = new System.Drawing.Point(8, 8);
			this.lblFolderName.Name = "lblFolderName";
			this.lblFolderName.Size = new System.Drawing.Size(72, 20);
			this.lblFolderName.TabIndex = 0;
			this.lblFolderName.Text = "Folder name:";
			this.lblFolderName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtFolderName
			// 
			this.txtFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFolderName.Location = new System.Drawing.Point(88, 8);
			this.txtFolderName.Name = "txtFolderName";
			this.txtFolderName.Size = new System.Drawing.Size(220, 20);
			this.txtFolderName.TabIndex = 1;
			this.txtFolderName.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnOK.Location = new System.Drawing.Point(8, 40);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(240, 40);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// NewFolderForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(320, 73);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtFolderName);
			this.Controls.Add(this.lblFolderName);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1024, 100);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(240, 100);
			this.Name = "NewFolderForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Folder";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;		
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		public string FolderName
		{
			get { return txtFolderName.Text; }
		}
	}
}
