/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package decaf.builder;

import java.util.*;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.rewrite.*;

import sharpen.core.framework.*;
import decaf.*;
import decaf.core.*;
import decaf.rewrite.*;
import decaf.util.*;

public abstract class DecafVisitorBase extends ASTVisitor {

	private static final String UNLESS_COMPATIBLE_ATTRIBUTE_NAME = "unlessCompatible";
	public static final String ERROR_MSG_UNLESS_INVALID = "Argument '"+UNLESS_COMPATIBLE_ATTRIBUTE_NAME+"' can only be used alone.";
	private static final String EXCEPT_ATTRIBUTE_NAME = "except";
	protected final DecafRewritingContext _context;

	public DecafVisitorBase(DecafRewritingContext context) {
		super(false);
		_context = context;
	}

	protected boolean isApplicableToTargetPlatform(final IAnnotationBinding annotationBinding) {
		if (targetPlatform().isNone())
			return true; // annotation is considered to be valid for all platforms
		
		final Set<String> platforms = applicablePlatformsFor(annotationBinding);
		
		return platforms != null && platforms.contains(targetPlatform().toString());
	}

	protected boolean typeHasQualifiedName(final ITypeBinding type, String qualifiedName) {
	    return BindingUtils.qualifiedName(type).equals(qualifiedName);
	}

	private Set<String> applicablePlatformsFor(IAnnotationBinding annotationBinding) {
		Set<String> declaredPlatforms = collectApplicablePlatforms(annotationBinding.getDeclaredMemberValuePairs());
		if (declaredPlatforms != null) {
			return declaredPlatforms;
		}
		return collectApplicablePlatforms(annotationBinding.getAllMemberValuePairs());
	}

	private Set<String> collectApplicablePlatforms(IMemberValuePairBinding[] pairs) {
		Set<String> platforms = new HashSet<String>();
		Set<String> except = new HashSet<String>();
		for (IMemberValuePairBinding valuePair : pairs) {
			
			String key = valuePair.getName();
			Object value = valuePair.getValue();

			if (UNLESS_COMPATIBLE_ATTRIBUTE_NAME.equals(key)) {
				
				if (value == null || (value instanceof Object[] && ((Object[])value).length == 0)) {
					continue;
				}
				
				return handleUnlessSupported(platforms, value, pairs.length == 1);
			}
			
			final Set<String> collection = key.equals(EXCEPT_ATTRIBUTE_NAME) ? except : platforms;
			visitVaribleBinding(value, new Visitor<IVariableBinding>() {
				public void visit(IVariableBinding variable) {
					collection.add(variable.getName());
				}
			});
	    }
		if (platforms.isEmpty()) {
			return null;
		}
		expandAll(platforms, except);
		platforms.removeAll(except);
		return platforms;
	}

	private Set<String> handleUnlessSupported(Set<String> platforms, Object value, boolean onlyOneArgument) {

		if (!onlyOneArgument) {
			// so far we couldnt find a situation where it would be
			// really illegal/harmfull to use unlessCompatible with
			// other arguments, but since we dont directly support using
			// it, for sanity reasons, we throw here.
			throw new IllegalArgumentException(ERROR_MSG_UNLESS_INVALID);
		}
		
		final ByRef<Boolean> compatible = new ByRef<Boolean>(true);
		visitVaribleBinding(value, new Visitor<IVariableBinding>() {
			public void visit(IVariableBinding variable) {
				Platform platform = Platform.valueOf(variable.getName());
				compatible.value &= targetPlatform().platform().compatibleWith(platform);
			}
		});
		
		if (!compatible.value) {
			platforms.add(targetPlatform().name());
		}
		return platforms;
	}

	private void visitVaribleBinding(Object value, Visitor<IVariableBinding> visitor) {
		if (value instanceof Object[]) {
			for(Object o : (Object[])value) {
				visitVaribleBinding(o, visitor);
			}
		} else {
			if (!(value instanceof IVariableBinding)) {
				return;
			}
				
			final IVariableBinding variable = (IVariableBinding)value;
			if (isDecafPlatform(variable.getType())) {
				visitor.visit(variable);
			}
		}
	}

	private void expandAll(Set<String>... platformSets) {
		for (Set<String> platforms : platformSets) {
			if(!platforms.remove(Platform.ALL.name())) {
				continue;
			}
			for (Platform platform : Platform.values()) {
				if(platform != Platform.ALL) {
					platforms.add(platform.name());
				}
			}
		}
	}
	
	private boolean isDecafPlatform(ITypeBinding type) {
		return typeHasSameQualifiedNameAs(type, decaf.Platform.class);
	}

	protected DecafASTNodeBuilder builder() {
		return _context.builder();
	}

	protected TargetPlatform targetPlatform() {
	    return _context.targetPlatform();
	}

	protected boolean typeHasSameQualifiedNameAs(final ITypeBinding type, Class<?> classToCompare) {
	    return typeHasQualifiedName(type, classToCompare.getName());
	}

	protected DecafRewritingServices rewrite() {
		return _context.rewrite();
	}

	protected Object[] memberValuesFrom(IAnnotationBinding annotation, String memberName) {
		final Object value = memberValueFrom(annotation, memberName);
		return value instanceof Object[] ? (Object[])value : new Object[] { value };
	}

	protected <T> T memberValueFrom(IAnnotationBinding annotation, String memberName) {
		for (IMemberValuePairBinding valuePair : annotation.getAllMemberValuePairs()) {
	        if (valuePair.getName().equals(memberName))
	        	return (T)valuePair.getValue();
	    }
		throw new IllegalArgumentException("No '" + memberName + "' member in annotation '" + annotation + "'.");
	}

	protected boolean isAnnotation(IAnnotationBinding annotation, Class<?> annotationClass) {
	    return typeHasSameQualifiedNameAs(annotation.getAnnotationType(), annotationClass);
	}

	protected ListRewrite getListRewrite(ASTNode node, ChildListPropertyDescriptor property) {
		return rewrite().getListRewrite(node, property);
	}
	
	protected boolean isIgnored(IBinding binding) {
		return containsAnnotation(binding, decaf.Ignore.class);
	}

	private boolean containsAnnotation(final IBinding binding, String annotation) {
		final IAnnotationBinding annotationBinding = findAnnotation(binding, annotation);
		if (annotationBinding == null)
			return false;
		return isApplicableToTargetPlatform(annotationBinding);
	}

	private IAnnotationBinding findAnnotation(IBinding binding, String annotationQualifiedName) {
	    for (IAnnotationBinding annotationBinding : binding.getAnnotations()) {
			final ITypeBinding annotationtype = annotationBinding.getAnnotationType();
			if (typeHasQualifiedName(annotationtype, annotationQualifiedName))
				return annotationBinding;
		}
		return null;
	}

	protected boolean containsAnnotation(IBinding binding, Class<?> annotationType) {
		return containsAnnotation(binding, annotationType.getName());
	}

	protected boolean isMarkedForRemoval(final IBinding binding) {
	    return containsAnnotation(binding, decaf.Remove.class);
	}

	protected void removeAll(final List nodes) {
	    for (Object importNode : nodes) {
			rewrite().remove((ASTNode) importNode);
		}
	}
}