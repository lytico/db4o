package decaf.rewrite;

import java.util.*;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.InfixExpression.*;
import org.eclipse.jdt.core.dom.Modifier.*;
import org.eclipse.jdt.core.dom.PrimitiveType.*;

import sharpen.core.framework.*;
import decaf.config.*;

@SuppressWarnings("unchecked")
public class DecafASTNodeBuilder {
	private final CompilationUnit _unit;
	private final AST _ast;
	private final DecafConfiguration _config;
	
	public DecafASTNodeBuilder(CompilationUnit unit, DecafConfiguration config) {
		this._unit = unit;
		this._ast = unit.getAST();
		this._config = config;
	}
	
	public CompilationUnit compilationUnit() {
		return _unit;
	}

	public <T extends ASTNode> T clone(T node) {
		return (T) ASTNode.copySubtree(_ast, node);
	}
	
	public SimpleName newSimpleName(String name) {
		return _ast.newSimpleName(name);
	}

	public Name newQualifiedName(String qualifiedName) {
		String[] components = qualifiedName.split("\\.");
		Name name = newSimpleName(components[0]);
		for(int idx = 1; idx < components.length; idx++) {
			name = _ast.newQualifiedName(name, newSimpleName(components[idx]));
		}
		
		return name;
	}

	public VariableDeclarationExpression newVariableDeclaration(Type variableType, String variableName, Expression initializer) {
		VariableDeclarationFragment indexFragment = newVariableFragment(variableName, initializer);

		VariableDeclarationExpression index = _ast.newVariableDeclarationExpression(indexFragment);
		index.setType(variableType);
		return index;
	}

	private VariableDeclarationFragment newVariableFragment(String variableName, Expression initializer) {
		final SimpleName name = newSimpleName(variableName);
		return newVariableFragment(name, initializer);
	}

	private VariableDeclarationFragment newVariableFragment(
			final SimpleName name, Expression initializer) {
		VariableDeclarationFragment indexFragment = _ast.newVariableDeclarationFragment();
		indexFragment.setName(name);
		indexFragment.setInitializer(initializer);
		return indexFragment;
	}

	public ForStatement newForStatement(
			Expression initializer,
			Expression comparison,
			Expression updater,
			Statement body) {
		ForStatement stmt = _ast.newForStatement();
		stmt.initializers().add(initializer);
		stmt.setExpression(comparison);
		if(updater != null) {
			stmt.updaters().add(updater);
		}
		stmt.setBody(body);
		return stmt;
	}

	public Block newBlock(Statement... stmts) {
		Block block = _ast.newBlock();
		for (Statement stmt : stmts) {
			block.statements().add(stmt);
		}
		return block;
	}

	public TypeLiteral newTypeLiteral(String name) {
		TypeLiteral literal = _ast.newTypeLiteral();
		literal.setType(newSimpleType(name));
		return literal;
	}
	
	public Type newType(ITypeBinding type) {
		if (type.isAnonymous()) {
			return newErasedType(implementedTypeFromAnonymous(type));
		}
		if (type.isArray()) {
			return _ast.newArrayType(newErasedType(type.getComponentType()));
		}
		if (type.isPrimitive()) {
			return newPrimitiveType(type.getName());
		}
		
		String typeName = requiresQualifier(type)
			? qualifiedName(type)
			: simpleTypeName(type);
		return newSimpleType(typeName);
	}

	private String simpleTypeName(ITypeBinding type) {
		if (type.isNested()) {
			return simpleTypeName(type.getDeclaringClass()) + "." + type.getName();
		}
	    return type.getName();
    }

	private ITypeBinding implementedTypeFromAnonymous(ITypeBinding type) {
		final ITypeBinding[] interfaces = type.getInterfaces();
		return (interfaces.length > 0)
			? interfaces[0]
			: type.getSuperclass();
    }

	private boolean requiresQualifier(ITypeBinding type) {
		final String packageName = packageName(type);
		return !importedPackages().contains(packageName);
	}

	private Set<String> importedPackages() {
		if (importedPackages == null) {
			importedPackages = buildImportedPackageSet();
		}
		return importedPackages;
	}

	private Set<String> buildImportedPackageSet() {
		final HashSet<String> imported = new HashSet<String>();
		imported.add("java.lang");
		final PackageDeclaration packageDecl = _unit.getPackage();
		if (null != packageDecl) {
			imported.add(packageDecl.getName().toString());
		}
		for (Object o : _unit.imports()) {
			final ImportDeclaration importDeclaration = ((ImportDeclaration)o);
			if (!importDeclaration.isOnDemand()) {
				continue;
			}
			imported.add(importDeclaration.getName().toString());
		}
		return imported;
	}

	private String packageName(ITypeBinding type) {
		return type.getPackage().getName();
	}

