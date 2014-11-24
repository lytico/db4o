package com.db4o.rmi;

import java.io.*;

public interface OutputStreamProxy {
    
    void write(byte[] buffer) throws IOException;

}
