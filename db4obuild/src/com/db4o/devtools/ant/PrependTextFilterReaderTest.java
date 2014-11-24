package com.db4o.devtools.ant;

import java.io.IOException;
import java.io.StringReader;

import junit.framework.TestCase;

import org.apache.tools.ant.types.Parameter;


public class PrependTextFilterReaderTest extends TestCase {
    private final static String HEADER="Header\nHeader";
    private final static String CONTENT="Content\nContent";
    
    public void testCombinations() throws IOException {
        assertHeaderPlusContent("","");
        assertHeaderPlusContent(HEADER,"");
        assertHeaderPlusContent("",CONTENT);
        assertHeaderPlusContent(HEADER,CONTENT);
    }
    
    public void testSkip() throws IOException {
        PrependTextFilterReader reader=buildReader(HEADER,CONTENT);
        assertEquals(HEADER.charAt(0),reader.read());
        reader.skip(HEADER.length()-2);
        assertEquals(HEADER.charAt(HEADER.length()-1),reader.read());
        assertEquals(CONTENT.charAt(0),reader.read());
        reader.skip(1);
        assertEquals(CONTENT.substring(2),readAll(reader,2));

        reader=buildReader(HEADER,CONTENT);
        reader.skip(HEADER.length()+1);
        assertEquals(CONTENT.substring(1),readAll(reader,2));
        
        reader=buildReader(HEADER,CONTENT);
        assertEquals(HEADER.length()+CONTENT.length(),reader.skip(HEADER.length()+CONTENT.length()+1));
    }

    public void testReset() throws IOException {
        PrependTextFilterReader reader=buildReader(HEADER,CONTENT);
        assertEquals(HEADER.charAt(0),reader.read());
        reader.reset();
        for(int idx=0;idx<HEADER.length();idx++) {
            assertEquals(HEADER.charAt(idx),reader.read());
        }
        assertEquals(CONTENT.charAt(0),reader.read());
        reader.reset();
        assertEquals(HEADER+CONTENT,readAll(reader,2));
    }

    private void assertHeaderPlusContent(String header,String content) throws IOException {
        String exp=header+content;
        assertReadAll(header,content);
        assertReadAll(header,content,1);
        if(header.length()>1) {
            assertReadAll(header,content,header.length()-1);
        }
        if(header.length()>0) {
            assertReadAll(header,content,header.length());
        }
        assertReadAll(header,content,header.length()+1);
        if(exp.length()>1) {
            assertReadAll(header,content,exp.length()-1);
        }
        if(exp.length()>0) {
            assertReadAll(header,content,exp.length());
        }
        assertReadAll(header,content,exp.length()+1);
    }
    
    private void assertReadAll(String header,String content) throws IOException {
        PrependTextFilterReader reader = buildReader(header, content);
        StringBuffer read=new StringBuffer();
        int curchar=-1;
        while((curchar=reader.read())>-1) {
            read.append((char)curchar);
        }
        reader.close();
        assertEquals(header+content,read.toString());
    }

    private void assertReadAll(String header,String content,int bufsize) throws IOException {
        PrependTextFilterReader reader = buildReader(header, content);
        assertEquals(header+content,readAll(reader, bufsize));
    }
    
    private String readAll(PrependTextFilterReader reader, int bufsize) throws IOException {
        StringBuffer read=new StringBuffer();
        int charsread=0;
        char[] buf=new char[bufsize];
        while((charsread=reader.read(buf))>-1) {
            read.append(buf,0,charsread);
        }
        reader.close();
        return read.toString();
    }

    private PrependTextFilterReader buildReader(String header, String content) {
        PrependTextFilterReader reader=new PrependTextFilterReader(new StringReader(content));
        reader.setParameters(buildParam(header));
        return reader;
    }

    private Parameter[] buildParam(String value) {
        Parameter param=new Parameter();
        param.setValue(value);
        param.setName("headertext");
        return new Parameter[]{param};
    }
}