	public TypeDeclaration newTypeDeclaration(final SimpleName typeName) { 
		final TypeDeclaration newType = _ast.newTypeDeclaration();
		newType.setName(newSimpleName(typeName.getIdentifier()));
		
		return newType;
	}
	
	public SimpleType newSimpleType(final String typeName) {
		return _ast.newSimpleType(_ast.newName(typeName));
	}

	public Type newArrayType(Type componentType) {
		return _ast.newArrayType(componentType);
	}
	
	private PrimitiveType newPrimitiveType(final String primitiveTypeName) {
		return _ast.newPrimitiveType(PrimitiveType.toCode(primitiveTypeName));
	}
	
	public Assignment newAssignment(Expression left, Expression right) {
		final Assignment assignment = _ast.newAssignment();
		assignment.setLeftHandSide(left);
		assignment.setRightHandSide(right);
		return assignment;
	}

	public ArrayAccess newArrayAccess(Expression array, Expression index) {
		ArrayAccess access = _ast.newArrayAccess();
		access.setArray(array);
		access.setIndex(index);
		return access;
	}

	public PrefixExpression newPrefixExpression(
			PrefixExpression.Operator operator,
			SimpleName operand) {
		PrefixExpression increment = _ast.newPrefixExpression();
		increment.setOperator(operator);
		increment.setOperand(operand);
		return increment;
	}

	public InfixExpression newInfixExpression(Operator operator,
			Expression left, Expression right) {
		InfixExpression e = _ast.newInfixExpression();
		e.setOperator(operator);
		e.setLeftOperand(left);
		e.setRightOperand(right);
		return e;
	}

	public VariableDeclarationStatement newVariableDeclarationStatement(
			String variableName, Type variableType, Expression initializer) {
		VariableDeclarationStatement variable = _ast.newVariableDeclarationStatement(newVariableFragment(variableName, initializer));
		variable.setType(variableType);
		variable.modifiers().add(newFinalModifier());
		return variable;
	}
	
	public Modifier newStaticModifier() {
		return _ast.newModifier(ModifierKeyword.STATIC_KEYWORD);
	}
	
	public Modifier newFinalModifier() {
		return _ast.newModifier(ModifierKeyword.FINAL_KEYWORD);
	}
	
	public Modifier newPrivateModifier() {
		return _ast.newModifier(ModifierKeyword.PRIVATE_KEYWORD);
	}
	
	public Modifier newPublicModifier() {
		return _ast.newModifier(ModifierKeyword.PUBLIC_KEYWORD);
	}
	
	public CastExpression newCast(final Expression expression, final ITypeBinding type) {
		return newCast(expression, safeMapTypeBinding(type.getErasure()));
	}

	private CastExpression newCast(final Expression expression, Type castType) {
		final CastExpression cast = _ast.newCastExpression();
		cast.setType(castType);
		cast.setExpression(expression);
		return cast;
	}
	
	public MethodDeclaration newMethodDeclaration(String name) {
		final MethodDeclaration method = _ast.newMethodDeclaration();
		method.setName(newSimpleName(name));

		return method;
	}

	public Object newSingleVariableDeclaration(String name, Type type, Expression initializer) {
		final SingleVariableDeclaration singleVarDecl = _ast.newSingleVariableDeclaration();
		
		singleVarDecl.setName(newSimpleName(name));
		singleVarDecl.setType(type);
		
		if (initializer != null) {
			singleVarDecl.setInitializer(initializer);
		}
		
		return singleVarDecl;
	}	
	
	public MethodDeclaration newConstructorDeclaration(SimpleName name) {
		final MethodDeclaration ctor = newMethodDeclaration(name.getIdentifier());
		ctor.setConstructor(true);
		ctor.modifiers().add(newPrivateModifier());
		
		return ctor;
	}
	
	public MethodInvocation newMethodInvocation(final Expression target,
			final String name) {
		final MethodInvocation invocation = _ast.newMethodInvocation();
		invocation.setExpression(target);
		invocation.setName(newSimpleName(name));
		return invocation;
	}

	public ParenthesizedExpression parenthesize(final Expression expression) {
		final ParenthesizedExpression pe = _ast.newParenthesizedExpression();
		pe.setExpression(expression);
		return pe;
	}

	public ArrayCreation newArrayCreation(final ITypeBinding arrayType, ArrayInitializer arrayInitializer) {
		ArrayCreation varArgsArray = _ast.newArrayCreation();
		varArgsArray.setInitializer(arrayInitializer);

		varArgsArray.setType((ArrayType) newErasedType(arrayType));
		return varArgsArray;
	}

	private Type newErasedType(final ITypeBinding arrayType) {
	    return newType(arrayType.getErasure());
    }
	
