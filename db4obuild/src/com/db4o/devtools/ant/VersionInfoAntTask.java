package com.db4o.devtools.ant;

import java.io.*;

import org.apache.tools.ant.*;

public class VersionInfoAntTask extends Task {
    
    private int major;
	private int minor;
    
	private int iteration;
	
    private String path;
    private String revision;    
    
    public void setPath(String path) {
        this.path = path;
    }
    
    public void setMajor(int major){
        this.major = major;
    }
    
    public void setMinor(int minor) {
    	this.minor = minor;
    }

    public void setIteration(int iteration){
        this.iteration = iteration;
    }

    public void setRevision(String revision){
        this.revision = revision;
    }
    
    private void outputJavaVersionInfo(PrintWriter pr) {
        String name = new StringBuffer().append(major).append(".")
                .append(minor).append(".").append(iteration).append(".")
                .append(revision).toString();
        
        pr.println("/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */");
        pr.println();
        pr.println("package com.db4o;");
        pr.println();
        pr.println("/**");
        pr.println("* @exclude");
        pr.println("*/");
        pr.println("public class Db4oVersion {");
        pr.println("    public static final String NAME = \"" + name + "\";");
        pr.println("    public static final int MAJOR = " + major + ";");
        pr.println("    public static final int MINOR = " + minor + ";");
        pr.println("    public static final int ITERATION = " + iteration + ";");
        pr.println("    public static final int REVISION = " + revision + ";");
        pr.println("}");
    }
    
    public void execute() throws BuildException {
        String fileName = "Db4oVersion.java";
        try{
	        File dir = new File(path);
	        File file = new File(dir, fileName);
	        file.delete();
	        FileOutputStream fos = new FileOutputStream(file);
	        PrintWriter pr = new PrintWriter(fos);
	        outputJavaVersionInfo(pr);
	        pr.close();
	        fos.close();
        }catch(Exception e){
            throw new BuildException(e);
        }
    }
}
