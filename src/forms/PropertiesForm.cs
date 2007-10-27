using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for PropertiesForm.
	/// </summary>
	public class PropertiesForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblFeedTitle;
		private System.Windows.Forms.TextBox txtFeedTitle;
		private System.Windows.Forms.Label lblFeedUrl;
		private System.Windows.Forms.TextBox txtFeedUrl;
		private System.Windows.Forms.Label lblHtmlUrl;
		private System.Windows.Forms.TextBox txtHtmlUrl;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region My Private Members
		private FeedNode m_feedNode;
		#endregion

		public PropertiesForm(FeedNode feedNode)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			m_feedNode = feedNode;
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
			this.lblFeedTitle = new System.Windows.Forms.Label();
			this.txtFeedTitle = new System.Windows.Forms.TextBox();
			this.lblFeedUrl = new System.Windows.Forms.Label();
			this.txtFeedUrl = new System.Windows.Forms.TextBox();
			this.lblHtmlUrl = new System.Windows.Forms.Label();
			this.txtHtmlUrl = new System.Windows.Forms.TextBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblFeedTitle
			// 
			this.lblFeedTitle.Location = new System.Drawing.Point(8, 8);
			this.lblFeedTitle.Name = "lblFeedTitle";
			this.lblFeedTitle.Size = new System.Drawing.Size(80, 20);
			this.lblFeedTitle.TabIndex = 0;
			this.lblFeedTitle.Text = "Feed title:";
			this.lblFeedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtFeedTitle
			// 
			this.txtFeedTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFeedTitle.Location = new System.Drawing.Point(96, 8);
			this.txtFeedTitle.Name = "txtFeedTitle";
			this.txtFeedTitle.Size = new System.Drawing.Size(320, 20);
			this.txtFeedTitle.TabIndex = 1;
			this.txtFeedTitle.Text = "";
			// 
			// lblFeedUrl
			// 
			this.lblFeedUrl.Location = new System.Drawing.Point(8, 36);
			this.lblFeedUrl.Name = "lblFeedUrl";
			this.lblFeedUrl.Size = new System.Drawing.Size(80, 20);
			this.lblFeedUrl.TabIndex = 2;
			this.lblFeedUrl.Text = "Feed URL:";
			this.lblFeedUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtFeedUrl
			// 
			this.txtFeedUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFeedUrl.Location = new System.Drawing.Point(96, 36);
			this.txtFeedUrl.Name = "txtFeedUrl";
			this.txtFeedUrl.ReadOnly = true;
			this.txtFeedUrl.Size = new System.Drawing.Size(320, 20);
			this.txtFeedUrl.TabIndex = 3;
			this.txtFeedUrl.Text = "";
			// 
			// lblHtmlUrl
			// 
			this.lblHtmlUrl.Location = new System.Drawing.Point(8, 64);
			this.lblHtmlUrl.Name = "lblHtmlUrl";
			this.lblHtmlUrl.Size = new System.Drawing.Size(80, 20);
			this.lblHtmlUrl.TabIndex = 4;
			this.lblHtmlUrl.Text = "HTML URL:";
			this.lblHtmlUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtHtmlUrl
			// 
			this.txtHtmlUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHtmlUrl.Location = new System.Drawing.Point(96, 64);
			this.txtHtmlUrl.Name = "txtHtmlUrl";
			this.txtHtmlUrl.ReadOnly = true;
			this.txtHtmlUrl.Size = new System.Drawing.Size(320, 20);
			this.txtHtmlUrl.TabIndex = 5;
			this.txtHtmlUrl.Text = "";
			// 
			// lblDescription
			// 
			this.lblDescription.Location = new System.Drawing.Point(8, 92);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(80, 20);
			this.lblDescription.TabIndex = 6;
			this.lblDescription.Text = "Description:";
			this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(96, 92);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(320, 124);
			this.txtDescription.TabIndex = 7;
			this.txtDescription.Text = "";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnOk.Location = new System.Drawing.Point(8, 224);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(72, 23);
			this.btnOk.TabIndex = 8;
			this.btnOk.Text = "OK";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(88, 224);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			// 
			// PropertiesForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(424, 253);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.txtFeedUrl);
			this.Controls.Add(this.txtFeedTitle);
			this.Controls.Add(this.txtHtmlUrl);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblHtmlUrl);
			this.Controls.Add(this.lblFeedUrl);
			this.Controls.Add(this.lblFeedTitle);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(320, 240);
			this.Name = "PropertiesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Feed properties";
			this.Load += new System.EventHandler(this.PropertiesForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void PropertiesForm_Load(object sender, System.EventArgs e)
		{
			if (m_feedNode != null)
			{
				txtFeedTitle.Text = m_feedNode.Title;
				txtFeedUrl.Text = m_feedNode.XmlUrl;
				txtHtmlUrl.Text = m_feedNode.HtmlUrl;
				txtDescription.Text = m_feedNode.Description;
			}
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			m_feedNode.Title = txtFeedTitle.Text;
			m_feedNode.Description = txtDescription.Text;

			DialogResult = DialogResult.OK;
		}
	}
}
