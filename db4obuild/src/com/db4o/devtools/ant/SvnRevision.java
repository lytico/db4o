/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.devtools.ant;

import java.io.*;

import org.apache.tools.ant.*;
import org.tmatesoft.svn.core.*;
import org.tmatesoft.svn.core.wc.*;

/**
 * Gets the svn revision from a specified resource and puts it
 * into the specified property.
 * 
 * If no resource is specified then the project's directory is
 * assumed to be the resource.
 */
public class SvnRevision extends Task {
	
	private String _property;
	private File _resource;
	
	public void setProperty(String property) {
		_property = property;
	}
	
	public void setResource(File resource) {
		_resource = resource;
	}
	
	@Override
	public void execute() throws BuildException {
		try {
			getProject().setProperty(_property, resourceRevision());
		} catch (SVNException e) {
			throw new BuildException(e, getLocation());
		}
	}

	private String resourceRevision() throws SVNException {
		SVNWCClient c = new SVNWCClient(SVNWCUtil.createDefaultAuthenticationManager(), SVNWCUtil.createDefaultOptions(true));
		SVNInfo info = c.doInfo(resource(), SVNRevision.COMMITTED);
		return Long.toString(info.getRevision().getNumber());
	}


	private File resource() {
		if (null == _resource) return getProject().getBaseDir();
		return _resource;
	}

}
