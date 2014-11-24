/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit
{
	/// <summary>Reflection based db4ounit.Test implementation.</summary>
	/// <remarks>Reflection based db4ounit.Test implementation.</remarks>
	public class TestMethod : ITest
	{
		private readonly object _subject;

		private readonly MethodInfo _method;

		public TestMethod(object instance, MethodInfo method)
		{
			if (null == instance)
			{
				throw new ArgumentException("instance");
			}
			if (null == method)
			{
				throw new ArgumentException("method");
			}
			_subject = instance;
			_method = method;
		}

		public virtual object GetSubject()
		{
			return _subject;
		}

		public virtual MethodInfo GetMethod()
		{
			return _method;
		}

		public virtual string Label()
		{
			return _subject.GetType().FullName + "." + _method.Name;
		}

		public override string ToString()
		{
			return "TestMethod(" + _method + ")";
		}

		public virtual void Run()
		{
			bool exceptionInTest = false;
			try
			{
				try
				{
					SetUp();
					Invoke();
				}
				catch (TargetInvocationException x)
				{
					exceptionInTest = true;
					throw new TestException(x.InnerException);
				}
				catch (Exception x)
				{
					exceptionInTest = true;
					throw new TestException(x);
				}
			}
			finally
			{
				try
				{
					TearDown();
				}
				catch (Exception exc)
				{
					if (!exceptionInTest)
					{
						throw;
					}
					Sharpen.Runtime.PrintStackTrace(exc);
				}
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected virtual void Invoke()
		{
			_method.Invoke(_subject, new object[0]);
		}

		protected virtual void TearDown()
		{
			if (_subject is ITestLifeCycle)
			{
				try
				{
					((ITestLifeCycle)_subject).TearDown();
				}
				catch (Exception e)
				{
					throw new TearDownFailureException(e);
				}
			}
		}

		protected virtual void SetUp()
		{
			if (_subject is ITestLifeCycle)
			{
				try
				{
					((ITestLifeCycle)_subject).SetUp();
				}
				catch (Exception e)
				{
					throw new SetupFailureException(e);
				}
			}
		}

		public virtual bool IsLeafTest()
		{
			return true;
		}

		public virtual ITest Transmogrify(IFunction4 fun)
		{
			return ((ITest)fun.Apply(this));
		}
	}
}
