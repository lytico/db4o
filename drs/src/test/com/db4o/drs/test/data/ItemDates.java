package com.db4o.drs.test.data;

import java.util.*;

public final class ItemDates {
	
	private Date date1;
	private Date date2;
	private Date[] dateArray;
	
	public ItemDates() {
	}
	
	public ItemDates(Date date1, Date date2) {
		this.setDate1(date1);
		this.setDate2(date2);
		this.setDateArray(new Date[] { date1, date2 });
	}
	
	@Override
	public boolean equals(Object obj) {
		ItemDates other = (ItemDates)obj;
		if (!other.getDate1().equals(getDate1())) {
			return false;
		}
		if (!other.getDate2().equals(getDate2())) {
			return false;
		}
		return Arrays.equals(getDateArray(), other.getDateArray());
	}

	@Override
	public String toString() {
		return "ItemDates [_date1=" + getDate1() + ", _date2=" + getDate2();
	}

	public void setDate1(Date date1) {
		this.date1 = date1;
	}

	public Date getDate1() {
		return date1;
	}

	public void setDate2(Date date2) {
		this.date2 = date2;
	}

	public Date getDate2() {
		return date2;
	}

	public void setDateArray(Date[] dateArray) {
		this.dateArray = dateArray;
	}

	public Date[] getDateArray() {
		return dateArray;
	}

	
}