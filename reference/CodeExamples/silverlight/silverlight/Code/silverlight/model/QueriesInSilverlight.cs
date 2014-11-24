using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Silverlight.Model
{
    public class QueriesInSilverlight
    {
        private void QueriesInSivlerlight()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.Storage = new IsolatedStorageStorage();

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            // #example: Queries in Silverlight
            var persons = from Person p in container
                          where p.FirstName.Contains("Roman")
                          select p;

            foreach (Person person in persons)
            {
                // do something with the person
            }
            // #end example

            
        }
    }
}