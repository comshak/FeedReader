using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// The main class.
	/// </summary>
	public class FeedReaderApp
	{
		public FeedReaderApp()
		{
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(/*string[] args*/) 
		{
			Application.Run(new FeedReaderForm());
		}
	}
}
