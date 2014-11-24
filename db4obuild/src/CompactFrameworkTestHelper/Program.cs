using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.DeviceEmulatorManager.Interop;
using Microsoft.SmartDevice.Connectivity;
using CommandLine.Utility;

namespace CompactFrameworkTestHelper
{
	/**
	 * This program starts the device emulator, deploys Db4objects.Db4o.Tests,
	 * starts the tests and finally waits these tests to complete.
	 */
	class Program
	{
		private struct FrameWorkInfo
		{
			public readonly ObjectId PackageId;
			public readonly string PackageFullPath;

			public FrameWorkInfo(ObjectId id, string packageFullPath)
			{
				PackageId = id;
				PackageFullPath = packageFullPath;
			}
		}

		private static readonly Dictionary<string, FrameWorkInfo> _deployment;

		private const int DoNotSaveState = 0;
        private const int SaveState = 1;

		private static readonly string DeviceTestPath = "/Temp/";

		private static readonly int ERROR_BASE = 0;
		private static readonly int ExceptionRunningTests = ERROR_BASE - 1;
		private static readonly int FailedLaunchingTests = ERROR_BASE - 2;
		private static readonly int InvalidProgramParameters = ERROR_BASE - 3;
        private static readonly int EmulatorProcessKilled =  ERROR_BASE - 4;

	    static Program()
		{
			_deployment = new Dictionary<string, FrameWorkInfo>();
			_deployment.Add("2.0", new FrameWorkInfo(new ObjectId(new Guid("ABD785F0-CDA7-41c5-8375-2451A7CBFF26")), "NETCFv2.ppc.armv4.cab"));
			_deployment.Add("3.5", new FrameWorkInfo(new ObjectId(new Guid("ABD785F0-CDA7-41c5-8375-2451A7CBFF37")), "NETCFv35.ppc.armv4.cab"));
		}

		static int Main(string[] args)
		{
            Arguments arguments = new Arguments(args);

            string targetFrameworkVersion = arguments["version"];
            if (targetFrameworkVersion == null)
            {
                targetFrameworkVersion = "2.0";
            }

            string db4oDistPath = arguments["dir.dll.compact"];
            if (db4oDistPath == null)
            {
                Help();
                return InvalidProgramParameters;
            }

		    string cfAppName = arguments["app.name"];
            if (string.IsNullOrEmpty(cfAppName))
            {
                Help();
                return InvalidProgramParameters;
            }

		    string deployFileFilter = AppendExtraFilterForDeploy(arguments["deploy.extra.files"]);
		    int ret;
			try
			{
			    Console.Error.WriteLine("CompactFrameworkTestHelper v1.2 - Copyright (C) 2009 Versant Inc.\r\n");
                Console.Error.WriteLine("\tCF App: {0}", cfAppName);
                Console.Error.WriteLine("\tCF Target Version: {0}", targetFrameworkVersion);
                Console.Error.WriteLine("\tFolder: {0}", db4oDistPath);
                Console.Error.WriteLine("\tDeployed File Types: {0}", deployFileFilter);
                Console.Error.WriteLine("\tArguments: {0}", arguments["app.args"] ?? "no argument");
                
                ConfigureEmulator(arguments["dir.storagecard"]);

				Device device = EmulatorHelper.GetDevice();
				device.Connect();

			    DeployDotNetFramework(device, targetFrameworkVersion);

				try
				{
                    EmulatorHelper.CopyFiles(device.GetFileDeployer(), Path.Combine(db4oDistPath, "*"), DeviceTestPath, deployFileFilter);

                    RemoteProcess process = device.GetRemoteProcess();
                    if (process.Start(DeviceTestPath + cfAppName, "\"" + arguments["app.args"] ?? String.Empty + "\""))
                    {
                        ret = WaitToFinishOrTimeout(process, new TimeSpan(0, 5, 0, 0));

                        EmulatorHelper.PublishTestLog(device.GetFileDeployer(), db4oDistPath);
                        if (ret != 0)
                        {
                            Console.Error.WriteLine("{0} returned: {1}", cfAppName, ret);
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Failled to start application '{0}' in emulator", cfAppName);
                        ret = FailedLaunchingTests;
                    }
				}
				finally
				{
					device.Disconnect();
                    IDeviceEmulatorManagerVMID virtualDevice = EmulatorHelper.GetVirtualDevice();
					virtualDevice.Shutdown(DoNotSaveState);
				}
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine("Error running {0}\r\n{1}", cfAppName, ex);
				ret = ExceptionRunningTests;
			}

			return ret;
		}

	    private static int WaitToFinishOrTimeout(RemoteProcess process, TimeSpan timeout)
	    {
	        Stopwatch timer = new Stopwatch();
	        timer.Start();

	        while (!process.HasExited() && timer.Elapsed < timeout)
	        {
	            Thread.Sleep(2000);
	        }

            return !process.HasExited() ? ForceProcessKill(process) : process.GetExitCode();
	    }

	    private static int ForceProcessKill(RemoteProcess process)
	    {
            Console.Error.WriteLine("Killing applicationon {0} in the emulator...", process.FileName);
            process.Kill();

	        return EmulatorProcessKilled;
	    }

	    private static string AppendExtraFilterForDeploy(string deployFileFilter)
	    {
	        return "dll,exe," + deployFileFilter ?? "";
	    }

	    private static void ConfigureEmulator(string storageCard)
	    {
            IDeviceEmulatorManagerVMID emulator = EmulatorHelper.GetVirtualDevice();
	        ConfigureStorageCard(emulator, storageCard);
	    }

	    private static void ConfigureStorageCard(IDeviceEmulatorManagerVMID emulator, string storageCardPath)
	    {
            if (!String.IsNullOrEmpty(storageCardPath))
            {
                emulator.Connect();
                EmulatorHelper.SetStorageCardPath(emulator, storageCardPath);
                emulator.Shutdown(SaveState);
            }
	    }

	    private static void Help()
		{
		    Console.Error.WriteLine("Invalid program parameter count.\r\n" +
		                            "Use: {0} [-version]=[2.0 | 3.5] <-app.name>=<executable name> <-dir.dll.compact>=<path to db4o .NET Compact Framework distribution> \r\n\r\n", 
                                    Assembly.GetExecutingAssembly().GetName().Name);
		}

		private static void DeployDotNetFramework(Device device, string version)
		{
			FileDeployer fd = device.GetFileDeployer();

			FrameWorkInfo info = _deployment[version];
			fd.DownloadPackage(info.PackageId);

			RemoteProcess installer = device.GetRemoteProcess();
			installer.Start("wceload.exe", String.Format(@"/noui \windows\{0}", info.PackageFullPath));
		    
            WaitToFinishOrTimeout(installer, new TimeSpan(0, 0, 30, 0));
		}
	}
}