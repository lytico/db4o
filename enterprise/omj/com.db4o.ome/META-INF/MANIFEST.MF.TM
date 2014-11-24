Manifest-Version: 1.0
Bundle-ManifestVersion: 2
Bundle-Name: OME Plug-in
Bundle-SymbolicName: com.db4o.ome;singleton:=true
Bundle-Version: @OMEJ_VERSION@
Bundle-Activator: com.db4o.omplus.Activator
Require-Bundle: org.eclipse.ui,
 org.eclipse.core.runtime,
 org.eclipse.core.resources,
 org.eclipse.core.databinding,
 org.eclipse.jface.databinding
Eclipse-LazyStart: true
Bundle-RequiredExecutionEnvironment: J2SE-1.5
Bundle-ClassPath: .,
 lib/webservices/activation-1.1.jar,
 lib/webservices/axiom-api-1.2.5.jar,
 lib/webservices/axiom-impl-1.2.5.jar,
 lib/webservices/axis2-adb-1.3.jar,
 lib/webservices/axis2-kernel-1.3.jar,
 lib/webservices/axis2-metadata-1.3.jar,
 lib/webservices/commons-codec-1.3.jar,
 lib/webservices/commons-httpclient-3.0.1.jar,
 lib/webservices/commons-logging-1.1.jar,
 lib/webservices/log4j-1.2.14.jar,
 lib/webservices/stax-api-1.0.1.jar,
 lib/webservices/wsdl4j-1.6.2.jar,
 lib/webservices/wstx-asl-3.2.1.jar,
 lib/webservices/XmlSchema-1.3.2.jar,
 lib/org.eclipse.nebula.widgets.cdatetime.jar,
 @DB4O_JAR_NAME@
Bundle-Vendor: db4objects
