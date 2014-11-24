using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OMCustomConfigImplementation;
using OMCustomConfigImplementation.CustomConfigAssemblyInfo;
using OMCustomConfigImplementation.UserCustomConfig;
using OME.Logging.ExceptionLogging;
using OME.Logging.Tracing;
using OManager.BusinessLayer.Config;


namespace OMControlLibrary.OMEAppDomain
{
    internal class CustomConfigAppDomain : IAppDomainDetails
    {
        private ICustomConfigAssemblyInspector customConfigAssemblyInspector;
        public AppDomain workerAppDomain;
        private bool checkLocal;
        public CustomConfigAppDomain(bool checkLocal)
        {
            this.checkLocal = checkLocal;
        }
        public bool LoadAppDomain(string searchPath)
        {
            if (customConfigAssemblyInspector == null)
                customConfigAssemblyInspector = CreateAppDomain();

            customConfigAssemblyInspector.LoadAssembly(searchPath);
            bool check = CustomConfigInspectorObject.CustomUserConfig.CheckIfCustomConfigImplemented(checkLocal);
            UnloadCustomConfigAppDomain();
            return check;
        }

        public bool LoadAppDomain(ISearchPath searchPath)
        {
            try
            {
                if (customConfigAssemblyInspector == null)
                    customConfigAssemblyInspector = CreateAppDomain();

                if (searchPath.Paths.Any())
                {

                    if (!customConfigAssemblyInspector.LoadAssembly(searchPath))
                    {
                        AppDomain.Unload(workerAppDomain);
                        workerAppDomain = null;
                        CustomConfigInspectorObject.ClearAll(); 
                        return false;

                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
          
        }

        private ICustomConfigAssemblyInspector CreateAppDomain()
        {
            AppDomainSetup setup = new AppDomainSetup();


#if DEBUG
            setup.ApplicationBase = @"E:\db4object\db4o\Trunk\omn\OMADDIN\bin\";
#else 
            setup.ApplicationBase = CommonForAppDomain.GetPath() + "\\";
#endif


            setup.ShadowCopyDirectories = Path.GetTempPath();
            setup.ShadowCopyFiles = "true";
            workerAppDomain = AppDomain.CreateDomain("CustomConfigWorkerAppDomain", null, setup);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            object anObject = workerAppDomain.CreateInstanceAndUnwrap("OMCustomConfigImplementation",
                                                                      "OMCustomConfigImplementation.CustomConfigAssemblyInfo.CustomConfigAssemblyInspector");
            ICustomConfigAssemblyInspector customConfigAssemblyInspector = anObject as ICustomConfigAssemblyInspector;

            object anObject1 = workerAppDomain.CreateInstanceAndUnwrap("OMCustomConfigImplementation",
                                                                        "OMCustomConfigImplementation.UserCustomConfig.UserConfig");
            IUserConfig conn = anObject1 as IUserConfig;
            CustomConfigInspectorObject.CustomUserConfig = conn;
           
            return customConfigAssemblyInspector;
        }


   
        public void UnloadCustomConfigAppDomain()
        {
            AppDomain.Unload(workerAppDomain);
            workerAppDomain = null;
            CustomConfigInspectorObject.ClearAll();


        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return (typeof(CustomConfigAssemblyInspector).Assembly.FullName == args.Name) ? (typeof(CustomConfigAssemblyInspector).Assembly) : null;
        }


    }
}
