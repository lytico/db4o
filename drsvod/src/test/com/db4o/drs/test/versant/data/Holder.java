/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant.data;

import java.util.*;

public class Holder {
	
	private Item _item;
	
	private List<Item> _items;
	
	public Holder() {
	}
	
	public Holder(Item item, Item...items){
		_item = item;
		_items = new ArrayList<Item>();
		for (Item listItem : items) {
			_items.add(listItem);
		}
	}
	
	@Override
	public boolean equals(Object obj) {
		if(! (obj instanceof Holder)){
			return false;
		}
		Holder other = (Holder) obj;
		if(_item == null){
			if(other._item != null){
				return false;
			}
		}else{
			if(! _item.equals(other._item)){
				return false;
			}
		}
		return _items.equals(other._items);
	}
	
	
	
	
	

}
