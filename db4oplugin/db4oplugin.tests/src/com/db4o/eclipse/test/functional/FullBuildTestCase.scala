package com.db4o.eclipse.test.functional


import com.db4o.eclipse.preferences._

import com.db4o.eclipse.test.util._

import org.junit._


class FullBuildTestCase extends Db4oPluginTestCaseTrait {

  @Before
  override def setUp {
    super.setUp
    WorkspaceUtilities.setAutoBuilding(false)
  }
  
  @Test
  def testFullBuild {
    val otherSrcFolder = project.addSourceFolder("src_other", "bin_other")
    project.createCompilationUnit("one", "One.java", "public class One {}")
    project.createCompilationUnit(otherSrcFolder, "other", "Other.java", "public class Other {}")
    project.buildProject(null)
    assertSingleClassInstrumented("one.One", true)
    assertSingleClassInstrumented(otherSrcFolder, "other.Other", true)
  }
  
}
