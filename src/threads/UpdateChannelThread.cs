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

		static readonly NodeContent[] s_arrAtomDesc = new NodeContent[]
		{
			new NodeContent("atom:content[@type=\"application/xhtml+xml\"]",           NCEncoding.String, NCType.Xml),
			new NodeContent("atom:content[@type=\"text/html\" and @mode=\"escaped\"]", NCEncoding.String, NCType.Xml),
			new NodeContent("atom:content[@type=\"text/html\"]",                       NCEncoding.String, NCType.Xml),
			new NodeContent("atom:content[@type=\"html\"]",                            NCEncoding.Raw,    NCType.Xml),
			new NodeContent("atom:content[@type=\"xhtml\"]",                           NCEncoding.String, NCType.Xml),
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
		static readonly NodeContent[] s_arrAtomPubDate = new NodeContent[]
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
		static readonly NodeContent[] s_arrRssAuthor = new NodeContent[]
		{
			new NodeContent("author",       NCEncoding.String, NCType.Text),
			new NodeContent("dc:creator",   NCEncoding.String, NCType.Text),
			new NodeContent("dc:publisher", NCEncoding.String, NCType.Text)
		};

//		static string[] s_arrRssAuthorTypes = new string[] {
//			"author",
//			"dc:creator"
//		};
		//-----------------------
		static readonly NodeContent[] s_arrRssDesc = new NodeContent[]
		{
			new NodeContent("content:encoded", NCEncoding.String, NCType.Text),
			new NodeContent("description",     NCEncoding.String, NCType.Text)
		};

//		static string[] s_arrRssDescTypes = new string[] {
//			"content:encoded",
//			"description"
//		};
		//-----------------------
		static readonly NodeContent[] s_arrRssPubDate = new NodeContent[]
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

		/// <summary>
		/// Opens the cached feed file to read the last updated date.
		/// </summary>
		/// <param name="rssFilename"></param>
		/// <returns>The DateTime when it was last updated, or DateTime.MinValue if never updated.</returns>
		public DateTime GetLastUpdate(string rssFilename)
		{
			DateTime dtLastUpdate = DateTime.MinValue;	// Never

			if (!System.IO.File.Exists(rssFilename))
			{
				return dtLastUpdate;
			}

			XmlTextReader xmlReader = null;
			try
			{
				string strElementName;
				XPath xPath = new XPath();

				xmlReader = new XmlTextReader(rssFilename);
				while (xmlReader.Read())
				{
					XmlNodeType type = xmlReader.NodeType;
					if (type == XmlNodeType.Element)
					{
						strElementName = xmlReader.Name;
						if (!xmlReader.IsEmptyElement && xPath.AddElement(strElementName) == "/rss/channel")
						{
							if (strElementName == "comshak:lastUpdate")
							{
								if (!xmlReader.Read() || (xmlReader.NodeType != XmlNodeType.Text))
								{
									continue;
								}
								dtLastUpdate = DateTime.Parse(xmlReader.Value);
								break;
							}
						}
					}
					else if (type == XmlNodeType.EndElement)
					{
						strElementName = xmlReader.Name;
						xPath.RemoveElement(strElementName);
					}
				}
			}
			finally
			{
				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}
			return dtLastUpdate;
		}

		public void UpdateChannel(string strXmlUrl)
		{
			try
			{
				DateTime dtLastUpdate = DateTime.MinValue;
				string strFileName = Names.FeedsFolder + m_feedNode.FileName;

				XmlDocument xmlResponse = null;
				if (!String.IsNullOrEmpty(m_strTempFeedFile))
				{
					if (System.IO.File.Exists(m_strTempFeedFile))
					{
						dtLastUpdate = DateTime.Now;
						xmlResponse = new XmlDocument();
						xmlResponse.Load(m_strTempFeedFile);
					}
				}
				else
				{
					dtLastUpdate = GetLastUpdate(strFileName);
#if DEBUG
					xmlResponse = Utils.DownloadXml(strXmlUrl, strFileName + ".xml", ref dtLastUpdate);
#else
					xmlResponse = Utils.DownloadXml(strXmlUrl, null, ref dtLastUpdate);
#endif
				}

				if (xmlResponse != null)
				{
					FeedManager = new FeedManager(xmlResponse.NameTable);
					FeedManager.LastUpdated = dtLastUpdate;
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
				if (!String.IsNullOrEmpty(m_strTempFeedFile))
				{
#if DEBUG
					Utils.MoveFile(m_strTempFeedFile, Names.FeedsFolder + m_feedNode.FileName + ".xml");
#endif
					Utils.DeleteFile(m_strTempFeedFile);
				}
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
					rssChannel.LastUpdated = FeedManager.LastUpdated;

					WriteRssItems(xmlUpdate, rssChannel);
				}
				else if (m_feedFormat == FeedFormat.Atom)
				{
					FeedManager.AddNamespace("atom", strDefNamespace);

					rssChannel.Title = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/atom:feed/atom:title");
					rssChannel.Link = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/atom:feed/atom:link[@rel=\"alternate\" and @type=\"text/html\"]/@href");
					rssChannel.Description = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/atom:feed/atom:subtitle");
					rssChannel.LastUpdated = FeedManager.LastUpdated;

					WriteAtomItems(xmlUpdate, rssChannel);
				}
				else if (m_feedFormat == FeedFormat.Rdf)
				{
					FeedManager.AddNamespace("rss", strDefNamespace);

					rssChannel.Title = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/rdf:RDF/rss:channel/rss:title");
					rssChannel.Link = FeedManager.GetNodeContent(xmlUpdate.DocumentElement, "/rdf:RDF/rss:channel/rss:link");
					rssChannel.LastUpdated = FeedManager.LastUpdated;

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
				rssItem.Enclosure = FeedManager.GetEnclosure(xmlNode, "enclosure");
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
				//localItem.Author = GetNodeText(xmlNode, "???");
				rssItem.Description = FeedManager.GetNodeContent(xmlNode, "rss:description");
				rssChannel.AddItem(rssItem);
			}
		}
	} // end of class
} // end of namespace
