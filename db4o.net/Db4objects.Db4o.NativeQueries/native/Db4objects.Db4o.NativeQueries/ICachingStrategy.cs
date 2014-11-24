/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Internal.Query
{
	public interface ICachingStrategy<K,V>
	{
		void Add(K key, V item);
		V Get(K key);
	}
}