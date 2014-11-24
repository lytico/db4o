package decaf.application;

import java.util.*;

import org.eclipse.core.resources.*;
import org.eclipse.core.runtime.*;
import org.eclipse.equinox.app.*;

import sharpen.core.*;
import sharpen.core.framework.*;
import sharpen.core.framework.resources.*;
import decaf.core.*;

public class DecafApplication implements IApplication {

	public Object start(IApplicationContext context) throws Exception {
		try {
			final DecafCommandLine commandLine = DecafCommandLineParser.parse(argv(context));
			if (commandLine.srcFolders.isEmpty()) commandLine.srcFolders.add("src");
			
			disableAutoBuilding();
			
			final ConsoleProgressMonitor monitor = new ConsoleProgressMonitor();
			final JavaProject project = new JavaProject.Builder(monitor, commandLine.project)
				.sourceFolders(commandLine.srcFolders)
				.nature(DecafNature.NATURE_ID)
				.projectReferences(commandLine.projectReferences)
				.classpath(commandLine.classpath)
				.project;
			
			final DecafProject decaf = DecafProject.create(project.getJavaProject());
			final List<TargetPlatform> targetPlatforms = commandLine.targetPlatforms;
			if (!targetPlatforms.isEmpty()) {
				decaf.setTargetPlatforms(toArray(targetPlatforms));
			}
			
			project.buildProject(monitor);
			
			if (commandLine.build) {
				for (DecafProject.OutputTarget target : decaf.targets()) {
					target.targetProject().getProject().build(IncrementalProjectBuilder.INCREMENTAL_BUILD, monitor);
				}
			}
		} 
		catch (CoreException x) {
			IStatus[] children = x.getStatus().getChildren();
			for(IStatus child : children) {
				System.err.println(child);
				Throwable childExc = child.getException();
				if(childExc != null) {
					childExc.printStackTrace();
				}
			}
			x.printStackTrace();
			throw x;
		}
		catch (Exception x) {
			x.printStackTrace();
			throw x;
		}
		return IApplication.EXIT_OK;
	}

	private TargetPlatform[] toArray(final List<TargetPlatform> targetPlatforms) {
		return targetPlatforms.toArray(new TargetPlatform[targetPlatforms.size()]);
	}
	
	private static void disableAutoBuilding() throws CoreException {
		WorkspaceUtilities.setAutoBuilding(false);
	}

	public void stop() {
		// nothing to do
	}
	
	private String[] argv(IApplicationContext context) {
		return (String[])context.getArguments().get(IApplicationContext.APPLICATION_ARGS);
	}
}
