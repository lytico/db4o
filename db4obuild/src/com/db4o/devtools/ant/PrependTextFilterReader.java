package com.db4o.devtools.ant;

import java.io.FilterReader;
import java.io.IOException;
import java.io.Reader;

import org.apache.tools.ant.types.Parameter;
import org.apache.tools.ant.types.Parameterizable;


public class PrependTextFilterReader extends FilterReader implements Parameterizable {    
    private int headeridx;
    private String text;
    
    public PrependTextFilterReader(Reader in) {        
        super(in);
    }
    
    public int read() throws IOException {
        if(hasHeaderCharsLeft()) {
            return text.charAt(headeridx++);
        }
        return super.read();
    }
    
    public int read(char[] data,int offset,int length) throws IOException {
        int totalnumread=0;
        if(hasHeaderCharsLeft()) {
            int numread=min(length,numHeaderCharsLeft());
            String headerpart=text.substring(headeridx,headeridx+numread);
            System.arraycopy(headerpart.toCharArray(),0,data,offset,numread);
            headeridx+=numread;
            totalnumread=numread;
        }
        if(totalnumread<length) {
            int numread=super.read(data,offset+totalnumread,length-totalnumread);
            if(numread>-1) {
                totalnumread+=numread;
            }
            if(totalnumread==0) {
            	totalnumread=-1;
            }
        }
        return totalnumread;
    }

    public boolean ready() throws IOException {
        return (hasHeaderCharsLeft() ? true : super.ready());
    }

    public int skip(int numchars) throws IOException {
        int skipped=0;
        if(hasHeaderCharsLeft()) {
            skipped=min(numchars,numHeaderCharsLeft());
            headeridx+=skipped;
        }
        if(skipped<numchars) {
            skipped+=super.skip(numchars-skipped);
        }
        return skipped;
    }
    
    public boolean markSupported() {
        return false;
    }
    
    public void mark(int limit) {
        throw new UnsupportedOperationException("mark() is not supported by this stream.");
    }

    public void reset() throws IOException {
        super.reset();
        headeridx=0;
    }
    
    public void setParameters(Parameter[] params) {
        this.text=params[0].getValue();
        headeridx=0;
    }

    private boolean hasHeaderCharsLeft() {
        return headeridx<text.length();
    }

    private int numHeaderCharsLeft() {
        return text.length()-headeridx;
    }
    
    private int min(int a,int b) {
        return (a<b ? a : b);
    }
}