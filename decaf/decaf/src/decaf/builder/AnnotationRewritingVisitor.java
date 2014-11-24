/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package decaf.builder;

import java.util.*;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.rewrite.*;

import decaf.*;

import sharpen.core.framework.*;


public class AnnotationRewritingVisitor extends DecafVisitorBase {

	public AnnotationRewritingVisitor(DecafRewritingContext context) {
		super(context);
	}
	
	private void processAnnotation(Annotation node) {
		if (isDecafAnnotation(node)) {
			rewrite().remove(node);
			
		} else if (!targetPlatform().platform().compatibleWith(Platform.ANNOTATION)) {
			rewrite().remove(node);
			
		} else if (annotationTarget(node).getNodeType() == ASTNode.METHOD_DECLARATION && node.resolveTypeBinding().getQualifiedName().equals(Override.class.getName())) {
			IMethodBinding mb = ((MethodDeclaration)annotationTarget(node)).resolveBinding();
			IMethodBinding s = findOverriddenMethodInParents(mb);
			if (s.getDeclaringClass().isInterface() && !targetPlatform().platform().compatibleWith(Platform.OVERRIDE_FOR_INTERFACE)) {
				rewrite().remove(node);
			}
		}
	}

	private ASTNode annotationTarget(Annotation node) {
		return node.getParent();
	}
	
	public static IMethodBinding findOverriddenMethodInParents(IMethodBinding binding) {
		return BindingUtils.findOverriddenMethodInHierarchy(binding.getDeclaringClass(), binding, true, false);
	}
	
	@Override
	public boolean visit(MarkerAnnotation node) {
		processAnnotation(node);
		return false;	
	}


	@Override
	public boolean visit(SingleMemberAnnotation node) {
		processAnnotation(node);
		return false;	
	}
	
	@Override
	public boolean visit(NormalAnnotation node) {
		processAnnotation(node);
	    return false;
	}
	
	@Override
	public void endVisit(ArrayInitializer node) {
		ListRewrite listRewrite = rewrite().getListRewrite(node, ArrayInitializer.EXPRESSIONS_PROPERTY);
		List rewrittenList = listRewrite.getRewrittenList();
		if(!rewrittenList.isEmpty()) {
			return;
		}
		ArrayInitializer emptyInitializer = builder().newArrayInitializer();
		rewrite().replace(node, emptyInitializer);
	}
	
	@Override
	public void endVisit(MethodDeclaration node) {
		processRewritingAnnotations(node);
	}
	
	public boolean visit(MethodDeclaration node) {
		if (handledAsIgnored(node, node.resolveBinding())) {
			return false;
		}

		return true;
	}
	
	@Override
	public boolean visit(TypeLiteral node) {
	    if (isMarkedForRemoval(node.getType().resolveBinding())) {
			rewrite().remove(node);
			return false;
		}
	    return super.visit(node);
	}

	@Override
	public boolean visit(TypeDeclaration node) {
		if (handledAsIgnored(node, node.resolveBinding()) || handledAsRemoved(node)) {
			return false;
		}
		
		return true;
	}
	
	@Override
	public void endVisit(TypeDeclaration node) {
		processIgnoreExtends(node);
		processIgnoreImplements(node);
	}
	
	@Override
	public void endVisit(CompilationUnit node) {
		if (allTopLevelTypesHaveBeenRemoved(node)) {
			// imports are removed to avoid possible compilation errors
			removeImports(node);
		}
		super.endVisit(node);
	}

	private void removeImports(CompilationUnit node) {
		removeAll(node.imports());
	}

	private boolean allTopLevelTypesHaveBeenRemoved(CompilationUnit node) {
		return rewrittenTypeListFor(node).isEmpty();
	}	
	
	private List rewrittenTypeListFor(CompilationUnit node) {
		return getListRewrite(node, CompilationUnit.TYPES_PROPERTY).getRewrittenList();
	}
	
	private boolean handledAsRemoved(TypeDeclaration node) {
	    if (isMarkedForRemoval(node.resolveBinding())) {
	    	rewrite().remove(node);
	    	return true;
	    }
	    return false;
    }
	
