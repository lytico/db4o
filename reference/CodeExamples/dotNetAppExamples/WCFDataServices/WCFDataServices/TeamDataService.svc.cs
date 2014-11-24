using System.Data.Services;

namespace Db4oDoc.WCFDataService
{
    //#example: Build the concrete service
    public class TeamDataService : DataService<TeamDataContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
        }
    }
    //#end example
}
