using System.Management.Automation;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Typehandlers;

namespace CmdLets.Db4objects
{
    public class Db4oObjectCommandBase : PSCmdlet
    {
        public static PSCmdlet Instance;

        public Db4oObjectCommandBase()
        {
            Instance = this;
        }

        protected static IEmbeddedConfiguration Configure()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
			
			configuration.Common.ObjectClass(typeof(PSObject)).CallConstructor(true);
            configuration.Common.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(PSObject)), new PSObjectTypeHandler());
            return configuration;
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string DatabasePath
        {
            set;
            get;
        }
    }
}
