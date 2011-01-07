using System;
using System.Xml;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for RssItem.
	/// </summary>
	public class RssItem
	{
		#region ---[ Private Fields ]---
		private string   m_strTitle = String.Empty;
		private string   m_strLink = String.Empty;
		private string   m_strDescription = String.Empty;
		private string   m_strAuthor = String.Empty;
		private string   m_strCategory = String.Empty;
		private string   m_strRetain = String.Empty;
		private DateTime m_dtPublished;
		private DateTime m_dtReceived;
		private Enclosure m_Enclosure;
		private NCEncoding m_ncEncoding = NCEncoding.String;
		private NCType     m_ncDescType = NCType.Text;
		#endregion

		public RssItem()
		{
		}

		public void CopyFrom(RssItem item)
		{
			if (item != null)
			{
				m_strTitle = item.m_strTitle;
				m_strLink = item.m_strLink;
				m_strDescription = item.m_strDescription;
				m_strAuthor = item.m_strAuthor;
				m_strCategory = item.m_strCategory;
				m_strRetain = item.m_strRetain;
				m_dtPublished = item.m_dtPublished;
				m_dtReceived = item.m_dtReceived;
				m_Enclosure = item.m_Enclosure;
				m_ncEncoding = item.m_ncEncoding;
				m_ncDescType = item.m_ncDescType;
			}
		}

		#region ---[ Public Properties ]---
		public string Title
		{
			get { return m_strTitle; }
			set { m_strTitle = value; }
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

		public string Published
		{
			get { return m_dtPublished.ToString(); }
			set { m_dtPublished = DateTimeExt.Parse(value); }
		}

		public DateTime PublishedDate
		{
			get { return m_dtPublished; }
		}

		public string Received
		{
			get { return m_dtReceived.ToString(); }
		}

		public DateTime ReceivedDate
		{
			set { m_dtReceived = value; }
			get { return m_dtReceived; }
		}

		public string Author
		{
			set { m_strAuthor = value; }
			get { return m_strAuthor; }
		}

		public string Category
		{
			get { return m_strCategory; }
			set { m_strCategory = value; }
		}

		public Enclosure Enclosure
		{
			get { return m_Enclosure; }
			set { m_Enclosure = value; }
		}

		public NCEncoding Encoding
		{
			set { m_ncEncoding = value; }
		}

		public bool NeedsRetain
		{
			get { return m_strRetain.Equals("true"); }
		}

		/// <summary>
		/// Type of the description element.
		/// </summary>
		public NCType DescriptionType
		{
			set { m_ncDescType = value; }
		}
		#endregion

		public void Write(XmlTextWriter xmlWriter)
		{
			try
			{
				xmlWriter.WriteStartElement("item");

				Utils.WriteStringElement(xmlWriter, "title", m_strTitle);
				Utils.WriteStringElement(xmlWriter, "link", m_strLink);
				Utils.WriteStringElement(xmlWriter, "pubDate", Published);
				Utils.WriteStringElement(xmlWriter, "comshak:rcvDate", Received);
				if (m_strRetain.Equals("true"))
				{
					Utils.WriteStringElement(xmlWriter, "comshak:retain", m_strRetain);
				}
				Utils.WriteStringElement(xmlWriter, "author", m_strAuthor);
				Utils.WriteStringElement(xmlWriter, "category", m_strCategory);

				if (m_ncEncoding == NCEncoding.Raw)
				{
					WriteRawElement(xmlWriter, "description", m_strDescription);
				}
				else if (m_ncEncoding == NCEncoding.String)
				{
					if (m_ncDescType == NCType.CDATA)
					{
						xmlWriter.WriteStartElement("description");
						xmlWriter.WriteCData(m_strDescription);
						xmlWriter.WriteEndElement();
					}
					else
					{
						Utils.WriteStringElement(xmlWriter, "description", m_strDescription);
					}
				}

				WriteEnclosure(xmlWriter, "enclosure");

				xmlWriter.WriteEndElement();	// item
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("RssItem::Write()", ex);
			}
		} // end of method Write()

		private void WriteEnclosure(XmlTextWriter xmlWriter, string strElemName)
		{
			if (m_Enclosure != null && !m_Enclosure.Empty)
			{
				xmlWriter.WriteStartElement(strElemName);
				xmlWriter.WriteAttributeString("url", m_Enclosure.Url);
				xmlWriter.WriteAttributeString("length", m_Enclosure.Size);
				xmlWriter.WriteAttributeString("type", m_Enclosure.Type);
				xmlWriter.WriteEndElement();
			}
		}

		private void WriteRawElement(XmlTextWriter xmlWriter, string strElemName, string strText)
		{
			xmlWriter.WriteStartElement(strElemName);
			xmlWriter.WriteRaw(strText);
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// Merges a non-empty element.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void ReadElement(string name, string value, XmlNodeType nodeType)
		{
			if (name == "title")
			{
				m_strTitle = value;
			}
			else if (name == "link")
			{
				m_strLink = value;
			}
			else if (name == "pubDate")
			{
				Published = value;
			}
			else if (name == "comshak:rcvDate")
			{
				DateTime dtRcv = DateTimeExt.Parse(value);
				m_dtReceived = dtRcv;
			}
			else if (name == "comshak:retain")
			{
				m_strRetain = value;
			}
			else if (name == "author")
			{
				m_strAuthor = value;
			}
			else if (name == "description")
			{
				m_strDescription = value;
				if (nodeType == XmlNodeType.CDATA)
				{
					m_ncDescType = NCType.CDATA;
				}
			}
			else if (name == "category")
			{
				m_strCategory = value;
			}
			else
			{
				Utils.DbgOut("WARNING: Found unknown element {0} with value {1}", name, value);
			}
		}

		/// <summary>
		/// Merges the "enclosure" element, which is empty but has attributes.
		/// </summary>
		/// <param name="xmlReader"></param>
		public void ReadEnclosure(XmlReader xmlReader)
		{
			if (xmlReader.HasAttributes)
			{
				m_Enclosure = new Enclosure();
				xmlReader.MoveToFirstAttribute();
				do
				{
					switch (xmlReader.Name)
					{
						case "url":
							m_Enclosure.Url = xmlReader.Value;
							break;
						case "length":
							m_Enclosure.Size = xmlReader.Value;
							break;
						case "type":
							m_Enclosure.Type = xmlReader.Value;
							break;
					}
				}
				while (xmlReader.MoveToNextAttribute());
				xmlReader.MoveToElement();

				Debug.WriteLine("\tFound Enclosure: " + m_Enclosure.Url);
			}
		}

		public override string ToString()
		{
			return String.Format("Published:{0}; {1}", m_dtPublished, m_strTitle);
		}

		/// <summary>
		/// Determines if two RssItems are equal.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Equals(RssItem item)
		{
			if ((this.PublishedDate == item.PublishedDate) && (this.Title == item.Title))
			{
				return true;
			}
			return false;
		}
	}
}
