using System;
using System.Collections.Generic;

namespace Db4oDoc.Code.Strategies.Paging
{
    public static class PagingUtility
    {
        // #example: Paging utility methods
        public static IList<T> Paging<T>(IList<T> listToPage, int limit)
        {
            return Paging(listToPage, 0, limit);
        }

        public static IList<T> Paging<T>(IList<T> listToPage, int start, int limit)
        {
            if (start > listToPage.Count)
            {
                throw new ArgumentException("You cannot start the paging outside the list." +
                                            " List-size: " + listToPage.Count + " start: " + start);
            }
            int end = calculateEnd(listToPage, start, limit);
            IList<T> list = new List<T>();
            for (int i = start; i < end; i++)
            {
                list.Add(listToPage[i]);
            }
            return list;
        }

        private static int calculateEnd<T>(IList<T> resultList, int start, int limit)
        {
            int end = start + limit;
            if (end >= resultList.Count)
            {
                return resultList.Count;
            }
            return end;
        }

        // #end example
    }
}