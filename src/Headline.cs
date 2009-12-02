using System;
using System.Collections;

namespace com.comshak.FeedReader
{
	/// <summary>
	/// Summary description for HeadlineCollection.
	/// </summary>
	public class HeadlineCollection : IEnumerable
	{
		private ArrayList m_arrList;

		public HeadlineCollection()
		{
			m_arrList = new ArrayList();
		}

		public void Add(Headline headline)
		{
			m_arrList.Add(headline);
		}

		public int Count
		{
			get { return m_arrList.Count; }
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			m_arrList.Sort();
			return m_arrList.GetEnumerator();
		}

		#endregion
	}

	public enum SortFilter
	{
		Title,
		DatePublished,
		DateReceived,
		Author
	}

	public enum SortOrder
	{
		Ascending,
		Descending
	}

	/// <summary>
	/// Summary description for Headline.
	/// </summary>
	public class Headline : IComparable
	{
		#region Private Fields
		private static SortFilter m_sortFilter = SortFilter.DatePublished; 
		private static SortOrder  m_sortOrder  = SortOrder.Descending;
		private string m_strTitle;
		private DateTime m_datePublished;
		private DateTime m_dateReceived;
		private string m_strAuthor;
		private string m_strDesc;
		private string m_strLink;
		private string m_strCategory;
		private Enclosure m_enclosure;
		#endregion

		public Headline(string title, DateTime dtPublished, DateTime dtReceived, string author, string desc, string category, Enclosure enc)
		{
			m_strTitle = title;
			m_datePublished = dtPublished;
			m_dateReceived = dtReceived;
			m_strAuthor = author;
			m_strDesc = desc;
			m_strCategory = category;
			m_enclosure = enc;
		}

		#region Public Properties
		public string Title
		{
			get { return m_strTitle; }
		}

		public string Author
		{
			get { return m_strAuthor; }
		}

		public string Description
		{
			get { return m_strDesc; }
		}

		public string Category
		{
			get { return m_strCategory; }
		}

		public Enclosure Enclosure
		{
			get { return m_enclosure; }
		}

		public DateTime DatePublished
		{
			get { return m_datePublished; }
		}

		public DateTime DateReceived
		{
			get { return m_dateReceived; }
		}

		public string Link
		{
			get { return m_strLink; }
			set { m_strLink = value; }
		}

		public static SortFilter SortingFilter
		{
			get { return m_sortFilter; }
			set { m_sortFilter = value; }
		}

		public static SortOrder SortingOrder
		{
			get { return m_sortOrder; }
			set { m_sortOrder = value; }
		}
		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			Headline headline = obj as Headline;
			if (obj == null)
			{
				throw new InvalidCastException("Not a valid Headline object.");
			}
			bool bAscending = (Headline.SortingOrder == SortOrder.Ascending);
			switch (Headline.SortingFilter)
			{
				case SortFilter.Title:
					return (bAscending) ? Title.CompareTo(headline.Title) : headline.Title.CompareTo(Title);

				case SortFilter.DatePublished:
					return (bAscending) ? DatePublished.CompareTo(headline.DatePublished) : headline.DatePublished.CompareTo(DatePublished);

				case SortFilter.DateReceived:
					return (bAscending) ? DateReceived.CompareTo(headline.DateReceived) : headline.DateReceived.CompareTo(DateReceived);

				case SortFilter.Author:
					return (bAscending) ? Author.CompareTo(headline.Author) : headline.Author.CompareTo(Author);
			}
			return 0;
		}

		#endregion
	}
}
