using System.Reflection;
using System;
using System.Collections.Generic;

namespace LinqConsole
{
    public static class PrettyPrintExtensions
    {
        public static void PrettyPrint<T>(this IEnumerable<T> self)
        {
            if (self == null) return;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in properties)
            {
                Console.Write(p.Name);
                Console.Write("\t");
            }
            Console.WriteLine();

            foreach (var p in properties)
            {
                Console.Write(new string('=', p.Name.Length));
                Console.Write("\t");
            }
            Console.WriteLine();

            foreach (var item in self)
            {
                foreach (var p in properties)
                {
                    Console.Write(p.GetValue(item, null));
                    Console.Write("\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
