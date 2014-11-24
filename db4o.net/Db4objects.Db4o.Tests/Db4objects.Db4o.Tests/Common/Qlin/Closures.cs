/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Tests.Common.Qlin;

namespace Db4objects.Db4o.Tests.Common.Qlin
{
	public class Closures : BasicQLinTestCase
	{
		public class Closure
		{
		}

		public virtual void With(object obj, object obj2)
		{
		}

		public virtual IList ListOf(BasicQLinTestCase.Cat[] cats)
		{
			return null;
		}

		public virtual void ClosureSample()
		{
			// List<Cat> occamAndZora = occamAndZora();
			BasicQLinTestCase.Cat cat = ((BasicQLinTestCase.Cat)QLinSupport.Prototype(typeof(
				BasicQLinTestCase.Cat)));
			IList cats = ListOf(new BasicQLinTestCase.Cat[] { new BasicQLinTestCase.Cat("Zora"
				), new BasicQLinTestCase.Cat("Wolke") });
			With(cats, new _Closure_31(cat));
		}

		private sealed class _Closure_31 : Closures.Closure
		{
			public _Closure_31(BasicQLinTestCase.Cat cat)
			{
				this.cat = cat;
				{
					cat.Feed();
				}
			}

			private readonly BasicQLinTestCase.Cat cat;
		}

		//            
		//            Iterable<Cat> query = occamAndZora();
		//            
		//            with(db().from(Cat.class).select()).feed();
		//        
		//        
		//            query = occamAndZora();
		//            
		//            Iterable<Color> colors = map(db().from(Cat.class).select(), cat.color());
		//        
		//		final Cat cat = prototype(Cat.class);
		//		List<Cat> occamAndZora = occamAndZora();
		//		with(occamAndZora, new Closure { cat.feed() } );
		//	   public <T> T with(Iterable<T> withOn){
		//	        // magic goes here
		//	        return null;
		//	    }
		//
		//	    public <T,TResult> Iterable<TResult> map(Iterable<T> withOn,TResult projection ){
		//	        // magic goe here
		//	        return null;
		//	    }
		private void With(IList occamAndZora, object closure)
		{
		}
	}
}
#endif // !SILVERLIGHT
