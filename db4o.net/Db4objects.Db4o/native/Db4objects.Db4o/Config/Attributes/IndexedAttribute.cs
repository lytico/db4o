/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;

namespace Db4objects.Db4o.Config.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class IndexedAttribute : Attribute, IDb4oAttribute
	{
		void IDb4oAttribute.Apply(object subject, ConfigurationIntrospector introspector)
		{
			FieldInfo field = (FieldInfo)subject;
			introspector.ClassConfiguration.ObjectField(field.Name).Indexed(true);
		}
	}
}
