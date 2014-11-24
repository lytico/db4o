package com.db4o.eclipse.test.functional

import com.db4o.eclipse.preferences._

import com.db4o.eclipse.test.util._

import org.junit._

import AndOrEnum.AndOr

import scala.collection.immutable._

class InstrumentSingleClassTestCase extends Db4oPluginTestCaseTrait {

  @Before
  override def setUp {
    super.setUp
    WorkspaceUtilities.setAutoBuilding(true)
  }
  
  @Test
  def classesAreInstrumentedByDefault {
    assertInstrumentSingleClass(true)
  }

  @Test
  def packageAndRegex {
    assertInstrumented("^foo\\..*", "foo", AndOrEnum.And, true)
  }

  @Test
  def packageOrRegex {
    assertInstrumented("^foo\\..*", "foo", AndOrEnum.Or, true)
  }

  @Test
  def packageAndNotRegex {
    assertInstrumented("^fooX\\..*", "foo", AndOrEnum.And, false)
  }

  @Test
  def packageOrNotRegex {
    assertInstrumented("^fooX\\..*", "foo", AndOrEnum.Or, true)
  }

  @Test
  def notPackageAndRegex {
    assertInstrumented("^foo\\..*", "fooX", AndOrEnum.And, false)
  }

  @Test
  def notPackageOrRegex {
    assertInstrumented("^foo\\..*", "fooX", AndOrEnum.Or, true)
  }

  @Test
  def notPackageAndNotRegex {
    assertInstrumented("^fooX\\..*", "fooX", AndOrEnum.And, false)
  }

  @Test
  def notPackageOrNotRegex {
    assertInstrumented("^fooX\\..*", "fooX", AndOrEnum.Or, false)
  }

  def assertInstrumented(regEx: String, packageName: String, combinator: AndOr, expected: Boolean) {
    Db4oPreferences.setFilterRegExp(project.getProject, java.util.regex.Pattern.compile(regEx))
    Db4oPreferences.setPackageList(project.getProject, ListSet(packageName))
    Db4oPreferences.setFilterCombinator(project.getProject, combinator)
    assertInstrumentSingleClass(expected)
  }
  
  def assertInstrumentSingleClass(expectInstrumentation: Boolean) {
    project.createCompilationUnit(
      "foo",
      "Foo.java",
      "package foo; public class Foo { private int bar; }"
    )
    project.joinAutoBuild
    assertSingleClassInstrumented("foo.Foo", expectInstrumentation)
  }

}
