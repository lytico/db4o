/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using System;
using System.Collections.Generic;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	public abstract class ValueTypeHandlerTestCaseBase<T> : AbstractDb4oTestCase where T : struct, IComparable<T>
	{
        public class ValueTypeHolder
        {
        	public ValueTypeHolder(T value)
            {
				Value = value;
        		UntypedValue = value;
            }

			public ValueTypeHolder(T value, ValueTypeHolder parent) : this(value)
			{
				Parent = parent;
			}

            public override bool Equals(object obj)
            {
				ValueTypeHolder rhs = obj as ValueTypeHolder;
                if (rhs == null) return false;

                if (rhs.GetType() != GetType()) return false;

            	return (rhs.Value.CompareTo(Value) == 0) && CompareParent(rhs);
            }

        	private bool CompareParent(ValueTypeHolder rhs)
        	{
        		return Parent == null 
					? rhs.Parent == null 
					: Parent.Equals(rhs.Parent) ;
        	}

        	public override int GetHashCode()
			{
				return Value.GetHashCode() + (Parent != null ? Parent.GetHashCode() : 0);
			}

            public override string ToString()
            {
				return "[" + typeof(T).Name + "]" + Value + " Parent {"  + (Parent != null ? Parent.ToString() : "") + "}";
            }

			public T Value;
			public object UntypedValue;
			public ValueTypeHolder Parent;
		}

		protected abstract ValueTypeHolder[] ObjectsToStore();

		protected abstract ValueTypeHolder[] ObjectsToOperateOn();

		protected virtual T UpdateValueFor(ValueTypeHolder holder)
		{
			return holder.Value;
		}

		protected IQuery NewDescendingQuery(Action<IQuery> action)
		{
			IQuery query = NewQuery(typeof(ValueTypeHolder));
			IQuery descendingQuery = query.Descend("Value");

			action(descendingQuery);

			return query;
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(ValueTypeHolder)).ObjectField("Value").Indexed(true);
			config.ObjectClass(typeof(ValueTypeHolder)).CascadeOnDelete(true);
		}

		protected override void Store()
        {
            foreach (ValueTypeHolder obj in ObjectsToStore())
            {
                Store(obj);
            }
        }

		public void TestNativeQuery()
        {
			foreach (ValueTypeHolder tbf in ObjectsToOperateOn())
			{
				AssertHolder(tbf.Value);	
			}
        }

        public void TestSODAQuery()
        {
			foreach (ValueTypeHolder tbf in ObjectsToOperateOn())
			{
				AssertSODAQuery(tbf.Value);
			}
		}

		public void TestQueryByExample()
		{
			foreach (ValueTypeHolder tbf in ObjectsToOperateOn())
			{
				ValueTypeHolder holder = FindHolderWithValue(tbf.Value);
				IObjectSet result = Db().QueryByExample(holder);
				Assert.AreEqual(1, result.Count);
				ValueTypeHolder found = (ValueTypeHolder)result[0];
				AssertHolder(holder, found);
			}
		}

		public void TestRetrieveAll()
        {
            AssertCanRetrieveAll();
        }

		public void TestNoClassIndex()
		{
			IStoredClass storedClass = Db().StoredClass(typeof(T));
			Assert.AreEqual(0, storedClass.InstanceCount());
		}

		public void TestQueryOnUntypedField()
		{
			IList<ValueTypeHolder> holders = new List<ValueTypeHolder>(Flatten(ObjectsToStore()));

			ValueTypeHolder greatest = holders[0];
			ValueTypeHolder secondGreatest = holders[0];

			foreach (ValueTypeHolder holder in holders)
			{
			    IComparable actual = (IComparable) holder.Value;
				if (actual.CompareTo(greatest.Value) > 0)
				{
					secondGreatest = greatest;
					greatest = holder;
				}
				else if (actual.CompareTo(secondGreatest.Value) > 0)
				{
					secondGreatest = holder;
				}
			}

			Assert.AreNotEqual(greatest, secondGreatest);

			IQuery query = NewQuery(typeof (ValueTypeHolder));
			query.Descend("UntypedValue").Constrain(secondGreatest.Value).Greater();

			IObjectSet result = query.Execute();

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(greatest, result[0]);
		}

		public void TestDefragment()
        {
            Defragment();
            AssertCanRetrieveAll();
        }
        
		public void TestIndexingLowLevel()
		{
			LocalObjectContainer container = Fixture().FileSession();
			ClassMetadata classMetadata = container.ClassMetadataForReflectClass(container.Reflector().ForClass(typeof(ValueTypeHolder)));
			FieldMetadata fieldMetadata = classMetadata.FieldMetadataForName("Value");

			Assert.IsTrue(fieldMetadata.CanLoadByIndex(), WithTypeName("Typehandler for type {0} should be indexable."));
			BTree index = fieldMetadata.GetIndex(container.SystemTransaction());
			Assert.IsNotNull(index, WithTypeName("No btree index found for field of type {0} ."));
		}

		public void TestIndexedQuery()
		{
			DiagnosticCollector<LoadedFromClassIndex> collector = DiagnosticCollectorFor<LoadedFromClassIndex>();

			ValueTypeHolder expected = ObjectsToOperateOn()[0];
			ValueTypeHolder actual = RetrieveHolderWithValue(expected.Value);

			Assert.IsNotNull(actual);
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(0, collector.Diagnostics.Count, WithTypeName("Query should go through {0} indexes"));
		}

		public void TestUpdate()
		{
			ValueTypeHolder updated = RetrieveHolderWithValue(ObjectsToOperateOn()[0].Value);
			T newValue = UpdateValueFor(updated);
			Store(updated);

			Reopen();

			ValueTypeHolder actual = RetrieveHolderWithValue(newValue);
			Assert.AreEqual(updated, actual);
		}

		public void TestDelete()
		{
			DiagnosticCollector<DeletionFailed> diagnosticCollector = DiagnosticCollectorFor<DeletionFailed>();

			IQuery query = NewQuery(typeof(ValueTypeHolder));
			IObjectSet result = query.Execute();
			while (result.HasNext())
			{
				ValueTypeHolder item = (ValueTypeHolder)result.Next();
				Db().Delete(item);
			}

			Assert.IsTrue(diagnosticCollector.Empty, diagnosticCollector.ToString());
		}

		private ValueTypeHolder FindHolderWithValue(T value)
		{
#if CF || SILVERLIGHT
    		foreach (ValueTypeHolder holder in ObjectsToStore())
    		{
    			if (holder.Value.CompareTo(value) == 0)
    			{
    				return holder;
    			}
    		}

    		return null;
#else
			return Array.Find(ObjectsToStore(), delegate(ValueTypeHolder candidate)
			{
				return candidate.Value.CompareTo(value) == 0;
			});
#endif
		}

		private void AssertCanRetrieveAll()
		{
			IQuery query = NewQuery(typeof(ValueTypeHolder));
			IObjectSet result = query.Execute();

			ValueTypeHolder[] expected = ObjectsToStore();
			Iterator4Assert.SameContent(Flatten(expected).GetEnumerator(), result.GetEnumerator());
		}

		private static IEnumerable<ValueTypeHolder> Flatten(IEnumerable<ValueTypeHolder> holders)
		{
			foreach (ValueTypeHolder holder in holders)
			{
				yield return holder;
				if (holder.Parent != null)
				{
					yield return holder.Parent;
				}
			}
		}

		private void AssertSODAQuery(T value)
    	{
    		IQuery query = NewQuery();
    		query.Constrain(typeof (ValueTypeHolder));
    		query.Descend("Value").Constrain(value);

    		IObjectSet result = query.Execute();
    		Assert.AreEqual(1, result.Count);
    		AssertHolder(FindHolderWithValue(value), (ValueTypeHolder) result[0]);
    	}

		private void AssertHolder(ValueTypeHolder actual, ValueTypeHolder template)
		{
			ValueTypeHolder expected = FindHolderWithValue(template.Value);
			Assert.AreEqual(expected, actual);
		}

		private void AssertHolder(T expectedValue)
        {
			IList<ValueTypeHolder> items = Db().Query<ValueTypeHolder>(delegate(ValueTypeHolder candidate) {return candidate.Value.Equals(expectedValue); });
            Assert.AreEqual(1, items.Count);
            Assert.IsNotNull(items[0]);

            ValueTypeHolder expected =  Find(ObjectsToStore(), delegate(ValueTypeHolder candidate) { return candidate.Value.Equals(expectedValue); });
            Assert.IsNotNull(expected);

            Assert.AreEqual(expected, items[0]);
        }
     
        private static ValueTypeHolder Find(ValueTypeHolder[] array, Predicate<ValueTypeHolder> expected)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (expected(array[i]))
                {
                    return array[i];
                }
            }
            return null;
        }

		protected ValueTypeHolder RetrieveHolderWithValue(T value)
		{
			IObjectSet result = RetrieveHoldersWith(value);
			Assert.AreEqual(1, result.Count);
			ValueTypeHolder actual = (ValueTypeHolder)result[0];
			Assert.AreEqual(value, actual.Value);

			return actual;
		}

		protected IObjectSet RetrieveHoldersWith(params T[] values)
		{
			IConstraint lastConstraint = null;

			IQuery query = NewQuery(typeof(ValueTypeHolder));
			foreach (T value in values)
			{
				IConstraint constraint = query.Descend("Value").Constrain(value);
				if (lastConstraint != null)
				{
					lastConstraint.Or(constraint);
				}

				lastConstraint = constraint;
			}

			return query.Execute();
		}

		private DiagnosticCollector<D> DiagnosticCollectorFor<D>()
		{
			DiagnosticCollector<D> collector = new DiagnosticCollector<D>();
			Db().Configure().Diagnostic().AddListener(collector);

			return collector;
		}

		private static string WithTypeName(string format)
		{
			return string.Format(format, typeof(T).Name);
		}
    }
}
