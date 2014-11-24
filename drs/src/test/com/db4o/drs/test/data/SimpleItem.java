package com.db4o.drs.test.data;

public class SimpleItem {
	
	private String value;
	
	private SimpleItem child;
	
	private SimpleListHolder parent;
	
	public SimpleItem() {
		
	}

	public SimpleItem(SimpleListHolder parent_, SimpleItem child_, String value_) {
		parent = parent_;
		value = value_;
		child = child_;
	}
	
	public SimpleItem(String value_) {
		this(null, null, value_);
	}
	
	public SimpleItem(SimpleItem child, String value_) {
		this(null, child, value_);
	}
	
	public SimpleItem(SimpleListHolder parent_, String value_) {
		this(parent_, null, value_);
	}
	
	public String getValue() {
		return value;
	}

	public void setValue(String value_) {
		value = value_;
	}

	public SimpleItem getChild() {
		return getChild(0);
	}
	
	public SimpleItem getChild(int level) {
		SimpleItem tbr = child;
		while (--level > 0 && tbr != null) {
			tbr = tbr.child;
		}
			
		return tbr;
	}	

	public void setChild(SimpleItem child_) {
		child = child_;
	}

	public SimpleListHolder getParent() {
		return parent;
	}

	public void setParent(SimpleListHolder parent_) {
		parent = parent_;
	}
	
	@Override
	public boolean equals(Object obj) {
		if (obj.getClass() != SimpleItem.class) {
			return false;
		}
		
		SimpleItem rhs = (SimpleItem) obj;
		return rhs.getValue().equals(getValue());
	}
	
	@Override
	public String toString() {
		String childString;
		
		if (child != null) {
			childString = child != this ? child.toString() : "this";
		}
		else {
			childString = "null";
		}
		
		return value + "[" + childString + "]";
	}
}
