/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package builder

import org.eclipse.jdt.core._

class BinaryRoot(classFile: IClassFile) {
  def path = PDEUtil.binaryRoots(javaProject).find(_.isPrefixOf(classFile.getPath)).get  
  def javaProject = classFile.getJavaProject  
}
