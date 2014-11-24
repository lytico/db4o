/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */

using System.Collections.Generic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal
{
    using System;
    using Query;
    using Query.Result;
    using Query.Processor;
    using Ext;

    public partial class ObjectContainerBase
    {
        void IDisposable.Dispose()
        {
            Close();
        }

        public IObjectSet Query(Predicate match, System.Collections.IComparer comparer)
        {
            if (null == match) throw new ArgumentNullException("match");
            return Query(null, match, new ComparerAdaptor(comparer));
        }

        class GenericComparerAdaptor<T> : IQueryComparator
        {
            private readonly IComparer<T> _comparer;

            public GenericComparerAdaptor(IComparer<T> comparer)
            {
                _comparer = comparer;
            }

            public int Compare(object first, object second)
            {
                return _comparer.Compare((T)first, (T)second);
            }
        }

        class GenericComparisonAdaptor<T> : DelegateEnvelope, IQueryComparator
        {
            public GenericComparisonAdaptor(Comparison<T> comparer) : base(comparer)
            {
            }

            public int Compare(object first, object second)
            {
                Comparison<T> _comparer = (Comparison<T>)GetContent();
                return _comparer((T)first, (T)second);
            }
        }

        public IList<Extent> Query<Extent>(Predicate<Extent> match)
        {
            return Query(null, match);
        }

        public IList<Extent> Query<Extent>(Transaction trans, Predicate<Extent> match)
        {
            return ExecuteNativeQuery(trans, match, null);
        }

        public IList<Extent> Query<Extent>(Predicate<Extent> match, IComparer<Extent> comparer)
        {
            return Query(null, match, comparer);
        }


        public IList<Extent> Query<Extent>(Transaction trans, Predicate<Extent> match, IComparer<Extent> comparer)
        {
            IQueryComparator comparator = null != comparer
                                                            ? new GenericComparerAdaptor<Extent>(comparer)
                                                            : null;
            return ExecuteNativeQuery(trans, match, comparator);
        }

        public IList<Extent> Query<Extent>(Predicate<Extent> match, Comparison<Extent> comparison)
        {
            return Query(null, match, comparison);
        }


        public IList<Extent> Query<Extent>(Transaction trans, Predicate<Extent> match, Comparison<Extent> comparison)
        {
            IQueryComparator comparator = null != comparison
                                                        ? new GenericComparisonAdaptor<Extent>(comparison)
                                                        : null;
            return ExecuteNativeQuery(trans, match, comparator);
        }

        public IList<ElementType> Query<ElementType>(Type extent)
        {
            return Query<ElementType>(null, extent, null);
        }


        public IList<ElementType> Query<ElementType>(Transaction trans, Type extent)
        {
            return Query<ElementType>(trans, extent, null);
        }

        public IList<ElementType> Query<ElementType>(Type extent, IComparer<ElementType> comparer)
        {
            return Query(null, extent, comparer);
        }


        public IList<ElementType> Query<ElementType>(Transaction trans, Type extent, IComparer<ElementType> comparer)
        {
            lock (Lock())
            {
                trans = CheckTransaction(trans);
                QQuery query = (QQuery)Query(trans);
                query.Constrain(extent);
                if (null != comparer) query.SortBy(new GenericComparerAdaptor<ElementType>(comparer));
                IQueryResult queryResult = query.GetQueryResult();
                return new GenericObjectSetFacade<ElementType>(queryResult);
            }
        }

        public IList<Extent> Query<Extent>()
        {
            return Query<Extent>(typeof(Extent));
        }

        public IList<Extent> Query<Extent>(IComparer<Extent> comparer)
        {
            return Query<Extent>(typeof(Extent), comparer);
        }

        private IList<Extent> ExecuteNativeQuery<Extent>(Transaction trans, Predicate<Extent> match, IQueryComparator comparator)
        {
            if (null == match) throw new ArgumentNullException("match");
            lock (Lock())
            {
            	IQuery query = Query(CheckTransaction(trans));
				return (IList<Extent>) ((QQuery)query).TriggeringQueryEvents(Closures4.ForDelegate(
				                                                     	delegate() {
				                                                     	           	return GetNativeQueryHandler().Execute(query, match, comparator);
				                                                     	}));
            }
        }

    	public delegate R SyncExecClosure<R>();

		public R SyncExec<R>(SyncExecClosure<R> closure)
		{
			return (R)SyncExec(new SyncExecClosure4<R>(closure));
		}

    	public class SyncExecClosure4<R> : IClosure4
    	{
    		private readonly SyncExecClosure<R> _closure;

    		public SyncExecClosure4(SyncExecClosure<R> closure)
    		{
    			_closure = closure;
    		}

    		#region Implementation of IClosure4

    		public object Run()
    		{
    			return _closure.Invoke();
    		}

    		#endregion
    	}

	    private object AsTopLevelCall(IFunction4 block, Transaction trans) 
	    {
	    	trans = CheckTransaction(trans);
	    	BeginTopLevelCall();
	        try
	        {            	
	        	return block.Apply(trans);
	        } 
	        catch(Db4oRecoverableException exc) 
	        {
	        	throw;
	        }
	        catch(SystemException exc) 
	        {
	        	throw;
	        }
	        catch(Exception exc) 
	        {
				FatalShutdown(exc);
	        }
	        finally 
	        {
	        	EndTopLevelCall();
	        }
	        // should never happen - just to make compiler happy
			throw new Db4oException();
	    }

		public void WithEnvironment(Action4 action)
		{
			WithEnvironment(new RunnableAction(action));
		}
    }
}