package com.db4o.objectManager.v2.util;

import com.db4o.objectManager.v2.MainPanel;

import java.util.Date;

/**
 * User: treeder
 * Date: Mar 9, 2007
 * Time: 12:15:14 PM
 */
public class Converter {
	public static Object convertFromString(Class c, String newValue) throws Exception {
		Object newOb;
		if(Date.class.isAssignableFrom(c)){
			newOb = MainPanel.dateFormatter.parse(newValue);
		} else {
			newOb = com.spaceprogram.db4o.sql.Converter.convertFromString(c, newValue);
		}
		return newOb;
	}
}
