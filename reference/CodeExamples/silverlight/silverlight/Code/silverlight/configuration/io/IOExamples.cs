using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;

namespace Db4oDoc.Silverlight.Configuration.IO
{
    public class IOExamples
    {
        public void AddSilverlightSupport()
        {
            // #example: Add Silverlight support
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.AddConfigurationItem(new SilverlightSupport());

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            // #end example
        }
        public void UseIsolatedStorage()
        {
            // #example: use the isolated storage on silverlight
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.Storage = new IsolatedStorageStorage();

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            // #end example
        }
    }
}