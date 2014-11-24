/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System.IO;
using System.Reflection;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit.Extensions;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
#if !CF
	public class Author
	{
		private int _id;
		private string _name;

		public int Id
		{
			get { return _id; }
		}

		public string Name
		{
			get { return _name; }
		}

		public Author(int id, string name)
		{
			_id = id;
			_name = name;
		}
	}
	
	public class OuterAuthor
	{
		public class InnerAuthor : Author
		{
			public InnerAuthor(int id, string name)
				: base(id, name)
			{
			}
		}
	}

	public class MultipleAssemblySupportTestCase : AbstractDb4oTestCase, Db4oUnit.Extensions.Fixtures.IOptOutMultiSession
	{	
#if !SILVERLIGHT 
		override protected void Store()
		{
			Store(new Author(1, "Kurt Vonnegut"));
			Store(new Author(2, "Kilgore Trout"));
			Store(new OuterAuthor.InnerAuthor(3, "Joao Saramago"));
			Store(new OuterAuthor.InnerAuthor(4, "Douglas Adams"));
		}

		public void TestPredicateAccessingTopLevelType()
		{
			string predicateCode = @"
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.CLI1.NativeQueries;

public class AuthorNamePredicate : Predicate
{
	public bool Match(Author candidate)
	{
		return candidate.Name == ""Kilgore Trout"";
	}
}
";
			AssertPredicate(2, predicateCode, "AuthorNamePredicate");
		}

		public void TestPredicateAccessingNestedType()
		{
			string predicateCode = @"
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.CLI1.NativeQueries;

public class InnerAuthorNamePredicate : Predicate
{
	public bool Match(OuterAuthor.InnerAuthor candidate)
	{
		return candidate.Name == ""Joao Saramago"" && candidate.Id > 1;
	}
}
";
			AssertPredicate(3, predicateCode, "InnerAuthorNamePredicate");
		}

		private void AssertPredicate(int expectedId, string predicateCode, string predicateTypeName)
		{
			Assembly assembly = EmitAssemblyAndLoad(predicateTypeName + ".dll", predicateCode);
	
			Predicate predicate = (Predicate)System.Activator.CreateInstance(assembly.GetType(predicateTypeName));

			Db().Configure().OptimizeNativeQueries(true);

			NativeQueryHandler handler = GetNativeQueryHandler(Db());
			handler.QueryExecution += OnQueryExecution;
			try
			{
				IObjectSet os = Db().Query(predicate);
				Assert.AreEqual(1, os.Count);
				Assert.AreEqual(expectedId, ((Author)os.Next()).Id);
			}
			finally
			{
				handler.QueryExecution -= OnQueryExecution;
			}
		}
		
		private static NativeQueryHandler GetNativeQueryHandler(IObjectContainer container)
		{
			return ((ObjectContainerBase)container).GetNativeQueryHandler();
		}

		private static Assembly EmitAssemblyAndLoad(string assemblyName, string code)
		{	
			string assemblyFile = Path.Combine(Path.GetTempPath(), assemblyName);
			CompilationServices.EmitAssembly(assemblyFile, code);
			return Assembly.LoadFrom(assemblyFile);
		}

		private void OnQueryExecution(object sender, QueryExecutionEventArgs args)
		{
			Assert.AreEqual(QueryExecutionKind.DynamicallyOptimized, args.ExecutionKind);
		}
#endif
	}
#endif
}
