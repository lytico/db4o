
package com.db4o.devtools.ant;

import java.util.*;
import java.util.regex.Pattern;

import org.apache.tools.ant.*;
import org.apache.tools.ant.types.*;

public class FileHeadAntTask extends Task {
    
    private List<FileSet> _sources = new ArrayList<FileSet>();

    String header;
    Pattern before;
    
    // not used for Ant taks, only for run from FileHead#main()
    String path;

    // not used for Ant taks, only for run from FileHead#main()
    String fileExt;
    
    public FileSet createSources() {
        FileSet set = new FileSet();
        _sources.add(set);
        return set;
    }
    
    public void setHeader(String header) {
        this.header = header;
    }
    
    public void setBeforePattern(String before) {
        this.before = Pattern.compile(before);
    }
    
    public void execute() throws BuildException {
        for (FileSet fs : _sources) {
            DirectoryScanner scanner = fs.getDirectoryScanner(this.getProject());
            String[] fileNames = scanner.getIncludedFiles();
            for (int i = 0; i < fileNames.length; i++) {
                FileHead fh = new FileHead(scanner.getBasedir().getAbsolutePath(), fileNames[i], this);
                try {
                    log("Adding header to " + fh.getAbsolutePath(), Project.MSG_VERBOSE);
                    fh.run();
                } catch (Exception e) {
                    e.printStackTrace();
                    throw new BuildException(e.getMessage());
                }
            }
        }
    }
}