	public ArrayCreation newArrayCreation(final Type elementType, ArrayInitializer arrayInitializer) {
		ArrayCreation array = _ast.newArrayCreation();
		array.setInitializer(arrayInitializer);

		array.setType(_ast.newArrayType(elementType));
		return array;
	}
	

	public ArrayInitializer newArrayInitializer() {
		return _ast.newArrayInitializer();
	}

	IMethodBinding originalMethodDefinitionFor(final IMethodBinding method) {
		if (method.isConstructor()) {
			return method;
		}
		return BindingUtils.findMethodDefininition(method, _ast);
	}
	
	public ClassInstanceCreation newClassInstanceCreation(Type type) {
		final ClassInstanceCreation creation = _ast.newClassInstanceCreation();

		creation.setType(type);
		return creation;
	}
	
	public PrimitiveType newPrimitiveType(Code typeCode) {
		return _ast.newPrimitiveType(typeCode);
	}
	
	public NumberLiteral newNumberLiteral(String literal) {
		return _ast.newNumberLiteral(literal);
	}
	
	public StringLiteral newStringLiteral(String literal) {
		final StringLiteral stringLiteral = _ast.newStringLiteral();
		stringLiteral.setLiteralValue(literal);
		
		return stringLiteral;
	}

	public IMethodBinding originalMethodDefinitionFor(MethodDeclaration node) {
		return originalMethodDefinitionFor(node.resolveBinding());
	}
	
	public Expression createParenthesizedCast(Expression node, final ITypeBinding type) {
		if(isObjectType(type)) {
			return node;
		}
		return parenthesize(newCast(node, type));
	}

	private boolean isObjectType(final ITypeBinding type) {
		return _ast.resolveWellKnownType(Object.class.getName()) == type;
	}
	
	public ITypeBinding resolveWellKnownType(String fullyQualifiedName) {
		return _ast.resolveWellKnownType(fullyQualifiedName);
	}
	
	public boolean isName(Expression array) {
		return array instanceof Name;
	}

	public SingleVariableDeclaration lastParameter(List parameters) {
		return (SingleVariableDeclaration) parameters.get(parameters.size()-1);
	}

	public boolean isErasedFieldAccess(IVariableBinding binding) {
		final ITypeBinding originalType = binding.getVariableDeclaration().getType();
		return originalType != binding.getType();
	}

	public IVariableBinding fieldBinding(Name node) {
		final IBinding binding = node.resolveBinding();
		if (binding.getKind() != IBinding.VARIABLE) {
			return null;
		}
		IVariableBinding variable = (IVariableBinding)binding;
		if (!variable.isField()) {
			return null;
		}
		return variable;
	}
	
	public String boxedTypeFor(ITypeBinding type) {
		final String typeName = type.getName();
		if ("byte".equals(typeName)) {
			return "Byte";
		}
		if ("boolean".equals(typeName)) {
			return "Boolean";
		}
		if ("short".equals(typeName)) {
			return "Short";
		}
		if ("int".equals(typeName)) {
			return "Integer";
		}
		if ("long".equals(typeName)) {
			return "Long";
		}
		if ("float".equals(typeName)) {
			return "Float";
		}
		if ("double".equals(typeName)) {
			return "Double";
		}
		if ("char".equals(typeName)) {
			return "Character";
		}
		throw new IllegalArgumentException(typeName);
	}
	
	private static final Map<String, String> _unboxing = new HashMap<String, String>();

	private Set<String> importedPackages;
	
	{
		unboxing("java.lang.Byte", "byteValue");
		unboxing("java.lang.Short", "shortValue");
		unboxing("java.lang.Integer", "intValue");
		unboxing("java.lang.Long", "longValue");
		unboxing("java.lang.Float", "floatValue");
		unboxing("java.lang.Double", "doubleValue");
		unboxing("java.lang.Boolean", "booleanValue");
		unboxing("java.lang.Character", "charValue");
	}

	private void unboxing(final String typeName, final String method) {
		_unboxing.put(typeName, method);
	}
	
	public String unboxingMethodFor(ITypeBinding type) {
		return unboxingMethodFor(type.getQualifiedName());
	}

	public String unboxingMethodFor(String typeName) {
		return _unboxing.get(typeName);
	}		

	public boolean isExpressionStatement(final ASTNode parent) {
		return parent.getNodeType() == ASTNode.EXPRESSION_STATEMENT;
	}

	public boolean hasGenericReturnType(final IMethodBinding method) {
		return method.getMethodDeclaration().getReturnType().getErasure() != method.getReturnType().getErasure();
	}

	private boolean lastArgumentIsAssignmentCompatibleWithLastParameter(
			final IMethodBinding binding, final List arguments) {
		final ITypeBinding[] parameters = binding.getParameterTypes();
		final int lastIndex = parameters.length - 1;
		final ITypeBinding lastArgumentType = expressionType(arguments.get(lastIndex));
		final ITypeBinding lastParameterType = parameters[lastIndex];
		return lastArgumentType.isAssignmentCompatible(lastParameterType);
	}

