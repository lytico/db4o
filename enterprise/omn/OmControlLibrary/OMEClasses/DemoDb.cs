using System;
using OMAddinDataTransferLayer;
using OManager.BusinessLayer.UIHelper;
using OManager.BusinessLayer.Login;
using OME.Logging.Common;
using System.IO;

namespace OMControlLibrary
{
	public class DemoDb
	{
	    private static string demoFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects"
			+ Path.DirectorySeparatorChar + "ObjectManagerEnterprise" + Path.DirectorySeparatorChar + "DemoDb.yap";

	    public static void Create()
		{
            try
            {
                dbInteraction.CreateDemoDb(demoFilePath);
                ConnParams conparam = new ConnParams(demoFilePath, false);
                ConnectionDetails connectionDetails = new ConnectionDetails(conparam);
                long id = OMEInteraction.ChkIfRecentConnIsInDb(conparam);
                if (id > 0)
                {

                    ConnectionDetails tempConnectionDetail = OMEInteraction.GetConnectionDetailsObject(id);
                    if (tempConnectionDetail != null)
                        connectionDetails = tempConnectionDetail;
                }
                connectionDetails.Timestamp = DateTime.Now;
                connectionDetails.ConnParam = conparam;
                AssemblyInspectorObject.Connection.ConnectToDatabase(connectionDetails, false);
                OMEInteraction.SetCurrentRecentConnection(connectionDetails.ConnParam );
                OMEInteraction.SaveRecentConnection(connectionDetails);
            }
            catch (Exception e)
            {
                LoggingHelper.ShowMessage(e);
            }
		}


	}
}
