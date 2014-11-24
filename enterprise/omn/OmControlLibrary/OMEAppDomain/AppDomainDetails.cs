using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.AssemblyInfo;
using OMAddinDataTransferLayer.Connection;
using OMAddinDataTransferLayer.DataBaseDetails;
using OMAddinDataTransferLayer.DataEditing;

using OMAddinDataTransferLayer.DataPopulation;
using OMAddinDataTransferLayer.TypeMauplation;
using OMControlLibrary.OMEAppDomain;
using OManager.BusinessLayer.Config;


namespace OMControlLibrary
{
    public class AppDomainDetails : IAppDomainDetails
	{
		public AppDomain workerAppDomain;

        public bool LoadAppDomain(ISearchPath searchPath)
        {

            try
            {

                AppDomainSetup setup = new AppDomainSetup();
#if DEBUG
                 setup.ApplicationBase = @"E:\db4object\db4o\Trunk\omn\OMADDIN\bin\";
#else 
                setup.ApplicationBase = CommonForAppDomain.GetPath() + "\\";
#endif             
                setup.ShadowCopyDirectories = Path.GetTempPath();
                setup.ShadowCopyFiles = "true";
                workerAppDomain = AppDomain.CreateDomain("WorkerAppDomain", null, setup);
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                object anObject = CreateGlobalObjects("OMAddinDataTransferLayer",
                                                         "OMAddinDataTransferLayer.AssemblyInfo.AssemblyInspector");

                IAssemblyInspector assemblyInspector = anObject as IAssemblyInspector;
                AssemblyInspectorObject.AssemblyInspector = assemblyInspector;
              
                if (searchPath.Paths.Any())
                {
                   
                    if (!assemblyInspector.LoadAssembly(searchPath))
                    {
                        AppDomain.Unload(workerAppDomain);
                        workerAppDomain = null;
                        AssemblyInspectorObject.ClearAll();
                        return false;

                    }
                }

                object anObject1 = CreateGlobalObjects("OMAddinDataTransferLayer",
                                                       "OMAddinDataTransferLayer.Connection.Connection");
                IConnection conn = anObject1 as IConnection;
                AssemblyInspectorObject.Connection = conn;

                object anObject2 = CreateGlobalObjects("OMAddinDataTransferLayer",
                                                       "OMAddinDataTransferLayer.DataPopulation.PopulateData");
                IPopulateData populate = anObject2 as IPopulateData;
                AssemblyInspectorObject.DataPopulation = populate;

                object anObject3 = CreateGlobalObjects("OMAddinDataTransferLayer",
                                                       "OMAddinDataTransferLayer.TypeMauplation.DataType");
                IDataType dataType = anObject3 as IDataType;
                AssemblyInspectorObject.DataType = dataType;

                object anObject4 = CreateGlobalObjects("OMAddinDataTransferLayer",
                                                       "OMAddinDataTransferLayer.DataEditing.SaveData");
                ISaveData saveData = anObject4 as ISaveData;
                AssemblyInspectorObject.DataSave = saveData;

                object anObject5 = CreateGlobalObjects("OMAddinDataTransferLayer",
                                                       "OMAddinDataTransferLayer.DataBaseDetails.ObjectProperties");
                IObjectProperties objectProp = anObject5 as IObjectProperties;
                AssemblyInspectorObject.ObjectProperties = objectProp;

                object anObject6 = CreateGlobalObjects("OMAddinDataTransferLayer",
                                                       "OMAddinDataTransferLayer.DataBaseDetails.ClassProperties");
                IClassProperties classProp = anObject6 as IClassProperties;
                AssemblyInspectorObject.ClassProperties = classProp;
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool LoadAppDomain(string searchPath)
        {
            return AssemblyInspectorObject.AssemblyInspector.LoadAssembly(searchPath);
        }

	  

		private object  CreateGlobalObjects(string assemblyName, string typeName)
		{
            return workerAppDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
		}

		Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			return (typeof(AssemblyInspector).Assembly.FullName == args.Name) ? (typeof(AssemblyInspector).Assembly) : null;
		}

	   
	}
}
