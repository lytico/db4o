/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using System.Security;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Logging;

namespace Db4objects.Db4o.Internal.Logging
{
	public sealed class LoggingWrapper : ILogging
	{
		private static readonly Type[] loggerConstructorParameterTypes = new Type[] { typeof(
			Db4objects.Db4o.Internal.Logging.LoggingWrapper), typeof(Level) };

		private readonly Type _logInterface;

		internal readonly object trace;

		internal readonly object debug;

		internal readonly object info;

		internal readonly object warn;

		internal readonly object error;

		internal readonly object fatal;

		private object _forward;

		private object nullImpl;

		private Level loggingLevel = null;

		private ConstructorInfo _ctorLoggerClass;

		internal LoggingWrapper(Type clazz)
		{
			_logInterface = clazz;
			try
			{
				string loggingImplBaseName = LoggingSupportBaseName() + "_LoggingSupport" + ReflectPlatform
					.InnerClassSeparator + LoggingQualifiedBaseName();
				string loggerClassName = ReflectPlatform.AdjustClassName(loggingImplBaseName + "Logger"
					, clazz);
				string nullImplClassName = ReflectPlatform.AdjustClassName(loggingImplBaseName + 
					"Adapter", clazz);
				Type logerClass = ReflectPlatform.ForName(loggerClassName);
				if (logerClass == null)
				{
					throw new ArgumentException("Cannot find logging support for " + ReflectPlatform.
						SimpleName(_logInterface));
				}
				_ctorLoggerClass = logerClass.GetConstructor(loggerConstructorParameterTypes);
				nullImpl = (object)ReflectPlatform.CreateInstance(nullImplClassName);
			}
			catch (SecurityException e)
			{
				throw new Exception("Error accessing logging support for class " + clazz.FullName
					, e);
			}
			catch (MissingMethodException e)
			{
				throw new Exception("Error accessing logging support for class " + clazz.FullName
					, e);
			}
			trace = CreateProxy(Logger.Trace);
			debug = CreateProxy(Logger.Debug);
			info = CreateProxy(Logger.Info);
			warn = CreateProxy(Logger.Warn);
			error = CreateProxy(Logger.Error);
			fatal = CreateProxy(Logger.Fatal);
		}

		private string LoggingSupportBaseName()
		{
			return ReflectPlatform.ContainerName(_logInterface) + "." + LoggingQualifiedBaseName
				();
		}

		private string LoggingQualifiedBaseName()
		{
			string simpleName = string.Empty;
			Type parent = _logInterface;
			while (parent != null)
			{
				if (simpleName.Length > 0)
				{
					simpleName = "_" + simpleName;
				}
				simpleName = ReflectPlatform.GetJavaInterfaceSimpleName(parent) + simpleName;
				parent = parent.DeclaringType;
			}
			return simpleName;
		}

		private object CreateProxy(Level loggingLevel)
		{
			try
			{
				return ReflectPlatform.NewInstance(_ctorLoggerClass, new object[] { this, loggingLevel
					 });
			}
			catch (Db4oException e)
			{
				throw new Exception("Error creating proxy", e);
			}
		}

		private object SelectLevel(Level level, object logger)
		{
			if (level.Ordinal() < LoggingLevel().Ordinal())
			{
				return nullImpl;
			}
			return logger;
		}

		public object Trace()
		{
			return SelectLevel(Logger.Trace, trace);
		}

		public object Debug()
		{
			return SelectLevel(Logger.Debug, debug);
		}

		public object Info()
		{
			return SelectLevel(Logger.Info, info);
		}

		public object Warn()
		{
			return SelectLevel(Logger.Warn, warn);
		}

		public object Error()
		{
			return SelectLevel(Logger.Error, error);
		}

		public object Fatal()
		{
			return SelectLevel(Logger.Fatal, fatal);
		}

		public void LoggingLevel(Level loggingLevel)
		{
			this.loggingLevel = loggingLevel;
		}

		public Level LoggingLevel()
		{
			return loggingLevel == null ? Logger.loggingLevel : loggingLevel;
		}

		public void Forward(object forward)
		{
			_forward = forward;
		}

		public object Forward()
		{
			return _forward;
		}

		public void Log(Level loggingLevel, string method, object[] args)
		{
			Logger.rootInterceptor.Log(loggingLevel, method, args);
		}

		public void ExceptionCaughtInForward(string methodName, object[] args, Exception 
			exceptionThrown)
		{
			Logger.rootInterceptor.Log(Logger.Warn, "exceptionCaughtInForward", new object[] 
				{ methodName });
		}

		public void PushCurrentLevel(Level level)
		{
			Logger.currentThreadLoggingLevel.Set(level);
		}

		public void PopCurrentLevel()
		{
			Logger.currentThreadLoggingLevel.Set(null);
		}
	}
}
