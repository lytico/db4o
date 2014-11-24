/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4oUnit
{
	public class TestException : Exception
	{
        public TestException(string message, Exception reason) : base(message, reason)
        {
        }

		public TestException(Exception reason) : base(reason.Message, reason)
		{
		}

#if !CF && !SILVERLIGHT
		public TestException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
#endif

		public Exception GetReason()
		{
			return InnerException;
		}
		
		override public string ToString()
		{
			if (null != InnerException) return InnerException.ToString();
			return base.ToString();
		}
	}
}
