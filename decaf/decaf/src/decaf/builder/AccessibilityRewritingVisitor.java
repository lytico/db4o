package decaf.builder;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.rewrite.*;

public class AccessibilityRewritingVisitor extends DecafVisitorBase {

	public AccessibilityRewritingVisitor(DecafRewritingContext context) {
	    super(context);
    }
	
	@Override
	public void endVisit(TypeDeclaration node) {
		handlePublicAnnotationOn(node, TypeDeclaration.MODIFIERS2_PROPERTY, node.resolveBinding());
	}
	
	@Override
	public void endVisit(MethodDeclaration node) {
		handlePublicAnnotationOn(node, MethodDeclaration.MODIFIERS2_PROPERTY, node.resolveBinding());
	}
	
	@Override
	public void endVisit(FieldDeclaration node) {
		handlePublicAnnotationOn(node, FieldDeclaration.MODIFIERS2_PROPERTY, firstVariableDeclarationFragmentOf(node).resolveBinding());
	}

	private VariableDeclarationFragment firstVariableDeclarationFragmentOf(FieldDeclaration fieldDeclaration) {
	    return (VariableDeclarationFragment)fieldDeclaration.fragments().get(0);
    }
	
	private void handlePublicAnnotationOn(ASTNode node,
			final ChildListPropertyDescriptor modifiersProperty,
            final IBinding binding) {
		
	    if (!hasPublicAnnotation(binding))
			return;
		
		final ListRewrite modifiers = getListRewrite(node, modifiersProperty);
		removeAccessibilityModifier(modifiers);
		insertPublicModifier(modifiers);
    }

	private boolean hasPublicAnnotation(final IBinding binding) {
	    return containsAnnotation(binding, decaf.Public.class);
    }
	
	private void insertPublicModifier(final ListRewrite modifiers) {
	    modifiers.insertLast(builder().newPublicModifier(), null);
    }

	private void removeAccessibilityModifier(final ListRewrite modifiers) {
	    for (Object o : modifiers.getOriginalList()) {
			
			if (!(o instanceof Modifier))
				continue;
			
			Modifier m = (Modifier) o;
			if (!isAccessibilityModifier(m))
				continue;
			
			modifiers.remove(m, null);
		}
    }

	private boolean isAccessibilityModifier(Modifier m) {
		return m.isPrivate() || m.isProtected() || m.isPublic();
    }

}
