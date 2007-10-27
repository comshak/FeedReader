using System;
using System.Xml;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// DiscoverFeedThread retrieves the feed information in the background, from a specified URL, and then
	/// provides this information back to the dialog that invoked it (so that it would not block its UI).
	/// </summary>
	public class DiscoverFeedThread : BackgroundThread
	{
		private ImportFeedForm m_form;
		private string m_strFeedUrl;
		private string m_strTitle;
		private string m_strHtmlUrl;
		private string m_strDescription;
		private string m_strTempFeedFile;

		public DiscoverFeedThread(ImportFeedForm form, string strFeedUrl)
		{
			m_form = form;
			m_strFeedUrl = strFeedUrl;
			m_strTitle = String.Empty;
			m_strHtmlUrl = String.Empty;
			m_strDescription = String.Empty;
			m_strTempFeedFile = String.Empty;
		}

		private delegate void OnEndCallback(string strTitle, string strHtmlUrl, string strDescription, string strTempFeedFile);

		private void OnEnd(string strTitle, string strHtmlUrl, string strDescription, string strTempFeedFile)
		{
			m_form.OnEnd_DiscoverFeed(m_strTitle, m_strHtmlUrl, m_strDescription, m_strTempFeedFile);
		}

		public override void Run()
		{
			try
			{
				m_strTempFeedFile = FileUtils.FeedsFolder + "_temp_" + Guid.NewGuid().ToString() + ".xml";
				XmlDocument xmlFeed = Utils.DownloadXml(m_strFeedUrl, m_strTempFeedFile);
				if (xmlFeed != null)
				{
					FeedManager = new FeedManager(xmlFeed.NameTable);
					FeedFormat feedFormat = FeedManager.TransferNamespaces(xmlFeed);
					Debug.WriteLine("The feed is " + feedFormat);

					string strDefNamespace = FeedManager.DefaultNamespace;

					if (feedFormat == FeedFormat.Rss || feedFormat == FeedFormat.Rss2)
					{
						XmlElement xmlDocElem = xmlFeed.DocumentElement;

						m_strTitle = FeedManager.GetNodeContent(xmlDocElem, "/rss/channel/title");
						m_strHtmlUrl = FeedManager.GetNodeContent(xmlDocElem, "/rss/channel/link");
						m_strDescription = FeedManager.GetNodeContent(xmlDocElem, "/rss/channel/description");
					}
					else if (feedFormat == FeedFormat.Atom)
					{
						XmlElement xmlDocElem = xmlFeed.DocumentElement;
						FeedManager.AddNamespace("atom", strDefNamespace);

						m_strTitle = FeedManager.GetNodeContent(xmlDocElem, "/atom:feed/atom:title");
						m_strHtmlUrl = FeedManager.GetNodeContent(xmlDocElem, "/atom:feed/atom:link[@rel=\"alternate\" and @type=\"text/html\"]/@href");
						m_strDescription = FeedManager.GetNodeContent(xmlDocElem, "/atom:feed/atom:tagline");
					}
					else if (feedFormat == FeedFormat.Rdf)
					{
						XmlElement xmlDocElem = xmlFeed.DocumentElement;
						FeedManager.AddNamespace("rss", strDefNamespace);

						m_strTitle = FeedManager.GetNodeContent(xmlDocElem, "/rdf:RDF/rss:channel/rss:title");
						m_strHtmlUrl = FeedManager.GetNodeContent(xmlDocElem, "/rdf:RDF/rss:channel/rss:link");
						m_strDescription = FeedManager.GetNodeContent(xmlDocElem, "/rdf:RDF/rss:channel/rss:description");
					}
				}
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("DiscoverFeedThread::Run()", ex);
			}
			finally
			{
				// Return the data back to the form
				m_form.Invoke(new OnEndCallback(OnEnd), new object[] { m_strTitle, m_strHtmlUrl, m_strDescription, m_strTempFeedFile });
			}
		}
	}
}
