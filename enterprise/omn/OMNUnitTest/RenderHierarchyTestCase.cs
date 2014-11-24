/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Thread = System.Threading.Thread;

using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using OManager.BusinessLayer.Common;
using OManager.DataLayer.Connection;
using OManager.DataLayer.QueryParser;
using OME.AdvancedDataGridView;
using Sharpen.Lang;

using NUnit.Framework;

namespace OMNUnitTest
{
	[TestFixture]
	public class RenderHierarchyTestCase : OMNTestCaseBase
	{
		private const string OBJECT_GRAPH = "rt(foo, 1)->vt(bar, 2)->rt(baz, 3)->rt(foo.bar, 4)->vt(foo.bar.baz, 5)->vt(ooi1, 6)->rt(ooi2, 7)->vt(ooi3, 8)";

		protected override void Store()
		{
			Store(Create(OBJECT_GRAPH));
		}

		[Test]
		public void TestSingleValueTypeChanged()
		{
			AssertSingleValueTypeChanged(6, 60);
		}

		[Test]
		public void TestFirstValueTypeChanged()
		{
			AssertSingleValueTypeChanged(2, 20);
		}

		[Test]
		public void TestLastValueTypeChanged()
		{
			AssertSingleValueTypeChanged(8, 80);
		}

		[Test]
		public void TestMultipleValueTypesChanged()
		{
			AssertValueTypeChanged(new Change(2, 20), new Change(5, 50), new Change(6, 60));
		}

		[Test]
		public void TestMultipleValueTypesChangedInReverseOrder()
		{
			AssertValueTypeChanged(new Change(6, 60), new Change(5, 50), new Change(2, 20));
		}

		[Test]
		public void TestCollectionItemsAreExcluded()
		{
			RunInSTAThread(
				delegate
				{
					Db.Store(new ComplexItem<SubItem>(new SubItem(1), new SubItem(2), new SubItem(3)));
					Reopen();

					object obj = RetrieveOnlyObject(typeof(ComplexItem<SubItem>));
					TreeGridNode rootNode = RootNodeFor(obj);

					Assert.IsFalse(RenderHierarchy.TryUpdateValueType(SubItemValueFieldFor(rootNode, 2), 10));
				});
		}

		[Test]
		public void TestSimpleCollectionsAreExcluded()
		{
			RunInSTAThread(
							delegate
							{
								Db.Store(new ComplexItem<int>(1, 2, 3));
								Reopen();

								object obj = RetrieveOnlyObject(typeof(ComplexItem<int>));
								TreeGridNode rootNode = RootNodeFor(obj);

								Assert.IsFalse(RenderHierarchy.TryUpdateValueType(SubItemValueFieldForIntCollection(rootNode, 2), 10));
							});			
		}


		public class Test
		{
			public Test(int id, Hashtable hashtable)
			{
				this.id = id;
				this.hashtable = hashtable;	
			}

			public Hashtable hashtable;
			public int id;
		}

		[Test]
		public void TestChildCollectionsAreExcluded()
		{
			RunInSTAThread(
							delegate
							{
								Db.Store(new Test(1, NewHashTable(1, 2, 3, 4, 5)));
								Reopen();

								object obj = RetrieveOnlyObject(typeof(Test));
								Db.Ext().Activate(obj, 10);
								TreeGridNode rootNode = RootNodeFor(obj);

								Assert.IsFalse(RenderHierarchy.TryUpdateValueType(FindTreeNodeWithInt(rootNode, 5), 10));
							});
		}

		private static TreeGridNode FindTreeNodeWithInt(TreeGridNode node, int candidate)
		{
			if (node != null)
			{
				if (node.Tag is int)
				{
					if ( (int)node.Tag == candidate)
						return node;
				}

				foreach (var childNode in node.Nodes)
				{
					TreeGridNode found = FindTreeNodeWithInt(childNode, candidate);
					if (found != null) return found;
				}
			}

			return null;
		}

		private static Hashtable NewHashTable(params int[] values)
		{
			Hashtable hashtable = new Hashtable();
			foreach (int i in values)
			{
				hashtable[i] = i.ToString();
			}

			return hashtable;
		}


		private static TreeGridNode SubItemValueFieldForIntCollection(TreeGridNode node, int candidate)
		{
			return node.Nodes.SelectMany(item => item.Nodes).Where(current => current.Tag != null && ((int)current.Tag) == candidate).Single();
		}

		private static TreeGridNode SubItemValueFieldFor(TreeGridNode node, int candidate)
		{
			return node.Nodes.SelectMany(item => item.Nodes).SelectMany(n => n.Nodes).Where(current => ((int) current.Tag) == candidate).Single();
		}

		private static object RetrieveOnlyObject(Type type)
		{
			IObjectSet result = Db.Query(type);
			Assert.AreEqual(1, result.Count);

			return result[0];
		}

		internal void AssertValueTypeChanged(params Change[] changes)
		{
			RunInSTAThread(
				delegate
				{
					object obj = RetrieveRootObject();
					AssertObjectUpdate(obj, RootNodeFor(obj), changes);
					Store(obj, 10);

					Reopen();

					object o = RetrieveRootObject();
					Db4oClient.Client.Ext().Activate(o, 20);
					AssertUpdatedObject(o, changes);
				});
		}

		private void AssertSingleValueTypeChanged(int candidate, int newValue)
		{
			AssertValueTypeChanged(new Change(candidate, newValue));
		}

		private void RunInSTAThread(ThreadStart code)
		{
			Thread staThread = new Thread(TestRunner);
			staThread.SetApartmentState(ApartmentState.STA);

			staThread.Start(code);
			staThread.Join();

			if (_exception != null)
			{
				throw new AssertionException("Failure", _exception);
			}
		}

