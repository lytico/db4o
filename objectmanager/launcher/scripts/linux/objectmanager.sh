#!/bin/bash
export VMEXE=java
$VMEXE -Djava.library.path=lib/linux -classpath lib/db4o-@db4oversion@-java1.2.jar:lib/kxml-plugin.jar:lib/org.eclipse.ve.sweet_1.0.0.jar:lib/osgi_core.jar:lib/xmlpull_1_1_3_1.jar:lib/jakarta-oro-2.0.7.jar:lib/runtime.jar:lib/xswt.jar:lib/jface.jar:lib/objectmanager-@omversion@.jar:lib/linux/swt.jar:lib/linux/swt-gtk.jar com.db4o.browser.gui.standalone.StandaloneBrowser "$1"
