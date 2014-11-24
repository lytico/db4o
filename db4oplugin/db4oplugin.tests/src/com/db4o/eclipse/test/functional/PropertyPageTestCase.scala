package com.db4o.eclipse.test.functional

import com.db4o.eclipse.ui._
import com.db4o.eclipse.preferences._

import org.eclipse.swt._
import org.eclipse.swt.widgets._

import org.junit._

import Assert._

class PropertyPageTestCase extends Db4oPluginTestCaseTrait {

  var shell: Shell = null
  var page: Db4oInstrumentationPropertyPage = null
  
  @Before
  override def setUp {
    super.setUp
    shell = new Shell
    page = new Db4oInstrumentationPropertyPage()
    page.setElement(project.getJavaProject)
    page.createControl(new Composite(shell, SWT.NULL))
  }
  
  @After
  override def tearDown {
    shell.dispose
    super.tearDown
  }
  
  @Test
  def testFilterRegExpText {
    assertTrue(findWidget(page.getControl, Db4oInstrumentationPropertyPage.REGEXP_TEXT_ID).isDefined)
    val regExpText = findWidget(page.getControl, Db4oInstrumentationPropertyPage.REGEXP_TEXT_ID).get.asInstanceOf[Text]
    assertEquals(Db4oPreferences.DEFAULT_REGEXP, regExpText.getText)
    regExpText.setText("\\")
    assertFalse(page.performOk)
    regExpText.setText("foo")
    assertTrue(page.performOk)
    assertEquals(Db4oPreferences.getFilterRegExp(project.getProject).toString, "foo")
  }

  @Test
  def testFilterCombinator {
    assertTrue(findWidget(page.getControl, Db4oInstrumentationPropertyPage.COMBINATOR_AND_BUTTON_ID).isDefined)
    assertTrue(findWidget(page.getControl, Db4oInstrumentationPropertyPage.COMBINATOR_OR_BUTTON_ID).isDefined)
    val andButton = findWidget(page.getControl, Db4oInstrumentationPropertyPage.COMBINATOR_AND_BUTTON_ID).get.asInstanceOf[Button]
    val orButton = findWidget(page.getControl, Db4oInstrumentationPropertyPage.COMBINATOR_OR_BUTTON_ID).get.asInstanceOf[Button]
    val defaultIsAnd = Db4oPreferences.DEFAULT_COMBINATOR == AndOrEnum.And
    val buttons = 
      if(defaultIsAnd) {
        (andButton, orButton)
      }
      else {
        (orButton, andButton)
      }
    // TODO setSelection() doesn't trigger any events :/
    assertTrue(buttons._1.getSelection)
    assertFalse(buttons._2.getSelection)
    buttons._2.setSelection(true)
    buttons._1.setSelection(false)
    assertTrue(page.performOk)
    assertEquals(Db4oPreferences.getFilterCombinator(project.getProject), if(defaultIsAnd) AndOrEnum.And else AndOrEnum.Or)
  }

  private def findWidget(root: Widget, id: String): Option[Widget] = {
    if(root == null) {
      return None
    }
    if(root.isInstanceOf[Composite]) {
      root.asInstanceOf[Composite].getChildren.foreach((child) => {
        val curResult = findWidget(child, id)
        if(curResult.isDefined) {
          return curResult
        }
      })
      return None
    }
    if(id.equals(root.getData)) Some(root) else None
  }
  
}
