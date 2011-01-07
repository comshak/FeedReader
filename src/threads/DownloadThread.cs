using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace com.comshak.FeedReader
{
	public class Media
	{
		public string Url;
		public string File;

		public Media(string url, string file)
		{
			Url = url;
			File = file;
		}
	}

	public class DownloadThread
	{
		//private static object s_instance;
		private static List<Media> s_list;
		private static bool s_bEnding;
		private static ManualResetEvent s_event;
		private static Thread s_thread;

		static DownloadThread()
		{
			s_bEnding = false;
			s_event = new ManualResetEvent(false);

			s_list = new List<Media>();

			s_thread = new Thread(new ThreadStart(Run));
			s_thread.Name = "DownloadThread";
			s_thread.Start();
		}

		//public static DownloadThread Instance
		//{
		//    get
		//    {
		//        Debug.WriteLine("DownloadThread.Instance called!");
		//        if (s_instance == null)
		//        {
		//            Interlocked.CompareExchange(ref s_instance, new DownloadThread(), null);
		//        }
		//        return s_instance as DownloadThread;
		//    }
		//}

		/// <summary>
		/// The thread function.
		/// </summary>
		private static void Run()
		{
			while (true)
			{
				s_event.WaitOne(/*500*/);
				if (s_bEnding)
				{
					return;
				}

				if (s_list.Count > 0)
				{
					Media media = s_list[0];
					s_list.RemoveAt(0);

					if (String.IsNullOrEmpty(media.Url) || String.IsNullOrEmpty(media.File))
					{
						continue;
					}

					DownloadFile(media.Url, media.File);

					if (s_list.Count == 0)
					{
						s_event.Reset();
					}
				}
			}
		}

		/// <summary>
		/// Asks the thread to end.
		/// </summary>
		/// <remarks>Is called from OTHER thread.</remarks>
		public static void Stop()
		{
			s_bEnding = true;
			s_event.Set();	// Wake the thread so it can end.
		}

		/// <summary>
		/// Adds a file to the download queue.
		/// </summary>
		/// <remarks>Is called from OTHER thread.</remarks>
		/// <param name="url"></param>
		/// <param name="file"></param>
		public static void Add(string url, string file)
		{
			s_list.Add(new Media(url, file));
			s_event.Set();
		}

		private static bool DownloadFile(string url, string file)
		{
			Debug.WriteLine(String.Format("Downloading media file {0} to {1}", url, file));

			int bufsize = 65536;
			FileStream fileStream = null;
			Stream responseStream = null;
			HttpWebResponse response = null;
			try
			{
				// Create the output file
				fileStream = new FileStream(file, FileMode.CreateNew, FileAccess.Write, FileShare.Read, bufsize, false);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.UserAgent = "Mozilla/5.0";
				request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

				response = (HttpWebResponse)request.GetResponse();
				if (response.StatusCode != HttpStatusCode.OK)
				{
					Debug.WriteLine("    Response StatusCode: " + response.StatusCode);
					Debug.WriteLine("    Response StatusDescription: " + response.StatusDescription);
					return false;
				}

				DateTime dtLastModified = response.LastModified;
				string contentEncoding = response.ContentEncoding.ToLower();

				Debug.WriteLine("    Content-Type: " + response.ContentType);
				Debug.WriteLine("    Content-Length: " + response.ContentLength);
				Debug.WriteLine("    Content-Encoding: " + contentEncoding);
				Debug.WriteLine("    Transfer-Encoding: " + response.Headers["Transfer-Encoding"]);

				// Get the stream associated with the response.
				responseStream = response.GetResponseStream();

				if (contentEncoding.Contains("gzip"))
				{
					responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
				}
				else if (contentEncoding.Contains("deflate"))
				{
					responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
				}

				int bytesRead;
				byte[] buffer = new byte[bufsize];
				while ((bytesRead = responseStream.Read(buffer, 0, bufsize)) != 0)
				{
					if (s_bEnding)
					{
						return false;
					}
					fileStream.Write(buffer, 0, bytesRead);
				}

				// Close the file
				fileStream.Close();
				fileStream = null;

				// Preserve the last modification time.
				FileInfo fi = new FileInfo(file);
				fi.LastWriteTime = dtLastModified;

				Debug.WriteLine(String.Format("Media file {0} downloaded successfully!", file));
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(String.Format("(!) {0} thrown in DownloadThread::DownloadFile() - {1}", ex.GetType(), ex.Message));
			}
			finally
			{
				if (responseStream != null) responseStream.Close();
				if (fileStream != null) fileStream.Close();
				if (response != null) response.Close();
			}
			return false;
		}
	}
}
