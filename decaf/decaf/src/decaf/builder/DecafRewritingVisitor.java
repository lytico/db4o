package decaf.builder;

import java.util.*;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.rewrite.*;

import static sharpen.core.framework.BindingUtils.*;

import static sharpen.core.framework.StaticImports.*;

import sharpen.core.framework.*;
import decaf.core.*;

@SuppressWarnings("unchecked")
public final class DecafRewritingVisitor extends DecafVisitorBase {
	
	public DecafRewritingVisitor(DecafRewritingContext context) {
		super(context);
	}
	
	@Override
	public boolean visit(EnumDeclaration node) {
		if (isIgnored(node.resolveBinding())) {
			rewrite().remove(node);
			return false;
		}
		
		final TypeDeclaration enumType = new EnumProcessor(_context).run(node);		
		if (enumType == null)
			return true;
		
		rewrite().replace(node, enumType);
		return false;
	}
	
	@Override
	public boolean visit(SwitchStatement node) {
		final ITypeBinding binding = node.getExpression().resolveTypeBinding();
		if (!binding.isEnum())
			return true;
		
		final SwitchStatement switchStatement = new EnumProcessor(_context).transformEnumSwitchStatement(node);
		if (switchStatement == null) return true;
		
		rewrite().replace(node, switchStatement);
		return false;
	}

	@Override
	public void endVisit(TypeDeclaration node) {
		processMixins(node);
	}
	
	private void processMixins(TypeDeclaration node) {
		for (TypeDeclaration mixin : node.getTypes()) {
			if (!isMixin(mixin))
				continue;
			processMixin(node, mixin);
		}
	}

	private boolean isMixin(TypeDeclaration type) {
		return containsAnnotation(type.resolveBinding(), decaf.Mixin.class);
    }

	private void processMixin(TypeDeclaration node, TypeDeclaration mixinType) {
		
		new MixinProcessor(_context, node, mixinType).run();
	}
		
	@Override
	public boolean visit(AnnotationTypeDeclaration node) {
		rewrite().remove(node);
		return false;	
	}
	
	@Override
	public boolean visit(MarkerAnnotation node) {
		rewrite().remove(node);
		return false;	
	}


	@Override
	public boolean visit(SingleMemberAnnotation node) {
		rewrite().remove(node);
		return false;	
	}
	
	@Override
	public boolean visit(NormalAnnotation node) {
		rewrite().remove(node);
	    return false;
	}
	
	@Override
	public boolean visit(TypeParameter node) {
		rewrite().remove(node);
		return false;
	}
	
	@Override
	public boolean visit(ParameterizedType node) {
		rewrite().replace(node, builder().mapType(node.getType()));
		return false;		
	}
	
	@Override
	public boolean visit(SimpleType node) {
		final ITypeBinding binding = node.resolveBinding();
		if (binding.isTypeVariable()) {
			rewrite().replace(node, builder().safeMapTypeBinding(binding.getErasure()));
			return false;
		}
		
		final Type mappedType = builder().mappedType(binding);
		if (null != mappedType) {
			rewrite().replace(node, mappedType);
			return false;
		}
		
		return true;
	}
	
	@Override
	public void postVisit(ASTNode node) {
		if (!(node instanceof Expression)) {
			return;
		}
		
		try {
			postVisitExpression((Expression)node);
		} catch (RuntimeException e) {
			unsupportedConstruct(node, e);
		}
	}
	
	private void unsupportedConstruct(ASTNode node, Exception cause) {
		unsupportedConstruct(node, "failed to map: '" + node + "'", cause);
	}
	
	protected String sourceInformation(ASTNode node) {
		return builder().sourceInformationFor(node);
	}

	private void unsupportedConstruct(ASTNode node, final String message, Exception cause) {
		throw new IllegalArgumentException(sourceInformation(node) + ": " + message, cause);
	}

	private void postVisitExpression(final Expression expression) {
		
		if (!isAutoBoxingTarget(expression)) {
			return;
			
		}
	    if (expression.resolveUnboxing()) {
			final Expression erasure = rewrite().erasureFor(expression);
			rewrite().replace(expression, rewrite().unboxedMethodInvocation(erasure != null ? erasure : expression, expression.resolveTypeBinding()));
			return;
		}
	    
	    if (expression.resolveBoxing()) {
			rewrite().replace(expression, rewrite().box(expression));
		}
    }

