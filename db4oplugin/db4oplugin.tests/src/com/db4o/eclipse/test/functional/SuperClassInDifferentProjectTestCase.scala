package com.db4o.eclipse.test.functional

import com.db4o.eclipse.nature._

import com.db4o.eclipse.test.util._

import org.junit._

class SuperClassInDifferentProjectTestCase {

  private var projectA: JavaProject = null
  private var projectB: JavaProject = null
  
  @Before
  def setUp {
	projectA = createProject("projectA")
	projectB = createProject("projectB")
  }
  
  @After
  def tearDown {
    projectB.dispose
    projectA.dispose
  }
  
  @Test
  def testFullBuild {
    assertBuild(false)
  }

    @Test
  def testAutoBuild {
    assertBuild(true)
  }

  private def assertBuild(auto: Boolean) {
	WorkspaceUtilities.setAutoBuilding(auto)
	projectB.addReferencedProject(projectA.getProject, null)
	projectA.createCompilationUnit("sup", "Sup.java", "public class Sup {}")
	ensureBuild(projectA, auto)
	projectB.createCompilationUnit("sub", "Sub.java", "public class Sub extends sup.Sup {}")
	ensureBuild(projectB, auto)
	Db4oPluginTestUtil.assertSingleClassInstrumented(projectB.getMainSourceFolder, "sub.Sub", true)
  }

  private def ensureBuild(project: JavaProject, auto: Boolean) {
    if(auto)
    	project.joinAutoBuild
    else
    	project.buildProject(null)
  }
  
  private def createProject(name: String) = {
	  val project = new JavaProject(name)
	  Db4oNature.toggleNature(project.getProject)
	  project.buildProject(null)
	  project
  }
}
