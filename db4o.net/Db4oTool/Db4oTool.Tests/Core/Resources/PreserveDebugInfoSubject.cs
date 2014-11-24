using System;
using System.Collections;
using System.Collections.Generic;
using Db4oUnit;
using System.Globalization;
using System.Threading;

class Foo : IEnumerable<Foo>
{
	Foo _child;

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<Foo>)this).GetEnumerator();
	}

	public IEnumerator<Foo> GetEnumerator()
	{
		foreach (Foo a in _child)
		{
			yield return a;
			foreach (Foo b in a)
			{
				yield return b;
			}
		}
	}

	public IEnumerable Bar(bool raise)
	{
		string prefix = "child is: ";
		if (raise) throw new ApplicationException();
		foreach (Foo child in _child.Bar(false))
			yield return prefix + child;
	}
}

public class PreserveDebugInfoSubject : ITestCase
{
	public void Test()
	{
        CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
        try
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            try
            {
                foreach (object o in new Foo().Bar(true))
				{
				}
            }
            catch (Exception x)
            {
                string message = x.ToString();
                Assert.IsTrue(message.Contains("PreserveDebugInfoSubject.cs:line 32"), message);
            }
        }
        finally
        {
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
        }
	}
}