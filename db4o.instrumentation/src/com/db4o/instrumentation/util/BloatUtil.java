package com.db4o.instrumentation.util;

import java.io.*;

import com.db4o.instrumentation.core.*;

import EDU.purdue.cs.bloat.editor.*;

/**
 * @exclude
 */
public class BloatUtil {

	public static String normalizeClassName(Type type) {
		return normalizeClassName(type.className());
	}

	public static String normalizeClassName(String className) {
		return className.replace('/', '.');
	}

	public static Class classForEditor(ClassEditor ce, ClassLoader loader) throws ClassNotFoundException {
		String clazzName = normalizeClassName(ce.name());
		return loader.loadClass(clazzName);
	}

	public static boolean isPlatformClassName(String name) {
		return name.startsWith("java.") || name.startsWith("javax.")
				|| name.startsWith("sun.");
	}

	public static String classNameForPath(String classPath) {
		String className = classPath.substring(0, classPath.length()-".class".length());
		return className.replace(File.separatorChar,'.');
	}

	public static String classPathForName(String className) {
		String classPath = className.replace('.', '/');
		return classPath + ".class";
	}

	private BloatUtil() {
	}

	public static LoadStoreInstructions loadStoreInstructionsFor(Type type) {
		if (type.isPrimitive()) {
			switch (type.typeCode()) {
			case Type.DOUBLE_CODE:
				return new LoadStoreInstructions(Opcode.opc_dload, Opcode.opc_dstore);
			case Type.FLOAT_CODE:
				return new LoadStoreInstructions(Opcode.opc_fload, Opcode.opc_fstore);
			case Type.LONG_CODE:
				return new LoadStoreInstructions(Opcode.opc_lload, Opcode.opc_lstore);
			default:
				return new LoadStoreInstructions(Opcode.opc_iload, Opcode.opc_istore);
			}
		}
		return new LoadStoreInstructions(Opcode.opc_aload, Opcode.opc_astore);
	}

	public static boolean implementsInHierarchy(ClassEditor ce, Class markerInterface, BloatLoaderContext context) throws ClassNotFoundException {
		while(ce != null) {
			if(implementsDirectly(ce, markerInterface)) {
				return true;
			}
			ce = context.classEditor(ce.superclass());
		}
		return false;
	}

	public static boolean extendsInHierarchy(ClassEditor ce, Class superClazz, BloatLoaderContext context) throws ClassNotFoundException {
		while(ce != null) {
			if(normalizeClassName(ce.name()).equals(superClazz.getName())) {
				return true;
			}
			ce = context.classEditor(ce.superclass());
		}
		return false;
	}

	public static boolean implementsDirectly(ClassEditor ce, Class markerInterface) {
		if(markerInterface.getName().equals(normalizeClassName(ce.type()))) {
			return true;
		}
		Type[] interfaces = ce.interfaces();
		for (int idx = 0; idx < interfaces.length; idx++) {
			Type type = interfaces[idx];
			if(normalizeClassName(type).equals(markerInterface.getName())) {
				return true;
			}
		}
		return false;
	}

}
