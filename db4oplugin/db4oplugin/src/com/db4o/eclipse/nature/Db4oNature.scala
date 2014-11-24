/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package nature

import com.db4o.eclipse.builder._
import org.eclipse.core.resources._

object Db4oNature {
  val NATURE_ID = "db4oplugin.db4oNature"
  
  def toggleNature(project: IProject) {
	val description = project.getDescription
	val origNatures = description.getNatureIds
    try {
	    val newNatures: Array[String] = 
	      if (origNatures.contains(NATURE_ID)) 
	        origNatures.filter(!_.equals(NATURE_ID))
	      else
	        origNatures ++ Array(NATURE_ID)
	    description.setNatureIds(newNatures)
		project.setDescription(description, null)
    }
    catch {
      case e: Exception => e.printStackTrace
    }
  }

  def hasDb4oNature(project: IProject) =
	project.getDescription.getBuildSpec.exists(_.getBuilderName.equals(Db4oInstrumentationBuilder.BUILDER_ID))

}

class Db4oNature extends IProjectNature {

  var project: IProject = null
  
  def configure {
    try {
	  if(Db4oNature.hasDb4oNature(project)) {
		return
	  }
	  val desc = project.getDescription
	  val commands = desc.getBuildSpec
	  val newCommand = desc.newCommand
	  newCommand.setBuilderName(Db4oInstrumentationBuilder.BUILDER_ID)
	  val newCommands = commands ++ Array(newCommand)
	  desc.setBuildSpec(newCommands)
	  project.setDescription(desc, null)
    }
    catch {
      case e: Exception => e.printStackTrace
    }
  }

  def deconfigure  {
    try {
	  val description = getProject.getDescription
	  val commands = description.getBuildSpec
	  val newCommands = commands.filter(!_.getBuilderName.equals(Db4oInstrumentationBuilder.BUILDER_ID))
	  description.setBuildSpec(newCommands)
    }
    catch {
      case e: Exception => e.printStackTrace
    }
  }

  def getProject = project

  def setProject(project: IProject) {
    this.project = project
  }

}
