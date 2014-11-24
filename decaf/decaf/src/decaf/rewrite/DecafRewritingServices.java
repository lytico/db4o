package decaf.rewrite;

import java.util.*;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.rewrite.*;
import org.eclipse.text.edits.*;

import sharpen.core.framework.*;

public class DecafRewritingServices {
	private final ASTRewrite _rewrite;
	private final DecafASTNodeBuilder _builder;
	private boolean _erasingParameters = true;
	private final DynamicVariable<Boolean> _forceCastOnErasure = new DynamicVariable<Boolean>(false);
	
	public DecafRewritingServices(ASTRewrite rewrite, DecafASTNodeBuilder builder) {
		_rewrite = rewrite;
		_builder = builder;
	}
	
	@SuppressWarnings("unchecked")
	public <T extends ASTNode> T move(T node) {
		return (T)rewrite().createMoveTarget(node);
	}
	
	public <T extends ASTNode> T safeMove(final T arg) {
		return builder().isExistingNode(arg) ? move(arg) : arg;
	}
	
	public void replace(ASTNode node, ASTNode replacement) {
		rewrite().replace(node, replacement, null);
	}

	public void remove(ASTNode node) {
		rewrite().remove(node, null);
	}
	
	public void replaceWithCast(Expression node, final ITypeBinding type) {
		replace(node, createCastForErasure(node, type));
	}
	
	public Expression erasureFor(final Expression expression) {
		if (expression instanceof Name) {
			return erasureForName((Name)expression);
		}
		if (expression instanceof FieldAccess) {
			return erasureForField(expression, ((FieldAccess)expression).resolveFieldBinding());
		}
		if(expression instanceof MethodInvocation) {
			return erasureForMethodInvocation(((MethodInvocation)expression));
		}
		return null;
	}

	private Expression erasureForMethodInvocation(MethodInvocation node) {
		final IMethodBinding method = node.resolveMethodBinding();
		if (builder().hasGenericReturnType(method)) {
			return createCastForErasure(node, method.getReturnType());
		}
		return null;
	}
	
	public Expression erasureForName(final Name name) {
		final IBinding binding = name.resolveBinding();
		if (!(binding instanceof IVariableBinding)) {
			return null;
		}
		final IVariableBinding variable = (IVariableBinding)binding;
		if (variable.isParameter()) {
			return erasureForParameter(name);
		}
		return erasureForField(name, variable);
	}

	private Expression erasureForParameter(Name name) {
		
		if (isErasedParameter(name))
			return createCastForErasure(name, name.resolveTypeBinding());
		
		return null;
    }

	private boolean isErasedParameter(Name name) {
		if (!_erasingParameters)
			return false;
		
		final ITypeBinding originalParameterType = originalParameterTypeFor(name);
		if (!originalParameterType.isTypeVariable())
			return false;
		
		if (hasAssignmentCompatibleErasure(name.resolveTypeBinding(), originalParameterType))
			if (!_forceCastOnErasure.value()
				&& !isExpressionPropertyOfEitherMethodInvocationOrFieldAccess(name))
				return false;
		
		return true;
    }

	private boolean isExpressionPropertyOfEitherMethodInvocationOrFieldAccess(Name name) {
	    return builder().isMethodInvocationExpressionProperty(name)
	    	|| builder().isFieldAccessExpressionProperty(name);
    }

	private ITypeBinding originalParameterTypeFor(Name parameterReference) {
	    final MethodDeclaration method = declaringMethodFor(parameterReference);
		final ITypeBinding originalParameterType = originalParameterType(method, (IVariableBinding) parameterReference.resolveBinding());
	    return originalParameterType;
    }

	private boolean hasAssignmentCompatibleErasure(final ITypeBinding parameterType,
            final ITypeBinding originalParameterType) {
	    return originalParameterType.getErasure().isAssignmentCompatible(parameterType.getErasure());
    }

	private ITypeBinding originalParameterType(final MethodDeclaration method, IVariableBinding parameter) {
	    return originalDeclaringMethod(parameter).getMethodDeclaration().getParameterTypes()[parameterIndexByName(method, parameter.getName())];
    }

	private IMethodBinding originalDeclaringMethod(IVariableBinding parameter) {
	    final IMethodBinding enclosingMethod = declaringMethod(parameter);
		final IMethodBinding original = builder().originalMethodDefinitionFor(enclosingMethod);
		return original == null ? enclosingMethod : original;
    }

	private IMethodBinding declaringMethod(IVariableBinding variable) {
	    return variable.getDeclaringMethod();
    }

