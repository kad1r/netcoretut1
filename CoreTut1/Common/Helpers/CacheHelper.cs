using Microsoft.Extensions.Caching.Memory;
using System;

namespace Common.Helpers
{
	public static class CacheHelper
	{
		/*
		 *
		 In-Memory Cache
		 Distributed Cache
		 Response Cache
		 *
		 */

		public static dynamic Add(IMemoryCache cache, string name, object obj, MemoryCacheEntryOptions cacheOptions, bool IsSetDefaultOptions = false)
		{
			if (IsSetDefaultOptions)
			{
				cacheOptions = SetDefaultOptions();
			}

			cache.Set(name, obj, cacheOptions);

			return cache;
		}

		public static dynamic Get(IMemoryCache cache, string name)
		{
			return cache.Get(name);
		}

		public static void Remove(IMemoryCache cache, string name)
		{
			cache.Remove(name);
		}

		public static MemoryCacheEntryOptions SetDefaultOptions()
		{
			return new MemoryCacheEntryOptions()
				.SetSlidingExpiration(TimeSpan.FromMinutes(10))
				.SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
		}
	}
}
