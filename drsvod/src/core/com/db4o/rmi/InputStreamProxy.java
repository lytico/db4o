package com.db4o.rmi;

import java.io.*;

public interface InputStreamProxy {
    
    byte[] read(int len) throws IOException;

}
