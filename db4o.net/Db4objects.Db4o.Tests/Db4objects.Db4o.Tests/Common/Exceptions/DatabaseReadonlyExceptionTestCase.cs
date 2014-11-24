/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class DatabaseReadonlyExceptionTestCase : AbstractDb4oTestCase, IOptOutTA, 
		IOptOutInMemory, IOptOutDefragSolo
	{
		public static void Main(string[] args)
		{
			new DatabaseReadonlyExceptionTestCase().RunAll();
		}

		public virtual void TestRollback()
		{
			ConfigReadOnly();
			Assert.Expect(typeof(DatabaseReadOnlyException), new _ICodeBlock_21(this));
		}

		private sealed class _ICodeBlock_21 : ICodeBlock
		{
			public _ICodeBlock_21(DatabaseReadonlyExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Rollback();
			}

			private readonly DatabaseReadonlyExceptionTestCase _enclosing;
		}

		public virtual void TestCommit()
		{
			ConfigReadOnly();
			Assert.Expect(typeof(DatabaseReadOnlyException), new _ICodeBlock_30(this));
		}

		private sealed class _ICodeBlock_30 : ICodeBlock
		{
			public _ICodeBlock_30(DatabaseReadonlyExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Commit();
			}

			private readonly DatabaseReadonlyExceptionTestCase _enclosing;
		}

		public virtual void TestSet()
		{
			ConfigReadOnly();
			Assert.Expect(typeof(DatabaseReadOnlyException), new _ICodeBlock_39(this));
		}

		private sealed class _ICodeBlock_39 : ICodeBlock
		{
			public _ICodeBlock_39(DatabaseReadonlyExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Store(new Item());
			}

			private readonly DatabaseReadonlyExceptionTestCase _enclosing;
		}

		public virtual void TestDelete()
		{
			ConfigReadOnly();
			Assert.Expect(typeof(DatabaseReadOnlyException), new _ICodeBlock_48(this));
		}

		private sealed class _ICodeBlock_48 : ICodeBlock
		{
			public _ICodeBlock_48(DatabaseReadonlyExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Delete(new Item());
			}

			private readonly DatabaseReadonlyExceptionTestCase _enclosing;
		}

		public virtual void TestNewFile()
		{
			Assert.Expect(typeof(DatabaseReadOnlyException), new _ICodeBlock_56(this));
		}

		private sealed class _ICodeBlock_56 : ICodeBlock
		{
			public _ICodeBlock_56(DatabaseReadonlyExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				AbstractDb4oTestCase.Fixture().Close();
				AbstractDb4oTestCase.Fixture().Clean();
				AbstractDb4oTestCase.Fixture().Config().ReadOnly(true);
				AbstractDb4oTestCase.Fixture().Open(this._enclosing);
			}

			private readonly DatabaseReadonlyExceptionTestCase _enclosing;
		}

		public virtual void TestReserveStorage()
		{
			ConfigReadOnly();
			Type exceptionType = IsMultiSession() && !IsEmbedded() ? typeof(NotSupportedException
				) : typeof(DatabaseReadOnlyException);
			Assert.Expect(exceptionType, new _ICodeBlock_70(this));
		}

		private sealed class _ICodeBlock_70 : ICodeBlock
		{
			public _ICodeBlock_70(DatabaseReadonlyExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Configure().ReserveStorageSpace(1);
			}

			private readonly DatabaseReadonlyExceptionTestCase _enclosing;
		}

		public virtual void TestStoredClasses()
		{
			ConfigReadOnly();
			Db().StoredClasses();
		}

		private void ConfigReadOnly()
		{
			Db().Configure().ReadOnly(true);
		}
	}
}
