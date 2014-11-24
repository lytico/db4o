
using System.Web;
using Db4objects.Db4o;
namespace Db4oDoc.WCFDataService
{
    public class Db4oEmbedded
    {

        private static IObjectContainer database;
        private static object sync = new object();

        public static IObjectContainer NewSession()
        {
            return RootContainer().Ext().OpenSession();
        }

        private static IObjectContainer RootContainer()
        {
            lock (sync)
            {
                return database ?? (database = CreateRootContainer());
            }
        }

        private static IEmbeddedObjectContainer CreateRootContainer()
        {
            var path = DatabaseFileLocation();
            var cfg = Db4objects.Db4o.Db4oEmbedded.NewConfiguration();
            var container = Db4objects.Db4o.Db4oEmbedded.OpenFile(cfg, path);
            return container;
        }

        private static string DatabaseFileLocation()
        {
            return HttpContext.Current.Server.MapPath("App_Data/database.db4o");
        }
    }
}