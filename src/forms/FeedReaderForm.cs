using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Text;

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
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.StatusBar statusBarMain;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.TreeView treeViewFeeds;
		private System.Windows.Forms.ListView listViewHeadlines;
		private AxSHDocVw.AxWebBrowser axWebBrowser;
		private System.Windows.Forms.ContextMenu contextMenu;
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
		private ColumnHeader colHdrCategory;
		private MenuItem menuItemGC;
		private ColumnHeader colHdrIcon;
		private ToolStrip toolbar;
		private ToolStripButton tsbNewFeed;
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
			m_imgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.feed.ico")));			// 1
			m_imgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.sync.ico")));			// 2
			treeViewFeeds.ImageList = m_imgList;

			AssemblyName an = a.GetName();
			Text = an.Name + " v" + an.Version;

			this.Size = new Size(1024, 768);
			ImageList lvImgList = new ImageList();
			lvImgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.text.ico")));			// 0
			lvImgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.download.ico")));		// 1
			lvImgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.play.ico")));			// 2
			lvImgList.Images.Add(Image.FromStream(a.GetManifestResourceStream("FeedReader.res.hourglass.ico")));	// 3
			listViewHeadlines.SmallImageList = lvImgList;
			listViewHeadlines.Height = (this.ClientSize.Height - statusBarMain.Height) / 2;
			treeViewFeeds.Width = 250;
			colHdrIcon.Width = 18;
			colHdrTitle.Width = 285;
			colHdrPublished.Width = 130;
			colHdrReceived.Width = 130;
			colHdrAuthor.Width = 100;
			colHdrCategory.Width = 80;
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
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemSave = new System.Windows.Forms.MenuItem();
			this.menuItemFileSep1 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemGC = new System.Windows.Forms.MenuItem();
			this.menuItemAbout = new System.Windows.Forms.MenuItem();
			this.statusBarMain = new System.Windows.Forms.StatusBar();
			this.pnlStatus = new System.Windows.Forms.StatusBarPanel();
			this.pnlProgress = new System.Windows.Forms.StatusBarPanel();
			this.treeViewFeeds = new System.Windows.Forms.TreeView();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
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
			this.colHdrIcon = new System.Windows.Forms.ColumnHeader();
			this.colHdrTitle = new System.Windows.Forms.ColumnHeader();
			this.colHdrPublished = new System.Windows.Forms.ColumnHeader();
			this.colHdrReceived = new System.Windows.Forms.ColumnHeader();
			this.colHdrAuthor = new System.Windows.Forms.ColumnHeader();
			this.colHdrCategory = new System.Windows.Forms.ColumnHeader();
			this.splitterH = new System.Windows.Forms.Splitter();
			this.axWebBrowser = new AxInterop.SHDocVw.AxWebBrowser();
			this.toolbar = new System.Windows.Forms.ToolStrip();
			this.tsbNewFeed = new System.Windows.Forms.ToolStripButton();
			this.browserHeader1 = new com.comshak.FeedReader.BrowserHeader();
			((System.ComponentModel.ISupportInitialize)(this.pnlStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pnlProgress)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).BeginInit();
			this.toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
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
            this.menuItemGC,
            this.menuItemAbout});
			this.menuItemHelp.Text = "&Help";
			// 
			// menuItemGC
			// 
			this.menuItemGC.Index = 0;
			this.menuItemGC.Text = "Collect Garbage";
			this.menuItemGC.Click += new System.EventHandler(this.menuItemGC_Click);
			// 
			// menuItemAbout
			// 
			this.menuItemAbout.Index = 1;
			this.menuItemAbout.Text = "&About...";
			this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
			// 
			// statusBarMain
			// 
			this.statusBarMain.Location = new System.Drawing.Point(0, 184);
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
			this.pnlStatus.Width = 642;
			// 
			// pnlProgress
			// 
			this.pnlProgress.Name = "pnlProgress";
			// 
			// treeViewFeeds
			// 
			this.treeViewFeeds.AllowDrop = true;
			this.treeViewFeeds.ContextMenu = this.contextMenu;
			this.treeViewFeeds.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewFeeds.HideSelection = false;
			this.treeViewFeeds.Location = new System.Drawing.Point(0, 25);
			this.treeViewFeeds.Name = "treeViewFeeds";
			this.treeViewFeeds.Size = new System.Drawing.Size(128, 159);
			this.treeViewFeeds.TabIndex = 1;
			this.treeViewFeeds.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewFeeds_DragDrop);
			this.treeViewFeeds.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFeeds_AfterSelect);
			this.treeViewFeeds.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewFeeds_MouseDown);
			this.treeViewFeeds.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewFeeds_DragEnter);
			this.treeViewFeeds.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeViewFeeds_KeyUp);
			this.treeViewFeeds.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewFeeds_ItemDrag);
			this.treeViewFeeds.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewFeeds_DragOver);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
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
			this.splitterV.Location = new System.Drawing.Point(128, 25);
			this.splitterV.Name = "splitterV";
			this.splitterV.Size = new System.Drawing.Size(3, 159);
			this.splitterV.TabIndex = 2;
			this.splitterV.TabStop = false;
			// 
			// listViewHeadlines
			// 
			this.listViewHeadlines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHdrIcon,
            this.colHdrTitle,
            this.colHdrPublished,
            this.colHdrReceived,
            this.colHdrAuthor,
            this.colHdrCategory});
			this.listViewHeadlines.Dock = System.Windows.Forms.DockStyle.Top;
			this.listViewHeadlines.FullRowSelect = true;
			this.listViewHeadlines.HideSelection = false;
			this.listViewHeadlines.Location = new System.Drawing.Point(131, 25);
			this.listViewHeadlines.MultiSelect = false;
			this.listViewHeadlines.Name = "listViewHeadlines";
			this.listViewHeadlines.Size = new System.Drawing.Size(628, 152);
			this.listViewHeadlines.TabIndex = 2;
			this.listViewHeadlines.UseCompatibleStateImageBehavior = false;
			this.listViewHeadlines.View = System.Windows.Forms.View.Details;
			this.listViewHeadlines.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewHeadlines_MouseClick);
			this.listViewHeadlines.SelectedIndexChanged += new System.EventHandler(this.listViewHeadlines_SelectedIndexChanged);
			this.listViewHeadlines.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewHeadlines_MouseMove);
			this.listViewHeadlines.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listViewHeadlines_ColumnWidthChanging);
			// 
			// colHdrIcon
			// 
			this.colHdrIcon.Text = "";
			this.colHdrIcon.Width = 18;
			// 
			// colHdrTitle
			// 
			this.colHdrTitle.Text = "Title";
			this.colHdrTitle.Width = 186;
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
			// colHdrCategory
			// 
			this.colHdrCategory.Text = "Category";
			// 
			// splitterH
			// 
			this.splitterH.BackColor = System.Drawing.SystemColors.Control;
			this.splitterH.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitterH.Location = new System.Drawing.Point(131, 177);
			this.splitterH.Name = "splitterH";
			this.splitterH.Size = new System.Drawing.Size(628, 3);
			this.splitterH.TabIndex = 4;
			this.splitterH.TabStop = false;
			// 
			// axWebBrowser
			// 
			this.axWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser.Enabled = true;
			this.axWebBrowser.Location = new System.Drawing.Point(131, 211);
			this.axWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser.OcxState")));
			this.axWebBrowser.Size = new System.Drawing.Size(628, 0);
			this.axWebBrowser.TabIndex = 5;
			this.axWebBrowser.BeforeNavigate2 += new AxInterop.SHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.axWebBrowser_BeforeNavigate2);
			this.axWebBrowser.StatusTextChange += new AxInterop.SHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler(this.axWebBrowser_StatusTextChange);
			this.axWebBrowser.DownloadBegin += new System.EventHandler(this.axWebBrowser_DownloadBegin);
			this.axWebBrowser.CommandStateChange += new AxInterop.SHDocVw.DWebBrowserEvents2_CommandStateChangeEventHandler(this.axWebBrowser_CommandStateChange);
			this.axWebBrowser.NavigateComplete2 += new AxInterop.SHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser_NavigateComplete2);
			this.axWebBrowser.DocumentComplete += new AxInterop.SHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser_DocumentComplete);
			// 
			// toolbar
			// 
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewFeed});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.Size = new System.Drawing.Size(759, 25);
			this.toolbar.TabIndex = 7;
			// 
			// tsbNewFeed
			// 
			this.tsbNewFeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbNewFeed.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewFeed.Image")));
			this.tsbNewFeed.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbNewFeed.Name = "tsbNewFeed";
			this.tsbNewFeed.Size = new System.Drawing.Size(23, 22);
			this.tsbNewFeed.Text = "New feed";
			// 
			// browserHeader1
			// 
			this.browserHeader1.Address = "";
			this.browserHeader1.Dock = System.Windows.Forms.DockStyle.Top;
			this.browserHeader1.Location = new System.Drawing.Point(131, 180);
			this.browserHeader1.Name = "browserHeader1";
			this.browserHeader1.Size = new System.Drawing.Size(628, 31);
			this.browserHeader1.TabIndex = 6;
			this.browserHeader1.ForwardButtonPressed += new System.EventHandler(this.browserHeader1_ForwardButtonPressed);
			this.browserHeader1.GoButtonPressed += new System.EventHandler(this.browserHeader1_GoButtonPressed);
			this.browserHeader1.BackButtonPressed += new System.EventHandler(this.browserHeader1_BackButtonPressed);
			// 
			// FeedReaderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(759, 206);
			this.Controls.Add(this.axWebBrowser);
			this.Controls.Add(this.browserHeader1);
			this.Controls.Add(this.splitterH);
			this.Controls.Add(this.listViewHeadlines);
			this.Controls.Add(this.splitterV);
			this.Controls.Add(this.treeViewFeeds);
			this.Controls.Add(this.toolbar);
			this.Controls.Add(this.statusBarMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "FeedReaderForm";
			this.Text = "FeedReader";
			this.Load += new System.EventHandler(this.FeedReaderForm_Load);
			this.Closed += new System.EventHandler(this.FeedReaderForm_Closed);
			((System.ComponentModel.ISupportInitialize)(this.pnlStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pnlProgress)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).EndInit();
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FeedReaderForm_Load(object sender, System.EventArgs e)
		{
			ParseFeedsThread thread = new ParseFeedsThread(this);
			thread.Start();

			axWebBrowser.Silent = true;		// Suppress script errors!

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

			PopulateHeadlinesList(treeNode);
		}

		private void PopulateHeadlinesList(TreeNode treeNode)
		{
			if (treeNode != null)
			{
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
			}
			NavigateTo("about:blank");
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
						SetupContextMenu(feedNode);
						m_feedNodeRightClicked = feedNode;
					}
				}
			}
			else
			{
				m_feedNodeRightClicked = null;
			}
		}

		/// <summary>
		/// Occurs when a key is first pressed.
		/// 
		/// Needs to check for the pressing of the Apps key to display the context menu for the current node.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeViewFeeds_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Apps)
			{
				TreeNode treeNode = treeViewFeeds.SelectedNode;
				if (treeNode != null)
				{
					FeedNode feedNode = (FeedNode)treeNode.Tag;
					if (feedNode != null)
					{
						SetupContextMenu(feedNode);
					}
					m_feedNodeRightClicked = feedNode;

					Point pt = new Point(treeNode.Bounds.Left, treeNode.Bounds.Bottom);
					contextMenu.Show(treeViewFeeds, pt);
				}
			}
		}

		/// <summary>
		/// Sets up the treeview's context menu items for a specified node.
		/// </summary>
		/// <param name="feedNode"></param>
		private void SetupContextMenu(FeedNode feedNode)
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
					feedNode.HtmlUrl = form.HtmlUrl;
					feedNode.Description = form.FeedDescription;

					m_feedNodeRightClicked.AddChildNode(feedNode);

					OnEnd_ParseFeeds();

					SelectTreeNode(feedNode);

					UpdateChannelThread thread = new UpdateChannelThread(this, feedNode);
					thread.SetFromTempFile(form.TempFeedFile);
					thread.Start();
				}
			}
		}

		/// <summary>
		/// Selects the TreeNode corresponding to the specified FeedNode.
		/// </summary>
		/// <param name="feedNode"></param>
		private void SelectTreeNode(FeedNode feedNode)
		{
			// Now select the newly added node in the tree.
			if (feedNode != null && feedNode.Tag != null)
			{
				treeViewFeeds.SelectedNode = feedNode.Tag;

				// Now read the channel
				m_feedNodeSelected = feedNode;
				ReadChannelThread thread = new ReadChannelThread(this, feedNode);
				thread.Start();
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

					m_feedNodeSelected = null;
					if (treeViewFeeds.SelectedNode != null)
					{
						m_feedNodeSelected = (FeedNode) treeViewFeeds.SelectedNode.Tag;
					}

					PopulateHeadlinesList(treeViewFeeds.SelectedNode);
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
				Debug.Assert(m_feedNodeRightClicked != null, "The right-clicked node is unknown!");
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

					string strFileName = FileUtils.TempFolder + String.Format("{0:X}_{1:X}.htm", headline.Title.GetHashCode(), headline.DatePublished.Ticks);

					if (!File.Exists(strFileName))
					{
						StreamWriter sw = new StreamWriter(strFileName, false, System.Text.Encoding.UTF8);
						StringBuilder sb = new StringBuilder();
						sb.Append("<html>\r\n<body style=\"margin:0px; font-family:Georgia,Serif\">\r\n");
						sb.Append("<div style=\"padding:5px 5px 10px 5px; background-color:#C0C0C0\">");
						sb.Append(String.Format("<a href=\"{0}\">{1}</a></div>\r\n", headline.Link, headline.Title));
						sb.Append("<div style=\"margin:8px\">");
						sb.Append(headline.Description);
						sb.Append("</div>\r\n</body>\r\n</html>");

						sw.Write(sb.ToString());
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

		private void listViewHeadlines_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			if (e.ColumnIndex == 0)	// Prevent resizing the icon column
			{
				e.Cancel = true;
				e.NewWidth = 18;
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

				// Select the root node to bring it into view.
				if (treeViewFeeds.Nodes.Count > 0)
				{
					treeViewFeeds.SelectedNode = treeViewFeeds.Nodes[0];
				}
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
						ListViewItem item = new ListViewItem(String.Empty, headline.Enclosure.ListIcon);
						item.Tag = headline;

						item.SubItems.Add(headline.Title);

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

						item.SubItems.Add(headline.Category);

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
			DownloadThread.Stop();
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

		private void menuItemGC_Click(object sender, EventArgs e)
		{
			GC.Collect();
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

		private void listViewHeadlines_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.X < 19)
			{
				ListViewItem item = listViewHeadlines.GetItemAt(e.X, e.Y);

				if (item != null && item.Tag != null)
				{
					Headline headline = item.Tag as Headline;
					if (headline != null)
					{
						if (headline.Enclosure.ListIcon > 0)
						{
							listViewHeadlines.Cursor = Cursors.Hand;
							return;
						}
					}
				}
			}
			listViewHeadlines.Cursor = this.Cursor;
		}

		private void listViewHeadlines_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.X < 19)
			{
				ListViewItem item = listViewHeadlines.GetItemAt(e.X, e.Y);

				if (item != null && item.Tag != null)
				{
					Headline headline = item.Tag as Headline;
					if (headline != null)
					{
						Enclosure enclosure = headline.Enclosure;
						if (enclosure.ListIcon == 1)
						{
							//DownloadThread.Add(enclosure.Url, enclosure.GetFile());
							item.ImageIndex = 3;
						}
						else if (enclosure.ListIcon == 2)
						{
							//string file = enclosure.GetFile();
							//if (File.Exists(file))
							//{
							//	System.Diagnostics.Process.Start(file);
							//}
						}
					}
				}
			}
		}
	}
}
