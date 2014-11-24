package com.db4o.devtools.ant;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;

public class IO {
    public static void writeAll(File file, String contents) throws IOException {
        OutputStreamWriter writer = new OutputStreamWriter(
                new FileOutputStream(file), "ISO-8859-1");
        try {
            writer.write(contents);
        } finally {
            writer.close();
        }
    }

    public static String readAll(File file) throws IOException {
        StringBuilder builder = new StringBuilder();
        InputStreamReader reader = new InputStreamReader(new FileInputStream(
                file), "ISO-8859-1");
        try {
            char[] buffer = new char[1024];
            int count;
            while (-1 != (count = reader.read(buffer))) {
                builder.append(buffer, 0, count);
            }
        } finally {
            reader.close();
        }
        return builder.toString();
    }
}
