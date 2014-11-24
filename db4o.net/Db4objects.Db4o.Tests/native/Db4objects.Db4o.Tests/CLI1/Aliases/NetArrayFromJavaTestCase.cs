using System;
using System.IO;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.CLI1.Aliases
{
	class NetArrayFromJavaTestCase : ITestCase, IOptOutSilverlight
	{
#if !CF
		public class Item
		{
			private readonly string _description;
			private readonly byte[] _byteArray;
			private readonly int[] _intArray;
			private readonly float[] _floatArray;

			public Item(string description, byte[] byteArray, int[] intArray, float[] floatArray)
			{
				_description = description;
				_byteArray = byteArray;
				_intArray = intArray;
				_floatArray = floatArray;
			}

			public override string ToString()
			{
				return "Item("
					+ _description
					+ ", " + ToString(_byteArray)
					+ ", " + ToString(_intArray)
					+ ", " + ToString(_floatArray)
					+ ")";
			}

			static string ToString(System.Collections.IEnumerable items)
			{
				if (items == null) return "null";
				return new Collection4(items).ToString();
			}

		}

		public void Test()
		{
			if (!JavaServices.CanRunJavaCompatibilityTests()) return;

			DeleteDataFile();
			GenerateNetDataFile();
			DumpDataFile();

			string output = CompileAndRunJavaApplication();
			AssertJavaOutput(output);
		}

		private static void DumpDataFile()
		{
			using (IObjectContainer container = Db4oFactory.OpenFile(DataFile))
			{
				foreach (Item item in container.Query(typeof(Item)))
				{
					Console.WriteLine(item);
				}
			}
		}

		private static void DeleteDataFile()
		{
			File4.Delete(DataFile);
		}

		private static void AssertJavaOutput(string output)
		{
			const string expected = @"**
Item(1) all null arrays, null, null, null)
Item(2) non null arrays, [0, 1, 127], [-2147483648, 0, 2147483647], [-3.4028235E38, 0.0, 3.4028235E38, NaN])
**";
			Assert.AreEqual(ns(expected), ns(output));
		}

		static string ns(string s)
		{
			return s.Trim().Replace("\r\n", Environment.NewLine);
		}

		private static string CompileAndRunJavaApplication()
		{
			CompileJavaApplication();
			return RunJavaApplication();
		}

		private static string RunJavaApplication()
		{
			return
				JavaServices.java("NetArrayFromJava.Program",
								  DataFile,
								  CrossPlatformServices.FullyQualifiedName(typeof(Item)));
		}

		private static void CompileJavaApplication()
		{
			const string code = @"
package NetArrayFromJava;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.config.*;
import com.db4o.query.*;

class Item {
    private String _description;
    private byte[] _byteArray;
    private int[] _intArray;
    private float[] _floatArray;

    public String toString() {
        return ""Item(""
            + _description
            + "", "" + byteArrayString()
            + "", "" + intArrayString()
            + "", "" + floatArrayString()
            + "")"";
    }

    public String byteArrayString() {
        if (null == _byteArray) return null;
        Collection4 c = new Collection4();
        for (int i=0; i<_byteArray.length; ++i) c.add(new Byte(_byteArray[i]));
        return c.toString();
    }

    public String intArrayString() {
        if (null == _intArray) return null;
        Collection4 c = new Collection4();
        for (int i=0; i<_intArray.length; ++i) c.add(new Integer(_intArray[i]));
        return c.toString();
    }

    public String floatArrayString() {
        if (null == _floatArray) return null;
        Collection4 c = new Collection4();
        for (int i=0; i<_floatArray.length; ++i) c.add(new Float(_floatArray[i]));
        return c.toString();
    }
}

public class Program {
    public static void main(String[] args) {

        String fname = args[0];
        String typeName = args[1];

		//System.out.println(fname);
		if (!new java.io.File(fname).exists()) {
			System.out.println(""'"" + fname + ""' not found."");
		}
        Configuration configuration = Db4o.newConfiguration();
        configuration.addAlias(new TypeAlias(typeName, ""NetArrayFromJava.Item""));
        configuration.add(new DotnetSupport());
        ObjectContainer container = Db4o.openFile(configuration, fname);
        try {  
            ObjectSet found = queryItems(container);
			System.out.println(""**"");
            while (found.hasNext()) {
                System.out.println(found.next());
            }
			System.out.println(""**"");
        } finally {
            container.close();
        }
    }

    static ObjectSet queryItems(ObjectContainer container) {
        Query q = container.query();
        q.constrain(Item.class);
        q.descend(""_description"").orderAscending();
        return q.execute();
    }
}
";
			JavaServices.CompileJavaCode("NetArrayFromJava/Program.java", code);
		}

		private static void GenerateNetDataFile()
		{
			using (IObjectContainer container = Db4oFactory.OpenFile(DataFile))
			{
				container.Store(new Item("1) all null arrays", null, null, null));
				container.Store(
					new Item(
						"2) non null arrays",
						ByteArray(),
						IntArray(),
						FloatArray()));
			}
			Console.WriteLine(DataFile);
		}

		private static string DataFile
		{
			get { return Path.Combine(Path.GetTempPath(), "NetArray.db4o"); }
		}

		private static byte[] ByteArray()
		{
			return new byte[] { 0, 1, 127 };
		}

		private static int[] IntArray()
		{
			return new int[] { int.MinValue, 0, int.MaxValue };
		}

		private static float[] FloatArray()
		{
			return new float[] { float.MinValue, 0, float.MaxValue, float.NaN };
		}
#endif
	}
}