	private boolean isAutoBoxingTarget(Expression exp) {
		if (!(exp instanceof Name)) {
			return true;
		}
		return exp.getLocationInParent() != MethodInvocation.NAME_PROPERTY
			&& !isPartOfFieldAccess(exp)
			&& !isPartOfQualifiedName(exp);
	}

	private boolean isPartOfFieldAccess(Expression exp) {
		if (exp.getLocationInParent() == FieldAccess.NAME_PROPERTY) {
			return true;
		}
	    return exp.getLocationInParent() == FieldAccess.EXPRESSION_PROPERTY;
    }

	private boolean isPartOfQualifiedName(Expression exp) {
		if (exp.getLocationInParent() == QualifiedName.NAME_PROPERTY) {
			return true;
		}
		return exp.getLocationInParent() == QualifiedName.QUALIFIER_PROPERTY;
    }
	
	@Override
	public void endVisit(ClassInstanceCreation node) {
		try {
			final IMethodBinding ctor = node.resolveConstructorBinding();
			final List arguments = node.arguments();
			rewriteVarArgsArguments(ctor, arguments, getListRewrite(node, ClassInstanceCreation.ARGUMENTS_PROPERTY));
		} catch (RuntimeException e) {
			System.err.println("Error processing node '" + node + "': " + e);
			throw e;
		}
	}
	
	@Override
	public void endVisit(ArrayAccess node) {
		final Expression erasure = rewrite().erasureFor(node.getArray());
		if (null != erasure) {
			rewrite().replace(node.getArray(), erasure);
		}
	}
	
	@Override
	public void endVisit(Assignment node) {
		if (isIterableInterface(node.getLeftHandSide().resolveTypeBinding())) {
			replaceCoercedIterable(node.getRightHandSide());
		}
	}
	
	public boolean visit(MethodInvocation node) {
		return true;
	}
	
	@Override
	public void endVisit(MethodInvocation node) {
		
		if (mapClassCastIdiom(node))
			return;
		
		removeAll(node.typeArguments());
		
		final IMethodBinding method = node.resolveMethodBinding();
		final List arguments = node.arguments();
		rewriteVarArgsArguments(method, arguments, getListRewrite(node, MethodInvocation.ARGUMENTS_PROPERTY));

		if (!builder().isExpressionStatement(node.getParent())) {
			if (builder().hasGenericReturnType(method)) {
				rewrite().replaceWithCast(node, method.getReturnType());
			}
		}
		coerceIterableMethodArguments(node, method);
		IdiomProcessor.processMethodInvocation(_context, node);
	}
	
	private boolean mapClassCastIdiom(MethodInvocation node) {
		if (!BindingUtils.qualifiedName(node.resolveMethodBinding()).equals("java.lang.Class.cast"))
			return false;
		
		rewrite().replace(
					node,
					builder().newCast(
							rewrite().move((Expression)node.arguments().get(0)),
							node.resolveMethodBinding().getReturnType()));
		return true;
	}

	@Override
	public void endVisit(FieldDeclaration node) {
		replaceCoercedIterableFieldInitializer(node.fragments(), node.getType());
	}
	
	@Override
	public void endVisit(VariableDeclarationStatement node) {
		replaceCoercedIterableFieldInitializer(node.fragments(), node.getType());
	}

	private void replaceCoercedIterableFieldInitializer(List<VariableDeclarationFragment> fragments, Type type) {
		if(isIterableInterface(type.resolveBinding())) {
			for (VariableDeclarationFragment fragment : fragments) {
				replaceCoercedIterable(fragment.getInitializer());
			}
		}
	}

	@Override
	public void endVisit(ReturnStatement node) {
		if(node.getExpression() == null) {
			return;
		}
		MethodDeclaration methodDeclaration = builder().findMethodDeclarationParent(node);
		if(!isIterableInterface(methodDeclaration.getReturnType2().resolveBinding())) {
			return;
		}
		replaceCoercedIterable(node.getExpression());
	}
	
	@Override
	public void endVisit(CastExpression node) {
		if(!isIterableInterface(node.getExpression().resolveTypeBinding())) {
			return;
		}
		Expression rewrittenExpr = (Expression) rewrite().get(node, CastExpression.EXPRESSION_PROPERTY);
		replaceUnwrappedIterable(rewrittenExpr);
	}
	
