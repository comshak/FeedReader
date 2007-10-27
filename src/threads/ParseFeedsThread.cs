using System;
using System.Xml;
using System.Threading;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for ParseFeedsThread.
	/// </summary>
	public class ParseFeedsThread : BackgroundThread
	{
		private FeedReaderForm m_form;

		public ParseFeedsThread(FeedReaderForm form)
		{
			m_form = form;
		}

		private delegate void OnEndCallback();

		private void OnEnd()
		{
			m_form.OnEnd_ParseFeeds();
		}

		/// <summary>
		/// Parses feeds.xml and constructs the tree of FeedNode's.
		/// </summary>
		public override void Run()
		{
			XmlTextReader reader = null;
			try
			{
				FeedNode nodeCurrent = m_form.RootFeedNode;
				nodeCurrent.Clear();
				string strXmlUrl      = String.Empty;
				string strFeedName    = String.Empty;
				string strGroupName   = String.Empty;
				string strElementName = String.Empty;
				reader  = new XmlTextReader("feeds.xml");
				reader.WhitespaceHandling = WhitespaceHandling.None;
				while (reader.Read())
				{
					XmlNodeType type = reader.NodeType;
					if (type == XmlNodeType.Element)
					{
						strElementName = reader.Name;
						if (strElementName == "group" && reader.HasAttributes)
						{
							strGroupName = reader.GetAttribute("title");
							FeedNode node = new FeedNode(strGroupName, null, nodeCurrent, true);
							nodeCurrent.AddChildNode(node);
							nodeCurrent = node;
						}
						else if (strElementName == "outline" && reader.HasAttributes)
						{
							strFeedName = reader.GetAttribute("title");
							strXmlUrl = reader.GetAttribute("xmlUrl");
							FeedNode node = new FeedNode(strFeedName, strXmlUrl, nodeCurrent, false);
							node.Text = reader.GetAttribute("text");
							node.HtmlUrl = reader.GetAttribute("htmlUrl");
							node.Description = reader.GetAttribute("description");
							nodeCurrent.AddChildNode(node);
						}
					}
					else if (type == XmlNodeType.EndElement)
					{
						strElementName = reader.Name;
						if (strElementName == "group")
						{
							nodeCurrent = nodeCurrent.Parent;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("ParseFeedsThread::Run()", ex);
			}
			finally
			{
				if (reader != null) { reader.Close(); }
				m_form.Invoke(new OnEndCallback(OnEnd));
			}
		}
	}
}
