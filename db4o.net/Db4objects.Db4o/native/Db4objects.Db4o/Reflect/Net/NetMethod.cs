/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Reflect.Net
{
	public class NetMethod : Db4objects.Db4o.Reflect.IReflectMethod
	{
		private readonly System.Reflection.MethodInfo method;

		private readonly Db4objects.Db4o.Reflect.IReflector _reflector;

		public NetMethod(Db4objects.Db4o.Reflect.IReflector reflector, System.Reflection.MethodInfo method)
		{
			_reflector = reflector;
			this.method = method;
		}

		public Db4objects.Db4o.Reflect.IReflectClass GetReturnType() 
		{
			return _reflector.ForClass(method.ReturnType);
		}

		public virtual object Invoke(object onObject, object[] parameters)
		{
            try
            {
                return method.Invoke(onObject, parameters);
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                throw new Db4objects.Db4o.Internal.ReflectException(e.InnerException);
            }
#if CF
            catch (System.Exception e)
			{
                throw new Db4objects.Db4o.Internal.ReflectException(e);
            }
#endif
		}
	}
}
