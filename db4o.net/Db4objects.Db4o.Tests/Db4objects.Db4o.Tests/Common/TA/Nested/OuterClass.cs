/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Nested;

namespace Db4objects.Db4o.Tests.Common.TA.Nested
{
	public partial class OuterClass : ActivatableImpl
	{
		public int _foo;

		public virtual int Foo()
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			return _foo;
		}

		public virtual OuterClass.InnerClass CreateInnerObject()
		{
			return new OuterClass.InnerClass(this);
		}

		public partial class InnerClass : ActivatableImpl
		{
			public virtual OuterClass GetOuterObject()
			{
				// TA BEGIN
				this.Activate(ActivationPurpose.Read);
				// TA END
				return this._enclosing;
			}

			public virtual OuterClass GetOuterObjectWithoutActivation()
			{
				return this._enclosing;
			}

			internal InnerClass(OuterClass _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly OuterClass _enclosing;
		}
	}
}
