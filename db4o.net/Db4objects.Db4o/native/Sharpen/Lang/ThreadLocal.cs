using System;
using System.Collections.Generic;

namespace Sharpen.Lang
{
#if CF

	class ThreadLocal : Db4objects.Db4o.Foundation.ThreadLocal4
	{
	}

#else

	class ThreadLocal
	{
		[ThreadStatic]
		private static Dictionary<ThreadLocal, object> _locals;

		public object Get()
		{
			object value;
			if (Locals.TryGetValue(this, out value))
				return value;
			return null;
		}

		public void Set(object value)
		{
			if (value == null)
				Locals.Remove(this);
			else
				Locals[this] = value;
		}

		private static Dictionary<ThreadLocal, object> Locals
		{
			get
			{
				Dictionary<ThreadLocal, object> value = _locals;
				if (value == null)
					_locals = value = new Dictionary<ThreadLocal, object>();
				return value;
			}
		}

	}
#endif
}

