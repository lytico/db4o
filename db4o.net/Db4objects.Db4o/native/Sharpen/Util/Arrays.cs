using System.Collections.Generic;

namespace Sharpen.Util
{
	public class Arrays
	{
		public static void Fill<T>(T[] array, T value)
		{	
			for (int i=0; i<array.Length; ++i)
			{
				array[i] = value;
			}
		}
        
        public static void Fill<T>(T[] array, int fromIndex, int toIndex, T value)
        {
            for (int i = fromIndex; i < toIndex; ++i)
            {
                array[i] = value;
            }
        }

		public static bool Equals<T>(T[] x, T[] y)
		{
			if (x == null) return y == null;
			if (y == null) return false;
			if (x.Length != y.Length) return false;
			for (int i = 0; i < x.Length; ++i)
			{
				if (!object.Equals(x[i], y[i])) return false;
			}
			return true;
		}

		public static List<T> AsList<T>(T[] array)
        {
            return new List<T>(array); 
        }
    }
}
