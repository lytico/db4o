package com.db4o.objectManager.v2;

/**
 * User: treeder
 * Date: Oct 23, 2006
 * Time: 10:46:33 AM
 */
public enum TabType {
	home			("home.png"),
	classSummary	("coffeebean_view.png"),
	query			("table_sql.png"),
	objectTree		("text_tree.png");

	private String icon;


	TabType(String icon) {
		this.icon = icon;
	}

	public String icon() {
		return icon;
	}
}
