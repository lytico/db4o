/* Copyright (C) 2007 - 2011  Versant Inc.  http://www.db4o.com */
namespace Db4objects.Db4o.Linq.Internals
{
    internal interface IDelayedSelectOperation<T> : IDb4oLinqQueryInternal<T>
    {
        IDb4oLinqQueryInternal<T> Skip(int itemsToSkip);      
    }
}