using System;
using System.Collections.Generic;

internal static class ListPool<T>
{
	public static List<T> Get()
	{
		return ListPool<T>.s_ListPool.Get();
	}

	public static void Release(List<T> toRelease)
	{
		ListPool<T>.s_ListPool.Release(toRelease);
	}

	private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, delegate(List<T> l)
	{
		l.Clear();
	});
}
