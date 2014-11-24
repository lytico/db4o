/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Collections;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.TA.Collections
{
	public class PagedBackingStore : ActivatableImpl
	{
		public const int InitialPageCount = 16;

		private Page[] _pages = new Page[InitialPageCount];

		private int _top = 0;

		public PagedBackingStore()
		{
			AddNewPage();
		}

		public virtual bool Add(object item)
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			return GetPageForAdd().Add(item);
		}

		public virtual int Size()
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			return _top * Page.Pagesize - LastPage().Capacity();
		}

		public virtual object Get(int itemIndex)
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			Page page = PageHolding(itemIndex);
			return page.Get(IndexInPage(itemIndex));
		}

		private Page LastPage()
		{
			return _pages[_top - 1];
		}

		private Page GetPageForAdd()
		{
			Page lastPage = LastPage();
			if (lastPage.AtCapacity())
			{
				lastPage = AddNewPage();
			}
			return lastPage;
		}

		private Page AddNewPage()
		{
			Page page = new Page(_top);
			if (_top == _pages.Length)
			{
				GrowPages();
			}
			_pages[_top] = page;
			_top++;
			return page;
		}

		private void GrowPages()
		{
			Page[] grown = new Page[_pages.Length * 2];
			System.Array.Copy(_pages, 0, grown, 0, _pages.Length);
			_pages = grown;
		}

		/// <summary>Will return the page that holds the index passed in.</summary>
		/// <remarks>
		/// Will return the page that holds the index passed in.
		/// For example, if pagesize == 100 and index == 525, then this will return page 5.
		/// </remarks>
		/// <param name="itemIndex"></param>
		/// <returns></returns>
		private Page PageHolding(int itemIndex)
		{
			return _pages[PageIndex(itemIndex)];
		}

		private int PageIndex(int itemIndex)
		{
			return itemIndex / Page.Pagesize;
		}

		private int IndexInPage(int itemIndex)
		{
			return itemIndex % Page.Pagesize;
		}
	}
}
