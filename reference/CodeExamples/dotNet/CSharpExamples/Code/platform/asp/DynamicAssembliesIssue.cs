using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Platform.Asp
{
    // #example: Replace the dynamic assembly name with a fixed one
    class AspAssemblyNamingFix : IAlias
    {
        private const string FixedName = "AspFixedAssemblyName";
        private readonly string DynamicName 
            = typeof(AspAssemblyNamingFix).Assembly.GetName().Name;

        public string ResolveRuntimeName(string runtimeTypeName)
        {
            return runtimeTypeName.Replace(DynamicName, FixedName);
        }

        public string ResolveStoredName(string storedTypeName)
        {
            return storedTypeName.Replace(FixedName, DynamicName);
        }
    }
    // #end example
    public class DynamicAssembliesIssue
    {
        public static void Main(string[] args)
        {
            // #example: Fix ASP.NET assembly names
            var config = Db4oEmbedded.NewConfiguration();
            config.Common.AddAlias(new AspAssemblyNamingFix());
            // #end example
            
        }     
    }
}