using Mono.Cecil;

namespace Db4oTool.Core
{
	public class ByAttributeFilter : ITypeFilter
	{
		public static bool ContainsCustomAttribute(TypeDefinition typeDef, string attributeType)
		{
			foreach (CustomAttribute attribute in typeDef.CustomAttributes)
			{
				if (attributeType == attribute.Constructor.DeclaringType.FullName)
				{
					return true;
				}
			}
			return false;
		}

		private string _attribute;

		public ByAttributeFilter(string attribute)
		{
			_attribute = attribute;
		}

		public bool Accept(TypeDefinition typeDef)
		{
			return ContainsCustomAttribute(typeDef, _attribute);
		}
	}
}
