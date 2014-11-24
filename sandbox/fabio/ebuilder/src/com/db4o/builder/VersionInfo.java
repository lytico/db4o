package com.db4o.builder;

import java.util.*;

public class VersionInfo {
	
	private int major = 8;
	private int minor = 1;
	private int iteration = currentWeek();
	private int revision = 0;
	
	private String versionLabel = major + "." + minor + "." + iteration + "." + revision;

	public static int currentWeek() {
		TimeZone utc = TimeZone.getTimeZone("UTC");
		Calendar calendar1 = Calendar.getInstance(utc);
		Calendar calendar2 = Calendar.getInstance(utc);
		calendar1.set(2011, 1, 22, 15, 0, 0);
		calendar2.setTime(new Date());
		long milliseconds1 = calendar1.getTimeInMillis();
		long milliseconds2 = calendar2.getTimeInMillis();
		long diff = milliseconds2 - milliseconds1;
		long diffWeeks = (long) (diff / (7 * 24 * 60 * 60 * 1000.));
		return (int) (diffWeeks + 188);
	}

	public int major() {
		return major;
	}

	public int minor() {
		return minor;
	}

	public int iteration() {
		return iteration;
	}

	public int revision() {
		return revision;
	}

	@Override
	public String toString() {
		return versionLabel;
	}

	
}
