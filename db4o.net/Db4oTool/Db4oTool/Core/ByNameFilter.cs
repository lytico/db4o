using System.Text.RegularExpressions;
using Mono.Cecil;

namespace Db4oTool.Core
{
    class ByNameFilter : ITypeFilter
    {
        private Regex _nameRegex;

        public ByNameFilter(string nameRegex)
        {
            _nameRegex = new Regex(nameRegex);
        }

        public bool Accept(TypeDefinition typeDef)
        {
            return _nameRegex.IsMatch(typeDef.FullName);
        }
    }
}