		private void TestRunner(object code)
		{
			ThreadStart start = (ThreadStart) code;
			try
			{
				start();
			}
			catch (AssertionException ae)
			{
				_exception = ae;
			}
		}

		private static void AssertObjectUpdate(object obj, TreeGridNode rootNode, params Change[] changes)
		{
			UpdateObjectWithValue(rootNode, changes);
			AssertUpdatedObject(obj, changes);
		}

		private static void AssertUpdatedObject(object obj, params Change[] changes)
		{
			string objGraph = OBJECT_GRAPH;
			foreach (Change change in changes)
			{
				objGraph = Regex.Replace(objGraph, String.Format(@", {0}", change.OldValue), ", " + change.NewValue);
			}
			
			Assert.AreEqual(objGraph, obj.ToString());
		}

		private static void UpdateObjectWithValue(TreeGridNode rootNode, params Change[] changes)
		{
			foreach (Change change in changes)
			{
				TreeGridNode found = FindObjectInTree(rootNode, change.OldValue);
				Assert.IsNotNull(found);

				RenderHierarchy.TryUpdateValueType(found, change.NewValue);
			}
		}

		private static TreeGridNode FindObjectInTree(TreeGridNode node, int candidate)
		{
			while (node != null)
			{
				INameValue named = node.Tag as INameValue;
				if (null != named && named.Value == candidate)
				{
					return FindCandidateNode(node, candidate);
				}

				node = NamedChildNode(node);
			}
			return null;
		}

		private static TreeGridNode FindCandidateNode(TreeGridNode node, int candidate)
		{
			foreach (TreeGridNode current in node.Nodes)
			{
				if (current.Tag is int)
				{
					int value = (int) current.Tag;
					if (value == candidate)
						return current;
				}
			}

			return null;
		}

		private static TreeGridNode NamedChildNode(TreeGridNode node)
		{
			foreach (TreeGridNode candidate in node.Nodes)
			{
				if  (candidate.Tag is INameValue)
					return candidate;
			}

			return null;
		}

		private static TreeGridNode RootNodeFor(object obj)
		{
			RenderHierarchy rh = new RenderHierarchy();
			TreeGridView treeView = rh.ReturnHierarchy(obj, TypeReference.FromType(obj.GetType()).GetUnversionedName());

			TreeGridNode rootNode = treeView.Nodes[0];
			
			ExpandAllNodes(rh, rootNode);

			return rootNode;
		}

		private static void ExpandAllNodes(RenderHierarchy rh, TreeGridNode node)
		{
			if (IsExpansionRequired(node))
			{
				if (node.Tag is ICollection)
					rh.ExpandCollectionNode(node);
				else
					rh.ExpandObjectNode(node, true);
			}

			foreach (TreeGridNode current in node.Nodes)
			{
				ExpandAllNodes(rh, current);
			}
		}

		private static bool IsExpansionRequired(TreeGridNode node)
		{
			return node.Nodes.Count == 1 && (string) node.Nodes[0].Cells[0].Value == BusinessConstants.DB4OBJECTS_DUMMY;
		}

		private static object RetrieveRootObject()
		{
			IQuery query = Db.Query();
			query.Descend("value").Constrain(1);

			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);

			return result[0];
		}

		private static object Create(string graph)
		{
			MatchCollection matches = Regex.Matches(graph, @"(?<class>[r|v]t)\((?<name>[^,]*)\s*,\s*(?<value>[0-9]*)\)");
			object current = null;
			for (int i = matches.Count - 1; i >= 0; i--)
			{
				Match m = matches[i];

				string className = ClassNameFor(m);
				current = Activator.CreateInstance(
									TypeFor(className, current), 
									ArgumentsFor(current, m.Groups["name"].Value, Int32.Parse(m.Groups["value"].Value)));
			}

			return current;
		}

		private static object[] ArgumentsFor(object current, string name, int value)
		{
			return new[] {name, value, current ?? 0xDB40};
		}

		private static Type TypeFor(string baseName, object current)
		{
			if (baseName == "VT")
			{
				return typeof (VT<>).MakeGenericType(current == null ? typeof (int) : current.GetType());
			}
			
			return typeof(RT<>).MakeGenericType(current.GetType());
		}

		private static string ClassNameFor(Match m)
		{
			return m.Groups["class"].Value.ToUpper();
		}

		private Exception _exception;
	}

	internal interface INameValue
	{
		int Value { get; set; }
		string Name { get; set; }
	}

	internal struct VT<T> : INameValue
	{
		public int value;
		public string name;
		public T child;

		public VT(string name_, int value_, T child_)
		{
			value = value_;
			name = name_;
			child = child_;
		}

		public int Value
		{
			get { return value; }
			set { this.value = value;}
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public override string ToString()
		{
			return "vt(" + name + ", " + value + ")" + (typeof(T) != typeof(int) ? "->" + child : "");
		}
	}

	internal class RT<T> : INameValue
	{
		public RT(string name_, int value_, T genType_) : this(name_, value_)
		{
			genType = genType_;
		}

		private RT(string name_, int value_)
		{
			name = name_;
			value = value_;
		}

		public string name;
		public int value;
		public T genType;
		
		public int Value
		{
			get { return value; }
			set { this.value = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public override string ToString()
		{
			return "rt(" + name + ", " + value + ")->" + genType;
		}
	}

	internal struct Change
	{
		public Change(int oldValue, int newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
		
		public int OldValue;
		public int NewValue;
	}
}
