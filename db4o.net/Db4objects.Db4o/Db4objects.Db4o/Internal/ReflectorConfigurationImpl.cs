/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	public class ReflectorConfigurationImpl : IReflectorConfiguration
	{
		private Config4Impl _config;

		public ReflectorConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		public virtual bool TestConstructors()
		{
			return _config.TestConstructors();
		}

		public virtual bool CallConstructor(IReflectClass clazz)
		{
			TernaryBool specialized = CallConstructorSpecialized(clazz);
			if (!specialized.IsUnspecified())
			{
				return specialized.DefiniteYes();
			}
			return _config.CallConstructors().DefiniteYes();
		}

		private TernaryBool CallConstructorSpecialized(IReflectClass clazz)
		{
			Config4Class clazzConfig = _config.ConfigClass(clazz.GetName());
			if (clazzConfig != null)
			{
				TernaryBool res = clazzConfig.CallConstructor();
				if (!res.IsUnspecified())
				{
					return res;
				}
			}
			if (Platform4.IsEnum(_config.Reflector(), clazz))
			{
				return TernaryBool.No;
			}
			IReflectClass ancestor = clazz.GetSuperclass();
			if (ancestor != null)
			{
				return CallConstructorSpecialized(ancestor);
			}
			return TernaryBool.Unspecified;
		}
	}
}
