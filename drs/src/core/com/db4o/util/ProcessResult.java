/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.util;

public class ProcessResult {

	public final String command;
	
	public final String out;
	
	public final String err;
	
	public final int returnValue;

	public ProcessResult(String command, String out, String err, int returnValue) {
		this.command = command;
		this.out = out;
		this.err = err;
		this.returnValue = returnValue;
	}
	
    private String formatOutput(String task, String output){
        return headLine(task) + output + "\n";
    }
    
    private String headLine(String task){
        return "\n" + task + "\n----------------\n";  
    }
    
    public String toString(){
        String res = formatOutput("IOServices.exec", command); 
        res += formatOutput("out", out); 
        res += formatOutput("err", err); 
        res += formatOutput("result", String.valueOf(returnValue));
        return res;  
    }

}
