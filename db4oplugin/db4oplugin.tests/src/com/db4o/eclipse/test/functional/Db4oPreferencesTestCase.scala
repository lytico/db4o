package com.db4o.eclipse.test.functional

import com.db4o.eclipse.preferences._

import com.db4o.eclipse.test.util._

import org.junit._

import Assert._

import scala.collection.immutable._

class Db4oPreferencesTestCase extends Db4oPluginTestCaseTrait {

  @Before
  override def setUp {
    super.setUp
    WorkspaceUtilities.setAutoBuilding(true)
  }

  @Test
  def testRegExpPersistence {
    assertEquals(".*", Db4oPreferences.getFilterRegExp(project.getProject).toString)
    Db4oPreferences.setFilterRegExp(project.getProject, pattern("foo"))
    assertEquals("foo", Db4oPreferences.getFilterRegExp(project.getProject).toString)
  }
  
  @Test
  def testPackageListPersistence {
    assertTrue(Db4oPreferences.getPackageList(project.getProject).isEmpty)
    Db4oPreferences.setPackageList(project.getProject, ListSet("foo"))
    val retrieved = Db4oPreferences.getPackageList(project.getProject)
    assertEquals(1, retrieved.size)
    assertEquals("foo", retrieved.elements.next)
  }
  
  @Test
  def testCombinatorPersistence {
    assertEquals(AndOrEnum.Or, Db4oPreferences.getFilterCombinator(project.getProject))
    Db4oPreferences.setFilterCombinator(project.getProject, AndOrEnum.And)
    assertEquals(AndOrEnum.And, Db4oPreferences.getFilterCombinator(project.getProject))
  }
  
  private def pattern(str: String) = java.util.regex.Pattern.compile(str)
}
