using System;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	public class XPath
	{
		private string m_strXPath;

		public XPath()
		{
			m_strXPath = "/";
		}

		/// <summary>
		/// Adds an element to the end of the current XPath (e.g. "/rss" -> "/rss/channel")
		/// </summary>
		/// <param name="strElementName">Name of element to add.</param>
		/// <returns>The previous XPath (before adding this element)</returns>
		public string AddElement(string strElementName)
		{
			string strLastXPath = m_strXPath;

			if (String.IsNullOrEmpty(strElementName))
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

		/// <summary>
		/// Removes an element from the end of the current XPath (e.g. "/rss/channel" -> "/rss")
		/// </summary>
		/// <param name="strElementName"></param>
		/// <returns>The new XPath (after removing this element).</returns>
		public string RemoveElement(string strElementName)
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

		/// <summary>
		/// Determines if the current XPath equals the provided one.
		/// </summary>
		/// <param name="xPath"></param>
		/// <returns></returns>
		public bool Equals(string xPath)
		{
			return m_strXPath.Equals(xPath);
		}

		public override string ToString()
		{
			return m_strXPath;
		}
	}
}
