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
		private DateTime  m_dtLastUpdated;
		#endregion

		public RssChannel()
		{
			m_arrItems = new ArrayList();
		}

		public void AddItem(RssItem item)
		{
			m_arrItems.Add(item);
		}

		/// <summary>
		/// Adds an RssItem to this channel's collection if it doesn't already exist in the channel.
		/// </summary>
		/// <param name="item">RssItem from the local rss channel.</param>
		public void AddIfNew(RssItem item)
		{
			RssItem existing = FindExistingItem(item);
			bool bExisting = (existing != null);
			if (!bExisting)
			{
				m_arrItems.Add(item);
			}
			else
			{
				Utils.DbgOut("existing.ReceivedDate = {0}; newItem.ReceivedDate = {1}", existing.ReceivedDate, item.ReceivedDate);

				if (item.NeedsRetain)
				{
					existing.CopyFrom(item);
				}
				else
				{
					existing.ReceivedDate = item.ReceivedDate;
				}
			}
			Utils.DbgOut("{0} (published {1}) was {2} already found in the channel.", item.Title, item.PublishedDate, (bExisting) ? String.Empty : "NOT");
		}

		/// <summary>
		/// Searches this channel's items for a similar item.
		/// </summary>
		/// <param name="item">An RssItem that might already exist in the channel.</param>
		/// <returns>The existing RssItem if found, null otherwise.</returns>
		public RssItem FindExistingItem(RssItem item)
		{
			if (item != null)
			{
				IEnumerator enumItems = m_arrItems.GetEnumerator();
				while (enumItems.MoveNext())
				{
					RssItem currentItem = enumItems.Current as RssItem;
					if (currentItem != null && currentItem.Equals(item))
					{
						return currentItem;
					}
				}
			}
			return null;
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

		public DateTime LastUpdated
		{
			set { m_dtLastUpdated = value; }
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

				Utils.WriteStringElement(xmlWriter, "title", m_strTitle);
				Utils.WriteStringElement(xmlWriter, "link", m_strLink);
				Utils.WriteStringElement(xmlWriter, "description", m_strDescription);
				Utils.WriteStringElement(xmlWriter, "generator", m_strGenerator);
				Utils.WriteStringElement(xmlWriter, "comshak:lastUpdate", m_dtLastUpdated.ToString());

				IEnumerator enumItems = GetEnumerator();
				while (enumItems.MoveNext())
				{// Write all children
					RssItem item = enumItems.Current as RssItem;
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
			XmlTextReader xmlReader = null;
			try
			{
				if (!File.Exists(m_strFileName))
				{
					Write();
					return;
				}

				string strElementName;
				XPath xPath = new XPath();
				RssItem localItem = null;

				xmlReader = new XmlTextReader(m_strFileName);
				while (xmlReader.Read())
				{
					XmlNodeType type = xmlReader.NodeType;
					if (type == XmlNodeType.Element)
					{
						strElementName = xmlReader.Name;
						if (xmlReader.IsEmptyElement)	// Check this first!
						{
							if (strElementName == "enclosure")
							{
								localItem.ReadEnclosure(xmlReader);
							}
						}
						else if (xPath.AddElement(strElementName) == "/rss/channel/item")
						{
							if (!xmlReader.Read())
							{
								continue;
							}
							XmlNodeType nt = xmlReader.NodeType;
							if ((nt != XmlNodeType.Text) && (nt != XmlNodeType.CDATA))
							{
								continue;
							}
							if (localItem == null)
							{
								localItem = new RssItem();
							}
							localItem.ReadElement(strElementName, xmlReader.Value, nt);
						}
					}
					else if (type == XmlNodeType.EndElement)
					{
						strElementName = xmlReader.Name;
						xPath.RemoveElement(strElementName);
						if ((strElementName == "item") && xPath.Equals("/rss/channel") && (localItem != null))
						{
							AddIfNew(localItem);
							localItem = null;
						}
					}
				}
				xmlReader.Close();
				xmlReader = null;
				Write();
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("RssChannel::Merge()", ex);
			}
			finally
			{
				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}
		}
	}
}
