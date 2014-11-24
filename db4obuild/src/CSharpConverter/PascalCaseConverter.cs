using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Parser;
using ICSharpCode.NRefactory.Parser.AST;

namespace CSharpConverter
{	
	class PascalCaseConverter : AbstractAstVisitor
	{
		public override object Visit(MethodDeclaration method, object data)
		{
			method.Name = ToPascalCase(method.Name);
			return base.Visit(method, data);
		}
		
		public override object Visit(InvocationExpression invocation, object data)
		{
			//System.Console.WriteLine(invocation.TargetObject);
			FieldReferenceExpression memberRef = invocation.TargetObject as FieldReferenceExpression;
			if (null != memberRef)
			{
				memberRef.FieldName = ToPascalCase(memberRef.FieldName);
			}
			else
			{
				IdentifierExpression identifier = invocation.TargetObject as IdentifierExpression;
				if (null != identifier)
				{
					identifier.Identifier = ToPascalCase(identifier.Identifier);
				}
			}
			return base.Visit(invocation, data);
		}
		
		private string ToPascalCase(string name)
		{
			return name.Substring(0, 1).ToUpper() + name.Substring(1);
		}
	}	
}
