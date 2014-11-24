package com.db4o.objectManager.v2.util;

import java.text.*;
import java.util.*;

/**
 * User: treeder
 * Date: Mar 9, 2007
 * Time: 11:51:26 AM
 */
public class DateFormatter {

	private String pattern = "yyyy-MM-dd HH:mm:ss";
	private SimpleDateFormat displayFormat = new SimpleDateFormat(pattern + " z");
	
	public static SimpleDateFormat editFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
	{
		//sdf.setTimeZone(TimeZone.getTimeZone("UTC"));
	}

	public String display(Date value) {
		return displayFormat.format(value);
	}

	public String edit(Date value){
		return editFormat.format(value);
	}

	public Date parse(String value) throws ParseException {
		return editFormat.parse(value);
	}
}
