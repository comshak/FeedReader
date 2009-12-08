using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public sealed class Utils
	{
		private static object s_rm;

		private Utils()
		{
		}

		public static ResourceManager ResMan
		{
			get
			{
				if (s_rm == null)
				{
					ResourceManager rm = new ResourceManager("FeedReader.AppStrings", typeof(Utils).Assembly);
					Interlocked.CompareExchange(ref s_rm, rm, null);
				}
				return s_rm as ResourceManager;
			}
		}

		public static string GetResString(string name)
		{
			return ResMan.GetString(name);
		}

		/// <summary>
		/// Determines if two DateTime's are on the same day.
		/// </summary>
		/// <param name="t1"></param>
		/// <param name="t2"></param>
		/// <returns></returns>
		public static bool IsSameDay(DateTime t1, DateTime t2)
		{
			if ((t1.Day == t2.Day) && (t1.Month == t2.Month) && (t1.Year == t2.Year))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Downloads the content from a Url and loads an XmlDocument with its contents.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="strDumpFile"></param>
		/// <returns></returns>
		public static XmlDocument DownloadXml(string url, string strDumpFile, /*[in, out]*/ ref DateTime dtLastUpdate)
		{
			Debug.WriteLine("--> DownloadXml(" + url + ", " + strDumpFile + ")");
			HttpWebResponse response = null;
			StreamReader readStream = null;
			XmlDocument xmlResponse = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.UserAgent = "Mozilla/5.0";
				request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
				if (dtLastUpdate > DateTime.MinValue)
				{
					request.IfModifiedSince = dtLastUpdate;
				}

				DateTime now = DateTime.Now;

				response = (HttpWebResponse)request.GetResponse();
				if (response.StatusCode != HttpStatusCode.OK)
				{
					Debug.WriteLine("    Response StatusCode is \"" + response.StatusCode.ToString() +
						"\" and StatusDescription is: " + response.StatusDescription);
					return null;
				}

				dtLastUpdate = DateTime.Now;

				string contentEncoding = response.ContentEncoding.ToLower();

				Debug.WriteLine("    Content type is " + response.ContentType);
				Debug.WriteLine("    Content length is " + response.ContentLength);
				Debug.WriteLine("    Content encoding is " + contentEncoding);
				Debug.WriteLine("    Transfer encoding is " + response.Headers["Transfer-Encoding"]);

				// Get the stream associated with the response.
				Stream responseStream = response.GetResponseStream();

				if (contentEncoding.Contains("gzip"))
				{
					responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
				}
				else if (contentEncoding.Contains("deflate"))
				{
					responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
				}

				// Pipe the stream to a higher level stream reader with the required encoding format. 
				readStream = new StreamReader(responseStream, Encoding.UTF8);

				string strResponse = readStream.ReadToEnd();
				Debug.WriteLine("    Response stream received (" + strResponse.Length + " characters long).");

				//---[ Dump the file to disk ]---

				if ((strResponse.Length > 0) && !IsNullOrEmpty(strDumpFile))
				{
					FileUtils.ConstructPathForFile(strDumpFile);

					using (StreamWriter sw = File.CreateText(strDumpFile))
					{
						sw.Write(strResponse);
					}
				}

				//---[ Load the XML ]---

				xmlResponse = new XmlDocument();
				xmlResponse.LoadXml(strResponse);
			}
			catch (WebException webex)
			{
				if (webex.Response == null && webex.Status != WebExceptionStatus.ProtocolError)
				{
					throw;
				}
				else
				{	// it's okay - it's probably a 304 error.
					Debug.WriteLine(String.Format("The feed has not changed since {0}", dtLastUpdate));
				}
			}
			catch (Exception ex)
			{
				string msg = String.Format("(!) {0} thrown in Utils::DownloadXml() :> {1}", ex.GetType().ToString(), ex.Message);
				Debug.WriteLine(msg);
			}
			finally
			{
				if (response != null) { response.Close(); }
				if (readStream != null) { readStream.Close(); }
				Debug.WriteLine("<-- DownloadXml()");
			}
			return xmlResponse;
		}

		/// <summary>
		/// Outputs the specified exception to the debug output.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="ex"></param>
		public static void DbgOutExc(string method, Exception ex)
		{
			if (ex != null)
			{
				string output = String.Format(CultureInfo.InvariantCulture,
					"(!) {0} thrown in {1} :> {2}", ex.GetType().ToString(), method, ex.Message);
				Debug.WriteLine(output);
			}
		}

		/// <summary>
		/// Determines whether the string is null or empty.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(string str)
		{
			if (str == null || str.Length <= 0)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Deletes a file, if it exists.
		/// </summary>
		/// <param name="strFilePath"></param>
		public static void DeleteFile(string strFilePath)
		{
			if ((strFilePath != null) && (strFilePath.Length > 0))
			{
				if (File.Exists(strFilePath))
				{
					File.SetAttributes(strFilePath, FileAttributes.Normal);
					File.Delete(strFilePath);
				}
			}
		}

		public static void MoveFile(string src, string dst)
		{
			try
			{
				FileInfo fi = new FileInfo(src);
				fi.MoveTo(dst);
			}
			catch (Exception)
			{
			}
		}
	}
}
