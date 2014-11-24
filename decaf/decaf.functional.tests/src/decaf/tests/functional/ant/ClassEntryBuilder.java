/**
 * 
 */
package decaf.tests.functional.ant;

import java.util.*;

import org.objectweb.asm.*;

final class ClassEntryBuilder implements ClassVisitor {
	
	private ClassEntry _classEntry;
	private int _classAccess;
	
	public ClassEntry classEntry() {
		return _classEntry;
	}

	public void visit(int version, int access, String name,
			String signature, String superName,
			String[] interfaces) {
		_classAccess = access;
		_classEntry = new ClassEntry(cleanName(name), cleanName(superName), cleanNameSet(interfaces));
	}

	private Set<String> cleanNameSet(String[] interfaces) {
		final HashSet<String> set = new HashSet<String>();
		for (String i : interfaces) {
			set.add(cleanName(i));
		}
		return set;
	}

	private String cleanName(String superName) {
		return superName.replace("/", ".");
	}

	public AnnotationVisitor visitAnnotation(String desc,
			boolean visible) {
		// we don't care about annotations
		return null;
	}

	public void visitAttribute(Attribute attr) {
		// we don't care about attributes
	}

	public void visitEnd() {
	}

	public FieldVisitor visitField(int access, String name,
			String desc, String signature, Object value) {
		// TODO Auto-generated method stub
		return null;
	}

	public void visitInnerClass(String name, String outerName,
			String innerName, int access) {
		// TODO Auto-generated method stub
		
	}

	public MethodVisitor visitMethod(int access, String name,
			String desc, String signature, String[] exceptions) {
		if (isSynthetic(access) || isPrivate(access)) {
			return null;
		}
		
		if (visitingAbstractClass() && isAbstract(access)) {
			// HACK: working around javac compiler idiosyncrasies - some compilers
			// want to add abstract methods when an abstract class implements an
			// interface
			return null;
		}
				
		_classEntry.methods().add(new MethodEntry(name, desc));
		return null;
	}

	private boolean isPrivate(int access) {
		return isBitSet(access, Opcodes.ACC_PRIVATE);
	}

	private boolean visitingAbstractClass() {
		return visitingClass() && isAbstract(_classAccess);
	}

	private boolean visitingClass() {
		return !isInterface(_classAccess);
	}

	private boolean isInterface(int access) {
		return isBitSet(access, Opcodes.ACC_INTERFACE);
	}

	private boolean isAbstract(int access) {
		return isBitSet(access, Opcodes.ACC_ABSTRACT);
	}

	private boolean isSynthetic(int access) {
		return isBitSet(access, Opcodes.ACC_SYNTHETIC);
	}

	private boolean isBitSet(int bits, int bit) {
		return bit == (bits & bit);
	}

	public void visitOuterClass(String owner, String name,
			String desc) {
		// TODO Auto-generated method stub
		
	}

	public void visitSource(String source, String debug) {
		// TODO Auto-generated method stub
		
	}
}