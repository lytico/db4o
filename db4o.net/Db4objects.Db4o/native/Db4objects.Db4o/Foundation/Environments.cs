/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Foundation
{
	public partial class Environments
	{
		public static string DefaultImplementationFor(Type type)
		{
			string implName = ("." + type.Name.Substring(1) + "Impl");
			if (type.Namespace.IndexOf(".Internal.") > 0)
				return type.Namespace + implName + ", " + AssemblyNameFor(type);

			int lastDot = type.Namespace.LastIndexOf('.');
			string typeName = type.Namespace.Substring(0, lastDot) + ".Internal." + type.Namespace.Substring(lastDot + 1) + implName;
			return typeName + ", " + AssemblyNameFor(type);
		}

		private static string AssemblyNameFor(Type type)
		{
#if SILVERLIGHT
			string fullyQualifiedTypeName = type.AssemblyQualifiedName;
			int assemblyNameSeparator = fullyQualifiedTypeName.IndexOf(',');
			return fullyQualifiedTypeName.Substring(assemblyNameSeparator + 1);
#else
			return type.Assembly.GetName().Name;
#endif
		}
	}
}
