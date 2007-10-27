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
		private DateTime m_dtPublished;
		private DateTime m_dtReceived;
		private NCEncoding m_ncEncoding = NCEncoding.String;
		#endregion

		public RssItem()
		{
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
			set { m_dtPublished = DateTimeExt.Parse(value); }
		}

		public DateTime PublishedDate
		{
			get { return m_dtPublished; }
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

		public NCEncoding Encoding
		{
			set { m_ncEncoding = value; }
		}
		#endregion

		public void Write(XmlTextWriter xmlWriter)
		{
			try
			{
				xmlWriter.WriteStartElement("item");

				WriteStringElement(xmlWriter, "title", m_strTitle);
				WriteStringElement(xmlWriter, "link", m_strLink);
				WriteStringElement(xmlWriter, "pubDate", m_dtPublished.ToString());
				string strRcvDate = m_dtReceived.ToString();
				WriteStringElement(xmlWriter, "comshak:rcvDate", strRcvDate);
				WriteStringElement(xmlWriter, "author", m_strAuthor);

				if (m_ncEncoding == NCEncoding.Raw)
				{
					WriteRawElement(xmlWriter, "description", m_strDescription);
				}
				else if (m_ncEncoding == NCEncoding.String)
				{
					WriteStringElement(xmlWriter, "description", m_strDescription);
				}

				xmlWriter.WriteEndElement();	// item
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("RssItem::Write()", ex);
			}
		} // end of method Write()

		private void WriteStringElement(XmlTextWriter xmlWriter, string strElemName, string strText)
		{
			xmlWriter.WriteStartElement(strElemName);
			xmlWriter.WriteString(strText);
			xmlWriter.WriteEndElement();
		}

		private void WriteRawElement(XmlTextWriter xmlWriter, string strElemName, string strText)
		{
			xmlWriter.WriteStartElement(strElemName);
			xmlWriter.WriteRaw(strText);
			xmlWriter.WriteEndElement();
		}
	}
}
