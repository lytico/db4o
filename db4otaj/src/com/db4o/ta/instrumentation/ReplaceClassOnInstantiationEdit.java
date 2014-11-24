package com.db4o.ta.instrumentation;

import java.util.*;

import EDU.purdue.cs.bloat.editor.*;

import com.db4o.instrumentation.core.*;

public class ReplaceClassOnInstantiationEdit implements BloatClassEdit {

	final Map _replacements;
	
	public ReplaceClassOnInstantiationEdit(Class origClass, Class replacementClass) {
		this(new ClassReplacementSpec[] {
				new ClassReplacementSpec(origClass, replacementClass)
		});
	}
	
	public ReplaceClassOnInstantiationEdit(ClassReplacementSpec[] replacementSpecs) {
		_replacements = new HashMap();
		for (int specIdx = 0; specIdx < replacementSpecs.length; specIdx++) {
			ClassReplacementSpec spec = replacementSpecs[specIdx];
			// TODO get type from runtime bloat environment and pass qualified names instead?
			Type origType = Type.getType(spec._origClass);
			Type replacementType = Type.getType(spec._replacementClass);
			_replacements.put(origType, replacementType);
		}
	}
	
	public InstrumentationStatus enhance(ClassEditor ce, ClassLoader origLoader, BloatLoaderContext loaderContext) {
		ArrayListInstantiationMethodVisitor methodVisitor = new ArrayListInstantiationMethodVisitor();
		try {
			ce.visit(methodVisitor);
		} catch(Exception exc) {
			exc.printStackTrace();
			return InstrumentationStatus.FAILED;
		}
		if (methodVisitor.instrumented()) {
			ce.commit();
			return InstrumentationStatus.INSTRUMENTED;
		}
		return InstrumentationStatus.NOT_INSTRUMENTED;
	}

	private final class ArrayListInstantiationMethodVisitor implements EditorVisitor {
		private boolean _instrumented;

		public void visitClassEditor(ClassEditor editor) {
			Type replacementType = (Type) _replacements.get(editor.superclass());
			if (replacementType == null) {
				return;
			}
			editor.setSuperclass(replacementType);
			_instrumented = true;
		}

		public void visitFieldEditor(FieldEditor editor) {
		}

		public void visitMethodEditor(MethodEditor editor) {
			boolean instrumented = false;
			final Iterator codeIterator = editor.code().iterator();
			while (codeIterator.hasNext()) {
				final Object instructionOrLabel = codeIterator.next();
				if(instructionOrLabel instanceof Label) {
					continue;
				}
				if(!(instructionOrLabel instanceof Instruction)) {
					throw new IllegalStateException();
				}
				final Instruction instruction = (Instruction)instructionOrLabel;
				switch(instruction.origOpcode()) {
					case Instruction.opc_new:
						Type newReplacementType = (Type) _replacements.get(instruction.operand());
						if(newReplacementType == null) {
							break;
						}
						instruction.setOperand(newReplacementType);
						break;
					// invokespecial covers instance initializer, super class method and private method invocations
					case Instruction.opc_invokespecial:
						MemberRef methodRef = (MemberRef) instruction.operand();
						Type invokeReplacementType = (Type) _replacements.get(methodRef.declaringClass());
						if(invokeReplacementType == null) {
							break;
						}
						instruction.setOperand(new MemberRef(invokeReplacementType, methodRef.nameAndType()));
						instrumented = true;
						break;
					default:
						// do nothing
				}
			}
			if(instrumented) {
				_instrumented = true;
				editor.commit();
			}
			
		}
		
		public boolean instrumented() {
			return _instrumented;
		}
	}

}
