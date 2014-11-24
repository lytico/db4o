/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	public class ClientTransactionPool
	{
		private readonly Hashtable4 _transaction2Container;

		private readonly Hashtable4 _fileName2Container;

		private readonly LocalObjectContainer _mainContainer;

		private bool _closed;

		public ClientTransactionPool(LocalObjectContainer mainContainer)
		{
			// Transaction -> ContainerCount
			// String -> ContainerCount
			ClientTransactionPool.ContainerCount mainEntry = new ClientTransactionPool.ContainerCount
				(mainContainer, 1);
			_transaction2Container = new Hashtable4();
			_fileName2Container = new Hashtable4();
			_fileName2Container.Put(mainContainer.FileName(), mainEntry);
			_mainContainer = mainContainer;
		}

		public virtual Transaction AcquireMain()
		{
			return Acquire(_mainContainer.FileName());
		}

		public virtual Transaction Acquire(string fileName)
		{
			lock (_mainContainer.Lock())
			{
				ClientTransactionPool.ContainerCount entry = (ClientTransactionPool.ContainerCount
					)_fileName2Container.Get(fileName);
				if (entry == null)
				{
					LocalObjectContainer container = (LocalObjectContainer)Db4oEmbedded.OpenFile(Db4oEmbedded
						.NewConfiguration(), fileName);
					container.ConfigImpl.SetMessageRecipient(_mainContainer.ConfigImpl.MessageRecipient
						());
					entry = new ClientTransactionPool.ContainerCount(container);
					_fileName2Container.Put(fileName, entry);
				}
				Transaction transaction = entry.NewTransaction();
				ObjectContainerSession objectContainerSession = new ObjectContainerSession(entry.
					Container(), transaction);
				transaction.SetOutSideRepresentation(objectContainerSession);
				_transaction2Container.Put(transaction, entry);
				return transaction;
			}
		}

		public virtual void Release(ShutdownMode mode, Transaction transaction, bool rollbackOnClose
			)
		{
			lock (_mainContainer.Lock())
			{
				ClientTransactionPool.ContainerCount entry = (ClientTransactionPool.ContainerCount
					)_transaction2Container.Get(transaction);
				entry.Container().CloseTransaction(transaction, false, mode.IsFatal() ? false : rollbackOnClose
					);
				_transaction2Container.Remove(transaction);
				entry.Release();
				if (entry.IsEmpty())
				{
					_fileName2Container.Remove(entry.FileName());
					try
					{
						entry.Close(mode);
					}
					catch (Exception t)
					{
						// If we are in fatal ShutdownMode close will
						// throw but we want to continue shutting down
						// all entries.
						Sharpen.Runtime.PrintStackTrace(t);
					}
				}
			}
		}

		public virtual void Close()
		{
			Close(ShutdownMode.Normal);
		}

		public virtual void Close(ShutdownMode mode)
		{
			lock (_mainContainer.Lock())
			{
				IEnumerator entryIter = _fileName2Container.Iterator();
				while (entryIter.MoveNext())
				{
					IEntry4 hashEntry = (IEntry4)entryIter.Current;
					ClientTransactionPool.ContainerCount containerCount = (ClientTransactionPool.ContainerCount
						)hashEntry.Value();
					try
					{
						containerCount.Close(mode);
					}
					catch (Exception t)
					{
						// If we are in fatal ShutdownMode close will
						// throw but we want to continue shutting down
						// all entries.
						Sharpen.Runtime.PrintStackTrace(t);
					}
				}
				_closed = true;
			}
		}

		public virtual int OpenTransactionCount()
		{
			return IsClosed() ? 0 : _transaction2Container.Size();
		}

		public virtual int OpenFileCount()
		{
			return IsClosed() ? 0 : _fileName2Container.Size();
		}

		public virtual bool IsClosed()
		{
			return _closed == true || _mainContainer.IsClosed();
		}

		public class ContainerCount
		{
			private LocalObjectContainer _container;

			private int _count;

			public ContainerCount(LocalObjectContainer container) : this(container, 0)
			{
			}

			public virtual LocalObjectContainer Container()
			{
				return _container;
			}

			public ContainerCount(LocalObjectContainer container, int count)
			{
				_container = container;
				_count = count;
			}

			public virtual bool IsEmpty()
			{
				return _count <= 0;
			}

			public virtual Transaction NewTransaction()
			{
				_count++;
				return _container.NewUserTransaction();
			}

			public virtual void Release()
			{
				if (_count == 0)
				{
					throw new InvalidOperationException();
				}
				_count--;
			}

			public virtual string FileName()
			{
				return _container.FileName();
			}

			public virtual void Close(ShutdownMode mode)
			{
				if (!mode.IsFatal())
				{
					_container.Close();
					_container = null;
					return;
				}
				_container.FatalShutdown(((ShutdownMode.FatalMode)mode).Exc());
			}

			public override int GetHashCode()
			{
				return FileName().GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null || GetType() != obj.GetType())
				{
					return false;
				}
				ClientTransactionPool.ContainerCount other = (ClientTransactionPool.ContainerCount
					)obj;
				return FileName().Equals(other.FileName());
			}
		}
	}
}
