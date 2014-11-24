/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Sharpen.Util;

namespace Db4objects.Drs.Tests.Data
{
	public sealed class ItemDates
	{
		private DateTime date1;

		private DateTime date2;

		private DateTime[] dateArray;

		public ItemDates()
		{
		}

		public ItemDates(DateTime date1, DateTime date2)
		{
			this.SetDate1(date1);
			this.SetDate2(date2);
			this.SetDateArray(new DateTime[] { date1, date2 });
		}

		public override bool Equals(object obj)
		{
			Db4objects.Drs.Tests.Data.ItemDates other = (Db4objects.Drs.Tests.Data.ItemDates)
				obj;
			if (!other.GetDate1().Equals(GetDate1()))
			{
				return false;
			}
			if (!other.GetDate2().Equals(GetDate2()))
			{
				return false;
			}
			return Arrays.Equals(GetDateArray(), other.GetDateArray());
		}

		public override string ToString()
		{
			return "ItemDates [_date1=" + GetDate1() + ", _date2=" + GetDate2();
		}

		public void SetDate1(DateTime date1)
		{
			this.date1 = date1;
		}

		public DateTime GetDate1()
		{
			return date1;
		}

		public void SetDate2(DateTime date2)
		{
			this.date2 = date2;
		}

		public DateTime GetDate2()
		{
			return date2;
		}

		public void SetDateArray(DateTime[] dateArray)
		{
			this.dateArray = dateArray;
		}

		public DateTime[] GetDateArray()
		{
			return dateArray;
		}
	}
}
