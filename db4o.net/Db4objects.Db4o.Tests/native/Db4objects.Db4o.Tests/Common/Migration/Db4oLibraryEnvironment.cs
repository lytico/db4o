using System;
using System.IO;
using System.Reflection;
using Db4objects.Db4o.Internal;

#if !CF && !SILVERLIGHT
using Db4oUnit.Extensions.Util;
using Mono.Cecil;
using Mono.Collections.Generic;
#endif

using File = Sharpen.IO.File;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	class CrossDomainRef<T> 
#if !SILVERLIGHT
		: MarshalByRefObject
#endif
	{
		private T _value;

		public virtual T Value
		{
			get { return _value;  }
			set { _value = value; }
		}
	}
	[Serializable]
	class MethodInvoker
	{
		private readonly string _typeName;
		private readonly string _methodName;
		private readonly object[] _arguments;
		private CrossDomainRef<object> _returnValue;

		public MethodInvoker(CrossDomainRef<object> returnValue, string typeName, string methodName, params object[] arguments)
		{
			_typeName = typeName;
			_methodName = methodName;
			_arguments = arguments;
			_returnValue = returnValue;
		}

		public void InvokeInstanceMethod()
		{
			Type type = Type.GetType(_typeName, true);
			_returnValue.Value = type.InvokeMember(_methodName, BindingFlags.InvokeMethod | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public, null, Activator.CreateInstance(type), _arguments);
		}

		public void InvokeStaticMethod()
		{
			Type type = Type.GetType(_typeName, true);
			_returnValue.Value = type.InvokeMember(_methodName, BindingFlags.InvokeMethod | BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public, null, null, _arguments);
		}
	}

	[Serializable]
	class InstallAssemblyResolver
	{
		private readonly string _assembly;
		private readonly string _assemblyName;

		public InstallAssemblyResolver(string assembly)
		{
			_assembly = assembly;
			_assemblyName = Path.GetFileNameWithoutExtension(_assembly);
		}

#if !CF
		public void Execute()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (SimpleName(args.Name) == _assemblyName)
			{
				return Assembly.LoadFrom(_assembly);
			}
			return null;
		}
#endif

		private string SimpleName(string name)
		{
			return name.Split(',')[0];
		}
	}

	public class Db4oLibraryEnvironment : IDisposable
	{
		private readonly AppDomain _domain;

		private readonly string _targetAssembly;

		private string _version;
		private readonly string _baseDirectory;
		private string _assemblyVersion;

		public Db4oLibraryEnvironment(File file, File additionalAssembly)
		{
			_targetAssembly = file.GetAbsolutePath();
#if !CF && !SILVERLIGHT
			_assemblyVersion = AssemblyVersionFor(_targetAssembly);
			_baseDirectory = IOServices.BuildTempPath("migration-domain-" + _assemblyVersion);
			_domain = CreateDomain(SetUpBaseDirectory());
			try
			{
				SetUpAssemblyResolver();
				SetUpLegacyAdapter();
			}
			catch (Exception x)
			{
				Dispose();
				throw new Exception("Failed to setup environment for '" + _targetAssembly + "'", x);
			}
#endif
			
		}

#if !CF && !SILVERLIGHT
		private string SetUpBaseDirectory()
		{
			CopyAssemblies(_baseDirectory);
			return _baseDirectory;
		}

		private void SetUpLegacyAdapter()
		{
			if (Path.GetFileNameWithoutExtension(_targetAssembly) == "Db4objects.Db4o")
				return;

			string adapterAssembly = Path.Combine(_baseDirectory, "Db4objects.Db4o.dll");
			new LegacyAdapterEmitter(_targetAssembly, _assemblyVersion).Emit(adapterAssembly);

		}

		private void SetUpAssemblyResolver()
		{
			_domain.DoCallBack(new InstallAssemblyResolver(_targetAssembly).Execute);
		}

		private static AppDomain CreateDomain(string baseDirectory)
		{
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = baseDirectory;
			return AppDomain.CreateDomain(Path.GetFileName(baseDirectory), null, setup);
		}

		private void CopyAssemblies(string domainBase)
		{
			CleanStrongName(IOServices.CopyTo(_targetAssembly, domainBase));
			CleanStrongName(IOServices.CopyEnclosingAssemblyTo(GetType(), domainBase));
			CleanStrongName(IOServices.CopyEnclosingAssemblyTo(typeof(Db4oUnit.ITest), domainBase));
			CleanStrongName(IOServices.CopyEnclosingAssemblyTo(typeof(Db4oUnit.Extensions.IDb4oTestCase), domainBase));
		}

		private static void CleanStrongName(string path)
		{
			AssemblyDefinition asm = AssemblyDefinition.ReadAssembly(path);
			CleanStrongNames(asm.Modules);
			CleanStrongName(asm.Name);

			asm.Write(asm.MainModule.FullyQualifiedName, WriterParametersFor(true));
		}

        private static WriterParameters WriterParametersFor(bool preserveDebugInfo)
        {
            WriterParameters parameters = new WriterParameters();
            parameters.WriteSymbols = preserveDebugInfo;
            return parameters;
        }

		private static void CleanStrongNames(Collection<ModuleDefinition> modules)
		{
			foreach (ModuleDefinition m in modules)
			{
				CleanStrongNames(m.AssemblyReferences);
			}
		}

		private static void CleanStrongNames(Collection<AssemblyNameReference> references)
		{
			foreach (AssemblyNameReference name in references)
			{
				if (name.Name.StartsWith("Db4objects.Db4o")
					|| name.Name.StartsWith("Db4oUnit"))
				{
					CleanStrongName(name);
				}
			}
		}

		private static void CleanStrongName(AssemblyNameReference name)
		{
			name.HasPublicKey = false;
			name.PublicKeyToken = new byte[0];
			name.PublicKey = new byte[0];
		}
#endif

		private string AssemblyVersionFor(string assembly)
		{
#if !CF && !SILVERLIGHT
			return Assembly.ReflectionOnlyLoadFrom(assembly).GetName().Version.ToString();
#else
			return System.Reflection.Assembly.LoadFrom(assembly).GetName().Version.ToString();
#endif
		}

		public string Version()
		{
			if (null != _version) return _version;
			return _version = GetVersion().Substring("db4o ".Length);
		}

		private string GetVersion()
		{
#if !CF && !SILVERLIGHT
			CrossDomainRef<object> returnValue = new CrossDomainRef<object>();
			_domain.DoCallBack(new MethodInvoker(returnValue, ReflectPlatform.FullyQualifiedName(typeof(Db4oFactory)), "Version").InvokeStaticMethod);
			return (string)returnValue.Value;	
#else
			return null;
#endif
		}

		public object InvokeInstanceMethod(Type type, string methodName, params object[] args)
		{
#if !CF && !SILVERLIGHT
			CrossDomainRef<object> returnValue = new CrossDomainRef<object>();
			_domain.DoCallBack(new MethodInvoker(returnValue, ReflectPlatform.FullyQualifiedName(type), methodName, args).InvokeInstanceMethod);
			return returnValue.Value;
#else
			return null;
#endif
		}

		public void Dispose()
		{
#if !CF && !SILVERLIGHT
			AppDomain.Unload(_domain);
#endif
		}
	}

}