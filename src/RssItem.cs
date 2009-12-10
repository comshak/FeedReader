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
		private Enclosure m_Enclosure;
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
		#endregion

		public void Write(XmlTextWriter xmlWriter)
		{
			try
			{
				xmlWriter.WriteStartElement("item");

				WriteStringElement(xmlWriter, "title", m_strTitle);
				WriteStringElement(xmlWriter, "link", m_strLink);
				WriteStringElement(xmlWriter, "pubDate", Published);
				WriteStringElement(xmlWriter, "comshak:rcvDate", Received);
				WriteStringElement(xmlWriter, "author", m_strAuthor);
				WriteStringElement(xmlWriter, "category", m_strCategory);

				if (m_ncEncoding == NCEncoding.Raw)
				{
					WriteRawElement(xmlWriter, "description", m_strDescription);
				}
				else if (m_ncEncoding == NCEncoding.String)
				{
					WriteStringElement(xmlWriter, "description", m_strDescription);
				}

				WriteEnclosure(xmlWriter, "enclosure");

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
		public void MergeElement(string name, string value)
		{
			if (name == "title")
			{
				m_strTitle = value;
				Debug.WriteLine("\tFound Title: " + value);
			}
			else if (name == "link")
			{
				m_strLink = value;
				Debug.WriteLine("\tFound Link: " + value);
			}
			else if (name == "pubDate")
			{
				Published = value;
				Debug.WriteLine("\tFound Published: " + value);
			}
			else if (name == "comshak:rcvDate")
			{
				DateTime dtRcv = DateTimeExt.Parse(value);
				m_dtReceived = dtRcv;
				Debug.WriteLine("\tFound ReceivedDate: " + value + " ==> " + dtRcv);
			}
			else if (name == "author")
			{
				m_strAuthor = value;
				Debug.WriteLine("\tFound Author: " + value);
			}
			else if (name == "description")
			{
				m_strDescription = value;
				Debug.WriteLine("\tFound Description: " + value);
			}
			else if (name == "category")
			{
				m_strCategory = value;
				Debug.WriteLine("\tFound Category: " + value);
			}
			else
			{
				Debug.WriteLine("\tFound Unknown " + name + ": " + value);
			}
		}

		/// <summary>
		/// Merges the "enclosure" element, which is empty but has attributes.
		/// </summary>
		/// <param name="xmlReader"></param>
		public void MergeEnclosure(XmlReader xmlReader)
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
	}
}
