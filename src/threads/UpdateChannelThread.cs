using System;
using System.Xml;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// This thread updates a channel's contents with new headlines.
	/// </summary>
	public class UpdateChannelThread : BackgroundThread
	{
		private FeedReaderForm m_form;
		private FeedNode       m_feedNode;
		private FeedFormat     m_feedFormat;
		private string         m_strTempFeedFile;

		static NodeContent[] s_arrAtomDesc = new NodeContent[]
		{
			new NodeContent("atom:content[@type=\"application/xhtml+xml\"]",           NCEncoding.String, NCType.Xml),
			new NodeContent("atom:content[@type=\"text/html\" and @mode=\"escaped\"]", NCEncoding.String, NCType.Xml),
			new NodeContent("atom:content[@type=\"text/html\"]",                       NCEncoding.String, NCType.Xml),
			new NodeContent("atom:content[@type=\"html\"]",                            NCEncoding.Raw,    NCType.Xml),
			new NodeContent("atom:summary[@type=\"application/xhtml+xml\"]",           NCEncoding.String, NCType.Xml),
			new NodeContent("atom:summary[@type=\"text/html\"]",                       NCEncoding.String, NCType.Xml),
			new NodeContent("atom:summary[@type=\"html\"]",                            NCEncoding.Raw,    NCType.Xml)
		};

//		static string[] s_arrAtomDescTypes = new string[] {
//			"atom:content[@type=\"application/xhtml+xml\"]",
//			"atom:content[@type=\"text/html\"]",
//			"atom:content[@type=\"html\"]",
//			"atom:summary[@type=\"application/xhtml+xml\"]",
//			"atom:summary[@type=\"text/html\"]",
//			"atom:summary[@type=\"html\"]"
//		};
		//-----------------------
		static NodeContent[] s_arrAtomPubDate = new NodeContent[]
		{
			new NodeContent("atom:issued",    NCEncoding.String, NCType.Text),
			new NodeContent("atom:published", NCEncoding.String, NCType.Text),
			new NodeContent("atom:updated",   NCEncoding.String, NCType.Text)
		};

//		static string[] s_arrAtomPubDateTypes = new string[] {
//			"atom:issued",
//			"atom:published",
//			"atom:updated"
//		};
		//-----------------------
		static NodeContent[] s_arrRssAuthor = new NodeContent[]
		{
			new NodeContent("author",     NCEncoding.String, NCType.Text),
			new NodeContent("dc:creator", NCEncoding.String, NCType.Text)
		};

//		static string[] s_arrRssAuthorTypes = new string[] {
//			"author",
//			"dc:creator"
//		};
		//-----------------------
		static NodeContent[] s_arrRssDesc = new NodeContent[]
		{
			new NodeContent("content:encoded", NCEncoding.String, NCType.Text),
			new NodeContent("description",     NCEncoding.String, NCType.Text)
		};

//		static string[] s_arrRssDescTypes = new string[] {
//			"content:encoded",
//			"description"
//		};
		//-----------------------
		static NodeContent[] s_arrRssPubDate = new NodeContent[]
		{
			new NodeContent("pubDate", NCEncoding.String, NCType.Text),
			new NodeContent("dc:date", NCEncoding.String, NCType.Text)
		};
		//-----------------------
		public UpdateChannelThread(FeedReaderForm form, FeedNode feedNode)
		{
			m_form = form;
			m_feedNode = feedNode;
			m_feedFormat = FeedFormat.Rss;
		}

		private delegate void OnBeginCallback();
		private delegate void OnEndCallback();

		private void OnBegin()
		{
			m_form.OnBegin_UpdateChannel(m_feedNode);
		}	

		private void OnEnd()
		{
			m_form.OnEnd_UpdateChannel(m_feedNode);
		}	

		/// <summary>
		/// The thread procedure.
		/// </summary>
		public override void Run()
		{
			if ((m_feedNode == null) || (m_feedNode.XmlUrl.Length <= 0))
			{
				return;
			}

			m_form.Invoke(new OnBeginCallback(OnBegin));

			UpdateChannel(m_feedNode.XmlUrl);

			m_form.Invoke(new OnEndCallback(OnEnd));
		}

		/// <summary>
		/// Points the thread to read the feed from a file instead of downloading it from the net.
		/// </summary>
		/// <param name="strTempFeedFile"></param>
		public void SetFromTempFile(string strTempFeedFile)
		{
			m_strTempFeedFile = strTempFeedFile;
		}

		public void UpdateChannel(string strXmlUrl)
		{
			try
			{
				string strFileName = Names.FeedsFolder + m_feedNode.FileName;

				XmlDocument xmlResponse = null;
				if (!Utils.IsNullOrEmpty(m_strTempFeedFile))
				{
					if (System.IO.File.Exists(m_strTempFeedFile))
					{
						xmlResponse = new XmlDocument();
						xmlResponse.Load(m_strTempFeedFile);
					}
				}
				else
				{
#if DEBUG
					xmlResponse = Utils.DownloadXml(strXmlUrl, strFileName + ".xml");
#else
					xmlResponse = Utils.DownloadXml(strXmlUrl, null);
#endif
				}

				if (xmlResponse != null)
				{
					FeedManager = new FeedManager(xmlResponse.NameTable);
					m_feedFormat = FeedManager.TransferNamespaces(xmlResponse);
					Debug.WriteLine("The feed is " + m_feedFormat);

					MergeFeeds(strFileName, xmlResponse);
				}
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("UpdateChannelThread::UpdateChannel()", ex);
			}
			finally
			{
#if DEBUG
				Utils.MoveFile(m_strTempFeedFile, Names.FeedsFolder + m_feedNode.FileName + ".xml");
				Utils.DeleteFile(m_strTempFeedFile);
#else
				Utils.DeleteFile(m_strTempFeedFile);
#endif
			}
		}

		/// <summary>
		/// Merges an update XML (in any format) into the older local RSS channel XML file.
		/// </summary>
		/// <param name="strFileName">File name/path of the local RSS channel XML file.</param>
		/// <param name="xmlUpdate">XML of the update with the latest entries.</param>
		private void MergeFeeds(string strFileName, XmlDocument xmlUpdate)
		{
			try
			{
				RssChannel rssChannel = new RssChannel();
				rssChannel.FileName = strFileName;
				string strDefNamespace = FeedManager.DefaultNamespace;

				if (m_feedFormat == FeedFormat.Rss || m_feedFormat == FeedFormat.Rss2)
				{
					rssChannel.Title = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/rss/channel/title");
					rssChannel.Link = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/rss/channel/link");
					rssChannel.Description = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/rss/channel/description");

					WriteRssItems(xmlUpdate, rssChannel);
				}
				else if (m_feedFormat == FeedFormat.Atom)
				{
					FeedManager.AddNamespace("atom", strDefNamespace);

					rssChannel.Title = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/atom:feed/atom:title");
					rssChannel.Link = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/atom:feed/atom:link[@rel=\"alternate\" and @type=\"text/html\"]/@href");
					rssChannel.Description = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/atom:feed/atom:subtitle");

					WriteAtomItems(xmlUpdate, rssChannel);
				}
				else if (m_feedFormat == FeedFormat.Rdf)
				{
					FeedManager.AddNamespace("rss", strDefNamespace);

					rssChannel.Title = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/rdf:RDF/rss:channel/rss:title");
					rssChannel.Link = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/rdf:RDF/rss:channel/rss:link");

					WriteRdfItems(xmlUpdate, rssChannel);
				}

				rssChannel.Merge();
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("UpdateChannelThread::MergeFeeds()", ex);
			}
		}

		/// <summary>
		/// Writes items from an RSS feed.
		/// </summary>
		/// <param name="xmlDocument"></param>
		/// <param name="xmlWriter"></param>
		private void WriteRssItems(XmlDocument xmlDocument, RssChannel rssChannel)
		{
			NCEncoding encoding = NCEncoding.String;
			DateTime now = DateTime.Now;
			XmlNodeList xmlNodes = xmlDocument.SelectNodes("/rss/channel//item", FeedManager.NsManager);
			foreach (XmlNode xmlNode in xmlNodes)
			{
				RssItem rssItem = new RssItem();
				rssItem.Title = FeedManager.GetNodeContent(xmlNode, "title");
				rssItem.Link = FeedManager.GetNodeContent(xmlNode, "link");
				rssItem.Published = FeedManager.GetNodeContentFrom(xmlNode, s_arrRssPubDate, ref encoding);
				rssItem.ReceivedDate = now;
				rssItem.Author = FeedManager.GetNodeContentFrom(xmlNode, s_arrRssAuthor, ref encoding);
				rssItem.Description = FeedManager.GetNodeContentFrom(xmlNode, s_arrRssDesc, ref encoding);
				rssItem.Category = FeedManager.GetNodeContent(xmlNode, "category");
				rssItem.Encoding = encoding;
				rssChannel.AddItem(rssItem);
			}
		}

		/// <summary>
		/// Writes items from an ATOM feed.
		/// </summary>
		/// <param name="xmlDocument"></param>
		/// <param name="xmlWriter"></param>
		private void WriteAtomItems(XmlDocument xmlDocument, RssChannel rssChannel)
		{
			NCEncoding encoding = NCEncoding.String;
			DateTime now = DateTime.Now;
			XmlNodeList xmlNodes = xmlDocument.SelectNodes("/atom:feed//atom:entry", FeedManager.NsManager);
			foreach (XmlNode xmlNode in xmlNodes)
			{
				RssItem rssItem = new RssItem();
				rssItem.Title = FeedManager.GetNodeContent(xmlNode, "atom:title");
				rssItem.Link = FeedManager.GetNodeContent(xmlNode, "atom:link[@rel=\"alternate\" and @type=\"text/html\"]/@href");
				rssItem.Published = FeedManager.GetNodeContentFrom(xmlNode, s_arrAtomPubDate, ref encoding);
				rssItem.ReceivedDate = now;
				rssItem.Author = FeedManager.GetNodeContent(xmlNode, "atom:author/atom:name");
				rssItem.Description = FeedManager.GetNodeContentFrom(xmlNode, s_arrAtomDesc, ref encoding);
				rssItem.Encoding = encoding;
				rssChannel.AddItem(rssItem);
			}
		}

		/// <summary>
		/// Writes items from an RSS feed (rdf:RDF).
		/// </summary>
		/// <param name="xmlDocument"></param>
		/// <param name="xmlWriter"></param>
		private void WriteRdfItems(XmlDocument xmlDocument, RssChannel rssChannel)
		{
			DateTime now = DateTime.Now;
			XmlNodeList xmlNodes = xmlDocument.SelectNodes("/rdf:RDF//rss:item", FeedManager.NsManager);
			foreach (XmlNode xmlNode in xmlNodes)
			{
				RssItem rssItem = new RssItem();
				rssItem.Title = FeedManager.GetNodeContent(xmlNode, "rss:title");
				rssItem.Link = FeedManager.GetNodeContent(xmlNode, "rss:link");
				rssItem.Published = FeedManager.GetNodeContent(xmlNode, "dc:date");
				rssItem.ReceivedDate = now;
				//rssItem.Author = GetNodeText(xmlNode, "???");
				rssItem.Description = FeedManager.GetNodeContent(xmlNode, "rss:description");
				rssChannel.AddItem(rssItem);
			}
		}
	} // end of class
} // end of namespace
