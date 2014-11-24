using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Config;
using OMCustomConfigurationforUser;

namespace OManager.DataLayer.Connection
{
    public class ManageCustomConfig
    {
       public static IEmbeddedConfiguration ConfigureEmbeddedCustomConfig()
        {
            var embeddedCustomConfiguration =
                (IOMNEmbeddedCustomConfigurationProvider)
                EvaluateAssembly(typeof (IOMNEmbeddedCustomConfigurationProvider));
            return embeddedCustomConfiguration != null ? embeddedCustomConfiguration.NewEmbeddedCustomConfiguration() : null;
        }

        public static IClientConfiguration ConfigureClientCustomConfig()
        {
            var clientCustomConfig =
                (IOMNClientCustomConfigurationProvider) EvaluateAssembly(typeof (IOMNClientCustomConfigurationProvider));
            return clientCustomConfig != null ? clientCustomConfig.NewClientCustomConfiguration() : null;
        }


        private static object EvaluateAssembly(Type customconfigType)
        {
            try
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Assembly assemblies = asm; 
                    if (!assemblies.GetName().Name.Contains("System") && !assemblies.GetName().Name.Contains("VisualStudio")
                         && !assemblies.GetName().FullName.Contains("mscorlib") && !assemblies.GetName().Name.Contains("Microsoft")) 
                    {
                        Type[] types = assemblies.GetTypes();
                     
                        foreach (var type in types)
                        {
                            Type t = type;
                            if (customconfigType.IsAssignableFrom(t) && t.IsPublic && !t.IsInterface)
                            {
                                var config = assemblies.CreateInstance(t.FullName, false);
                                return config;
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                throw new Exception(sb.ToString());
            }
            catch (FileNotFoundException e)
            {
                if (e.Message.Contains("Could not load file"))
                    throw new Exception(e.Message + "Use 'Add Assemblies' button to load appropriate file.");

            }
            return null;
        }


      
        public static Assembly LoadAssembly(string searchPath)
        {
            byte[] assemblyBuffer = File.ReadAllBytes(searchPath);
           string pdbPath = searchPath.Remove(searchPath.Length - 3); 
            pdbPath = pdbPath  + "pdb";
            Assembly assembly;
            if (File.Exists(pdbPath))
            {
                byte[] symbolAssemblyBuffer = File.ReadAllBytes(pdbPath);
                assembly = AppDomain.CurrentDomain.Load(assemblyBuffer, symbolAssemblyBuffer);
            }
            else
            {
                assembly = AppDomain.CurrentDomain.Load(assemblyBuffer);
            }

            return assembly;
        }

        public static bool CheckConfig(bool local)
        {
            if (local)
            {
                return ConfigureEmbeddedCustomConfig() != null;
            }
            return ConfigureClientCustomConfig() != null;
        }
    }

}
