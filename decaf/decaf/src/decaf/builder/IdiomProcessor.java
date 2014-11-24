package decaf.builder;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.InfixExpression.*;

/**
 * FIXME: Just a temporary place for idiom replacement patterns. Needs to be integrated into overall design.
 */
public class IdiomProcessor {

	public static void processMethodInvocation(DecafRewritingContext context, MethodInvocation expr) {
		IMethodBinding method = expr.resolveMethodBinding();
		if(!String.class.getName().equals(method.getDeclaringClass().getQualifiedName())) {
			return;
		}
		if(!method.getName().equals("contains")) {
			return;
		}
		MethodInvocation indexOfInv = context.builder().newMethodInvocation(context.rewrite().safeMove(expr.getExpression()), "indexOf");
		indexOfInv.arguments().add(context.rewrite().safeMove((ASTNode)expr.arguments().get(0)));
		InfixExpression cmp = context.builder().newInfixExpression(Operator.GREATER_EQUALS, indexOfInv, context.builder().newNumberLiteral("0"));
		context.rewrite().replace(expr, context.builder().parenthesize(cmp));
	}
	
}
