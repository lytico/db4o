using System;
using System.Collections;
using System.Text;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Util;
using Db4objects.Db4o.Typehandlers;
using Db4oUnit;

#if !CF && !SILVERLIGHT
namespace Db4objects.Db4o.Tests.CLI1
{
	class CollectionHolder
	{
		public ArrayList _list;	
	}

	class JavaCollectionCompatibilityTestCase : TestWithTempFile
	{
		protected JavaSnippet JavaCode()
		{
			return new JavaSnippet(
						"JavaCollectionProducer",
						@"
import java.util.*;
import com.db4o.*;

class CollectionHolder {
	public ArrayList _list;

	public CollectionHolder(ArrayList list) {
		_list = list;
	}
	
	
}

public class JavaCollectionProducer {
	public static void main(String[] args) {
		
		EmbeddedObjectContainer db = null;
		try {
			String databasePath = args[0];		
			
			db = Db4oEmbedded.openFile(databasePath);			
			db.store(new CollectionHolder(newListFor(_ITEMS_ARRAY_HERE_)));			
		}
		finally {
			db.close();			
		}
	}

	private static ArrayList newListFor(String ...values) {
		return new ArrayList(Arrays.asList(values));
	}
}
");
		}

		private static string[] ItemNames = new string[] {"foo", "bar"};

		public void TestArrayList()
		{
			CompileJavaCode();
			JavaServices.java(JavaCode().MainClassName, TempFile());

			using (IObjectContainer db = OpenDatabase())
			{
				CollectionHolder holder = RetrieveOnlyInstance<CollectionHolder>(db);
			}
		}

		private void CompileJavaCode()
		{
			JavaSnippet snippet = JavaCode();

			JavaServices.ResetJavaTempPath();
			string @out = JavaServices.CompileJavaCode(snippet.MainClassFile, PopulateItemValues(snippet.SourceCode));
			Console.Error.WriteLine(@out);
		}

		private static string PopulateItemValues(string code)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string item in ItemNames)
			{
				sb.AppendFormat("\"{0}\",", item);
			}
			sb.Remove(sb.Length - 1, 1);

			return code.Replace("_ITEMS_ARRAY_HERE_", sb.ToString());
		}

		private static T RetrieveOnlyInstance<T>(IObjectContainer db)
		{
			IQuery query = db.Query();
			query.Constrain(typeof (T));

			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);

			return (T) result[0];
		}

		private IObjectContainer OpenDatabase()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			Configure(config);

			return Db4oEmbedded.OpenFile(config, TempFile());
		}

		protected void Configure(IEmbeddedConfiguration config)
		{
			config.Common.Add(new JavaSupport());

			config.Common.AddAlias(new TypeAlias("CollectionHolder", TypeName(typeof(CollectionHolder))));
			config.Common.AddAlias(new TypeAlias("java.util.ArrayList", TypeName(typeof(ArrayList))));

			config.Common.AddAlias(new TypeAlias("java.util.AbstractCollection", TypeName(typeof(DummyAbstractCollection))));
			config.Common.AddAlias(new TypeAlias("java.util.AbstractList", TypeName(typeof(DummyAbstractList))));

			config.Common.AddAlias(new TypeAlias("com.db4o.typehandlers.IgnoreFieldsTypeHandler", TypeName(typeof(IgnoreFieldsTypeHandler))));
			config.Common.AddAlias(new TypeAlias("com.db4o.typehandlers.CollectionTypeHandler", TypeName(typeof(IgnoreFieldsTypeHandler))));

			config.Common.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(DummyAbstractList)), IgnoreFieldsTypeHandler.Instance);
			config.Common.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(DummyAbstractCollection)), IgnoreFieldsTypeHandler.Instance);
		}

		private static string TypeName(Type type)
		{
			return ReflectPlatform.FullyQualifiedName(type);
		}

		private class DummyAbstractCollection
		{
		}
		
		private class DummyAbstractList
		{
		}
	}
}
#endif