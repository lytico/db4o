package decaf.application;

import java.util.*;

import decaf.core.*;

public class DecafCommandLine {

	public String project;
	public final List<String> projectReferences = new ArrayList<String>();
	public final List<String> classpath = new ArrayList<String>();
	public final List<TargetPlatform> targetPlatforms = new ArrayList<TargetPlatform>();
	public boolean build;
	public final List<String> srcFolders = new ArrayList<String>();

}
