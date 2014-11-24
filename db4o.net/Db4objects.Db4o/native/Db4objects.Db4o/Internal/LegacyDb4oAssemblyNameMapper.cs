/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System.Text;
using Db4objects.Db4o.Internal.Encoding;

namespace Db4objects.Db4o.Internal
{
	internal class LegacyDb4oAssemblyNameMapper
	{
		static LegacyDb4oAssemblyNameMapper()
        {   
            LatinStringIO stringIO = new UnicodeStringIO();
            oldAssemblies = new byte[oldAssemblyNames.Length][];
            for (int i = 0; i < oldAssemblyNames.Length; i++)
            {
                oldAssemblies[i] = stringIO.Write(oldAssemblyNames[i]);
            }
        }

		internal byte[] MappedNameFor(byte[] nameBytes)
		{
			for (int i = 0; i < oldAssemblyNames.Length; i++)
			{
				byte[] assemblyName = oldAssemblies[i];

				int j = assemblyName.Length - 1;
				for (int k = nameBytes.Length - 1; k >= 0; k--)
				{
					if (nameBytes[k] != assemblyName[j])
					{
						break;
					}
					j--;
					if (j < 0)
					{
						return UpdateInternalClassName(nameBytes, i);
					}
				}
			}
			return nameBytes;
		}

		private static byte[] UpdateInternalClassName(byte[] bytes, int candidateMatchingAssemblyIndex)
		{
			UnicodeStringIO io = new UnicodeStringIO();
			string typeFQN = io.Read(bytes);

			string[] assemblyNameParts = typeFQN.Split(',');
			if (assemblyNameParts[1].Trim() != oldAssemblyNames[candidateMatchingAssemblyIndex])
			{
				return bytes;
			}

			string typeName = assemblyNameParts[0];
			return io.Write(FullyQualifiedNameFor(typeName).ToString());
		}

		private static StringBuilder FullyQualifiedNameFor(string typeName)
		{
			StringBuilder typeNameBuffer = new StringBuilder(typeName);
			ApplyNameSpaceRenamings(typeNameBuffer);
			typeNameBuffer.Append(", ");
			typeNameBuffer.Append(GetCurrentAssemblyName());
			return typeNameBuffer;
		}

		private static void ApplyNameSpaceRenamings(StringBuilder typeNameBuffer)
		{
			foreach (string[] renaming in NamespaceRenamings)
			{
				typeNameBuffer.Replace(renaming[0], renaming[1]);
			}
		}

		private static string GetCurrentAssemblyName()
		{
			return typeof(Platform4).Assembly.GetName().Name;
		}
		
		private static readonly string[] oldAssemblyNames = new string[] { "db4o-4.0-net1", "db4o-4.0-compact1" };
		private static readonly byte[][] oldAssemblies;

		private static readonly string[][] NamespaceRenamings = new string[][]
                {
                    new string[] { "com.db4o.ext", "Db4objects.Db4o.Ext"},
                    new string[] { "com.db4o.inside", "Db4objects.Db4o.Internal"},
                    new string[] { "com.db4o", "Db4objects.Db4o"}, 
                };

	}
}