	private void coerceIterableMethodArguments(MethodInvocation node,
			final IMethodBinding method) {
		ListRewrite rewrittenArgs = getListRewrite(node, MethodInvocation.ARGUMENTS_PROPERTY);
		List<Expression> rewrittenArgList = rewrittenArgs.getRewrittenList();
		int paramIdx = 0;
		for (ITypeBinding paramType : method.getParameterTypes()) {
			if(isIterableInterface(paramType)) {
				Expression iterableArg = rewrittenArgList.get(paramIdx);
				replaceCoercedIterable(iterableArg);
			}
			paramIdx++;
		}
	}

	private void replaceCoercedIterable(Expression iterableArg) {
		Expression coerced = iterablePlatformMapping().coerceIterableExpression(iterableArg);
		if(coerced != iterableArg) {
			rewrite().replace(iterableArg, coerced);
		}
	}

	private IterablePlatformMapping iterablePlatformMapping() {
	    return targetPlatform().iterablePlatformMapping();
    }

	private void replaceUnwrappedIterable(Expression iterableArg) {
		Expression unwrapped = iterablePlatformMapping().unwrapIterableExpression(iterableArg);
		if(unwrapped != iterableArg) {
			rewrite().replace(iterableArg, unwrapped);
		}
	}

	private boolean isIterableInterface(ITypeBinding paramType) {
		return Iterable.class.getName().equals(paramType.getErasure().getQualifiedName());
	}

	@Override
	public void endVisit(SimpleName node) {
		
		if (node.isDeclaration()) {
			return;
		}
		
		if (mapNameOfStaticMethodInvocation(node)) {
			return;
		}
		
		if (mapStaticInvocationClassName(node)) {
			return;
		}
		
		
		if (node.getLocationInParent() == QualifiedName.NAME_PROPERTY) {
			return;
		}
		
		if (node.getLocationInParent() == FieldAccess.NAME_PROPERTY) {
			return;
		}

		processNameErasure(node);
	}

	private boolean mapNameOfStaticMethodInvocation(SimpleName node) {
		return mapNameOfStaticMethodInvocation(node, builder().compilationUnit().imports());
	}

	private boolean mapNameOfStaticMethodInvocation(SimpleName node, List imports ) {
		
		final IMethodBinding method = staticImportMethodBinding(node, imports);
		if(method == null){
			return false;
		}
		
		rewrite().replace(node, 
				builder().newQualifiedName(BindingUtils.qualifiedName(method)));
			
		return true;
	}



	private boolean mapStaticInvocationClassName(Name node) {
		// FIXME overcomplicated and fragile, too many unjustified assumptions here - find better way to handle static method invocation type mappings
		if(!isExpressionOfMethodInvocation(node)) {
			return false;
		}
		final MethodInvocation invocation = (MethodInvocation) node.getParent();
		final ITypeBinding binding = node.resolveTypeBinding();
		if(!isStatic(invocation) || binding == null) {
			return false;
		}
		SimpleType mapped = (SimpleType)builder().mappedType(binding);
		if(mapped == null) {
			return false;
		}
		rewrite().replace(node, mapped.getName());
		return true;
	}


	private boolean isExpressionOfMethodInvocation(Name node) {
		return node.getLocationInParent() == MethodInvocation.EXPRESSION_PROPERTY;
	}
	
	@Override
	public void endVisit(QualifiedName node) {
		if(mapStaticInvocationClassName(node)) {
			return;
		}
		processNameErasure(node);
	}

	@Override
	public void endVisit(FieldAccess node) {
		if (isLeftHandSideOfAssignment(node)) {
			return;
		}
		final Expression erasure = rewrite().erasureForField(node, node.resolveFieldBinding());
		if (null != erasure) {
			rewrite().replace(node, erasure);
		}
	}

	public boolean visit(MethodDeclaration node) {
		if (isIgnored(node.resolveBinding())) {
			return false;
		}
		
		rewrite().erasingParameters(!isPredicateMatchMethod(node));
		return true;
	}
	
	private boolean isPredicateMatchMethod(MethodDeclaration node) {
		return builder().isPredicateMatchOverride(node);
    }

	@Override
	public void endVisit(MethodDeclaration node) {
		
		if (node.isVarargs()) {
			handleVarArgsMethod(node);
		}
		
		processMethodDeclarationErasure(node);
	}
	
