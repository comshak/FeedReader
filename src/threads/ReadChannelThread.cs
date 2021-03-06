using System;
using System.IO;
using System.Xml;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for ReadChannelThread.
	/// </summary>
	public class ReadChannelThread : BackgroundThread
	{
		private FeedReaderForm m_form;
		private FeedNode       m_feedNode;

		public ReadChannelThread(FeedReaderForm form, FeedNode feedNode)
		{
			m_feedNode = feedNode;
			m_form = form;
		}

		private delegate void OnEndCallback(HeadlineCollection headlines);

		public override void Run()
		{
			HeadlineCollection headlines = new HeadlineCollection();
			try
			{
				//DateTime now = DateTime.Now;
				DateTime now = DateTime.FromFileTime(0);

				string strFileName = Names.FeedsFolder + m_feedNode.FileName;
				if (File.Exists(strFileName))
				{
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.Load(strFileName);

					FeedManager = new FeedManager(xmlDoc.NameTable);
					FeedManager.TransferNamespaces(xmlDoc);

					XmlNodeList xmlNodes = xmlDoc.SelectNodes("/rss/channel//item");

					foreach (XmlNode xmlNode in xmlNodes)
					{
						string title = FeedManager.GetNodeContent(xmlNode, "title");
						DateTime dtPublished = FeedManager.GetNodeDate(xmlNode, "pubDate", now);
						DateTime dtReceived = FeedManager.GetNodeDate(xmlNode, "comshak:rcvDate", now);
						string author = FeedManager.GetNodeContent(xmlNode, "author");
						string desc = FeedManager.GetNodeContent(xmlNode, "description", NCEncoding.String, NCType.Text);
						string link = FeedManager.GetNodeContent(xmlNode, "link");
						string categ = FeedManager.GetNodeContent(xmlNode, "category");
						Enclosure enc = FeedManager.GetEnclosure(xmlNode, "enclosure");

						Headline headline = new Headline(title, dtPublished, dtReceived, author, desc, categ, enc);
						headline.Link = link;
						headlines.Add(headline);
					}
				}
			}
			catch (FileNotFoundException ex)
			{
				Utils.DbgOutExc("ReadChannelThread::Run()", ex);
			}
			catch (XmlException xmlEx)
			{
				Utils.DbgOutExc("ReadChannelThread::Run()", xmlEx);
			}
			//catch (Exception ex)
			//{
			//	Utils.DbgOutExc("ReadChannelThread::Run()", ex);
			//}
			finally
			{
				m_form.Invoke(new OnEndCallback(OnEnd), new object[] { headlines });
			}
		}

		private void OnEnd(HeadlineCollection headlines)
		{
			m_form.OnEnd_ReadChannel(headlines);
		}
	}
}
