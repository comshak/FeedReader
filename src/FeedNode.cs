using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Globalization;

// TODO: What happens with nodes whose Title contains a path separator (forward or back slash)?

namespace com.comshak.FeedReader
{
	public class FeedNodeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			FeedNode f1 = x as FeedNode;
			FeedNode f2 = y as FeedNode;
			if ((f1 == null) || (f2 == null))
			{
				return 0;
			}
			bool b1 = f1.IsFolder;
			bool b2 = f2.IsFolder;
			if (b1 && !b2)
			{
				return -1;
			}
			else if (!b1 && b2)
			{
				return 1;
			}
			return String.Compare(f1.Title, f2.Title, true);
		}
	}

	/// <summary>
	/// Represents a node in a feeds tree.
	/// </summary>
	public class FeedNode
	{
		private string     m_strText;           // 1. "text" attribute of the "outline" element
		private string     m_strTitle;          // 2. "title" attribute of the "outline" element
		//private string     m_strType;			// 3. "type" attribute of the "outline" element
		//private string     m_strVersion;		// 4. "version" attribute of the "outline" element
		private string     m_strXmlUrl;			// 5. "xmlUrl" attribute of the "outline" element
		private string     m_strHtmlUrl;        // 6. "htmlUrl" attribute of the "outline" element
		private string     m_strDescription;	// 7. "description" attribute of the "outline" element

		private bool       m_bFolder;
		private string     m_strPath;
		private FeedNode   m_parent;			// link to its parent in the feeds tree
		private TreeNode   m_treeNode;			// link to its node in the tree
		private SortedList m_listChildren;		// list of children of the current node

		public FeedNode(string strTitle, string strXmlUrl, FeedNode parent, bool bFolder)
		{
			m_strTitle = strTitle;
			m_strText  = strTitle;
			m_parent   = parent;
			m_bFolder  = bFolder;
			if (!bFolder)
			{
				m_strXmlUrl = strXmlUrl;
			}
			else
			{
				m_listChildren = new SortedList(new FeedNodeComparer());
				m_strXmlUrl = String.Empty;
			}
			if (parent != null)
			{
				m_strPath = parent.Path;
				if (!parent.Path.Equals("\\"))
				{
					m_strPath += "\\";
				}
				m_strPath += m_strTitle;
			}
			else
			{
				m_strPath = "\\";
			}
		}

		/// <summary>
		/// Adds a child FeedNode to the current node.
		/// </summary>
		/// <param name="node"></param>
		public void AddChildNode(FeedNode node)
		{
			node.m_parent = this;
			DumpChildren();
			m_listChildren.Add(node, node.Title);
		}


		/// <summary>
		/// Removes a FeedNode from the child nodes of the current node.
		/// </summary>
		/// <param name="node"></param>
		public void DeleteChildNode(FeedNode node)
		{
			// TODO: Make sure this Remove is okay as far as uniqueness goes.
			m_listChildren.Remove(node);
		}


		#region Public Properties
		public string Text
		{
			get { return m_strText; }
			set { m_strText = value; }
		}

		public string Title
		{
			get { return m_strTitle; }
			set { m_strTitle = value; }
		}

		public string Path
		{
			get { return m_strPath; }
		}

		public string XmlUrl
		{
			get { return m_strXmlUrl; }
		}

		public string HtmlUrl
		{
			get { return m_strHtmlUrl; }
			set { m_strHtmlUrl = value; }
		}

		public string Description
		{
			get { return m_strDescription; }
			set { m_strDescription = value; }
		}

		public FeedNode Parent
		{
			get { return m_parent; }
			set { m_parent = value; }
		}

		public bool IsFolder
		{
			get { return m_bFolder; }
		}

		public TreeNode Tag
		{
			get { return m_treeNode;}
			set { m_treeNode = value; }
		}

		public string FileName
		{
			get
			{
				if (m_bFolder)
				{
					return String.Empty;
				}

				char[] chIllegalChars = new char[]{'\\', '/', ':', '*', '?', '"', '<', '>', '|'};
				string strTitle = m_strTitle;

				int iPos = 0;
				do
				{
					iPos = strTitle.IndexOfAny(chIllegalChars, iPos);
					if (iPos == -1)
					{
						break;
					}
					char ch = strTitle[iPos];
					string strOld = String.Format("{0}", ch);
					string strNew = String.Format("&#{0:X};", (int)ch);
					strTitle = strTitle.Replace(strOld, strNew);
				}
				while (iPos >= 0);
				return strTitle + ".rss";
			}
		}

		public IDictionaryEnumerator Enumerator
		{
			get
			{
				if (m_listChildren != null)
				{
					return m_listChildren.GetEnumerator();
				}
				return null;
			}
		}

		public string Status
		{
			get
			{
				if (m_bFolder)
				{
					return String.Format("Folder \"{0}\" ({1} children)", m_strTitle, m_listChildren.Count);
				}
				return String.Format("Feed \"{0}\" ({1})", m_strTitle, m_strXmlUrl);
			}
		}
		#endregion

		/// <summary>
		/// Populates the tree nodes of the current FeedNode.
		/// Recursive method to generate the TreeView tree structure
		/// in accordance to the FeedNode tree structure.
		/// </summary>
		/// <param name="crtTreeNode">TreeNode with which this FeedNode is to be associated with.</param>
		public void PopulateTreeNodes(TreeNode crtTreeNode, System.Windows.Forms.TreeView tvFeeds)
		{
			if ((m_parent == null) && (crtTreeNode == null))
			{	// root node?
				TreeNode treeNode = tvFeeds.Nodes.Add(m_strTitle);
				treeNode.Tag = this;
				this.Tag = treeNode;
				crtTreeNode = treeNode;
			}
			IDictionaryEnumerator enumNodes = this.Enumerator;
			while (enumNodes.MoveNext())
			{
				FeedNode feedNode = (FeedNode)enumNodes.Key;
				if (feedNode != null)
				{
					TreeNode treeNode = crtTreeNode.Nodes.Add(feedNode.Title);
					treeNode.Tag = feedNode;
					feedNode.Tag = treeNode;
					if (feedNode.IsFolder)
					{
						treeNode.ImageIndex = 0;
						treeNode.SelectedImageIndex = 0;
						feedNode.PopulateTreeNodes(treeNode, tvFeeds);
					}
					else
					{
						treeNode.ImageIndex = 1;
						treeNode.SelectedImageIndex = 1;
					}
				}
			}
		}

		/// <summary>
		/// Go through all the child nodes (leaf nodes and folder nodes), starting from the current node, and see if
		/// the provided FeedUrl already exists in any of the child nodes.
		/// </summary>
		/// <param name="strFeedUrl"></param>
		/// <returns></returns>
		public bool AlreadyExistsFeed(string strFeedUrl)
		{
			IDictionaryEnumerator enumNodes = m_listChildren.GetEnumerator();
			while (enumNodes.MoveNext())
			{
				FeedNode feedNode = (FeedNode)enumNodes.Key;
				if (feedNode != null)
				{
					if (feedNode.IsFolder)
					{
						if (feedNode.AlreadyExistsFeed(strFeedUrl))
						{
							return true;
						}
					}
					else
					{
						if (strFeedUrl.Equals(feedNode.m_strXmlUrl))
						{
							return true;
						}
					}
				}
			}
			return false;
		}


		/// <summary>
		/// Tests whether there is already a folder with the same name in the current folder.
		/// </summary>
		/// <param name="strFolderName"></param>
		/// <returns></returns>
		public bool AlreadyContainsFolder(string strFolderName)
		{
			if (!IsFolder)
			{
				return false;
			}
			IDictionaryEnumerator enumNodes = m_listChildren.GetEnumerator();
			while (enumNodes.MoveNext())
			{
				FeedNode feedNode = (FeedNode)enumNodes.Key;
				if ((feedNode != null) && (feedNode.IsFolder))
				{
					if (String.Compare(feedNode.Title, strFolderName, true, CultureInfo.InvariantCulture) == 0)
					{
						return true;
					}
				}
			}
			return false;
		}


		/// <summary>
		/// Recursive method to duplicate (deep copy) the current node and its children.
		/// </summary>
		/// <param name="fnParent">The FeedNode to which the new node will be parented under.</param>
		/// <returns>The new FeedNode which contains the same information as the current node (this).</returns>
		public FeedNode Duplicate(FeedNode fnParent)
		{
			FeedNode fnNew = new FeedNode(Title, XmlUrl, fnParent, IsFolder);
			fnNew.m_strText = m_strText;
			fnNew.m_strHtmlUrl = m_strHtmlUrl;
			fnNew.m_strDescription = m_strDescription;
			fnNew.m_strPath = m_strPath;
			if (fnParent != null)
			{
				fnParent.AddChildNode(fnNew);
			}

			if (m_bFolder)
			{
				IDictionaryEnumerator enumNodes = m_listChildren.GetEnumerator();
				while (enumNodes.MoveNext())
				{
					FeedNode feedNode = (FeedNode)enumNodes.Key;
					if (feedNode != null)
					{
						feedNode.Duplicate(fnNew);
					}
				}
			}
			return fnNew;
		}


		/// <summary>
		/// Moves the current FeedNode from one folder to another.
		/// </summary>
		/// <param name="fnDest">Destination folder that this FeedNode will be moved to.</param>
		/// <returns></returns>
		public bool MoveTo(FeedNode fnDest, System.Windows.Forms.TreeView tvFeeds)
		{
			Debug.WriteLine(String.Format("MoveTo() {0} -> {1}", this.ToString(), fnDest.ToString()));

			if (fnDest.IsFolder == false)
			{
				return false;	// Can't move into anything else than a folder.
			}
			if (m_parent == fnDest)
			{
				return false;	// The FeedNode can't be moved into the same folder it already is in.
			}

			TreeNode tnOld = (TreeNode) Tag;
			tnOld.Remove();		// remove the treenode from the old folder

			m_parent = fnDest;
			fnDest.AddChildNode(this);

			TreeNode tnDest = (TreeNode) fnDest.Tag;
			tnDest.Nodes.Clear();

			fnDest.PopulateTreeNodes(tnDest, tvFeeds);

			return true;
		}


		/// <summary>
		/// Recursively dumps the current node and its children into an OPML file.
		/// </summary>
		/// <param name="writer"></param>
		public void DumpAsOpml(XmlWriter writer)
		{
			// Start the element
			if (m_bFolder)
			{
				if (m_parent == null)
				{	// root node
					writer.WriteStartElement("feeds");
				}
				else
				{
					writer.WriteStartElement("group");
					writer.WriteAttributeString("title", m_strTitle);
				}

				// Write the children
				IDictionaryEnumerator enumNodes = m_listChildren.GetEnumerator();
				while (enumNodes.MoveNext())
				{
					FeedNode feedNode = (FeedNode)enumNodes.Key;
					if (feedNode != null)
					{
						feedNode.DumpAsOpml(writer);
					}
				}
			}
			else
			{
				writer.WriteStartElement("outline");
				writer.WriteAttributeString("text", m_strText);
				writer.WriteAttributeString("title", m_strTitle);
				writer.WriteAttributeString("type", "rss");
				writer.WriteAttributeString("version", "RSS");
				writer.WriteAttributeString("xmlUrl", m_strXmlUrl);
				writer.WriteAttributeString("htmlUrl", m_strHtmlUrl);
				writer.WriteAttributeString("description", m_strDescription);
			}

			// End the element
			writer.WriteEndElement();
		}

		public override string ToString()
		{
			return Status;
		}

		/// <summary>
		/// For debugging purposes, shows the children nodes of the current node.
		/// </summary>
		public void DumpChildren()
		{
			Debug.WriteLine( "\t-KEY-\t-VALUE-" );
			for ( int i = 0; i < m_listChildren.Count; i++ )  
			{
				Debug.WriteLine(String.Format("\t{0}:\t{1}", m_listChildren.GetKey(i), m_listChildren.GetByIndex(i)));
			}
			Debug.WriteLine("");
		}

		/// <summary>
		/// Deletes all child nodes of the current node.
		/// </summary>
		public void Clear()
		{
			FeedNode[] keys = new FeedNode[m_listChildren.Keys.Count];
			m_listChildren.Keys.CopyTo(keys, 0);

			foreach(FeedNode key in keys)
			{
				if (key.IsFolder)
				{
					key.Clear();
				}
				m_listChildren.Remove(key);
			}
		}
	}
}
