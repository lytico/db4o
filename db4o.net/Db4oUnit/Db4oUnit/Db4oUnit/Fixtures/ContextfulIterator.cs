/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4oUnit.Fixtures
{
	public class ContextfulIterator : Contextful, IEnumerator
	{
		private readonly IEnumerator _delegate;

		public ContextfulIterator(IEnumerator delegate_)
		{
			_delegate = delegate_;
		}

		public virtual object Current
		{
			get
			{
				return Run(new _IClosure4_17(this));
			}
		}

		private sealed class _IClosure4_17 : IClosure4
		{
			public _IClosure4_17(ContextfulIterator _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				return this._enclosing._delegate.Current;
			}

			private readonly ContextfulIterator _enclosing;
		}

		public virtual bool MoveNext()
		{
			BooleanByRef result = new BooleanByRef();
			Run(new _IRunnable_26(this, result));
			return result.value;
		}

		private sealed class _IRunnable_26 : IRunnable
		{
			public _IRunnable_26(ContextfulIterator _enclosing, BooleanByRef result)
			{
				this._enclosing = _enclosing;
				this.result = result;
			}

			public void Run()
			{
				result.value = this._enclosing._delegate.MoveNext();
			}

			private readonly ContextfulIterator _enclosing;

			private readonly BooleanByRef result;
		}

		public virtual void Reset()
		{
			Run(new _IRunnable_35(this));
		}

		private sealed class _IRunnable_35 : IRunnable
		{
			public _IRunnable_35(ContextfulIterator _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing._delegate.Reset();
			}

			private readonly ContextfulIterator _enclosing;
		}
	}
}
