/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package builder

import org.eclipse.core.resources._
import org.eclipse.jdt.core._

private class ModifiedClassFileCollectorVisitor extends IResourceDeltaVisitor {
		
  private var classSources = scala.collection.mutable.HashSet[SelectionClassSource]()
				
  override def visit(delta: IResourceDelta): Boolean = {
    if(delta.getKind == IResourceDelta.REMOVED) {
      return false
    }
    val resource = delta.getResource
    if(resource.getType != IResource.FILE) {
      return true
    }
    if(!"class".equals(resource.getFileExtension)) {
      return false
    }
    val classFileOption = classFile(resource)
    if(!classFileOption.isDefined) {
      return false
    }
    classSources += new SelectionClassSource(classFileOption.get)
    false
  }
  
  def getClassSources: scala.collection.Set[SelectionClassSource] = classSources
  
  private def classFile(resource: IResource) = {
    val javaElement = JavaCore.create(resource)
    if(javaElement == null || !javaElement.isInstanceOf[IClassFile]) 
      None 
    else 
      Some(javaElement.asInstanceOf[IClassFile])
  }	
}
