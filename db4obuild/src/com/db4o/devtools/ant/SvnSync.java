package com.db4o.devtools.ant;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.channels.FileChannel;

import org.apache.tools.ant.BuildException;
import org.apache.tools.ant.Task;
import org.tmatesoft.svn.core.SVNDepth;
import org.tmatesoft.svn.core.SVNException;
import org.tmatesoft.svn.core.wc.SVNWCClient;
import org.tmatesoft.svn.core.wc.SVNWCUtil;

public class SvnSync extends Task {
	private String _workingCopy;
	private String _sourceFolder;

	public void setWorkingCopy(String workingCopy) {
		_workingCopy = workingCopy;
	}
	
	public void setSourceFolder(String sourceFolder) {
		_sourceFolder = sourceFolder;
	}
	
	public void execute() throws BuildException {		
		syncSnvFolder(_workingCopy, _sourceFolder);		
	}

	private void syncSnvFolder(String workingCopy, String source) {
		try {
			FolderDiff diff = FolderDiff.diff(workingCopy, source, new FilterFoldersInList(new String[] {".svn"}));
			SVNWCClient workingCopyClient = new SVNWCClient(SVNWCUtil.createDefaultAuthenticationManager(), SVNWCUtil.createDefaultOptions(true));
			
			addNewFilesToWorkingCopy(diff, workingCopyClient);			
			deleteRemovedFilesFromWorkingCopy(diff, workingCopyClient);
			copyChangedFilesToWorkingCopy(diff);
			
		} catch (IOException e) {
			throw new BuildException("Failed to generate folder diff for '" + workingCopy + "' and '" + source + "'", e);
		} catch (SVNException svne) {
			throw new BuildException("Failed sync folders '" + workingCopy + "' and '" + source + "'", svne);
		}
	}

	private void copyChangedFilesToWorkingCopy(FolderDiff diff) throws IOException {
		String sourceFolder = diff.sourceFolder();
		String compareToFolder = diff.compareToFolder();
		for (String file : diff.changedFiles()) {
			copyFile(sourceFolder + file, compareToFolder + file);
		}		
	}

	private void copyFile(String target, String source) throws IOException {
		FileChannel targetChannel = null;
		FileChannel sourceChannel = null;
		
		try {
			new File(target).getParentFile().mkdirs();
			targetChannel = new FileOutputStream(target).getChannel();
			sourceChannel = new FileInputStream(source).getChannel();
			
			targetChannel.transferFrom(sourceChannel, 0, sourceChannel.size());
		}
		finally {
			if (targetChannel != null) targetChannel.close();
			if (sourceChannel != null) sourceChannel.close();			
		}
	}

	private void deleteRemovedFilesFromWorkingCopy(FolderDiff diff, SVNWCClient workingCopyClient) throws SVNException {
		String sourceFolder = diff.sourceFolder();
		for (String deletedFile : diff.deletedFiles()) {
			workingCopyClient.doDelete(new File(sourceFolder + deletedFile), true, false);
		}
	}

    private void addNewFilesToWorkingCopy(FolderDiff diff, SVNWCClient workingCopyClient) throws SVNException, IOException {
		for (String toBeAdded : diff.newFiles()) {
			String newFileInRepository = diff.sourceFolder() + toBeAdded;
			String sourceFile = diff.compareToFolder() + toBeAdded;
			
			copyFile(newFileInRepository, sourceFile);
			workingCopyClient.doAdd(new File(newFileInRepository), true, false, true, SVNDepth.INFINITY, false, true);
		}
	}
}
