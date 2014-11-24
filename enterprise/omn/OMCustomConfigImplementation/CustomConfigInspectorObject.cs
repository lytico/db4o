using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMCustomConfigImplementation.CustomConfigAssemblyInfo;
using OMCustomConfigImplementation.UserCustomConfig;

namespace OMCustomConfigImplementation
{
    public class CustomConfigInspectorObject
    {
        public static ICustomConfigAssemblyInspector CustomConfigAssemblyInspector;
        public  static IUserConfig CustomUserConfig;
        public static void ClearAll()
        {
            CustomConfigAssemblyInspector = null;
            CustomUserConfig = null;

        }
    }
}
