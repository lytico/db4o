/* Copyright (C) 2008 Versant Inc. http://www.db4o.com */

package com.db4o.devtools.ant;


import java.util.*;
import org.apache.tools.ant.*;

public class Db4oIterationTask extends Task {
	
	private String _property;
	
	public void setProperty(String property) {
		_property = property;
	}
	
	@Override
	public void execute() throws BuildException {
		getProject().setProperty(_property, String.valueOf(calcWeek()));
	}

	static int calcWeek() {
		TimeZone timeZone = TimeZone.getTimeZone("UTC");
		Calendar startCalendar = new GregorianCalendar(timeZone);
		startCalendar.set(Calendar.YEAR, 2007);
		startCalendar.set(Calendar.WEEK_OF_YEAR, 30);
		startCalendar.set(Calendar.DAY_OF_WEEK, Calendar.WEDNESDAY);
		startCalendar.set(Calendar.HOUR_OF_DAY, 0);
		startCalendar.set(Calendar.MINUTE, 0);
		startCalendar.set(Calendar.SECOND, 0);
		int weekCount = 0;
		Calendar nowCalendar = new GregorianCalendar(timeZone);
		while(startCalendar.before(nowCalendar)) {
			weekCount++;
			startCalendar.add(Calendar.WEEK_OF_YEAR, 1);
		}
		return weekCount;
	}

	public static void main(String[] args) {
		System.out.println(calcWeek());
	}
}
