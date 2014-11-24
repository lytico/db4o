/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Reflection;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit
{
	public class ReflectionTestSuiteBuilder : ITestSuiteBuilder
	{
		private Type[] _classes;

		public ReflectionTestSuiteBuilder(Type clazz) : this(new Type[] { clazz })
		{
		}

		public ReflectionTestSuiteBuilder(Type[] classes)
		{
			if (null == classes)
			{
				throw new ArgumentException("classes");
			}
			_classes = classes;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return Iterators.Flatten(Iterators.Map(_classes, new _IFunction4_26(this)));
		}

		private sealed class _IFunction4_26 : IFunction4
		{
			public _IFunction4_26(ReflectionTestSuiteBuilder _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object arg)
			{
				Type klass = (Type)arg;
				try
				{
					return this._enclosing.FromClass(klass);
				}
				catch (Exception e)
				{
					return new FailingTest(klass.FullName, e);
				}
			}

			private readonly ReflectionTestSuiteBuilder _enclosing;
		}

		/// <summary>
		/// Can be overriden in inherited classes to inject new fixtures into
		/// the context.
		/// </summary>
		/// <remarks>
		/// Can be overriden in inherited classes to inject new fixtures into
		/// the context.
		/// </remarks>
		/// <param name="closure"></param>
		/// <returns></returns>
		protected virtual object WithContext(IClosure4 closure)
		{
			return closure.Run();
		}

		/// <exception cref="System.Exception"></exception>
		protected virtual IEnumerator FromClass(Type clazz)
		{
			if (typeof(IClassLevelFixtureTest).IsAssignableFrom(clazz))
			{
				return Iterators.Iterate(new ClassLevelFixtureTestSuite[] { new ClassLevelFixtureTestSuite
					(clazz, new _IClosure4_52(this, clazz)) });
			}
			return (IEnumerator)WithContext(new _IClosure4_63(this, clazz));
		}

		private sealed class _IClosure4_52 : IClosure4
		{
			public _IClosure4_52(ReflectionTestSuiteBuilder _enclosing, Type clazz)
			{
				this._enclosing = _enclosing;
				this.clazz = clazz;
			}

			public object Run()
			{
				return (IEnumerator)this._enclosing.WithContext(new _IClosure4_54(this, clazz));
			}

			private sealed class _IClosure4_54 : IClosure4
			{
				public _IClosure4_54(_IClosure4_52 _enclosing, Type clazz)
				{
					this._enclosing = _enclosing;
					this.clazz = clazz;
				}

				public object Run()
				{
					return new ContextfulIterator(this._enclosing._enclosing.SuiteFor(clazz));
				}

				private readonly _IClosure4_52 _enclosing;

				private readonly Type clazz;
			}

			private readonly ReflectionTestSuiteBuilder _enclosing;

			private readonly Type clazz;
		}

		private sealed class _IClosure4_63 : IClosure4
		{
			public _IClosure4_63(ReflectionTestSuiteBuilder _enclosing, Type clazz)
			{
				this._enclosing = _enclosing;
				this.clazz = clazz;
			}

			public object Run()
			{
				return new ContextfulIterator(this._enclosing.SuiteFor(clazz));
			}

			private readonly ReflectionTestSuiteBuilder _enclosing;

			private readonly Type clazz;
		}

		private IEnumerator SuiteFor(Type clazz)
		{
			if (!IsApplicable(clazz))
			{
				TestPlatform.EmitWarning("DISABLED: " + clazz.FullName);
				return Iterators.EmptyIterator;
			}
			if (typeof(ITestSuiteBuilder).IsAssignableFrom(clazz))
			{
				return ((ITestSuiteBuilder)NewInstance(clazz)).GetEnumerator();
			}
			if (typeof(ITest).IsAssignableFrom(clazz))
			{
				return Iterators.SingletonIterator(NewInstance(clazz));
			}
			ValidateTestClass(clazz);
			return FromMethods(clazz);
		}

		private void ValidateTestClass(Type clazz)
		{
			if (!(typeof(ITestCase).IsAssignableFrom(clazz)))
			{
				throw new ArgumentException(string.Empty + clazz + " is not marked as " + typeof(
					ITestCase));
			}
		}

		protected virtual bool IsApplicable(Type clazz)
		{
			return clazz != null;
		}

		// just removing the 'parameter not used' warning
		private IEnumerator FromMethods(Type clazz)
		{
			return Iterators.Map(clazz.GetMethods(), new _IFunction4_96(this, clazz));
		}

		private sealed class _IFunction4_96 : IFunction4
		{
			public _IFunction4_96(ReflectionTestSuiteBuilder _enclosing, Type clazz)
			{
				this._enclosing = _enclosing;
				this.clazz = clazz;
			}

			public object Apply(object arg)
			{
				MethodInfo method = (MethodInfo)arg;
				if (!this._enclosing.IsTestMethod(method))
				{
					this._enclosing.EmitWarningOnIgnoredTestMethod(clazz, method);
					return Iterators.Skip;
				}
				return this._enclosing.FromMethod(clazz, method);
			}

			private readonly ReflectionTestSuiteBuilder _enclosing;

			private readonly Type clazz;
		}

		private void EmitWarningOnIgnoredTestMethod(Type clazz, MethodInfo method)
		{
			if (!StartsWithIgnoreCase(method.Name, "_test"))
			{
				return;
			}
			TestPlatform.EmitWarning("IGNORED: " + CreateTest(NewInstance(clazz), method).Label
				());
		}

		protected virtual bool IsTestMethod(MethodInfo method)
		{
			return HasTestPrefix(method) && TestPlatform.IsPublic(method) && !TestPlatform.IsStatic
				(method) && !TestPlatform.HasParameters(method);
		}

		private bool HasTestPrefix(MethodInfo method)
		{
			return StartsWithIgnoreCase(method.Name, "test");
		}

		protected virtual bool StartsWithIgnoreCase(string s, string prefix)
		{
			return s.ToUpper().StartsWith(prefix.ToUpper());
		}

		protected virtual object NewInstance(Type clazz)
		{
			try
			{
				return System.Activator.CreateInstance(clazz);
			}
			catch (Exception e)
			{
				throw new TestException("Failed to instantiate " + clazz, e);
			}
		}

		protected virtual ITest CreateTest(object instance, MethodInfo method)
		{
			return new TestMethod(instance, method);
		}

		protected ITest FromMethod(Type clazz, MethodInfo method)
		{
			return new ContextfulTest(new _ITestFactory_143(this, clazz, method));
		}

		private sealed class _ITestFactory_143 : ITestFactory
		{
			public _ITestFactory_143(ReflectionTestSuiteBuilder _enclosing, Type clazz, MethodInfo
				 method)
			{
				this._enclosing = _enclosing;
				this.clazz = clazz;
				this.method = method;
			}

			public ITest NewInstance()
			{
				return this._enclosing.CreateTest(this._enclosing.NewInstance(clazz), method);
			}

			private readonly ReflectionTestSuiteBuilder _enclosing;

			private readonly Type clazz;

			private readonly MethodInfo method;
		}
	}
}
