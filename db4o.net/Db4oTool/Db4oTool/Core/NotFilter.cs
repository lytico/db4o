using Mono.Cecil;

namespace Db4oTool.Core
{
	public class NotFilter : ITypeFilter
	{
		private ITypeFilter _filter;

		public NotFilter(ITypeFilter filter)
		{
			_filter = filter;
		}


		public bool Accept(TypeDefinition typeDef)
		{
			return !_filter.Accept(typeDef);
		}
	}
}
