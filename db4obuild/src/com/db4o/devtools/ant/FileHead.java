package com.db4o.devtools.ant;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.regex.Matcher;

@SuppressWarnings("serial")
public class FileHead extends File {
    
    private final FileHeadAntTask task;

    public static void main(String[] args) throws Exception {
        if (args == null || args.length != 4) {
            String errMsg = "Usage: FileHead#main(path, header, before, fileExt)";
            System.err.println(errMsg);
            throw new IllegalArgumentException(errMsg);
        }
        FileHeadAntTask task = new FileHeadAntTask();
        task.path = args[0];
        task.setHeader(args[1]);
        task.setBeforePattern(args[2]);
        task.fileExt = args[3];
        new FileHead(args[0], task).run();
    }

    public FileHead(String file, FileHeadAntTask task) {
        super(file);
        this.task = task;
    }

    public FileHead(String dir, String file, FileHeadAntTask task) {
        super(dir, file);
        this.task = task;
    }

    public void run() throws Exception {
        if (isDirectory()) {
            processDirectory();
        } else {
            processFile();
        }
    }

	private void processFile() throws IOException {
		String contents = read();
		Matcher matcher = task.before.matcher(contents);
		if (matcher.find()) {
			writeWithHeader(contents.substring(matcher.start()));
		}
	}
	
	private void writeWithHeader(String contents) throws IOException {
		FileOutputStream fos = new FileOutputStream(this);
		try {
			OutputStreamWriter writer = new OutputStreamWriter(fos);
			writer.write(task.header);
			writer.write(contents);
			writer.flush();
		} finally {
			fos.flush();
			fos.close();
		}
	}
	
	private String read() throws IOException {
		FileInputStream fis = new FileInputStream(this);
		try {			
			int read;
			StringBuilder builder = new StringBuilder();
			char[] buffer = new char[32*1024];
			InputStreamReader reader = new InputStreamReader(fis);
			while ((read = reader.read(buffer)) > 0) {
				builder.append(buffer, 0, read);
			}
			return builder.toString();
		} finally {
			fis.close();
		}
	}
	
	private void processDirectory() throws Exception {
		String[] files = list();
		if (files != null) {
		    for (int i = 0; i < files.length; i++) {
		        FileHead child = new FileHead(getAbsolutePath(),
		            files[i], task);
		        if (child.isDirectory() || task.fileExt == null
		            || files[i].endsWith(task.fileExt)) {
		            child.run();
		        }
		    }
		}
	}
}