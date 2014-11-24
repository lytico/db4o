package com.db4o.eclipse.test.functional

import com.db4o.eclipse.ui._

import org.junit._
import Assert._

import org.eclipse.jface.viewers._

import scala.collection._

class Db4oInstrumentationPropertyPageModelTestCase extends Db4oPluginTestCaseTrait {
  
  private var model: Db4oInstrumentationPropertyPageModel = null
  
  @Before
  override def setUp {
    super.setUp
    model = new Db4oInstrumentationPropertyPageModel(project.getProject)
  }

  // FIXME: eclipse scalac crashes when moving these classes into test method?!
  abstract case class PackageChange(val packageNames: Set[String])    
  case class PackageAdd(override val packageNames: Set[String]) extends PackageChange(packageNames)
  case class PackageRemove(override val packageNames: Set[String]) extends PackageChange(packageNames)
  case class PackageExpectation(change: PackageChange, result: Set[String])

  @Test
  def testPackageListChanges {

    def add(packageNames: String*) = PackageAdd(immutable.ListSet(packageNames:_*))
    def remove(packageNames: String*) = PackageRemove(immutable.ListSet(packageNames:_*))
    def expect(change: PackageChange, result: String*) = PackageExpectation(change, immutable.ListSet(result:_*))
    
    val expectations = Array(
      expect(add("foo", "bar"), "foo", "bar"),
      expect(remove("foo"), "bar"),
      expect(remove("baz"), "bar"),
      expect(add("foo", "baz"), "foo", "bar", "baz"),
      expect(remove("bar", "baz"), "foo")
    )
    
    object MockChangeListener extends PackageListChangeListener {
      private var expIdx = 0
      
      override def packagesAdded(packageNames: Set[String]) {
        expectations(expIdx).change match {
          case PackageAdd(packageNames) => 
          case _ => fail
        }
        expIdx += 1
      }

      override def packagesRemoved(packageNames: Set[String]) {
        expectations(expIdx).change match {
          case PackageRemove(packageNames) => 
          case _ => fail
        }
        expIdx += 1
      }
      
      def validate {
        assertEquals(expectations.length, expIdx)
      }
    }

    model.addPackageListChangeListener(MockChangeListener)
    
    expectations.foreach((expectation) => {
      expectation.change match {
        case PackageAdd(packageNames) => model.addPackages(packageNames)
        case PackageRemove(packageNames) => model.removePackages(packageNames)
      }
      assertEquals(expectation.result, model.getPackages)
    })
    
    MockChangeListener.validate
  }
  
  @Test
  def testPackageSelectionChanges {
    
    object MockSelectionProvider extends ISelectionProvider {
      var listener: ISelectionChangedListener = null
      
      override def addSelectionChangedListener(listener: ISelectionChangedListener) {
        this.listener = listener
	  }
	
	  override def getSelection = null
	
	  override def removeSelectionChangedListener(listener: ISelectionChangedListener) {
        this.listener = null
	  }
	
	  override def setSelection(selection: ISelection) {}
   
	  def getListener = listener
    }
        
    object MockSelectionListener extends PackageSelectionChangeListener {
      var count = 0
      var selected = false
      
      override def packagesSelected(state: Boolean) {
        count += 1
        selected = state
      }
      
      def validate(count: Int, selected: Boolean) {
        assertEquals(count, this.count)
        assertEquals(selected, this.selected)
      }
    }

    def assertEvent(count: Int, selected: String*) {
      val selection = new StructuredSelection(selected.toArray[Object])
      val event = new SelectionChangedEvent(MockSelectionProvider, selection)
      MockSelectionProvider.getListener.selectionChanged(event)
      MockSelectionListener.validate(count, selected.length > 0)
      assertEquals(immutable.ListSet(selected:_*), model.getSelectedPackages)
    }
    
    model.setSelectionProvider(MockSelectionProvider)
    model.addPackageSelectionChangeListener(MockSelectionListener)
    
    assertEvent(0)
    assertEvent(1, "foo")
    assertEvent(1, "foo", "bar")
    assertEvent(2)
    assertEvent(2)
    assertEvent(3, "foo", "bar")
    assertEvent(3, "foo")
    assertEvent(4)
    assertEvent(5, "foo")
  }
}
