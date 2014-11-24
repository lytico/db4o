/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
namespace Db4oUnit
{
	public partial class AssertionException
	{
#if !CF && !SILVERLIGHT
		public AssertionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
#endif
	}
}