	private void processMethodDeclarationErasure(MethodDeclaration node) {		
		if (!rewrite().erasingParameters())
			return;
		
		if (node.isConstructor())
			return;
		
		final IMethodBinding definition = builder().originalMethodDefinitionFor(node);
		if (definition == null)
			return;
		
		final IMethodBinding declaration = definition.getMethodDeclaration();
		if (declaration == definition)
			return;
		
		eraseMethodDeclaration(node, declaration);
	}

	public void endVisit(final EnhancedForStatement node) {
		new EnhancedForProcessor(_context).run(node);
	}

	@Override
	public boolean visit(PackageDeclaration node) {
		return false;
	}
	
	@Override
	public void endVisit(ImportDeclaration node) {
		if (node.isStatic()) {
			rewrite().remove(node);
		}
	}

	private void eraseMethodDeclaration(MethodDeclaration node, IMethodBinding originalMethodDeclaration) {
		eraseReturnType(node, originalMethodDeclaration);
		eraseParametersWhereNeeded(node, originalMethodDeclaration);
	}

	private void eraseReturnType(MethodDeclaration node, IMethodBinding originalMethodDeclaration) {
		eraseIfNeeded(node.getReturnType2(), originalMethodDeclaration.getReturnType());
	}
	
	private void eraseParametersWhereNeeded(MethodDeclaration node, IMethodBinding originalMethodDeclaration) {
		final ITypeBinding[] parameterTypes = originalMethodDeclaration.getParameterTypes();
		for (int i = 0; i < parameterTypes.length; i++) {
			final SingleVariableDeclaration actualParameter = parameterAt(node, i);
			eraseIfNeeded(actualParameter.getType(), parameterTypes[i]);
		}
	}

	private SingleVariableDeclaration parameterAt(MethodDeclaration node, int i) {
	    return (SingleVariableDeclaration) node.parameters().get(i);
    }

	private boolean eraseIfNeeded(final Type actualType, final ITypeBinding expectedType) {	
		final ITypeBinding expectedErasure = expectedType.getErasure();
		if (actualType.resolveBinding() == expectedErasure) {
			return false;
		}		
		
		Type mappedType = builder().mappedType(expectedErasure);
		rewrite().replace(actualType, mappedType == null ? newType(expectedErasure) : mappedType);
		return true;
	}

	private Type newType(final ITypeBinding typeBinding) {
		return builder().newType(typeBinding);
	}

	private void handleVarArgsMethod(MethodDeclaration method) {
		SingleVariableDeclaration varArgsParameter = builder().lastParameter(method.parameters());

		set(varArgsParameter, SingleVariableDeclaration.VARARGS_PROPERTY, Boolean.FALSE);
		rewrite().replace(varArgsParameter.getType(), builder().newType(varArgsParameter.resolveBinding().getType().getErasure()));
	}

	private void set(final ASTNode node, final SimplePropertyDescriptor property, final Object value) {
		rewrite().set(node, property, value, null);
	}

	private void rewriteVarArgsArguments(final IMethodBinding method,
			final List arguments, final ListRewrite argumentListRewrite) {
		
		if (!builder().requiresVarArgsTranslation(method, arguments))
			return;
		
		final ITypeBinding[] parameters = method.getParameterTypes();
		final List rewrittenArguments = argumentListRewrite.getRewrittenList();
		
		ArrayInitializer arrayInitializer = builder().newArrayInitializer();
		for (int i = parameters.length-1; i < rewrittenArguments.size(); ++i) {
			final Expression arg = (Expression) rewrittenArguments.get(i);
			arrayInitializer.expressions().add(rewrite().safeMove(arg));
		}
		
		final List originalList = argumentListRewrite.getOriginalList();
		for (int i = parameters.length-1; i < originalList.size(); ++i) {
			argumentListRewrite.remove((ASTNode)originalList.get(i), null);
		}
		
		argumentListRewrite.insertLast(
				builder().newArrayCreation(parameters[parameters.length-1], arrayInitializer),
				null);
	}

	private void processNameErasure(Name node) {
		
		if (isLeftHandSideOfAssignment(node)) {
			return;
		}		
		
		final Expression erasure = rewrite().erasureForName(node);
		if (null != erasure) {
			rewrite().replace(node, erasure);
		}
	}	

	private boolean isLeftHandSideOfAssignment(ASTNode node) {
		return node.getLocationInParent() == Assignment.LEFT_HAND_SIDE_PROPERTY;
    }
}