/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.types.arrays;

import db4ounit.*;
import db4ounit.extensions.*;

public class ArrayNOrderTestCase extends AbstractDb4oTestCase {

	public static class Data {
	    public String[][][] _strArr;
	    public Object[][] _objArr;

	    public Data(String[][][] strArr, Object[][] objArr) {
			this._strArr = strArr;
			this._objArr = objArr;
		}
	}
	
	
    protected void store() {
        String[][][] strArr = new String[2][2][3];
        strArr[0][0][0] = "000";
		strArr[0][0][1] = "001";
		strArr[0][0][2] = "002";
        strArr[0][1][0] = "010";
		strArr[0][1][1] = "011";
		strArr[0][1][2] = "012";
        strArr[1][0][0] = "100";
		strArr[1][0][1] = "101";
		strArr[1][0][2] = "102";
        strArr[1][1][0] = "110";
		strArr[1][1][1] = "111";
		strArr[1][1][2] = "112";

        Object[][] objArr = new Object[2][2];
        objArr[0][0] = new Integer(0);
        objArr[0][1] = "01";
        objArr[1][0] = new Float(10);
        objArr[1][1] = new Double(1.1);
        db().store(new Data(strArr,objArr));
    }

    public void test() {
    	Data data = (Data)retrieveOnlyInstance(Data.class);
    	check(data);
    }
    
    public void check(Data data){
		Assert.areEqual("000",data._strArr[0][0][0]);
		Assert.areEqual("001",data._strArr[0][0][1]);
		Assert.areEqual("002",data._strArr[0][0][2]);
		Assert.areEqual("010",data._strArr[0][1][0]);
		Assert.areEqual("011",data._strArr[0][1][1]);
		Assert.areEqual("012",data._strArr[0][1][2]);
		Assert.areEqual("100",data._strArr[1][0][0]);
		Assert.areEqual("101",data._strArr[1][0][1]);
		Assert.areEqual("102",data._strArr[1][0][2]);
		Assert.areEqual("110",data._strArr[1][1][0]);
		Assert.areEqual("111",data._strArr[1][1][1]);
		Assert.areEqual("112",data._strArr[1][1][2]);

		Assert.areEqual(new Integer(0),data._objArr[0][0]);
		Assert.areEqual("01",data._objArr[0][1]);
		Assert.areEqual(new Float(10),data._objArr[1][0]);
		Assert.areEqual(new Double(1.1),data._objArr[1][1]);
    }
    

}
