using System;
using System.Xml;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	public enum FeedFormat
	{
		Rss = 0,
		Rss2,
		Atom,
		Rdf
	}

	public enum NCEncoding	// Node Content Encoding
	{
		Raw = 0,
		String = 1
	}

	public enum NCType		// Node Content Type
	{
		Xml = 0,
		Text = 1
	}

	public struct NodeContent
	{
		public string     m_strXPath;
		public NCEncoding m_ncEncoding;
		public NCType     m_ncType;

		public NodeContent(string strXPath)
		{
			m_strXPath   = strXPath;
			m_ncEncoding = NCEncoding.String;
			m_ncType     = NCType.Text;
		}

		public NodeContent(string strXPath, NCEncoding encoding, NCType type)
		{
			m_strXPath   = strXPath;
			m_ncEncoding = encoding;
			m_ncType     = type;
		}
	}

	/// <summary>
	/// Summary description for FeedManager.
	/// </summary>
	public class FeedManager
	{
		protected XmlNamespaceManager m_nsManager;

		public FeedManager(XmlNameTable xmlNameTable)
		{
			//
			// TODO: Add constructor logic here
			//
			m_nsManager = new XmlNamespaceManager(xmlNameTable);
		}

		public string DefaultNamespace
		{
			get
			{
				return m_nsManager.DefaultNamespace;
			}
		}

		public void AddNamespace(string prefix, string uri)
		{
			m_nsManager.AddNamespace(prefix, uri);
		}

		public XmlNamespaceManager NsManager
		{
			get { return m_nsManager; }
		}

		public FeedFormat TransferNamespaces(XmlDocument xmlDoc)
		{
			FeedFormat format = FeedFormat.Rss;
			XmlElement xmlRss = xmlDoc.DocumentElement;
			string strFeedFormat = xmlRss.Name;
			string strFeedVersion = String.Empty;
			foreach (XmlAttribute xmlAttr in xmlRss.Attributes)
			{
				string strAttrName = xmlAttr.Name;
				if (strAttrName == "version")
				{
					strFeedVersion = xmlAttr.Value;
				}
				else if (strAttrName == "xmlns")
				{
					m_nsManager.AddNamespace(String.Empty, xmlAttr.Value);
					//Debug.WriteLine("Added def_ns :> " + xmlAttr.Value);
				}
				else
				{
					int iPos = strAttrName.IndexOf(':');
					if (iPos > -1)
					{// "xmlns:attr"
						if (strAttrName.Substring(0, iPos) == "xmlns")
						{
							//Debug.WriteLine("Added " + strAttrName.Substring(iPos + 1) + " :> " + xmlAttr.Value);
							m_nsManager.AddNamespace(strAttrName.Substring(iPos + 1), xmlAttr.Value);
						}
					}
				}
			}
			//Debug.WriteLine("Default namespace is " + xmlNsMgr.DefaultNamespace);
			if (strFeedFormat == "rss")
			{
				if (strFeedVersion == "2.0")
				{
					format = FeedFormat.Rss2;
				}
				else
				{
					format = FeedFormat.Rss;
				}
			}
			else if (strFeedFormat == "feed")
			{
				format = FeedFormat.Atom;
			}
			else if (strFeedFormat == "rdf:RDF")
			{
				format = FeedFormat.Rdf;
			}
			return format;
		}

		public string GetNodeContent(XmlNode xmlNode, NodeContent nodeContent)
		{
			string strContent = null;
			try
			{
				XmlNode node = null;
				if (m_nsManager != null)
				{
					node = xmlNode.SelectSingleNode(nodeContent.m_strXPath, m_nsManager);
				}
				else
				{
					node = xmlNode.SelectSingleNode(nodeContent.m_strXPath);
				}
				if (node != null)
				{
					if ((node.ChildNodes.Count == 1) && (node.FirstChild.NodeType == XmlNodeType.CDATA))
					{
						strContent = node.InnerText;
					}
					else if (nodeContent.m_ncType == NCType.Text)
					{
						strContent = node.InnerText;
					}
					else if (nodeContent.m_ncType == NCType.Xml)
					{
						strContent = node.InnerXml;
					}
				}
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("BackgroundThread::GetNodeContent()", ex);
			}
			return strContent;
		}

		public string GetNodeContent(XmlNode xmlNode, string strXPath, NCEncoding ncEncoding, NCType ncType)
		{
			NodeContent content = new NodeContent(strXPath, ncEncoding, ncType);
			return GetNodeContent(xmlNode, content);
		}

		public string GetNodeContent(XmlNode xmlNode, string strXPath)
		{
			NodeContent content = new NodeContent(strXPath);
			return GetNodeContent(xmlNode, content);
		}

		public string GetNodeContentFrom(XmlNode xmlNode, NodeContent[] arrContents, ref NCEncoding encoding)
		{
			string strReturn = null;
			NCEncoding encReturn = NCEncoding.String;
			try
			{
				int iCrt = 0;
				int iMax = arrContents.Length;
				if (iMax > 0)
				{
					do
					{
						strReturn = GetNodeContent(xmlNode, arrContents[iCrt]);
						if (strReturn != null)
						{
							encReturn = arrContents[iCrt].m_ncEncoding;
						}
						iCrt++;
					}
					while ((iCrt < iMax) && ((strReturn == null) || (strReturn.Length < 1)));
				}
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("BackgroundThread::GetNodeContentFrom()", ex);
			}
			if (strReturn == null)
			{
				encoding = NCEncoding.String;
				return String.Empty;
			}
			else
			{
				encoding = encReturn;
				return strReturn;
			}
		}

		public DateTime GetNodeDate(XmlNode xmlNode, string strElementName, DateTime dtDefault)
		{
			DateTime date = dtDefault;
			try
			{
				NodeContent content = new NodeContent(strElementName);
				string strDateTime = GetNodeContent(xmlNode, content);
				date = DateTimeExt.Parse(strDateTime);
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("BackgroundThread::GetNodeDate()", ex);
			}
			return date;
		}
	}
}
