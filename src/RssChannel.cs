using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for RssChannel.
	/// </summary>
	public class RssChannel
	{
		#region Private Fields
		private string    m_strTitle;
		private string    m_strLink;
		private string    m_strDescription;
		private string    m_strGenerator;
		private string    m_strFileName;
		private ArrayList m_arrItems;
		private string    m_strXPath;
		#endregion

		public RssChannel()
		{
			m_arrItems = new ArrayList();
		}

		public void AddItem(RssItem item)
		{
			m_arrItems.Add(item);
		}

		public RssItem ExistingItem(RssItem newItem)
		{
			RssItem existing = null;
			if (newItem == null)
			{
				return existing;
			}
			IEnumerator enumItems = m_arrItems.GetEnumerator();
			while (enumItems.MoveNext())
			{
				RssItem item = (RssItem) enumItems.Current;
				if (item != null)
				{
					if ((item.PublishedDate == newItem.PublishedDate) && (item.Title == newItem.Title))
					{
						existing = item;
						break;
					}
				}
			}
			return existing;
		}

		public void AddIfNew(RssItem newItem)
		{
			if (newItem == null)
			{
				return;
			}
			RssItem existing = ExistingItem(newItem);
			bool bExisting = (existing != null);
			if (!bExisting)
			{
				m_arrItems.Add(newItem);
			}
			else
			{
				string strMsg = String.Format("existing.ReceivedDate = {0}; newItem.ReceivedDate = {1}", existing.ReceivedDate, newItem.ReceivedDate);
				Debug.WriteLine(strMsg);

				existing.ReceivedDate = newItem.ReceivedDate;
			}
			string strPrefix = (bExisting) ? String.Empty : " NOT";
			Debug.WriteLine(newItem.Title + " (" + newItem.PublishedDate + ") was" + strPrefix + " already found in the channel.");
		}

		#region Public Properties
		public string Title
		{
			get { return m_strTitle; }
			set { m_strTitle = value.Trim(); }
		}

		public string Link
		{
			get { return m_strLink; }
			set { m_strLink = value; }
		}

		public string Description
		{
			get { return m_strDescription; }
			set { m_strDescription = value; }
		}

		public string Generator
		{
			get { return m_strGenerator; }
			set { m_strGenerator = value; }
		}

		public string FileName
		{
			set { m_strFileName = value.Trim(); }
		}
		#endregion

		public IEnumerator GetEnumerator()
		{
			return m_arrItems.GetEnumerator();
		}

		public void Write()
		{
			try
			{
				XmlTextWriter xmlWriter = new XmlTextWriter(m_strFileName, Encoding.UTF8);
				xmlWriter.Formatting = Formatting.Indented;

				xmlWriter.WriteStartElement("rss");
				xmlWriter.WriteAttributeString("xmlns", "dc", null, "http://purl.org/dc/elements/1.1/");
				xmlWriter.WriteAttributeString("xmlns", "wfw", null, "http://wellformedweb.org/CommentAPI/");
				xmlWriter.WriteAttributeString("xmlns", "comshak", null, "http://www.comshak.com/feedreader/");

				xmlWriter.WriteStartElement("channel");

				xmlWriter.WriteStartElement("title");
				xmlWriter.WriteString(m_strTitle);
				xmlWriter.WriteEndElement();

				xmlWriter.WriteStartElement("link");
				xmlWriter.WriteString(m_strLink);
				xmlWriter.WriteEndElement();

				xmlWriter.WriteStartElement("description");
				xmlWriter.WriteString(m_strDescription);
				xmlWriter.WriteEndElement();

				xmlWriter.WriteStartElement("generator");
				xmlWriter.WriteString(m_strGenerator);
				xmlWriter.WriteEndElement();

				IEnumerator enumItems = GetEnumerator();
				while (enumItems.MoveNext())
				{// Write all children
					RssItem item = (RssItem) enumItems.Current;
					if (item != null)
					{
						item.Write(xmlWriter);
					}
				}

				xmlWriter.WriteEndElement();	// channel
				xmlWriter.WriteEndElement();	// rss

				xmlWriter.Close();
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("RssChannel::Write()", ex);
			}
		}

		// Read the items from the rss channel stored on the local storage, and add them to this collection.
		// Be sure to keep the date from the existing item (copy it to the current element).
		public void Merge()
		{
			try
			{
				if (!File.Exists(m_strFileName))
				{
					Write();
					return;
				}

				XmlTextReader xmlReader = new XmlTextReader(m_strFileName);

				string strElementName;
				RssItem rssItem = null;
				while (xmlReader.Read())
				{
					XmlNodeType type = xmlReader.NodeType;
					if (type == XmlNodeType.Element)
					{
						strElementName = xmlReader.Name;
						if (!xmlReader.IsEmptyElement && UpdateXPath(strElementName, true) == "/rss/channel/item")
						{
							if (!xmlReader.Read() || (xmlReader.NodeType != XmlNodeType.Text))
							{
								continue;
							}
							if (rssItem == null)
							{
								rssItem = new RssItem();
							}
							rssItem.MergeElement(strElementName, xmlReader.Value);
						}
					}
					else if (type == XmlNodeType.EndElement)
					{
						strElementName = xmlReader.Name;
						UpdateXPath(strElementName, false);
						if ((strElementName == "item") && (m_strXPath == "/rss/channel") && (rssItem != null))
						{
							AddIfNew(rssItem);
							rssItem = null;
						}
					}
				}
				xmlReader.Close();
				Write();
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("RssChannel::Merge()", ex);
			}
		}

		private string UpdateXPath(string strElementName, bool bAdding)
		{
			string strLastXPath;
			if ((m_strXPath == null) || (m_strXPath.Length == 0))
			{
				m_strXPath = "/";
			}

			strLastXPath = m_strXPath;

			if (bAdding == true)
			{
				if ((strElementName == null) || (strElementName.Length == 0))
				{
					Debug.WriteLine("Error: Nothing to add!");
					return strLastXPath;
				}
				if (m_strXPath != "/")
				{
					m_strXPath += "/";
				}
				m_strXPath += strElementName;
				return strLastXPath;
			}
			else
			{
				if (m_strXPath == "/")
				{
					Debug.WriteLine("Error: Nothing to remove!");
					return String.Empty;
				}

				int iPos = m_strXPath.LastIndexOf('/');
				if ((iPos < 0) ||
					(m_strXPath[iPos] != '/') ||
					(m_strXPath.Substring(iPos + 1) != strElementName))
				{
					Debug.WriteLine("Ooops!!!!");
					return String.Empty;
				}

				m_strXPath = m_strXPath.Substring(0, iPos);
				return m_strXPath;
			}
		}
	}
}