	private boolean isDecafAnnotation(Annotation node) {
		final ITypeBinding binding = node.resolveTypeBinding();
		return binding.getPackage().getName().equals("decaf");
	}
	
	private void processRewritingAnnotations(MethodDeclaration node) {
		
		if (node.getBody() == null) {
			return;
		}
		
		final ListRewrite bodyRewrite = bodyListRewriteFor(node);
		for (IAnnotationBinding annotation : node.resolveBinding().getAnnotations()) {
			
			if (!isApplicableToTargetPlatform(annotation))
				continue;
			
			if (isAnnotation(annotation, decaf.InsertFirst.class)) {
				bodyRewrite.insertFirst(statementFrom(annotation), null);
			} else if (isAnnotation(annotation, decaf.ReplaceFirst.class)) {
				bodyRewrite.replace(firstNode(bodyRewrite), statementFrom(annotation), null);
			} else if (isAnnotation(annotation, decaf.RemoveFirst.class)) {
				bodyRewrite.remove(firstNode(bodyRewrite), null);
			}
		}
	}

	private ASTNode statementFrom(IAnnotationBinding annotation) {
	    return statementFrom((String)valueFrom(annotation));
    }	
	
	private ASTNode firstNode(final ListRewrite bodyRewrite) {
		return originalNodeAt(bodyRewrite, 0);
	}

	private ASTNode statementFrom(final String code) {
	    return rewrite().createStringPlaceholder(code, ASTNode.EXPRESSION_STATEMENT);
    }

	private ListRewrite bodyListRewriteFor(MethodDeclaration node) {
		return getListRewrite(node.getBody(), Block.STATEMENTS_PROPERTY);
	}

	private ASTNode originalNodeAt(final ListRewrite bodyRewrite,
			final int index) {
		return (ASTNode) bodyRewrite.getOriginalList().get(index);
	}

	private <T> T valueFrom(IAnnotationBinding annotation) {
	    return (T)memberValueFrom(annotation, "value");
    }	
	
	private boolean handledAsIgnored(BodyDeclaration node, IBinding binding) {
		if (isIgnored(binding)) {
			rewrite().remove(node);
			return true;
		}
		return false;
	}
	
	private void processIgnoreExtends(TypeDeclaration node) {
		if (ignoreExtends(node)) {
			rewrite().remove(node.getSuperclassType());
		}
	}

	public boolean ignoreExtends(TypeDeclaration node) {
		return containsAnnotation(node, decaf.IgnoreExtends.class);
	}
	
	private void processIgnoreImplements(TypeDeclaration node) {
		final Set<ITypeBinding> ignoredImplements = ignoredImplements(node);
		if (ignoredImplements.isEmpty()) {
			return;
		}
		
		for (Object o : node.superInterfaceTypes()) {
			final Type type = (Type)o;
			if (ignoredImplements.contains(type.resolveBinding().getTypeDeclaration())) {
				rewrite().remove(type);
			}
		}
	}
	
	private boolean containsAnnotation(TypeDeclaration node, Class<?> annotationType) {
		return containsAnnotation(node.resolveBinding(), annotationType);
	}
	
	private Set<ITypeBinding> allSuperInterfaceBindings(TypeDeclaration node) {
		final List superInterfaces = node.superInterfaceTypes();
		final HashSet<ITypeBinding> set = new HashSet<ITypeBinding>(superInterfaces.size());
		for (Object o : superInterfaces) {
			set.add(((Type)o).resolveBinding().getTypeDeclaration());
		}
		return set;
	}
	
	private Set<ITypeBinding> ignoredImplements(TypeDeclaration node) {
		final HashSet<ITypeBinding> ignored = new HashSet<ITypeBinding>();
		for (IAnnotationBinding annotation : node.resolveBinding().getAnnotations()) {
			if (!isAnnotation(annotation, decaf.IgnoreImplements.class))
				continue;
			if (!isApplicableToTargetPlatform(annotation))
				continue;
			
			final Object[] interfaces = memberValuesFrom(annotation, "interfaces");
			if (interfaces.length == 0)
				return allSuperInterfaceBindings(node);

			for (Object itf : interfaces)
				ignored.add(((ITypeBinding)itf).getTypeDeclaration());
        }
		return ignored;
	}
}
