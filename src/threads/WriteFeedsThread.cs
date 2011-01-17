using System;
using System.Collections;
using System.Threading;
using System.Xml;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace com.comshak.FeedReader
{
	public class Job
	{
		private FeedNode m_root;

		/// <summary>
		/// Gets the root node of the hierarchy.
		/// </summary>
		public FeedNode Root
		{
			get { return m_root; }
		}

		public Job(FeedNode fn)
		{
			if (fn != null)
			{
				m_root = fn.Duplicate(null);
			}
		}
	}

	/// <summary>
	/// Summary description for WriteFeedsThread.
	/// </summary>
	public class WriteFeedsThread
	{
		private static object s_instance;
		private static bool   s_bBusy;
		private static bool   s_bEnding;
		private static Queue  s_jobs;
		private static ManualResetEvent s_event;

		private Thread s_thread;

		private WriteFeedsThread()
		{
			//s_bBusy = false;	// It's already set to false by the runtime
			s_jobs = new Queue();
			s_event = new ManualResetEvent(false);

			s_thread = new Thread(new ThreadStart(Run));
			s_thread.Name = "Write Feeds";
			s_thread.Start();
		}

		public static bool Busy
		{
			get { return s_bBusy; }
		}

		public static WriteFeedsThread Instance
		{
			get
			{
				if (s_instance == null)
				{
					Interlocked.CompareExchange(ref s_instance, new WriteFeedsThread(), null);
				}
				return s_instance as WriteFeedsThread;
			}
		}

		/// <summary>
		/// Stops the thread.
		/// </summary>
		public static void Stop()
		{
			Debug.WriteLine("<-> WriteFeedsThread::Stop()");
			if (Instance == null)
			{
				return;
			}
			if (!s_bEnding)
			{
				s_bEnding = true;
				if (s_event != null)
				{	// If the thread has been started, then the event is valid, so set it.
					s_event.Set();
				}
			}
		}

		public static void QueueJob(FeedNode hierarchy)
		{
			Debug.WriteLine("--> QueueJob()");
			if (Instance == null)
			{
				return;
			}
			if (!s_bEnding)
			{
				s_jobs.Enqueue(new Job(hierarchy));
				s_event.Set();	// Wake the thread up, if it's sleeping.
			}
			Debug.WriteLine("<-- QueueJob()");
		}

		/// <summary>
		/// The thread procedure.
		/// </summary>
		private void Run()
		{
			while (true)
			{
				s_event.WaitOne();		// Sleeping...
				Debug.WriteLine("WriteFeedsThread has been woken...");

				int iJobs = s_jobs.Count;
				if (iJobs > 0)
				{
					Debug.WriteLine("...found " + iJobs.ToString() + " job(s) waiting to be executed!");

					s_bBusy = true;

					Job job = (Job) s_jobs.Dequeue();
					FeedNode root = job.Root;

					try
					{
						Utils.DeleteFile("feeds.xml.bak");
						FileInfo fi = new FileInfo("feeds.xml");
						fi.MoveTo("feeds.xml.bak");
					}
					catch (FileNotFoundException fnfex)
					{
						Utils.DbgOutExc("WriteFeedsThread::Run()", fnfex);
					}
					catch (Exception ex)
					{
						Utils.DbgOutExc("WriteFeedsThread::Run()", ex);
						MessageBox.Show("Error while saving feeds:\n\n" + ex.Message, "FeedReader", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					XmlTextWriter writer = null;
					try
					{
						writer = new XmlTextWriter("feeds.xml", Encoding.UTF8);
						writer.Formatting = Formatting.Indented;
						writer.IndentChar = ' ';
						writer.Indentation = 4;

						root.DumpAsOpml(writer);
					}
					catch (Exception ex)
					{
						Utils.DbgOutExc("WriteFeedsThread::Run()", ex);
						MessageBox.Show("Error while saving feeds:\n\n" + ex.Message, "FeedReader", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					finally
					{
						if (writer != null) { writer.Close(); }
					}

					s_bBusy = false;
				}
				s_event.Reset();
				Debug.WriteLine("...Job done!");

				if (s_bEnding)
				{	// Did you wake me up to terminate?
					Debug.WriteLine("Ending the WriteFeeds thread...");
					break;
				}
			}
		}
	}
}
