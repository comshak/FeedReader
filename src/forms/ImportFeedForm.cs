using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for ImportFeedForm.
	/// </summary>
	public class ImportFeedForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblFeedUrl;
		private System.Windows.Forms.TextBox txtFeedUrl;
		private System.Windows.Forms.Label lblHtmlUrl;
		private System.Windows.Forms.TextBox txtHtmlUrl;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Button btnAutoDiscover;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;

		private string m_strTempFeedFile;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ImportFeedForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			btnOK.Enabled = false;

			IDataObject dataobj = Clipboard.GetDataObject();
			if (dataobj != null)
			{
				string strData = dataobj.GetData(DataFormats.Text) as string;
				if (strData != null)
				{
					txtFeedUrl.Text = strData;
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			this.lblFeedUrl = new System.Windows.Forms.Label();
			this.txtFeedUrl = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtHtmlUrl = new System.Windows.Forms.TextBox();
			this.lblHtmlUrl = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.btnAutoDiscover = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblFeedUrl
			// 
			this.lblFeedUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblFeedUrl.Location = new System.Drawing.Point(8, 8);
			this.lblFeedUrl.Name = "lblFeedUrl";
			this.lblFeedUrl.Size = new System.Drawing.Size(80, 20);
			this.lblFeedUrl.TabIndex = 0;
			this.lblFeedUrl.Text = "Feed &URL:";
			this.lblFeedUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtFeedUrl
			// 
			this.txtFeedUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFeedUrl.Location = new System.Drawing.Point(96, 8);
			this.txtFeedUrl.Name = "txtFeedUrl";
			this.txtFeedUrl.Size = new System.Drawing.Size(320, 20);
			this.txtFeedUrl.TabIndex = 1;
			this.txtFeedUrl.Text = "";
			this.txtFeedUrl.TextChanged += new System.EventHandler(this.txtFeedUrl_TextChanged);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(264, 224);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(72, 23);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(344, 224);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtHtmlUrl
			// 
			this.txtHtmlUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHtmlUrl.Location = new System.Drawing.Point(96, 36);
			this.txtHtmlUrl.Name = "txtHtmlUrl";
			this.txtHtmlUrl.Size = new System.Drawing.Size(320, 20);
			this.txtHtmlUrl.TabIndex = 3;
			this.txtHtmlUrl.Text = "";
			// 
			// lblHtmlUrl
			// 
			this.lblHtmlUrl.Location = new System.Drawing.Point(8, 36);
			this.lblHtmlUrl.Name = "lblHtmlUrl";
			this.lblHtmlUrl.Size = new System.Drawing.Size(80, 20);
			this.lblHtmlUrl.TabIndex = 2;
			this.lblHtmlUrl.Text = "&HTML URL:";
			this.lblHtmlUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTitle
			// 
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(8, 64);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(80, 20);
			this.lblTitle.TabIndex = 4;
			this.lblTitle.Text = "&Title:";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitle.Location = new System.Drawing.Point(96, 64);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(320, 20);
			this.txtTitle.TabIndex = 5;
			this.txtTitle.Text = "";
			this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
			// 
			// lblDescription
			// 
			this.lblDescription.Location = new System.Drawing.Point(8, 92);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(80, 20);
			this.lblDescription.TabIndex = 6;
			this.lblDescription.Text = "&Description:";
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
			// btnAutoDiscover
			// 
			this.btnAutoDiscover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAutoDiscover.Location = new System.Drawing.Point(8, 224);
			this.btnAutoDiscover.Name = "btnAutoDiscover";
			this.btnAutoDiscover.Size = new System.Drawing.Size(120, 23);
			this.btnAutoDiscover.TabIndex = 9;
			this.btnAutoDiscover.Text = "&Auto discover";
			this.btnAutoDiscover.Click += new System.EventHandler(this.btnAutoDiscover_Click);
			// 
			// ImportFeedForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(424, 258);
			this.ControlBox = false;
			this.Controls.Add(this.btnAutoDiscover);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.txtHtmlUrl);
			this.Controls.Add(this.txtFeedUrl);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.lblHtmlUrl);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblFeedUrl);
			this.MinimumSize = new System.Drawing.Size(320, 240);
			this.Name = "ImportFeedForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Import feed";
			this.ResumeLayout(false);

		}
		#endregion

		public string FeedUrl
		{
			get { return txtFeedUrl.Text; }
		}

		public string FeedTitle
		{
			get { return txtTitle.Text; }
		}

		public string TempFeedFile
		{
			get { return m_strTempFeedFile; }
		}

		private void btnAutoDiscover_Click(object sender, System.EventArgs e)
		{
			try
			{
				btnAutoDiscover.Text = "Discovering...";
				btnAutoDiscover.Enabled = false;

				DiscoverFeedThread thread = new DiscoverFeedThread(this, txtFeedUrl.Text);
				thread.Start();
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("ImportFeedForm::btnAutoDiscover_Click()", ex);
			}
		}

		/// <summary>
		/// Event handler called when the DiscoverFeedThread finishes processing.
		/// </summary>
		/// <param name="strTitle"></param>
		/// <param name="strHtmlUrl"></param>
		/// <param name="strDescription"></param>
		public void OnEnd_DiscoverFeed(string strTitle, string strHtmlUrl, string strDescription, string strTempFeedFile)
		{
			txtTitle.Text = strTitle;
			txtHtmlUrl.Text = strHtmlUrl;
			txtDescription.Text = strDescription;
			m_strTempFeedFile = strTempFeedFile;

			btnAutoDiscover.Enabled = true;
			btnAutoDiscover.Text = "&Auto discover";
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Utils.DeleteFile(m_strTempFeedFile);
			DialogResult = DialogResult.Cancel;
		}

		private void txtFeedUrl_TextChanged(object sender, System.EventArgs e)
		{
			MyValidate();
		}

		private void txtTitle_TextChanged(object sender, System.EventArgs e)
		{
			MyValidate();
		}

		/// <summary>
		/// Hides the OK button until there's a valid feed url and title.
		/// </summary>
		private void MyValidate()
		{
			bool stateOK = true;
			if (txtFeedUrl.Text.Length <= 0)
			{
				stateOK = false;
			}
			else if (txtTitle.Text.Length <= 0)
			{
				stateOK = false;
			}
			btnOK.Enabled = stateOK;
		}
	}
}
