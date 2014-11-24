/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
using System;
using System.IO;
using System.Reflection;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Diagnostic;

namespace Db4objects.Db4o.Internal.Query
{
	public class NativeQueryHandler
	{
		private IObjectContainer _container;

		private INQOptimizer _builder;

		public event QueryExecutionHandler QueryExecution;

		public event QueryOptimizationFailureHandler QueryOptimizationFailure;

		public NativeQueryHandler(IObjectContainer container)
		{
			_container = container;
		}

        public virtual Db4objects.Db4o.IObjectSet Execute(Db4objects.Db4o.Query.IQuery query, Db4objects.Db4o.Query.Predicate predicate, Db4objects.Db4o.Query.IQueryComparator comparator)
		{
			Db4objects.Db4o.Query.IQuery q = ConfigureQuery(query, predicate);
			q.SortBy(comparator);
			return q.Execute();
		}

        public virtual System.Collections.Generic.IList<Extent> Execute<Extent>(Db4objects.Db4o.Query.IQuery query, System.Predicate<Extent> match,
																				Db4objects.Db4o.Query.IQueryComparator comparator)
		{
#if CF
			return ExecuteUnoptimized<Extent>(QueryForExtent<Extent>(query, comparator), match);
#else
			// XXX: check GetDelegateList().Length
			// only 1 delegate must be allowed
			// although we could use it as a filter chain
			// (and)
			return ExecuteImpl<Extent>(query, match, match.Target, match.Method, match, comparator);
#endif
		}

		public static System.Collections.Generic.IList<Extent> ExecuteEnhancedFilter<Extent>(IObjectContainer container, IDb4oEnhancedFilter predicate)
		{
			return NQHandler(container).ExecuteEnhancedFilter<Extent>(predicate);
		}

		public System.Collections.Generic.IList<T> ExecuteEnhancedFilter<T>(IDb4oEnhancedFilter filter)
		{
			IQuery query = _container.Query();
			query.Constrain(typeof(T));
			filter.OptimizeQuery(query);
			OnQueryExecution(filter, QueryExecutionKind.PreOptimized);
			return WrapQueryResult<T>(query);
		}

		private static NativeQueryHandler NQHandler(IObjectContainer container)
		{
			return ((ObjectContainerBase)container).GetNativeQueryHandler();
		}

		private System.Collections.Generic.IList<Extent> ExecuteImpl<Extent>(
                                                                        Db4objects.Db4o.Query.IQuery query, 
																		object originalPredicate,
																		object matchTarget,
																		System.Reflection.MethodBase matchMethod,
																		System.Predicate<Extent> match,
																		Db4objects.Db4o.Query.IQueryComparator comparator)
		{
			Db4objects.Db4o.Query.IQuery q = QueryForExtent<Extent>(query, comparator);
			try
			{
				if (OptimizeNativeQueries())
				{
					OptimizeQuery(q, matchTarget, matchMethod);
					OnQueryExecution(originalPredicate, QueryExecutionKind.DynamicallyOptimized);

					return WrapQueryResult<Extent>(q);
				}
			}
            catch(FileNotFoundException fnfe)
            {
                NativeQueryOptimizerNotLoaded(fnfe);
            }
            catch(TargetInvocationException tie)
		    {
                NativeQueryOptimizerNotLoaded(tie);
		    }
			catch(TypeLoadException tle)
			{
				NativeQueryOptimizerNotLoaded(tle);
			}
            catch (System.Exception e)
			{
				OnQueryOptimizationFailure(e);
			    
                NativeQueryUnoptimized(e);
			}
            
            return ExecuteUnoptimized(q, match);
		}

	    private void NativeQueryUnoptimized(Exception e)
	    {
            DiagnosticProcessor dp = Container()._handlers.DiagnosticProcessor();
            if (dp.Enabled()) dp.NativeQueryUnoptimized(null, e);
	    }

