using System;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using OManager.BusinessLayer.Login;
using System.IO;
using OManager.BusinessLayer.QueryManager;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;

namespace OManager.DataLayer.Connection
{
	public class Db4oClient
	{
	    private static IObjectContainer objContainer;
		private static IObjectContainer userConfigDatabase;
        public static string OMN_Connection { get; set; }
	    public static bool boolExceptionForRecentConn;
        private static IEmbeddedConfiguration EmbeddedConfig { get; set; }
	    public static string ExceptionConnection { get; set; }
	    public static TypeResolver TypeResolver { get; private set; }
        public static ConnParams CurrentConnParams { get; set; }
	    public static bool CustomConfig { get; set; }
	    public static bool IsConnected { get; set; }

	    public static bool IsClient
        {
            get { return ((IInternalObjectContainer) objContainer).IsClient; }
           
        }

		/// <summary>
		/// Static property which either returns a new object container for a specific logon identity or returns the object container already 
		/// allocated to the logon identity.
		/// </summary>
		public static IObjectContainer Client
		{
			get
			{
                ExceptionConnection = "";
				try
				{
					if (objContainer == null)
					{
                        if (CurrentConnParams != null)
						{
                            if (CurrentConnParams.Host != null)
							{
								objContainer=ConnectClient();
							}
							else
							{
                                if (File.Exists(CurrentConnParams.Connection))
                                {
									objContainer=ConnectEmbedded();
                                }
                                else
                                {
                                    ExceptionConnection = "File does not exist!";
                                }
							}
							if (objContainer != null)
							{
								TypeResolver = new TypeResolver(objContainer.Ext().Reflector());
								IsConnected = true;
							}
						}
					}
				}
				catch (InvalidPasswordException)
				{
                    ExceptionConnection = "Incorrect Credentials. Please enter again.";
                    EmbeddedConfig = null;
				}
				catch (DatabaseFileLockedException)
				{
                    ExceptionConnection = "Database is locked and is used by another application.";
                    EmbeddedConfig = null;
				}
				catch (IncompatibleFileFormatException ex)
				{
                    ExceptionConnection = ex.Message;
                    EmbeddedConfig = null;
				}
				catch (System.Net.Sockets.SocketException)
				{
                    ExceptionConnection = "No connection could be made because the target machine actively refused it.";
                    EmbeddedConfig = null;
				}
				catch (InvalidCastException)
				{
                    ExceptionConnection = "Java Database is not supproted.";
                    EmbeddedConfig = null;
				}
				catch (Exception oEx)
				{

                    ExceptionConnection = oEx.Message;
                    EmbeddedConfig = null;
				}

				return objContainer;
			}
		}

		private static IObjectContainer ConnectEmbedded()
		{
			if(CustomConfig )
                EmbeddedConfig = ManageCustomConfig.ConfigureEmbeddedCustomConfig();
            if (EmbeddedConfig == null)
                EmbeddedConfig = Db4oEmbedded.NewConfiguration();
            ConfigureCommon(EmbeddedConfig.Common);
            EmbeddedConfig.File.ReadOnly = CurrentConnParams.ConnectionReadOnly;
            return Db4oEmbedded.OpenFile(EmbeddedConfig, CurrentConnParams.Connection);
		}

		private static IObjectContainer  ConnectClient()
		{
		    IClientConfiguration config = null;
            if (CustomConfig)
            {
                 config = ManageCustomConfig.ConfigureClientCustomConfig();
            }
		    if ( config == null)
                config = Db4oClientServer.NewClientConfiguration();
			ConfigureCommon(config.Common);
            return Db4oClientServer.OpenClient(config, CurrentConnParams.Host, CurrentConnParams.Port, CurrentConnParams.UserName, CurrentConnParams.PassWord);
			
		}
      
	    protected static void ConfigureCommon(ICommonConfiguration config)
		{
			config.Queries.EvaluationMode(QueryEvaluationMode.Lazy);
			config.ActivationDepth = 1;
			config.AllowVersionUpdates = true ;
		}
		
		public static IObjectContainer OMNConnection
		{
			get
			{
				try
				{
                    OMN_Connection = GetOMNConfigdbPath();
                    if (userConfigDatabase == null && OMN_Connection != null)
					{
						IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
						config.Common.Diagnostic.AddListener(new DiagnosticToTrace());
						config.Common.UpdateDepth = int.MaxValue;
						config.Common.ActivationDepth = int.MaxValue;
					    config.Common.ObjectClass(typeof (OMQuery)).CascadeOnDelete(true);
					    config.Common.ObjectClass(typeof (OMQueryGroup)).CascadeOnDelete(true);
                        config.Common.ObjectClass(typeof(OMQueryClause)).CascadeOnDelete(true);
					    config.Common.ObjectClass(typeof (ConnectionDetails)).CascadeOnDelete(true);
                        userConfigDatabase = Db4oEmbedded.OpenFile(config, OMN_Connection);
					}
				}
				catch (Exception oEx)
				{
					LoggingHelper.HandleException(oEx);
					boolExceptionForRecentConn = true;
				}

				return userConfigDatabase;
			}

		}


		/// <summary>
		/// Static property which closes the corresponding object container for the current logon identity.
		/// </summary>
		public static void CloseConnection()
		{
			try
			{
				if (objContainer != null)
				{
					objContainer.Close();
					objContainer = null;
					IsConnected = false;
                    CurrentConnParams = null;
				}
                EmbeddedConfig = null;
				

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		public static void CloseRecentConnectionFile()
		{
			
			try
			{
                if (userConfigDatabase != null)
				{
                    userConfigDatabase.Close();
					userConfigDatabase = null;
				}

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);

			}
		}

		public static string GetOMNConfigdbPath()
		{
			string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			return Path.Combine(applicationDataPath, Path.Combine("db4objects", Path.Combine("ObjectManagerEnterprise", "ObjectManagerPlus.yap")));
		}
	}
}
