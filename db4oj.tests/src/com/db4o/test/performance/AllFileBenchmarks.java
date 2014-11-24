/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.test.performance;

import java.io.*;


public class AllFileBenchmarks {

    public static void main(String[] args) throws IOException {
        FileDeleteBenchmark.main(args);
        FileSeekBenchmark.main(args);
        FileSyncBenchmark.main(args);
    }

}
