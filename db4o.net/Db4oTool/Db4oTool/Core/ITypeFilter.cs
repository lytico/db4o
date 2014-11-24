using Mono.Cecil;

namespace Db4oTool.Core
{
    public interface ITypeFilter
    {
        bool Accept(TypeDefinition typeDef);
    }
}