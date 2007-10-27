using System;
using System.Xml;
using System.Threading;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for BackgroundThread.
	/// </summary>
	public abstract class BackgroundThread
	{
		private   Thread              m_thread;
		//protected XmlNamespaceManager m_nsManager = null;
		private   FeedManager         m_feedManager;

		protected BackgroundThread()
		{
			m_thread = new Thread(new ThreadStart(Run));
		}

		public void Start()
		{
			m_thread.Start();
		}

		/// <summary>
		/// The thread procedure.
		/// </summary>
		public abstract void Run();

		public FeedManager FeedManager
		{
			get { return m_feedManager; }
			set { m_feedManager = value; }
		}
	}
}
