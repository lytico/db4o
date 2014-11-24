using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oTutorialCode.Code.Indexes
{
    public class IndexExamples
    {
        private static void ConfigureIndexes()
        {
            // #example: Configure index externally
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Car)).ObjectField("carName").Indexed(true);

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            // #end example
        }
    }
}