	private int parameterIndexByName(MethodDeclaration method, String parameterName) {
		int index = 0;
		for (Object o : method.parameters()) {
			final SingleVariableDeclaration parameter = ((SingleVariableDeclaration) o);
			if (parameter.getName().getIdentifier().equals(parameterName)) {
				return index;
			}
			++index;
		}
		throw new IllegalArgumentException("No parameter named '" + parameterName + "' in '" + method + "'");
	}

	private MethodDeclaration declaringMethodFor(Name parameter) {
		return enclosingMethod(parameter, ((IVariableBinding)parameter.resolveBinding()).getDeclaringMethod());
    }

	private MethodDeclaration enclosingMethod(ASTNode node, IMethodBinding binding) {
		final ASTNode parent = node.getParent();
		if (parent instanceof MethodDeclaration) {
			final MethodDeclaration method = ((MethodDeclaration)parent);
			if (method.resolveBinding() == binding) {
				return method;
			}
		}
		return enclosingMethod(parent, binding);
    }

	public Expression erasureForField(final Expression expression, final IVariableBinding field) {
		
		if (builder().isErasedFieldAccess(field)) {
			return createCastForErasure(expression, field.getType());
		}
		return null;
	}
	
	@SuppressWarnings("unchecked")
    public <T extends ASTNode> T get(ASTNode node, StructuralPropertyDescriptor parentLocation) {
		return (T)rewrite().get(node, parentLocation);
	}
	
	public void set(ASTNode node, SimplePropertyDescriptor property, Object value, TextEditGroup editGroup) {
		rewrite().set(node, property, value, editGroup);		
	}	
	
	public ListRewrite getListRewrite(ASTNode node, ChildListPropertyDescriptor property) {
		return rewrite().getListRewrite(node, property);
	}
	
	public ASTNode createStringPlaceholder(String code, int nodeType) {
		return rewrite().createStringPlaceholder(code, nodeType);
	}

	@SuppressWarnings("unchecked")
	public MethodInvocation unboxedMethodInvocation(final Expression expression, ITypeBinding toType) {
		Expression modified = expression;
		if(expression.getNodeType() == ASTNode.METHOD_INVOCATION) {
			ASTNode parent = expression.getParent();
			StructuralPropertyDescriptor parentLocation = expression.getLocationInParent();
			if(parentLocation.isChildListProperty()) {
				// FIXME This assumes that original and rewritten list are of the same size.
				ListRewrite listRewrite = getListRewrite(parent, (ChildListPropertyDescriptor) parentLocation);
				List originalList = listRewrite.getOriginalList();
				int originalIdx = originalList.indexOf(expression);
				modified = (Expression) listRewrite.getRewrittenList().get(originalIdx);
			}
			else {
				modified = (Expression) rewrite().get(parent, parentLocation);
			}
		}
		return (expression == modified ? unbox(modified, toType) : unboxModified(expression, modified));
	}

	@SuppressWarnings("unchecked")
	public ClassInstanceCreation box(final Expression expression) {
		SimpleType type = builder().newSimpleType(builder().boxedTypeFor(expression.resolveTypeBinding()));
		final ClassInstanceCreation creation = builder().newClassInstanceCreation(type);
		creation.arguments().add(safeMove(expression));
		return creation;
	}

	public MethodInvocation unbox(final Expression expression, ITypeBinding type) {
		return builder().newMethodInvocation(
			parenthesizedMove(expression),
			builder().unboxingMethodFor(type));
	}

	public MethodInvocation unboxModified(final Expression expression, final Expression modified) {
		return unboxModified(modified, expression.resolveTypeBinding());
	}

	public MethodInvocation unboxModified(final Expression modified, final ITypeBinding typeBinding) {
		return unboxModified(modified, typeBinding.getQualifiedName());
	}

	public MethodInvocation unboxModified(final Expression modified,
			String name) {
		return builder().newMethodInvocation(
			builder().parenthesize(modified),
			builder().unboxingMethodFor(name));
	}

	private Expression parenthesizedMove(final Expression expression) {
		Expression moved = safeMove(expression);
		final Expression target = expression instanceof Name
			? moved
			: builder().parenthesize(moved);
		return target;
	}
	
	private Expression createCastForErasure(Expression node, final ITypeBinding type) {
		return builder().createParenthesizedCast(safeMove(node), type);
	}
	
	private ASTRewrite rewrite() {
		return _rewrite;
	}

	private DecafASTNodeBuilder builder() {
		return _builder;
	}
	
	public void erasingParameters(boolean value) {
		_erasingParameters = value;
	}

	public boolean erasingParameters() {
    	return _erasingParameters;
    }

	public DynamicVariable<Boolean> forceCastOnErasure() {
		return _forceCastOnErasure;
    }
}
