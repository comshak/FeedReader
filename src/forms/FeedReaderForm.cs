using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

using mshtml;

using AxSHDocVw = AxInterop.SHDocVw;
using SHDocVw   = Interop.SHDocVw;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FeedReaderForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.StatusBar statusBarMain;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.TreeView treeViewFeeds;
		private System.Windows.Forms.ListView listViewHeadlines;
		private AxSHDocVw.AxWebBrowser axWebBrowser;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemUpdate;
		private IContainer components;

		#region My member fields
		private System.Windows.Forms.ImageList		m_imgList;
		public  com.comshak.FeedReader.FeedNode		m_rootFeedNode;
		private com.comshak.FeedReader.FeedNode		m_feedNodeRightClicked;
		private com.comshak.FeedReader.FeedNode		m_feedNodeSelected;
		private System.Windows.Forms.ColumnHeader	colHdrTitle;
		private System.Windows.Forms.ColumnHeader	colHdrPublished;
		private System.Windows.Forms.ColumnHeader	colHdrAuthor;
		private System.Windows.Forms.ColumnHeader	colHdrReceived;
		private System.Windows.Forms.StatusBarPanel	pnlStatus;
		private System.Windows.Forms.StatusBarPanel	pnlProgress;
		private System.Windows.Forms.Splitter		splitterV;
		private System.Windows.Forms.Splitter		splitterH;
		private System.Windows.Forms.MenuItem		menuItemImport;
		private System.Windows.Forms.MenuItem		menuItemDelete;
		private System.Windows.Forms.MenuItem		menuItemNewFolder;
		private System.Windows.Forms.MenuItem		menuItemSep1;
		private System.Windows.Forms.MenuItem		menuItemSep2;
		private HeadlineCollection					m_headlines;
		private System.Windows.Forms.MenuItem		menuItemSave;
		private System.Windows.Forms.MenuItem		menuItemFileSep1;
		private System.Windows.Forms.MenuItem		menuItemExit;
		private System.Windows.Forms.MenuItem		menuItemAbout;
		private System.Windows.Forms.MenuItem		menuItemSep3;
		private System.Windows.Forms.MenuItem		menuItemProperties;
		private System.Windows.Forms.MenuItem		menuItemHelp;
		private com.comshak.FeedReader.BrowserHeader browserHeader1;
		private com.comshak.FeedReader.FeedNode		m_dragNode;		// Node being dragged
		#endregion

		public FeedNode RootFeedNode
		{
			get { return m_rootFeedNode; }
		}

		public FeedReaderForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			m_rootFeedNode = new FeedNode("Feeds", null, null, true);
			m_imgList = new ImageList();
			Assembly a = Assembly.GetExecutingAssembly();
			m_imgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.folder.ico")));		// 0
			m_imgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.feed.ico")));		// 1
			m_imgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.sync.ico")));	// 2
			treeViewFeeds.ImageList = m_imgList;

			this.Size = new Size(1024, 768);
			listViewHeadlines.Height = (this.ClientSize.Height - statusBarMain.Height) / 2;
			treeViewFeeds.Width = 250;
			colHdrTitle.Width = 325;
			colHdrPublished.Width = 134;
			colHdrReceived.Width = 134;
			colHdrAuthor.Width = 150;

			RepositionBrowser();
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedReaderForm));
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemSave = new System.Windows.Forms.MenuItem();
			this.menuItemFileSep1 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemAbout = new System.Windows.Forms.MenuItem();
			this.statusBarMain = new System.Windows.Forms.StatusBar();
			this.pnlStatus = new System.Windows.Forms.StatusBarPanel();
			this.pnlProgress = new System.Windows.Forms.StatusBarPanel();
			this.treeViewFeeds = new System.Windows.Forms.TreeView();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemUpdate = new System.Windows.Forms.MenuItem();
			this.menuItemSep1 = new System.Windows.Forms.MenuItem();
			this.menuItemImport = new System.Windows.Forms.MenuItem();
			this.menuItemNewFolder = new System.Windows.Forms.MenuItem();
			this.menuItemSep2 = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.menuItemSep3 = new System.Windows.Forms.MenuItem();
			this.menuItemProperties = new System.Windows.Forms.MenuItem();
			this.splitterV = new System.Windows.Forms.Splitter();
			this.listViewHeadlines = new System.Windows.Forms.ListView();
			this.colHdrTitle = new System.Windows.Forms.ColumnHeader();
			this.colHdrPublished = new System.Windows.Forms.ColumnHeader();
			this.colHdrReceived = new System.Windows.Forms.ColumnHeader();
			this.colHdrAuthor = new System.Windows.Forms.ColumnHeader();
			this.splitterH = new System.Windows.Forms.Splitter();
			this.browserHeader1 = new com.comshak.FeedReader.BrowserHeader();
			this.axWebBrowser = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.pnlStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pnlProgress)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItem1,
			this.menuItemHelp});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItemSave,
			this.menuItemFileSep1,
			this.menuItemExit});
			this.menuItem1.Text = "&File";
			// 
			// menuItemSave
			// 
			this.menuItemSave.Index = 0;
			this.menuItemSave.Text = "&Save";
			this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
			// 
			// menuItemFileSep1
			// 
			this.menuItemFileSep1.Index = 1;
			this.menuItemFileSep1.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 2;
			this.menuItemExit.Text = "E&xit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Index = 1;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItemAbout});
			this.menuItemHelp.Text = "&Help";
			// 
			// menuItemAbout
			// 
			this.menuItemAbout.Index = 0;
			this.menuItemAbout.Text = "&About...";
			this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
			// 
			// statusBarMain
			// 
			this.statusBarMain.Location = new System.Drawing.Point(0, 338);
			this.statusBarMain.Name = "statusBarMain";
			this.statusBarMain.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
			this.pnlStatus,
			this.pnlProgress});
			this.statusBarMain.ShowPanels = true;
			this.statusBarMain.Size = new System.Drawing.Size(759, 22);
			this.statusBarMain.TabIndex = 0;
			this.statusBarMain.Text = "statusBarMain";
			// 
			// pnlStatus
			// 
			this.pnlStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.pnlStatus.Name = "pnlStatus";
			this.pnlStatus.Width = 643;
			// 
			// pnlProgress
			// 
			this.pnlProgress.Name = "pnlProgress";
			// 
			// treeViewFeeds
			// 
			this.treeViewFeeds.AllowDrop = true;
			this.treeViewFeeds.ContextMenu = this.contextMenu1;
			this.treeViewFeeds.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewFeeds.HideSelection = false;
			this.treeViewFeeds.Location = new System.Drawing.Point(0, 0);
			this.treeViewFeeds.Name = "treeViewFeeds";
			this.treeViewFeeds.Size = new System.Drawing.Size(128, 338);
			this.treeViewFeeds.TabIndex = 1;
			this.treeViewFeeds.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewFeeds_DragDrop);
			this.treeViewFeeds.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewFeeds_DragOver);
			this.treeViewFeeds.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFeeds_AfterSelect);
			this.treeViewFeeds.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewFeeds_DragEnter);
			this.treeViewFeeds.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewFeeds_ItemDrag);
			this.treeViewFeeds.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewFeeds_MouseDown);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItemUpdate,
			this.menuItemSep1,
			this.menuItemImport,
			this.menuItemNewFolder,
			this.menuItemSep2,
			this.menuItemDelete,
			this.menuItemSep3,
			this.menuItemProperties});
			// 
			// menuItemUpdate
			// 
			this.menuItemUpdate.Index = 0;
			this.menuItemUpdate.Text = "Update";
			this.menuItemUpdate.Click += new System.EventHandler(this.contextMenuItemUpdate_Click);
			// 
			// menuItemSep1
			// 
			this.menuItemSep1.Index = 1;
			this.menuItemSep1.Text = "-";
			// 
			// menuItemImport
			// 
			this.menuItemImport.Index = 2;
			this.menuItemImport.Text = "&Import feed...";
			this.menuItemImport.Click += new System.EventHandler(this.contextMenuItemImport_Click);
			// 
			// menuItemNewFolder
			// 
			this.menuItemNewFolder.Index = 3;
			this.menuItemNewFolder.Text = "&New folder...";
			this.menuItemNewFolder.Click += new System.EventHandler(this.contextMenuItemNewFolder_Click);
			// 
			// menuItemSep2
			// 
			this.menuItemSep2.Index = 4;
			this.menuItemSep2.Text = "-";
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 5;
			this.menuItemDelete.Text = "Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.contextMenuItemDelete_Click);
			// 
			// menuItemSep3
			// 
			this.menuItemSep3.Index = 6;
			this.menuItemSep3.Text = "-";
			// 
			// menuItemProperties
			// 
			this.menuItemProperties.Index = 7;
			this.menuItemProperties.Text = "Properties";
			this.menuItemProperties.Click += new System.EventHandler(this.menuItemProperties_Click);
			// 
			// splitterV
			// 
			this.splitterV.BackColor = System.Drawing.SystemColors.Control;
			this.splitterV.Location = new System.Drawing.Point(128, 0);
			this.splitterV.Name = "splitterV";
			this.splitterV.Size = new System.Drawing.Size(3, 338);
			this.splitterV.TabIndex = 2;
			this.splitterV.TabStop = false;
			this.splitterV.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitterV_SplitterMoved);
			// 
			// listViewHeadlines
			// 
			this.listViewHeadlines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.colHdrTitle,
			this.colHdrPublished,
			this.colHdrReceived,
			this.colHdrAuthor});
			this.listViewHeadlines.Dock = System.Windows.Forms.DockStyle.Top;
			this.listViewHeadlines.FullRowSelect = true;
			this.listViewHeadlines.HideSelection = false;
			this.listViewHeadlines.Location = new System.Drawing.Point(131, 0);
			this.listViewHeadlines.MultiSelect = false;
			this.listViewHeadlines.Name = "listViewHeadlines";
			this.listViewHeadlines.Size = new System.Drawing.Size(628, 152);
			this.listViewHeadlines.TabIndex = 3;
			this.listViewHeadlines.UseCompatibleStateImageBehavior = false;
			this.listViewHeadlines.View = System.Windows.Forms.View.Details;
			this.listViewHeadlines.SelectedIndexChanged += new System.EventHandler(this.listViewHeadlines_SelectedIndexChanged);
			// 
			// colHdrTitle
			// 
			this.colHdrTitle.Text = "Title";
			this.colHdrTitle.Width = 205;
			// 
			// colHdrPublished
			// 
			this.colHdrPublished.Text = "Date Published";
			this.colHdrPublished.Width = 130;
			// 
			// colHdrReceived
			// 
			this.colHdrReceived.Text = "Date Received";
			this.colHdrReceived.Width = 130;
			// 
			// colHdrAuthor
			// 
			this.colHdrAuthor.Text = "Author";
			this.colHdrAuthor.Width = 100;
			// 
			// splitterH
			// 
			this.splitterH.BackColor = System.Drawing.SystemColors.Control;
			this.splitterH.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitterH.Location = new System.Drawing.Point(131, 152);
			this.splitterH.Name = "splitterH";
			this.splitterH.Size = new System.Drawing.Size(628, 3);
			this.splitterH.TabIndex = 4;
			this.splitterH.TabStop = false;
			this.splitterH.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitterH_SplitterMoved);
			// 
			// browserHeader1
			// 
			this.browserHeader1.Address = "";
			this.browserHeader1.Dock = System.Windows.Forms.DockStyle.Top;
			this.browserHeader1.Location = new System.Drawing.Point(131, 155);
			this.browserHeader1.Name = "browserHeader1";
			this.browserHeader1.Size = new System.Drawing.Size(628, 31);
			this.browserHeader1.TabIndex = 6;
			this.browserHeader1.GoButtonPressed += new System.EventHandler(this.browserHeader1_GoButtonPressed);
			this.browserHeader1.BackButtonPressed += new System.EventHandler(this.browserHeader1_BackButtonPressed);
			this.browserHeader1.ForwardButtonPressed += new System.EventHandler(this.browserHeader1_ForwardButtonPressed);
			// 
			// axWebBrowser
			// 
			this.axWebBrowser.Enabled = true;
			this.axWebBrowser.Location = new System.Drawing.Point(137, 192);
			this.axWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser.OcxState")));
			this.axWebBrowser.Size = new System.Drawing.Size(80, 80);
			this.axWebBrowser.TabIndex = 5;
			this.axWebBrowser.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.axWebBrowser_BeforeNavigate2);
			this.axWebBrowser.StatusTextChange += new AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler(this.axWebBrowser_StatusTextChange);
			this.axWebBrowser.DownloadBegin += new System.EventHandler(this.axWebBrowser_DownloadBegin);
			this.axWebBrowser.CommandStateChange += new AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEventHandler(this.axWebBrowser_CommandStateChange);
			this.axWebBrowser.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser_DocumentComplete);
			this.axWebBrowser.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser_NavigateComplete2);
			// 
			// FeedReaderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(759, 360);
			this.Controls.Add(this.browserHeader1);
			this.Controls.Add(this.axWebBrowser);
			this.Controls.Add(this.splitterH);
			this.Controls.Add(this.listViewHeadlines);
			this.Controls.Add(this.splitterV);
			this.Controls.Add(this.treeViewFeeds);
			this.Controls.Add(this.statusBarMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "FeedReaderForm";
			this.Text = "FeedReader";
			this.Closed += new System.EventHandler(this.FeedReaderForm_Closed);
			this.Resize += new System.EventHandler(this.FeedReaderForm_Resize);
			this.Load += new System.EventHandler(this.FeedReaderForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pnlStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pnlProgress)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void FeedReaderForm_Load(object sender, System.EventArgs e)
		{
			ParseFeedsThread thread = new ParseFeedsThread(this);
			thread.Start();

			NavigateTo("about:blank");
		}


		/// <summary>
		/// Utility method to make the WebBrowser control navigate to a URL.
		/// </summary>
		/// <param name="strUrl"></param>
		private void NavigateTo(string strUrl)
		{
			if ((strUrl != null) && (strUrl.Length > 0))
			{
				object url = (object)strUrl;
				object obj = null;
				axWebBrowser.Navigate2(ref url, ref obj, ref obj, ref obj, ref obj);
			}
		}


		/// <summary>
		/// Occurs when the selection has been changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeViewFeeds_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (m_feedNodeRightClicked != null)
			{
				return;
			}

			TreeNode treeNode = e.Node;
			FeedNode feedNode = (FeedNode)treeNode.Tag;
			if (!feedNode.IsFolder)
			{
				m_feedNodeSelected = feedNode;
				ReadChannelThread thread = new ReadChannelThread(this, feedNode);
				thread.Start();
			}
			else
			{
				listViewHeadlines.Items.Clear();
			}
			NavigateTo("about:blank");
		}


		/// <summary>
		/// Recursive method to update all the feeds within a folder and all its subfolders.
		/// </summary>
		/// <param name="feedNode"></param>
		private void UpdateFolder(FeedNode feedNode)
		{
			IDictionaryEnumerator enumNodes = feedNode.Enumerator;
			while (enumNodes.MoveNext())
			{
				FeedNode fNode = (FeedNode)enumNodes.Key;
				if (fNode != null)
				{
					if (fNode.IsFolder)
					{
						UpdateFolder(fNode);
					}
					else
					{
						UpdateChannelThread thread = new UpdateChannelThread(this, fNode);
						thread.Start();
					}
				}
			}
		}


		/// <summary>
		/// Occurs when a mouse button is pressed.
		/// 
		/// Let's handle the right-click on TreeView items here...
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeViewFeeds_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeNode treeNode = treeViewFeeds.GetNodeAt(e.X, e.Y);
				if (treeNode != null)
				{
					FeedNode feedNode = (FeedNode)treeNode.Tag;
					if (feedNode != null)
					{
						if (feedNode.IsFolder)
						{
							menuItemUpdate.Text = "&Update group";
							menuItemSep1.Visible = true;
							menuItemImport.Visible = true;
							menuItemNewFolder.Visible = true;
							menuItemSep2.Visible = true;
							menuItemDelete.Text = "&Delete folder";
							menuItemDelete.Enabled = (feedNode.Parent != null);
						}
						else
						{
							menuItemUpdate.Text = "&Update channel";
							menuItemSep1.Visible = false;
							menuItemImport.Visible = false;
							menuItemNewFolder.Visible = false;
							menuItemSep2.Visible = true;
							menuItemDelete.Text = "&Delete feed";
						}
						m_feedNodeRightClicked = feedNode;
					}
				}
			}
			else
			{
				m_feedNodeRightClicked = null;
			}
		}


		//-----------------------------------------------------------------------------------------

		#region Event handlers for the Context Menu.
		/// <summary>
		/// Update channel/group menu item handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuItemUpdate_Click(object sender, System.EventArgs e)
		{
			if (m_feedNodeRightClicked == null)
			{
				return;
			}
			if (m_feedNodeRightClicked.IsFolder)
			{
				UpdateFolder(m_feedNodeRightClicked);
			}
			else
			{
				UpdateChannelThread thread = new UpdateChannelThread(this, m_feedNodeRightClicked);
				thread.Start();
			}
		}


		/// <summary>
		/// Event handler for the "Import feed..." context menu item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuItemImport_Click(object sender, System.EventArgs e)
		{
			ImportFeedForm form = new ImportFeedForm();
			DialogResult dr = form.ShowDialog();
			if (dr == DialogResult.OK)
			{
				string strFeedUrl = form.FeedUrl;
				if (m_rootFeedNode.AlreadyExistsFeed(strFeedUrl))
				{
					//"A feed with the same URL already exists in the feeds tree"
					MessageBox.Show(this, Utils.GetResString("SameFeedExists"), Utils.GetResString("AppName"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					Utils.DeleteFile(form.TempFeedFile);
					return;
				}
				else
				{
					if (m_feedNodeRightClicked == null)
					{
						Debug.Assert(m_feedNodeRightClicked != null);
						return;
					}
					//Utils.MoveFile(form.TempFeedFile, );

					FeedNode feedNode = new FeedNode(form.FeedTitle, strFeedUrl, m_feedNodeRightClicked, false);
					m_feedNodeRightClicked.AddChildNode(feedNode);

					OnEnd_ParseFeeds();

					UpdateChannelThread thread = new UpdateChannelThread(this, feedNode);
					thread.SetFromTempFile(form.TempFeedFile);
					thread.Start();
				}
			}
		}


		/// <summary>
		/// Event handler for the context menu item "Delete".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuItemDelete_Click(object sender, System.EventArgs e)
		{
			if (m_feedNodeRightClicked == null)
			{
				Debug.Assert(m_feedNodeRightClicked != null, "The right-clicked node is unknown!");
				return;
			}

			FeedNode parent = m_feedNodeRightClicked.Parent;
			if (parent != null)
			{
				string text = String.Format("Do you really want to delete the {0}?",
					m_feedNodeRightClicked.IsFolder ? "folder and its contents" : "feed");
				MessageBoxButtons buttons = MessageBoxButtons.YesNo;
				if (MessageBox.Show(this, text, "FeedReader", buttons, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					parent.DeleteChildNode(m_feedNodeRightClicked);
					TreeNode tn = m_feedNodeRightClicked.Tag;
					tn.Remove();
				}
			}

			m_feedNodeRightClicked = null;
		}


		/// <summary>
		/// Event handler for the context menu item "New folder..."
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuItemNewFolder_Click(object sender, System.EventArgs e)
		{
			if (m_feedNodeRightClicked == null)
			{
				Debug.Assert(m_feedNodeRightClicked != null, "The right clicked node is not known!");
				return;
			}

			NewFolderForm form = new NewFolderForm();
			DialogResult dr = form.ShowDialog();
			if (dr == DialogResult.OK)
			{
				string strFolderName = form.FolderName;
				if (m_feedNodeRightClicked.AlreadyContainsFolder(strFolderName))
				{
					MessageBox.Show("A sub-folder with the same name already exists in the current folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					FeedNode fnNewFolder = new FeedNode(strFolderName, null, m_feedNodeRightClicked, true);
					m_feedNodeRightClicked.AddChildNode(fnNewFolder);

					TreeNode tnRightClicked = m_feedNodeRightClicked.Tag;
					tnRightClicked.Nodes.Clear();
					m_feedNodeRightClicked.PopulateTreeNodes(tnRightClicked, treeViewFeeds);

					treeViewFeeds.SelectedNode = fnNewFolder.Tag;
				}
			}
			m_feedNodeRightClicked = null;
		}


		/// <summary>
		/// Event handler for the context menu item "New folder..."
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItemProperties_Click(object sender, System.EventArgs e)
		{
			if (m_feedNodeRightClicked != null)
			{
				PropertiesForm form = new PropertiesForm(m_feedNodeRightClicked);
				DialogResult dr = form.ShowDialog();
				if (dr == DialogResult.OK)
				{
					TreeNode tnRightClicked = m_feedNodeRightClicked.Tag;
					tnRightClicked.Text = m_feedNodeRightClicked.Title;
				}
			}
		}
		#endregion

		/// <summary>
		/// Occurs whenever the 'SelectedIndex' property for this ListView changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listViewHeadlines_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (listViewHeadlines.SelectedItems.Count < 1)
				{
					return;
				}
				ListViewItem item = listViewHeadlines.SelectedItems[0];
				if (item != null)
				{
					Headline headline = (Headline) item.Tag;

					string strFileName = FileUtils.TempFolder + String.Format("{0:X}_{1:X}", headline.Title.GetHashCode(), headline.DatePublished.Ticks);

					if (!File.Exists(strFileName))
					{
						//StreamWriter sw = File.CreateText(strFileName);
						StreamWriter sw = new StreamWriter(strFileName, false, System.Text.Encoding.UTF8);
						//string str1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n\r\n";
						string str1 = "<html>\r\n\r\n<body>\r\n";
						str1 += "<a href=\"" + headline.Link + "\">" + headline.Title + "</a><hr />\r\n";
						string str2 = "\r\n</body>\r\n</html>\r\n";
						sw.Write(str1);
						sw.Write(headline.Description);
						sw.Write(str2);
						sw.Close();
					}
					NavigateTo(strFileName);
				}
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("FeedReaderForm::listViewHeadlines_SelectedIndexChanged()", ex);
			}
		}


		public void OnEnd_ParseFeeds()
		{
			try
			{
				treeViewFeeds.BeginUpdate();
				treeViewFeeds.Nodes.Clear();

				m_rootFeedNode.PopulateTreeNodes(null, treeViewFeeds);
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("FeedReaderForm::UpdateTree()", ex);
			}
			finally
			{
				treeViewFeeds.EndUpdate();
				treeViewFeeds.ExpandAll();
			}
		}


		public void OnEnd_ReadChannel(HeadlineCollection headlines)
		{
			bool bUpdating = false;
			try
			{
				m_headlines = headlines;
				bUpdating = true;
				listViewHeadlines.BeginUpdate();
				listViewHeadlines.Items.Clear();

				DateTime today = DateTime.Today;

				if ((headlines != null) && (headlines.Count > 0))
				{
					foreach (Headline headline in headlines)
					{
						ListViewItem item = new ListViewItem(headline.Title);
						item.Tag = headline;

						string strDatePublished;
						DateTime dtDatePublished = headline.DatePublished;
						if (Utils.IsSameDay(today, dtDatePublished))
						{
							strDatePublished = dtDatePublished.ToString("h:mm:ss tt");
						}
						else
						{
							strDatePublished = dtDatePublished.ToString();
						}
						item.SubItems.Add(strDatePublished);

						string strDateReceived;
						DateTime dtDateReceived = headline.DateReceived;
						if (Utils.IsSameDay(today, dtDateReceived))
						{
							strDateReceived = dtDateReceived.ToString("h:mm:ss tt");
						}
						else
						{
							strDateReceived = dtDateReceived.ToString();
						}
						item.SubItems.Add(strDateReceived);

						item.SubItems.Add(headline.Author);
						listViewHeadlines.Items.Add(item);
					}
				}
				else
				{
					ListViewItem item = new ListViewItem("(There are no downloaded headlines for this feed)");
					item.ForeColor = Color.FromArgb(96, 96, 96);
					listViewHeadlines.Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("FeedReaderForm::UpdateListView()", ex);
			}
			finally
			{
				if (bUpdating) { listViewHeadlines.EndUpdate(); }
			}
		}


		public void OnBegin_UpdateChannel(FeedNode feedNode)
		{
			TreeNode tn = (TreeNode)feedNode.Tag;
			if (tn != null)
			{
				tn.ImageIndex = 2;
				tn.SelectedImageIndex = 2;
			}
		}


		public void OnEnd_UpdateChannel(FeedNode feedNode)
		{
			TreeNode tn = (TreeNode)feedNode.Tag;
			if (tn != null)
			{
				if (feedNode.IsFolder)
				{
					tn.ImageIndex = 0;
					tn.SelectedImageIndex = 0;
				}
				else
				{
					tn.ImageIndex = 1;
					tn.SelectedImageIndex = 1;
				}
			}

			if (feedNode == m_feedNodeSelected)
			{
				ReadChannelThread thread = new ReadChannelThread(this, feedNode);
				thread.Start();
				NavigateTo("about:blank");
			}
		}


		/// <summary>
		/// Occurs when the WebBrowser control finishes loading a document.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axWebBrowser_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			IHTMLDocument2 htmlDoc = axWebBrowser.Document as IHTMLDocument2;
			if (htmlDoc == null)
			{
				Debug.WriteLine("Error: HTML document couldn't be obtained!");
				return;
			}

			IHTMLElementCollection heads = (IHTMLElementCollection)htmlDoc.all.tags("HEAD");
			if (heads != null)
			{
				IHTMLElement head = (IHTMLElement) heads.item(0, 0);
				IHTMLElement2 head2 = (IHTMLElement2) heads.item(0, 0);
				if (head != null)
				{
					string s = head.innerHTML;
					if ((s != null) && (s.Length > 0))
					{
						IHTMLElementCollection links = (IHTMLElementCollection) head2.getElementsByTagName("link");

						foreach (IHTMLElement link in links)
						{
							string strRel = link.getAttribute("rel", 0) as string;
							string strType = link.getAttribute("type", 0) as string;
							string strHref = link.getAttribute("href", 0) as string;
							if (strRel == "alternate")
							{
								if (strType == "application/rss+xml")
								{
									Debug.WriteLine("!!! Found RSS feed: " + strHref);
								}
								else if (strType == "application/atom+xml")
								{
									Debug.WriteLine("!!! Found Atom feed: " + strHref);
								}
							}
						}
					}
				}
			}

//			IHTMLElement body = (IHTMLElement) htmlDoc.body;
//			IHTMLElement2 body2 = (IHTMLElement2) htmlDoc.body;
			//string s = body.outerHTML;
			//string s = body.parentElement.innerHTML;
			// Create a collection of all the "input" elements in the page.
//			IHTMLElementCollection wbrAll = body2.getElementsByTagName("a");
//			foreach (IHTMLElement wbrElm in wbrAll)
//			{
//				object rel = wbrElm.getAttribute("href", 0);
//				if (wbrElm.tagName.ToLower() == "link" && wbrElm.outerHTML.IndexOf("name", 1) > 0)
//				{
//					string strName = wbrElm.getAttribute("name", 0).ToString();
//					Debug.WriteLine(wbrElm.tagName.ToLower() + " -- " + wbrElm.outerHTML);
//					// We are only interested in filling text boxes,
//					// and only interested in a specific  one, i.e. "q"    
//
////					if (strName != null && strName.ToLower() == userNameFormField)          // Set the "value" with the setAttribute method.
////						wbrElm.setAttribute("value", userNameValue, 0);
////
////					if (strName != null && strName.ToLower() == passwordFormField)          // Set the "value" with the setAttribute method.
////						wbrElm.setAttribute("value", passwordValue, 0);
//				}
//			}
		}


		private void axWebBrowser_DownloadBegin(object sender, System.EventArgs e)
		{
			//pnlStatus.Text = "Loading...";
		}


		private void axWebBrowser_StatusTextChange(object sender, AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEvent e)
		{
			pnlStatus.Text = e.text;
		}

		
		/// <summary>
		/// Occurs when the user begins dragging an item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeViewFeeds_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			TreeNode tn = (TreeNode)e.Item;
			FeedNode fn = (FeedNode)tn.Tag;

			if (fn != m_rootFeedNode)
			{	// Can't move the root node!
				m_dragNode = fn;

				this.DoDragDrop(fn, DragDropEffects.Move);
			}
		}


		/// <summary>
		/// Occurs when the mouse drags an item and is moving over the client area of this Control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeViewFeeds_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (m_dragNode == null)
			{	// If an object other than a FeedNode is being dragged...
				e.Effect = DragDropEffects.None;
				return;
			}

			TreeNode tn = FindTreeNode(e.X, e.Y);
			if (tn == null)
			{
				return;
			}

			FeedNode fn = (FeedNode)tn.Tag;

			if (fn.IsFolder)
			{
				e.Effect = DragDropEffects.Move;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}


		/// <summary>
		/// Occurs when the mouse drags an item into the client area for this Control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeViewFeeds_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Debug.WriteLine("DragEnter()");
		}


		/// <summary>
		/// Occurs when the mouse drags an item and the user releases the mouse indicating
		/// that the item should be 'dropped' into this Control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeViewFeeds_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (m_dragNode == null)
			{	// If an object other than a FeedNode is being dropped...
				return;
			}

			if (e.Effect != DragDropEffects.Move)
			{
				return;
			}

			// determine the node we are dropping into from the coordinates of the DragEvent argument
			TreeNode dropTreeNode = FindTreeNode(e.X, e.Y);
			FeedNode dropFeedNode = (FeedNode)dropTreeNode.Tag;

			if (m_dragNode.MoveTo(dropFeedNode, treeViewFeeds))
			{
				//this.Invalidate(new Region(this.ClientRectangle)); // redraw the treeview
				treeViewFeeds.SelectedNode = dropTreeNode; // select the drop node as the current selection
			}
			m_dragNode = null;
		}


		/// <summary>
		/// Finds a tree node in the TreeView by the node's coordinates.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		protected TreeNode FindTreeNode(int x, int y)
		{
			TreeNode aNode = (TreeNode)m_rootFeedNode.Tag;
			
			Point pt = new Point(x,y);
			pt = PointToClient(pt);

			while (aNode != null)
			{
				if (aNode.Bounds.Contains(pt))
				{
					return aNode;
				}
				aNode = aNode.NextVisibleNode;
			}

			return null;
		}


		/// <summary>
		/// Menu handler for File->Exit.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			// TODO: Stop all running threads.
			this.Close();
			Application.Exit();
		}


		/// <summary>
		/// Menu handler for File->Save.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItemSave_Click(object sender, System.EventArgs e)
		{
			WriteFeedsThread.QueueJob(m_rootFeedNode);
		}


		/// <summary>
		/// Occurs whenever the user closes the form, after the form has been closed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FeedReaderForm_Closed(object sender, System.EventArgs e)
		{
			Debug.WriteLine("FeedReaderForm_Closed()");
			WriteFeedsThread.QueueJob(m_rootFeedNode);
			WriteFeedsThread.Stop();
		}

		/// <summary>
		/// Menu handler for Help->About...
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			AboutForm form = new AboutForm();
			form.ShowDialog(this);
		}


		/// <summary>
		/// Repositions the WebBrowser control to fit between the V Splitter on the left,
		/// the dialog border on the right, the bottom of the browserHearder on the top,
		/// and the top of the status bar on the bottom.
		/// </summary>
		private void RepositionBrowser()
		{
			axWebBrowser.Left = browserHeader1.Left;
			axWebBrowser.Top = browserHeader1.Bottom + 1;
			axWebBrowser.Width = browserHeader1.Width;
			axWebBrowser.Height = statusBarMain.Top - axWebBrowser.Top;
		}


		private void FeedReaderForm_Resize(object sender, System.EventArgs e)
		{
			RepositionBrowser();
		}


		private void splitterH_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			RepositionBrowser();
		}


		private void splitterV_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			RepositionBrowser();
		}


		private void browserHeader1_GoButtonPressed(object sender, System.EventArgs e)
		{
			NavigateTo(browserHeader1.Address);
		}


		private void browserHeader1_BackButtonPressed(object sender, System.EventArgs e)
		{
			axWebBrowser.GoBack();
		}


		private void browserHeader1_ForwardButtonPressed(object sender, System.EventArgs e)
		{
			axWebBrowser.GoForward();
		}


		private void axWebBrowser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			//browserHeader1.Address = (string) e.uRL;
		}


		private void axWebBrowser_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			browserHeader1.Address = (string) e.uRL;
		}


		/// <summary>
		/// Fires when the enabled state of a command changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axWebBrowser_CommandStateChange(object sender, AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEvent e)
		{
			if (e.command == (int)SHDocVw.CommandStateChangeConstants.CSC_NAVIGATEBACK)
			{
				browserHeader1.EnableBack(e.enable);
			}
			else if (e.command == (int)SHDocVw.CommandStateChangeConstants.CSC_NAVIGATEFORWARD)
			{
				browserHeader1.EnableForward(e.enable);
			}
		}
	}
}
