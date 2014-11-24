package com.db4o.eclipse.test.util

import org.eclipse.core.resources._
import org.eclipse.core.runtime._

object TestProject {

  def apply(name: String) = new TestProject(WorkspaceUtilities.getProject(name), null)
  def apply(name: String, monitor: IProgressMonitor) = new TestProject(WorkspaceUtilities.getProject(name), monitor)
  
}

class TestProject(val project: IProject, monitor: IProgressMonitor) {
  
    WorkspaceUtilities.initializeProject(project, monitor)

	def getName = project.getName()

	def createFolder(name: String): IFolder = createFolder(name, null)
	
	def createFolder(name: String, monitor: IProgressMonitor) = {
		val folder = getFolder(name)
		folder.create(false, true, monitor)
		folder
	}

	def getFolder(name: String) = project.getFolder(name)

	def dispose {
		try {
			project.delete(true, true, null)
		} 
        catch  {
          case e => e.printStackTrace
		}
	}
	
	def addReferencedProject(reference: IProject, monitor: IProgressMonitor) {
		if (null == reference) throw new IllegalArgumentException("reference")
		WorkspaceUtilities.addProjectReference(project, reference, monitor)
	}

	def getFile(path: String) = project.getFile(path)
}