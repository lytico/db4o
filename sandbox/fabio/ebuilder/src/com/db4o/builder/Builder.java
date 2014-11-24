package com.db4o.builder;

import java.io.*;
import java.text.*;
import java.util.*;
import java.util.jar.*;

import com.db4o.util.eclipse.parser.*;
import com.db4o.util.eclipse.parser.impl.*;
import com.db4o.util.file.*;

public class Builder {

	public static void main(String[] args) throws IOException {

		final IFile output = new RealFile("output");
		final IFile mainClasses = output.file("classes");
		final IFile testClasses = output.file("test-classes");

		log("resolving workspace");
		Workspace w = resolveWorkspace();

		VersionInfo versionInfo = new VersionInfo();

		log("updating version file");
		updateVersionFile(w, versionInfo);

		List<Project> coreProjects = reolveCoreProjects(w);

		log("building core project's and their dependencies main classes");
		Set<Project> testsBuildList = buildMainClasses(mainClasses, buildListFor(coreProjects));

		log("building core project's test classes");
		buildTestClasses(mainClasses, testClasses, testsBuildList);

		log("creating no-deps jar from core projects");
		createNoDepsJar(output, mainClasses, versionInfo, coreProjects);
		
		log("done");

	}

	private static void log(String string) {
		System.out.println(string);
	}

	private static List<Project> reolveCoreProjects(Workspace w) {
		List<Project> coreProjects = new ArrayList<Project>();

		coreProjects.add(w.project("db4oj"));
		coreProjects.add(w.project("db4o.cs"));
		coreProjects.add(w.project("db4o.cs.optional"));
		coreProjects.add(w.project("db4ounit"));
		coreProjects.add(w.project("db4o.instrumentation"));
		coreProjects.add(w.project("db4onqopt"));
		coreProjects.add(w.project("db4oj.optional"));
		coreProjects.add(w.project("db4otaj"));
		coreProjects.add(w.project("db4otools"));
		return coreProjects;
	}

	private static Workspace resolveWorkspace() {
		IFile root = findWorkspace(".");
		Workspace w = new EclipseWorkspace(root);

		w.addProjectRoot(root.file("../polepos"));
		w.importUserLibrary(new RealFile("versant.userlibraries"));
		return w;
	}

	private static void updateVersionFile(Workspace w, final VersionInfo versionInfo) {
		w.project("db4oj").accept(new ProjectClasspathVisitorAdapter() {
			@Override
			public void visitSourceFolder(IFile dir) {
				IFile versionFile = dir.file("com/db4o/Db4oVersion.java");
				if (versionFile.exists()) {
					PrintWriter out = new PrintWriter(versionFile.openOutputStream(false));

					out.println("/* Copyright (C) " + new SimpleDateFormat("yyyy").format(new Date()) + "   Versant Inc.   http://www.db4o.com */");
					out.println();
					out.println("package com.db4o;");
					out.println();
					out.println("/**");
					out.println("* @exclude");
					out.println("*/");
					out.println("public class Db4oVersion {");
					out.println("    public static final String NAME = \"" + versionInfo + "\";");
					out.println("    public static final int MAJOR = " + versionInfo.major() + ";");
					out.println("    public static final int MINOR = " + versionInfo.minor() + ";");
					out.println("    public static final int ITERATION = " + versionInfo.iteration() + ";");
					out.println("    public static final int REVISION = " + versionInfo.revision() + ";");
					out.println("}");

					out.flush();
					out.close();
				}
			}
		});
	}

	private static void buildTestClasses(final IFile mainClasses, final IFile testClasses, final Set<Project> buildList) {
		for (final Project project : buildList) {

			log("   " + project.name());
			project.accept(new ProjectBuilderVisitor() {

				@Override
				protected IFile resolveExternalProjectOutputFolder(Project p, IFile dir) {
					return mainClasses.file(p.name());
				}

				@Override
				public void visitOutputFolder(IFile dir) {
					super.visitOutputFolder(testClasses.file(project.name()));
				}

				@Override
				public void visitSourceFolder(IFile dir) {
					if (isTestSourceFolder(dir)) {
						super.visitSourceFolder(dir);
						return;
					}
					addClasspathEntry(mainClasses.file(project.name()));
				}
			});
		}
	}

	private static Set<Project> buildMainClasses(final IFile mainClasses, Set<Project> buildList) {
		final Set<Project> testsBuildList = new LinkedHashSet<Project>();

		for (final Project project : buildList) {

			log("   " + project.name());
			project.accept(new ProjectBuilderVisitor() {

				@Override
				protected IFile resolveExternalProjectOutputFolder(Project p, IFile dir) {
					return mainClasses.file(p.name());
				}

				@Override
				public void visitOutputFolder(IFile dir) {
					super.visitOutputFolder(mainClasses.file(project.name()));
				}

				@Override
				public void visitSourceFolder(IFile dir) {
					if (isTestSourceFolder(dir)) {
						testsBuildList.add(project);
						return;
					}
					super.visitSourceFolder(dir);
				}
			});
		}
		return testsBuildList;
	}

	private static void createNoDepsJar(final IFile output, final IFile mainClasses, final VersionInfo versionInfo, List<Project> coreProjects)
			throws IOException {
		IFile all = output.file("db4o-" + versionInfo + "-all-java5.jar");
		OutputStream out = new BufferedOutputStream(all.openOutputStream(false));
		Manifest manifest = new Manifest();
		manifest.getMainAttributes().put(Attributes.Name.MANIFEST_VERSION, "1.0");

		JarOutputStream jout = new JarOutputStream(out, manifest);

		Set<String> knownDirs = new HashSet<String>();

		log("   bloat");
		mainClasses.file("bloat").accept(new JarFileCollector(jout, knownDirs));

		for (Project project : coreProjects) {
			log("   " + project.name());
			mainClasses.file(project.name()).accept(new JarFileCollector(jout, knownDirs));
		}
		try {
			jout.flush();
			jout.close();
		} catch (IOException e) {
			throw new java.lang.RuntimeException(e);
		}
	}

	private static Set<Project> buildListFor(List<Project> coreProjects) {
		Set<Project> buildList = new LinkedHashSet<Project>();

		for (Project p : coreProjects) {
			p.accept(new DependencyCollectorVisitor(buildList));
		}
		return buildList;
	}

	private static IFile findWorkspace(String fileName) {
		IFile file = new RealFile(fileName);
		while (file != null && !file.file(".metadata").exists()) {
			file = file.parent();
		}
		return file;
	}

	private static boolean isTestSourceFolder(IFile dir) {
		return "test".equals(dir.name()) || "test".equals(dir.parent().name()) || "tutorial".equals(dir.name()) || "tutorial".equals(dir.parent().name());
	}

}
