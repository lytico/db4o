Manifest-Version: 1.0
Class-Path: 
Bundle-Name: db4o OSGi test
Created-By: 1.5.0-beta2 (Sun Microsystems Inc.)
Bundle-Activator: @bundle.activator@
Import-Package: org.osgi.framework
Bundle-Vendor: db4objects, Inc.
Bundle-Category: db
Bundle-Version: @db4o.version.iteration.revision@
Bundle-SymbolicName: db4o_osgi_test
Require-Bundle: db4o_osgi
Bundle-ClassPath: .,
 lib/db4o-@db4o.version.iteration.revision@-db4ounit-java@db4o.java.version@.jar,
 lib/db4o-tests.jar,
 lib/db4o-tests-nq.jar,
 lib/easymock.jar
Bundle-ActivationPolicy: lazy 
Provide-Package: com.db4o.osgi.test
