/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Config.Attributes
{
	class ConfigurationIntrospector
	{
		private readonly Type _type;
		private Config4Class _classConfig;
		private readonly IConfiguration _config;

		public ConfigurationIntrospector(Type type, Config4Class classConfig, IConfiguration config)
		{
			if (null == type) throw new ArgumentNullException("type");
			if (null == config) throw new ArgumentNullException("config");
			_type = type;
			_classConfig = classConfig;
			_config = config;
		}

		public Type Type
		{
			get { return _type; }
		}

		public Config4Class ClassConfiguration
		{
			get
			{
				if (null == _classConfig)
				{
					_classConfig = (Config4Class)_config.ObjectClass(_type);
				}
				return _classConfig;
			}
		}

		public IConfiguration IConfiguration
		{
			get { return _config; }
		}		

		public void Apply()
		{
			Apply(_type);
			foreach (FieldInfo field in _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				Apply(field);
		}
		
		private void Apply(ICustomAttributeProvider provider)
		{
			foreach (object o in provider.GetCustomAttributes(false))
			{
				IDb4oAttribute attr = o as IDb4oAttribute;
				if (null == attr)
					continue;
				
				attr.Apply(provider, this);
			}
		}
	}
}
