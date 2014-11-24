package com.db4o.container.internal;

import java.lang.reflect.*;

import org.objectweb.asm.*;
import org.objectweb.asm.Type;

public class BindingEmitter {
	
	private Constructor<?> ctor;

	public BindingEmitter(Constructor<?> ctor) {
		this.ctor = ctor;
    }

	public Class<Binding> emit() throws SecurityException, NoSuchMethodException {
		final String concreteTypeName = internalNameFor(ctor.getDeclaringClass());
		final ClassWriter classWriter = new ClassWriter(ClassWriter.COMPUTE_MAXS | ClassWriter.COMPUTE_FRAMES);
		classWriter.visit(Opcodes.V1_5, Opcodes.ACC_PUBLIC, bindingClassName(), null, internalNameFor(Object.class), new String[] { internalNameFor(Binding.class) });
		
		final MethodVisitor bindingCtor = classWriter.visitMethod(Opcodes.ACC_PUBLIC, "<init>", Type.getConstructorDescriptor(ctor), null, null);
		bindingCtor.visitIntInsn(Opcodes.ALOAD, 0);
		bindingCtor.visitMethodInsn(Opcodes.INVOKESPECIAL, internalNameFor(Object.class), "<init>", Type.getConstructorDescriptor(ctor));
		bindingCtor.visitInsn(Opcodes.RETURN);
		bindingCtor.visitMaxs(0, 0);
		bindingCtor.visitEnd();
		
		final MethodVisitor method = classWriter.visitMethod(Opcodes.ACC_PUBLIC, "get", Type.getMethodDescriptor(Binding.class.getMethod("get")), null, null);
		method.visitTypeInsn(Opcodes.NEW, concreteTypeName);
		method.visitInsn(Opcodes.DUP);
		method.visitMethodInsn(Opcodes.INVOKESPECIAL, concreteTypeName, "<init>", Type.getConstructorDescriptor(ctor));
		method.visitInsn(Opcodes.ARETURN);
		method.visitMaxs(0, 0);
		method.visitEnd();
		classWriter.visitEnd();
		return defineClass(bindingClassName(), classWriter.toByteArray());
	}

	private String bindingClassName() {
	    return bindingClassNameFor(ctor);
    }
	
	private Class<Binding> defineClass(final String className, final byte[] classBytes) {
		return bindingClassLoader().define(className.replace('/', '.'), classBytes);
	}
	
	private String internalNameFor(final Class<?> klass) {
		return typeFor(klass).getInternalName();
	}

	private Type typeFor(final Class<?> klass) {
		return Type.getType(klass);
	}
	
	private String bindingClassNameFor(final Constructor<?> ctor) {
		return ctor.getDeclaringClass().getSimpleName() + "Binding";
	}

	private BindingClassLoader bindingClassLoader() {
		return new BindingClassLoader(getClass().getClassLoader());
	}
}
