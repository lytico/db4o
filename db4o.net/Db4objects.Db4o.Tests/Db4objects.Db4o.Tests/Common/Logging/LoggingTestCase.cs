/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Logging;
using Db4objects.Db4o.Tests.Common.Logging;
using Sharpen.IO;

namespace Db4objects.Db4o.Tests.Common.Logging
{
	public class LoggingTestCase : ITestLifeCycle
	{
		public interface ITestLogger
		{
			void Msg();
		}

		public interface IInvalidTestLogger
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			Logger.PurgeCache();
		}

		public virtual void TearDown()
		{
		}

		public virtual void TestInvalidTestLogger()
		{
			try
			{
				Logger.Get(typeof(LoggingTestCase.IInvalidTestLogger));
				Assert.Fail("An exception should have been generated since our " + typeof(LoggingTestCase.IInvalidTestLogger
					).Name + " is not annotated with @LogInterface");
			}
			catch (ArgumentException)
			{
			}
		}

		public virtual void TestWithNoRootInterceptor()
		{
			ILogging logger = Logger.Get(typeof(LoggingTestCase.ITestLogger));
			((LoggingTestCase.ITestLogger)logger.Debug()).Msg();
		}

		public virtual void TestLoggingLevels()
		{
			IList methodsCalled = SetRootInterceptor();
			ILogging logger = Logger.Get(typeof(LoggingTestCase.ITestLogger));
			((LoggingTestCase.ITestLogger)logger.Trace()).Msg();
			((LoggingTestCase.ITestLogger)logger.Debug()).Msg();
			((LoggingTestCase.ITestLogger)logger.Info()).Msg();
			((LoggingTestCase.ITestLogger)logger.Warn()).Msg();
			((LoggingTestCase.ITestLogger)logger.Error()).Msg();
			((LoggingTestCase.ITestLogger)logger.Fatal()).Msg();
			Assert.AreEqual(Pair.Of(Logger.Trace, "msg"), ((Pair)PopFirst(methodsCalled)));
			Assert.AreEqual(Pair.Of(Logger.Debug, "msg"), ((Pair)PopFirst(methodsCalled)));
			Assert.AreEqual(Pair.Of(Logger.Info, "msg"), ((Pair)PopFirst(methodsCalled)));
			Assert.AreEqual(Pair.Of(Logger.Warn, "msg"), ((Pair)PopFirst(methodsCalled)));
			Assert.AreEqual(Pair.Of(Logger.Error, "msg"), ((Pair)PopFirst(methodsCalled)));
			Assert.AreEqual(Pair.Of(Logger.Fatal, "msg"), ((Pair)PopFirst(methodsCalled)));
		}

		private IList SetRootInterceptor()
		{
			IList methodsCalled = new ArrayList();
			Logger.Intercept(new _ILoggingInterceptor_72(methodsCalled));
			return methodsCalled;
		}

		private sealed class _ILoggingInterceptor_72 : ILoggingInterceptor
		{
			public _ILoggingInterceptor_72(IList methodsCalled)
			{
				this.methodsCalled = methodsCalled;
			}

			public void Log(Level loggingLevel, string method, object[] args)
			{
				methodsCalled.Add(Pair.Of(loggingLevel, method));
			}

			private readonly IList methodsCalled;
		}

		public virtual void TestSetLoggingLevel()
		{
			IList methodsCalled = SetRootInterceptor();
			ILogging logger = Logger.Get(typeof(LoggingTestCase.ITestLogger));
			Logger.LoggingLevel(Logger.Debug);
			((LoggingTestCase.ITestLogger)logger.Trace()).Msg();
			((LoggingTestCase.ITestLogger)logger.Debug()).Msg();
			((LoggingTestCase.ITestLogger)logger.Info()).Msg();
			Assert.AreEqual(Pair.Of(Logger.Debug, "msg"), ((Pair)PopFirst(methodsCalled)));
			Assert.AreEqual(Pair.Of(Logger.Info, "msg"), ((Pair)PopFirst(methodsCalled)));
			logger.LoggingLevel(Logger.Info);
			((LoggingTestCase.ITestLogger)logger.Debug()).Msg();
			((LoggingTestCase.ITestLogger)logger.Info()).Msg();
			((LoggingTestCase.ITestLogger)logger.Error()).Msg();
			Assert.AreEqual(Pair.Of(Logger.Info, "msg"), ((Pair)PopFirst(methodsCalled)));
			Assert.AreEqual(Pair.Of(Logger.Error, "msg"), ((Pair)PopFirst(methodsCalled)));
		}

		/// <exception cref="System.Security.SecurityException"></exception>
		/// <exception cref="System.MissingMethodException"></exception>
		public virtual void TestPrintWriterLogger()
		{
			ByteArrayOutputStream bout = new ByteArrayOutputStream();
			PrintWriterLoggerInterceptor interceptor = new PrintWriterLoggerInterceptor(new PrintWriter
				(bout, true));
			Logger.Intercept(interceptor);
			ILogging logger = Logger.Get(typeof(LoggingTestCase.ITestLogger));
			((LoggingTestCase.ITestLogger)logger.Debug()).Msg();
			((LoggingTestCase.ITestLogger)logger.Info()).Msg();
			string actual = Platform4.AsUtf8(bout.ToByteArray());
			string debugMsg = PrintWriterLoggerInterceptor.FormatMessage(Logger.Debug, "msg", 
				null);
			string infoMsg = PrintWriterLoggerInterceptor.FormatMessage(Logger.Info, "msg", null
				);
			Assert.IsTrue((actual.IndexOf(debugMsg) >= 0));
			Assert.IsTrue((actual.IndexOf(infoMsg) >= 0));
		}

		public virtual void TestInterceptor()
		{
			ILogging logger = Logger.Get(typeof(LoggingTestCase.ITestLogger));
			Logger.Intercept(new _ILoggingInterceptor_136());
			ByRef called = new ByRef();
			logger.Forward(new _ITestLogger_145(called));
			((LoggingTestCase.ITestLogger)logger.Debug()).Msg();
			Assert.AreEqual(Logger.Debug, ((Level)called.value));
			called.value = null;
			logger = Logger.Get(typeof(LoggingTestCase.ITestLogger));
			((LoggingTestCase.ITestLogger)logger.Debug()).Msg();
			Assert.AreEqual(Logger.Debug, ((Level)called.value));
		}

		private sealed class _ILoggingInterceptor_136 : ILoggingInterceptor
		{
			public _ILoggingInterceptor_136()
			{
			}

			public void Log(Level loggingLevel, string method, object[] args)
			{
				Assert.Fail("The root interceptor should not be called");
			}
		}

		private sealed class _ITestLogger_145 : LoggingTestCase.ITestLogger
		{
			public _ITestLogger_145(ByRef called)
			{
				this.called = called;
			}

			public void Msg()
			{
				called.value = Logger.ContextLoggingLevel();
			}

			private readonly ByRef called;
		}

		public static object PopFirst(IList list)
		{
			object first = list[0];
			list.Remove(0);
			return first;
		}
	}
}
