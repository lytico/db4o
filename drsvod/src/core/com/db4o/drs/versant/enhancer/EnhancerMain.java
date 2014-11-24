/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.enhancer;

import java.io.*;

import com.versant.core.jdo.tools.enhancer.*;

/**
 * @sharpen.ignore
 */
public class EnhancerMain {
	
	public static void main(String[] args) throws Exception {
		
		if(args == null || args.length < 1 || args.length > 2){
			throw new RuntimeException("Expected one or two argument: PropertiesFilePath [outputDirectory]");
		}
		String propertiesFilePath = args[0];
		
		
		
		Enhancer enhancer = new Enhancer();
		if (args.length == 2) {
			enhancer.setOutputDir(new File(args[1]));
		}
		enhancer.setPropertiesFile(new File(propertiesFilePath));
		enhancer.enhance();
	}

}
