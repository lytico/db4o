package com.db4o.eclipse.test.functional

import org.junit.runner.RunWith;
import org.junit.runners.Suite;

@RunWith(classOf[Suite])
@Suite.SuiteClasses(Array(
  classOf[InstrumentSingleClassTestCase],
  classOf[FullBuildTestCase],
  classOf[Db4oPreferencesTestCase],
  classOf[PropertyPageTestCase],
  classOf[Db4oInstrumentationPropertyPageModelTestCase],
  classOf[SuperClassInDifferentProjectTestCase]
//classOf[InstrumentationLogViewTestCase]
//classOf[ClassFileChangeNotificationTestCase]
))
class AllTests {
}
