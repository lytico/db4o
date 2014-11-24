#if !CF
using System;
using System.Linq;
using System.Linq.Expressions;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.SharpenLang
{
	public class SharpenRuntimeTestCase : ITestCase
	{
		public void Test()
		{
			//The following expression triggers this bug in CLR
			//https://connect.microsoft.com/VisualStudio/feedback/details/566678/field-of-generic-class-missing-when-getmembers-called-with-declaredonly-if-it-was-used-in-an-expression
			
			Expression<Func<Item<string>, bool>> exp = item => item.a == 100;

			var declaredFieldNames = Sharpen.Runtime.GetDeclaredFields(typeof (Item<string>)).Select( f => f.Name );
			IteratorAssert.SameContent(new [] {"a", "b"}, declaredFieldNames);
		}

		class Item<T>
		{
			public int a;
			public int b;
		}
	}
}
#endif