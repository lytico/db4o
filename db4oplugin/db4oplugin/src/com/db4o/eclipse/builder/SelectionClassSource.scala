/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package builder

import com.db4o.instrumentation.file._
import org.eclipse.jdt.core._
import org.eclipse.core.runtime._
import java.io._

class SelectionClassSource(classFile: IClassFile) extends InstrumentationClassSource {
  
  val binaryRoot = new BinaryRoot(classFile)
  
  override def sourceFile = PDEUtil.workspaceFile(classFile.getPath)

  override def className = {
    val withExt = relativePath.segments.mkString(".")
    withExt.substring(0, withExt.length - ".class".length)
  }

  override def targetPath(targetBase: java.io.File) =
    new java.io.File(targetBase, relativePath.segments.mkString("/"))
    
  override def inputStream = new FileInputStream(sourceFile)

  override def toString = className + ": " + sourceFile.toString
  
  private def relativePath = {
    val path = classFile.getPath
    path.removeFirstSegments(path.matchingFirstSegments(binaryRoot.path))
  }
  
}
