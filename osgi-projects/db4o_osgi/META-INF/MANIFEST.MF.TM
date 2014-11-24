Manifest-Version: 1.0
Bundle-ManifestVersion: 2
Bundle-Name: db4o OSGI
Bundle-SymbolicName: db4o_osgi
Bundle-Version: @db4o.version.iteration.revision@
Bundle-Activator: com.db4o.osgi.Db4oActivator
Bundle-Vendor: Versant, Inc.
Import-Package: org.osgi.framework;version="1.3.0"
Bundle-ClassPath: .,
 lib/db4o-@db4o.version.iteration.revision@-all-java@db4o.java.version@.jar
Export-Package: EDU.purdue.cs.bloat.benchmark,
 EDU.purdue.cs.bloat.cfg,
 EDU.purdue.cs.bloat.codegen,
 EDU.purdue.cs.bloat.context,
 EDU.purdue.cs.bloat.decorate,
 EDU.purdue.cs.bloat.diva,
 EDU.purdue.cs.bloat.dump,
 EDU.purdue.cs.bloat.editor,
 EDU.purdue.cs.bloat.file,
 EDU.purdue.cs.bloat.inline,
 EDU.purdue.cs.bloat.optimize,
 EDU.purdue.cs.bloat.reflect,
 EDU.purdue.cs.bloat.shrink,
 EDU.purdue.cs.bloat.ssa,
 EDU.purdue.cs.bloat.strip,
 EDU.purdue.cs.bloat.tbaa,
 EDU.purdue.cs.bloat.tools,
 EDU.purdue.cs.bloat.trans,
 EDU.purdue.cs.bloat.tree,
 EDU.purdue.cs.bloat.util,
@db4o.osgi.exported.packages@
