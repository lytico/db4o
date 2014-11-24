/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	public interface IReferenceTypeHandler : ITypeHandler4
	{
		/// <summary>gets called when an object is to be activated.</summary>
		/// <remarks>gets called when an object is to be activated.</remarks>
		/// <param name="context"></param>
		void Activate(IReferenceActivationContext context);
	}
}
