/* Copyright (C) 2005   Versant Inc.   http://www.db4o.com */
using System;
using System.Reflection;
using System.Text;

namespace Sharpen.Lang
{	
	public abstract class TypeReference
	{
		public static TypeReference FromString(string s)
		{
			if (null == s) throw new ArgumentNullException("s");
			return new TypeReferenceParser(s).Parse();
		}
		
		public static TypeReference FromType(Type type)
		{
			if (null == type) throw new ArgumentNullException("type");
			return FromString(type.AssemblyQualifiedName);
		}

		public abstract string SimpleName
		{
			get;
		}

		public abstract AssemblyName AssemblyName
		{
			get;
		}

		public abstract Type Resolve();

		public abstract void AppendTypeName(StringBuilder builder);

		public override string ToString()
		{
			return GetUnversionedName();
		}

		public string GetUnversionedName()
		{
			StringBuilder builder = new StringBuilder();
			AppendUnversionedName(builder);
			return builder.ToString();
		}

		internal virtual void AppendUnversionedName(StringBuilder builder)
		{
			AppendTypeName(builder);
			AppendUnversionedAssemblyName(builder);
		}

		protected void AppendUnversionedAssemblyName(StringBuilder builder)
		{
			AssemblyName assemblyName = AssemblyName;
			if (null == assemblyName) return;

			builder.Append(", ");
			builder.Append(assemblyName.Name);
		}
	}

	public partial class SimpleTypeReference : TypeReference
	{
		protected string _simpleName;

		protected AssemblyName _assemblyName;

		internal SimpleTypeReference(string simpleName)
		{
			_simpleName = simpleName;
		}

		public override string SimpleName
		{
			get { return _simpleName; }
		}

		public override AssemblyName AssemblyName
		{
			get { return _assemblyName; }
		}

		public override Type Resolve()
		{
			return _assemblyName == null
				? Type.GetType(SimpleName)
				: ResolveAssembly().GetType(SimpleName);
		}

		public override void AppendTypeName(StringBuilder builder)
		{
			builder.Append(SimpleName);
		}

		public override bool Equals(object obj)
		{
			SimpleTypeReference other = obj as SimpleTypeReference;
			if (null == other) return false;
			return _simpleName == other._simpleName;
		}

		internal void SetSimpleName(string simpleName)
		{
			_simpleName = simpleName;
		}

		internal void SetAssemblyName(AssemblyName assemblyName)
		{
			_assemblyName = assemblyName;
		}

		private Assembly ResolveAssembly()
		{
#if SILVERLIGHT
			return ResolveAssemblySilverlight();
#else
			if (null == _assemblyName.Version)
			{
				return LoadUnversionedAssembly(_assemblyName);
			}

			Assembly found;
			try
			{
				found = Assembly.Load(_assemblyName);
			}
			catch (Exception)
			{
				AssemblyName unversioned = (AssemblyName)_assemblyName.Clone();
				unversioned.Version = null;
				found = LoadUnversionedAssembly(unversioned);
			}
			return found;
#endif
		}

		private static Assembly LoadUnversionedAssembly(AssemblyName unversioned)
		{
#if CF || SILVERLIGHT
            return Assembly.Load(unversioned);
#else
			Assembly found = LoadFromCurrentAppDomain(unversioned.FullName);
			return found != null 
						? found 
						: Assembly.Load(unversioned);
#endif
		}

#if !CF && !SILVERLIGHT
		private static Assembly LoadFromCurrentAppDomain(string fullName)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			return Array.Find(assemblies, a => a.GetName().Name == fullName);
		}
#endif
	}

	public abstract class QualifiedTypeReference : TypeReference
	{
		protected TypeReference _elementType;

		protected QualifiedTypeReference(TypeReference elementType)
		{
			_elementType = elementType;
		}

		public override string SimpleName
		{
			get
			{
				return _elementType.SimpleName;
			}
		}

		public override AssemblyName AssemblyName
		{
			get { return _elementType.AssemblyName; }
		}

		public override void AppendTypeName(StringBuilder builder)
		{
			_elementType.AppendTypeName(builder);
			AppendQualifier(builder);
		}

		protected abstract void AppendQualifier(StringBuilder builder);
	}

	public class PointerTypeReference : QualifiedTypeReference
	{
		public PointerTypeReference(TypeReference elementType)
			: base(elementType)
		{
		}

		protected override void AppendQualifier(StringBuilder builder)
		{
			builder.Append('*');
		}

		public override Type Resolve()
		{
#if !CF
			return _elementType.Resolve().MakePointerType();
#else
			StringBuilder builder = new StringBuilder();
			AppendTypeName(builder);
			return _elementType.Resolve().Assembly.GetType(builder.ToString(), true);
#endif
		}
	}

	public class ArrayTypeReference : QualifiedTypeReference
	{
		public static Type MakeArrayType(Type elementType, int rank)
		{
#if !CF
			if (rank == 1) return elementType.MakeArrayType();
			return elementType.MakeArrayType(rank);
#else
			if (rank == 1) return Array.CreateInstance(elementType, 0).GetType();
			return Array.CreateInstance(elementType, new int[rank]).GetType();
#endif
		}


		private readonly int _rank;

		internal ArrayTypeReference(TypeReference elementType, int rank) : base(elementType)
		{
			_rank = rank;
		}

		public int Rank
		{
			get { return _rank; }
		}

		public override Type Resolve()
		{
			return MakeArrayType(_elementType.Resolve(), _rank);
		}

		protected override void AppendQualifier(StringBuilder builder)
		{
			builder.Append('[');
			for (int i = 1; i < _rank; ++i)
			{
				builder.Append(',');
			}
			builder.Append(']');
		}
	}

	public class GenericTypeReference : SimpleTypeReference
	{
		private readonly TypeReference[] _genericArguments;

		internal GenericTypeReference(string simpleName, TypeReference[] genericArguments)
			: base(simpleName)
		{
			_genericArguments = genericArguments;
		}

		public TypeReference[] GenericArguments
		{
			get { return _genericArguments; }
		}

		public override Type Resolve()
		{
			Type baseType = base.Resolve();
			return _genericArguments.Length > 0
				? baseType.MakeGenericType(Resolve(_genericArguments))
				: baseType;
		}

		static Type[] Resolve(TypeReference[] typeRefs)
		{
			Type[] types = new Type[typeRefs.Length];
			for (int i = 0; i < types.Length; ++i)
			{
				types[i] = typeRefs[i].Resolve();
			}
			return types;
		}

		public override void AppendTypeName(StringBuilder builder)
		{
			builder.Append(_simpleName);
			AppendUnversionedGenericArguments(builder);
		}

		private void AppendUnversionedGenericArguments(StringBuilder builder)
		{
			if (_genericArguments.Length == 0) return;

			builder.Append("[");
			for (int i = 0; i < _genericArguments.Length; ++i)
			{
				if (i > 0) builder.Append(", ");
				builder.Append("[");
				_genericArguments[i].AppendUnversionedName(builder);
				builder.Append("]");
			}
			builder.Append("]");
		}
	}
}
