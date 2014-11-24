using System;
using OManager.DataLayer.Connection;

namespace OMCustomConfigImplementation.UserCustomConfig
{
    class UserConfig : MarshalByRefObject, IUserConfig
    {
        public bool CheckIfCustomConfigImplemented(bool local)
        {
            return ManageCustomConfig.CheckConfig(local); 
        }
    }
}
