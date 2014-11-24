using System;
using System.Collections.Generic;

namespace OManager.BusinessLayer.UIHelper
{
	[Serializable ]
	public class PagingData
	{
		public static readonly int PAGE_SIZE = 50;
		private const int START_PAGE_INDEX = 1;

		public PagingData(int startIndex, int endIndex)
		{
			m_startIndex = startIndex;
			m_endIndex =endIndex;
		}

		public PagingData(int startIndex) : this(startIndex, PAGE_SIZE)
		{
		}

		int m_startIndex;
		public int StartIndex
		{
			get { return m_startIndex; }
			set { m_startIndex = value; }
		}
		
		int m_endIndex;
		public int EndIndex
		{
			get { return m_endIndex; }
			set { m_endIndex = value; }
		}
		
		IList<long> m_objectId;
		public IList<long> ObjectId
		{
			get { return m_objectId; }
			set { m_objectId = value; }
		}

		public int GetPageCount()
		{
			int objectCount = m_objectId.Count;
			
			double pageCount = objectCount / (double)PAGE_SIZE;
			if (pageCount <= 0)
				pageCount = START_PAGE_INDEX;

			return (int) Math.Ceiling(pageCount);
		}

		public static PagingData StartingAtPage(int startPage)
		{
			int startIndex = NormalizePageIndex(startPage) * PAGE_SIZE;
			return new PagingData(startIndex, startIndex + PAGE_SIZE);
		}

		private static int NormalizePageIndex(int pageIndex)
		{
			return pageIndex - START_PAGE_INDEX;
		}

		public override string ToString()
		{
			return "PagingData(Start : " + m_startIndex + ", End: " + m_endIndex + ")";
		}
	}
}