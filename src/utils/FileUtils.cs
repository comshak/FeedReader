using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for FileUtils.
	/// </summary>
	public sealed class FileUtils
	{
		public const string FeedsFolderInitial = @"feeds\";
		public const string TempFolderInitial = @"temp\";

		private static string m_strFeedsFolder = ConstructPath(FeedsFolderInitial);
		private static string m_strTempFolder = ConstructPath(TempFolderInitial);

		private FileUtils()
		{
		}

		public static string FeedsFolder
		{
			get
			{
				return m_strFeedsFolder;
			}
		}

		public static string TempFolder
		{
			get
			{
				return m_strTempFolder;
			}
		}

		/// <summary>
		/// Create the path and return it.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string ConstructPath(string path)
		{
			try
			{
				if (!Directory.Exists(path))
				{
					return Directory.CreateDirectory(path).FullName;
				}
				return Directory.GetCurrentDirectory() + "\\" + path;
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("FileUtils::ConstructPath()", ex);
			}
			return String.Empty;
		}

		/// <summary>
		/// Creates the folder for the specified file path/name.
		/// </summary>
		/// <param name="filepath"></param>
		public static void ConstructPathForFile(string filepath)
		{
			try
			{
				int index = filepath.LastIndexOf('\\');
				if (index > 0)
				{
					Directory.CreateDirectory(filepath.Substring(0, index));
				}
				// Nothing to create: it's in the current folder already (index == -1), or in the root folder (index == 0)!
			}
			catch (Exception ex)
			{
				Utils.DbgOutExc("FileUtils::ConstructPath()", ex);
			}
		}
	}
}