	private ITypeBinding expressionType(final Object expression) {
		return ((Expression)expression).resolveTypeBinding();
	}
	
	private boolean argumentCountDoesNotMatchParameterCount(final IMethodBinding method, final List arguments) {
		return method.getParameterTypes().length != arguments.size();
	}

	public boolean requiresVarArgsTranslation(final IMethodBinding method,
			final List arguments) {
		if (!method.isVarargs()) {
			return false;
		}
		if (argumentCountDoesNotMatchParameterCount(method, arguments)) {
			return true;
		}
		if (lastArgumentIsAssignmentCompatibleWithLastParameter(method, arguments)) {
			return false;
		}
		return true;
	}

	public boolean isExistingNode(ASTNode arg) {
		return arg.getStartPosition() != -1;
	}	

	public String qualifiedName(ITypeBinding type) {
		return type.getTypeDeclaration().getQualifiedName();
	}
	
	public Type mappedType(ITypeBinding origBinding) {
		String name = qualifiedName(origBinding);
		String mappedName = _config.typeNameMapping(name);
		return mappedName == null ? null : newSimpleType(mappedName);
	}

	public FieldDeclaration newField(Type fieldType, String fieldName, Expression initializer) {
		final FieldDeclaration field = _ast.newFieldDeclaration(newVariableFragment(fieldName, initializer));
		field.setType(fieldType);
		return field;
	}

	public ThisExpression newThisExpression() {
		return _ast.newThisExpression();
	}

	public Expression newFieldAccess(Expression e, SimpleName fieldName) {
		FieldAccess field = _ast.newFieldAccess();
		field.setExpression(e);
		field.setName(fieldName);
		return field;
	}
	
	public Expression newFieldAccess(Expression expression, String fieldName) {
		return newFieldAccess(expression, newSimpleName(fieldName));
	}

	public ReturnStatement newReturnStatement(Expression e) {
		final ReturnStatement stmt = _ast.newReturnStatement();
		stmt.setExpression(e);
		return stmt;
	}

	public ExpressionStatement newExpressionStatement(Expression e) {
		return _ast.newExpressionStatement(e);
	}
	
	public Type mapType(Type type) {
		final ITypeBinding typeBinding = type.resolveBinding();
		final ITypeBinding erasure = typeBinding.getErasure();		
		return (erasure != null) ? safeMapTypeBinding(erasure) : safeMapTypeBinding(typeBinding);
	}

	public Type safeMapTypeBinding(ITypeBinding binding) {
		final Type mappedType = mappedType(binding);
		return mappedType == null ? newType(binding) : mappedType;
	}
	
	public MethodDeclaration findMethodDeclarationParent(ASTNode node) {
		ASTNode curNode = node;
		while(curNode != null && curNode.getNodeType() != ASTNode.METHOD_DECLARATION) {
			curNode = curNode.getParent();
		}
		return (MethodDeclaration) curNode;
	}

	public boolean isMethodInvocationExpressionProperty(ASTNode node) {
		return node.getLocationInParent() == MethodInvocation.EXPRESSION_PROPERTY;
    }

	public boolean isEnhancedForStatementExpressionProperty(ASTNode node) {
		return node.getLocationInParent() == EnhancedForStatement.EXPRESSION_PROPERTY;
    }

	public boolean isFieldAccessExpressionProperty(ASTNode node) {
	   return node.getLocationInParent() == FieldAccess.EXPRESSION_PROPERTY
	   	|| node.getLocationInParent() == QualifiedName.QUALIFIER_PROPERTY;
    }

	public boolean isPredicateMatchOverride(MethodDeclaration node) {
		final IMethodBinding definition = originalMethodDefinitionFor(node);
		if (definition == null) {
			return false;
		}
		return isPredicateMatchMethod(definition.getMethodDeclaration());
    }
	
	private boolean isPredicateMatchMethod(final IMethodBinding method) {
		return "com.db4o.query.Predicate".equals(method.getDeclaringClass().getQualifiedName()) &&
			"match".equals(method.getName());
	}

	public SuperConstructorInvocation newSuperConstructorInvocation() {
		return _ast.newSuperConstructorInvocation();
	}
	
	public FieldDeclaration newConstant(final Type fieldType, final String name, final Expression initializer) {
		final FieldDeclaration constantDecl = newField(fieldType, name, initializer);
		
		constantDecl.modifiers().add(newPublicModifier());
		constantDecl.modifiers().add(newStaticModifier());
		constantDecl.modifiers().add(newFinalModifier());
		
		return constantDecl;
	}

	public String sourceInformationFor(ASTNode node) {
		return ASTUtility.sourceInformation(this._unit, node);
    }
}