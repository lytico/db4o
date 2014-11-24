package com.db4o.omplus.datalayer;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

// TODO: to be moved or renamed to a helper
	public class DateFormatter {
		
		private String pattern = "yyyy-MM-dd HH:mm:ss";
		
		 private SimpleDateFormat displayFormat = new SimpleDateFormat(pattern + " z");
		 
		 public static SimpleDateFormat editFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		 
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

