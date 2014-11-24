using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oTutorialCode.Code.Configuration
{
    public class ConfigurationExamples
    {
        public void ConfigureDb4O()
        {
            // #example: Important configuration switches
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #end example


            // #example: A few examples
            // Add index
            configuration.Common.ObjectClass(typeof (Driver)).Indexed(true);
            // #end example

            // #example: Finally pass the configuration container factory
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            // #end example
        }

        private class Driver
        {
        }
    }
}