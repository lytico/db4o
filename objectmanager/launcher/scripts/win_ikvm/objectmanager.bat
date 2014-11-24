set VMEXE=ikvm\bin\ikvm.exe
%VMEXE% -Djava.library.path=lib\windows -classpath lib\osgi_core.jar;lib\runtime.jar;lib\objectmanager-@omversion@.jar;lib\db4o-@db4oversion@-java1.2.jar;lib\jakarta-oro-2.0.7.jar;lib\jface.jar;lib\kxml-plugin.jar;lib\org.eclipse.ve.sweet_1.0.0.jar;lib\windows\swt_win32.jar;lib\xmlpull_1_1_3_1.jar;lib\xswt.jar;lib\windows\swt.jar com.db4o.browser.gui.standalone.StandaloneBrowser %1



