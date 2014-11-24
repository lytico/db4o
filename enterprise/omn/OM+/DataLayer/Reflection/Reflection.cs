/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */

using System.Collections.Generic;
using Db4objects.Db4o.Reflect;
using OManager.Business.Config;

namespace OManager.DataLayer.Reflection
{
    public interface ITypeResolver
    {
        IType Resolve(string typeFQN);
        IType Resolve(IReflectClass klass);
    }

    public class TypeResolver : ITypeResolver
    {
		private readonly IReflector _reflector;
		private readonly IDictionary<string, IType> _resolved = new Dictionary<string, IType>();

		public TypeResolver(IReflector reflector)
		{
			_reflector = reflector;
		}

		public IType Resolve(string typeFQN)
		{
			return Resolve(_reflector.ForName(typeFQN));
		}

        public IType Resolve(IReflectClass klass)
        {
            if (klass == null)
            {
                return null;
            }

            string className = klass.GetName();
            if (!_resolved.ContainsKey(className))
            {
                _resolved[className] = new TypeImpl(klass, this);
            }
            return _resolved[className];

        }
    }
}
