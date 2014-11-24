/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.util.test;

import java.io.*;

import com.db4o.db4ounit.util.*;

import db4ounit.*;
import db4ounit.extensions.util.IOServices.*;

public class JavaServicesTestCase implements TestCase {
    
    public static class ShortProgram {
        
        public static final String OUTPUT = "XXshortXX";  
        
        public static void main(String[] arguments) {
            System.out.println(OUTPUT);
        }
    }
    
    public static class LongProgram {
        
        public static final String OUTPUT = "XXlongXX";
        
        public static void main(String[] arguments) {
            System.out.println(OUTPUT);
            try {
                Thread.sleep(Long.MAX_VALUE);
            } catch (InterruptedException e) {
            }
        }
    }
    
    /**
     * @sharpen.remove
     */
    public void _testJava() throws IOException, InterruptedException{
        String output = JavaServices.java(ShortProgram.class.getName());
        Assert.isTrue(output.indexOf(ShortProgram.OUTPUT) >=0 );
    }
    
    /**
     * @sharpen.remove
     */
    public void _testStartAndKillJavaProcess() throws IOException{
        Assert.expect(DestroyTimeoutException.class, new CodeBlock() {
            public void run() throws Throwable {
                JavaServices.startAndKillJavaProcess(LongProgram.class.getName(), ShortProgram.OUTPUT, 50);
            }
        });
        JavaServices.startAndKillJavaProcess(LongProgram.class.getName(), LongProgram.OUTPUT, 5000);
    }

}
