/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import java.util.*;

@SuppressWarnings("unused")
public class TypedPrivate extends RTest
{
	private Boolean oBoolean;
	private Boolean nBoolean;
	private boolean sBoolean;
	
	private Byte oByte;
	private Byte nByte;
	private byte sByte;
	
	private Character oCharacter;
	private Character nCharacter;
	private char sChar;

	private Double oDouble;
	private Double nDouble;
	private double sDouble;
	
	private Float oFloat;
	private Float nFloat;
	private float sFloat;
	
	private Integer oInteger;
	private Integer nInteger;
	private int sInteger;
	
	private Long oLong;
	private Long nLong;
	private long sLong;

	private Short oShort;
	private Short nShort;
	private short sShort;
	
	private String oString;
	private String nString;
	
	private Date oDate;
	private Date nDate;
	
	private ObjectSimplePrivate oObject;
	private ObjectSimplePrivate nObject;

	public void set(int ver){
		if(ver == 1){
			oBoolean = new Boolean(true);
			nBoolean = null;
			sBoolean = false;
	
			oByte = new Byte((byte)(Byte.MAX_VALUE - 2));
			nByte = new Byte((byte)3);
			sByte = Byte.MIN_VALUE + 1;
		
			oCharacter = new Character((char)(Character.MAX_VALUE - 1));
			nCharacter = null;
			sChar = Character.MIN_VALUE;

			oDouble = new Double(Double.MAX_VALUE - 1);
			nDouble = null;
			sDouble = Double.MIN_VALUE;
	
			oFloat = new Float(Float.MAX_VALUE - 1);
			nFloat = null;
			sFloat = Float.MIN_VALUE;
	
			oInteger = new Integer(Integer.MAX_VALUE - 1);
			nInteger = null;
			sInteger = Integer.MIN_VALUE;
	
			oLong = new Long(Long.MAX_VALUE - 1);
			nLong = null;
			sLong = Long.MIN_VALUE;

			oShort = new Short((short)(Short.MAX_VALUE - 1));
			nShort = null;
			sShort = Short.MIN_VALUE;
	
			oString = "db4o rules";
			nString = null;
		
			oDate = new GregorianCalendar(2000,0,1).getTime();
			nDate = null;
		
			oObject = new ObjectSimplePrivate("s1");
			nObject = null;
		}else{
			oBoolean = new Boolean(false);
			nBoolean = new Boolean(true);
			sBoolean = true;
	
			oByte = new Byte((byte)0);
			nByte = new Byte((byte)(Byte.MIN_VALUE + 1));
			sByte = Byte.MAX_VALUE - 1;
		
			oCharacter = new Character((char)0);
			nCharacter = new Character(Character.MIN_VALUE);
			sChar = (char)(Character.MAX_VALUE - 1);

			oDouble = new Double(0);
			nDouble = new Double(Double.MIN_VALUE);
			sDouble = Double.MAX_VALUE - 1;
	
			oFloat = new Float(0);
			nFloat = new Float(Float.MIN_VALUE);
			sFloat = Float.MAX_VALUE - 1;
	
			oInteger = new Integer(0);
			nInteger = new Integer(Integer.MIN_VALUE);
			sInteger = Integer.MAX_VALUE - 1;
	
			oLong = new Long(0);
			nLong = new Long(Long.MIN_VALUE);
			sLong = Long.MAX_VALUE - 1;

			oShort = new Short((short)0);
			nShort = new Short(Short.MIN_VALUE);
			sShort = (short)(Short.MAX_VALUE - 1);
	
			oString = "db4o rules of course";
			nString = "yeah";
		
			oDate = null;
			nDate = new GregorianCalendar(2001,1,1).getTime();
		
			oObject = new ObjectSimplePrivate("s2o");
			nObject = new ObjectSimplePrivate("s2n");
		}
	}
	
	public boolean jdk2(){
		return true;
	}
}
