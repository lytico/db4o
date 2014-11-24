package decaf.builder;

import java.util.*;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.rewrite.*;

import decaf.rewrite.*;

public class MixinProcessor {
	
	private final ListRewrite _nodeDeclarationsRewrite;
	private final TypeDeclaration _mixin;
	private final FieldDeclaration _mixinField;
	private final TypeDeclaration _node;
	private final DecafRewritingContext _context;

	public MixinProcessor(DecafRewritingContext context, TypeDeclaration node, TypeDeclaration mixinType) {
		_context = context;
		_node = node;
		_nodeDeclarationsRewrite = getListRewrite(node, TypeDeclaration.BODY_DECLARATIONS_PROPERTY);
		_mixin = mixinType;
		_mixinField = newMixinFieldDeclaration();
		
	}

	public void run() {
		_nodeDeclarationsRewrite.insertFirst(_mixinField, null);
		
		generateMixinDelegators();
		
		introduceMixinInstantiations();
	}
	
	@SuppressWarnings("unchecked")
	private void introduceMixinInstantiations() {
		for (MethodDeclaration node : _node.getMethods()) {
			if (!node.isConstructor()) {
				continue;
			}
			
			final ClassInstanceCreation mixinInstantiation = newClassInstanceCreation(_mixin);
			mixinInstantiation.arguments().add(builder().newThisExpression());
			addParametersToArgumentList(node, mixinInstantiation.arguments());
			bodyListRewriteFor(node).insertLast(
				builder().newExpressionStatement(
					builder().newAssignment(
						newMixinFieldAccess(),
						mixinInstantiation)),
				null);
		}
	}
	
	private void generateMixinDelegators() {
		final MethodDeclaration[] methods = _mixin.getMethods();
		for (MethodDeclaration node : methods) {
			if (node.isConstructor()) {
				continue;
			}
			
			final MethodInvocation delegation = builder().newMethodInvocation(
				newMixinFieldAccess(),
				node.getName().toString());
			addParametersToArgumentList(node, delegation.arguments());
							
			final Statement stmt = returnsValue(node)
				? builder().newReturnStatement(delegation)
				: builder().newExpressionStatement(delegation);
			
			final MethodDeclaration delegator = builder().clone(node);
			delegator.setBody(builder().newBlock(stmt));
			
			_nodeDeclarationsRewrite.insertLast(delegator, null);
		}
	}
	
	private boolean returnsValue(MethodDeclaration node) {
		final Type returnType = node.getReturnType2();		
		if (!returnType.isPrimitiveType()) {
			return true;
		}
		return PrimitiveType.VOID != ((PrimitiveType)returnType).getPrimitiveTypeCode();
	}
	
	private FieldDeclaration newMixinFieldDeclaration() {
		return newPublicField(_mixin.resolveBinding(), "_mixin");
	}

	@SuppressWarnings("unchecked")
	private FieldDeclaration newPublicField(final ITypeBinding fieldType,
			final String fieldName) {
		final FieldDeclaration mixinField = newField(newType(fieldType), fieldName, null);
		mixinField.modifiers().add(builder().newPublicModifier());
		return mixinField;
	}

	private ClassInstanceCreation newClassInstanceCreation(
			final TypeDeclaration type) {
		return builder().newClassInstanceCreation(newType(type.resolveBinding()));
	}

	private FieldDeclaration newField(Type fieldType, String fieldName, Expression initializer) {
		return builder().newField(fieldType, fieldName, initializer);
	}
	
	@SuppressWarnings("unchecked")
	private void addParametersToArgumentList(MethodDeclaration node,
			final List argumentList) {
		for (Object o : node.parameters()) {
			final SingleVariableDeclaration parameter = (SingleVariableDeclaration)o;
			argumentList.add(builder().clone(parameter.getName()));
		}
	}

	private Expression newMixinFieldAccess() {
		return builder().newFieldAccess(
			builder().newThisExpression(),
			"_mixin");
	}
	
	private DecafASTNodeBuilder builder() {
		return _context.builder();
	}
	
	private ListRewrite bodyListRewriteFor(MethodDeclaration node) {
		return getListRewrite(node.getBody(), Block.STATEMENTS_PROPERTY);
	}

	private ListRewrite getListRewrite(final ASTNode node,
			final ChildListPropertyDescriptor property) {
		return rewrite().getListRewrite(node, property);
	}

	private DecafRewritingServices rewrite() {
		return _context.rewrite();
	}
	
	private Type newType(final ITypeBinding typeBinding) {
		return builder().newType(typeBinding);
	}
}