	    private void NativeQueryOptimizerNotLoaded(Exception exception)
	    {
	        DiagnosticProcessor dp = Container()._handlers.DiagnosticProcessor();
	        if (dp.Enabled()) dp.NativeQueryOptimizerNotLoaded(Db4o.Diagnostic.NativeQueryOptimizerNotLoaded.NqNotPresent, exception);
	    }

	    private System.Collections.Generic.IList<Extent> ExecuteUnoptimized<Extent>(IQuery q, Predicate<Extent> match)
		{
			q.Constrain(new GenericPredicateEvaluation<Extent>(match));
			OnQueryExecution(match, QueryExecutionKind.Unoptimized);
			return WrapQueryResult<Extent>(q);
		}

        private Db4objects.Db4o.Query.IQuery QueryForExtent<Extent>(Db4objects.Db4o.Query.IQuery query, Db4objects.Db4o.Query.IQueryComparator comparator)
		{
			query.Constrain(typeof(Extent));
			query.SortBy(comparator);
			return query;
		}

		private static System.Collections.Generic.IList<Extent> WrapQueryResult<Extent>(Db4objects.Db4o.Query.IQuery query)
		{
			IQueryResult queryResult = ((QQuery)query).GetQueryResult();
			return new GenericObjectSetFacade<Extent>(queryResult);
		}

        private Db4objects.Db4o.Query.IQuery ConfigureQuery(Db4objects.Db4o.Query.IQuery query, Db4objects.Db4o.Query.Predicate predicate)
		{
			IDb4oEnhancedFilter filter = predicate as IDb4oEnhancedFilter;
			if (null != filter)
			{
				filter.OptimizeQuery(query);
				OnQueryExecution(predicate, QueryExecutionKind.PreOptimized);
				return query;
			}

			query.Constrain(predicate.ExtentType());

			try
			{
				if (OptimizeNativeQueries())
				{
					OptimizeQuery(query, predicate, predicate.GetFilterMethod());
					OnQueryExecution(predicate, QueryExecutionKind.DynamicallyOptimized);
					return query;
				}
			}
			catch (System.Exception e)
			{
				OnQueryOptimizationFailure(e);

                if (OptimizeNativeQueries())
                {
                    DiagnosticProcessor dp = Container()._handlers.DiagnosticProcessor();
                    if (dp.Enabled()) dp.NativeQueryUnoptimized(predicate, e);
                }
            }

			query.Constrain(new Db4objects.Db4o.Internal.Query.PredicateEvaluation(predicate));
			OnQueryExecution(predicate, QueryExecutionKind.Unoptimized);
			return query;
		}

	    private ObjectContainerBase Container()
	    {
	        return ((ObjectContainerBase)_container);
	    }

	    private bool OptimizeNativeQueries()
		{
			return _container.Ext().Configure().OptimizeNativeQueries();
		}

		void OptimizeQuery(Db4objects.Db4o.Query.IQuery q, object predicate, System.Reflection.MethodBase filterMethod)
		{
			if (_builder == null)
				_builder = NQOptimizerFactory.CreateExpressionBuilder();

			_builder.Optimize(q, predicate, filterMethod);
		}

		private void OnQueryExecution(object predicate, QueryExecutionKind kind)
		{
			if (null == QueryExecution) return;
			QueryExecution(this, new QueryExecutionEventArgs(predicate, kind));
		}

		private void OnQueryOptimizationFailure(System.Exception e)
		{
			if (null == QueryOptimizationFailure) return;
			QueryOptimizationFailure(this, new QueryOptimizationFailureEventArgs(e));
		}
	}

	class GenericPredicateEvaluation<T> : DelegateEnvelope, Db4objects.Db4o.Query.IEvaluation
	{
		public GenericPredicateEvaluation()
		{
			// for db4o c/s when CallConstructors == true
		}

		public GenericPredicateEvaluation(System.Predicate<T> predicate)
			: base(predicate)
		{
		}

		public void Evaluate(Db4objects.Db4o.Query.ICandidate candidate)
		{
			// use starting _ for PascalCase conversion purposes
			System.Predicate<T> _predicate = (System.Predicate<T>)GetContent();
			candidate.Include(_predicate((T)candidate.GetObject()));
		}
	}
}


