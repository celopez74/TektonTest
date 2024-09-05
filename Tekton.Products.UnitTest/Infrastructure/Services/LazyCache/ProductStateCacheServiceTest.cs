using LazyCache;
using System;
using System.Collections.Generic;
using Xunit;
using Tekton.Products.Infraestructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

public class ProductStateCacheServiceTests
{
    private readonly FakeAppCache _fakeCache;
    private readonly ProductStateCacheService _service;

    public ProductStateCacheServiceTests()
    {
        _fakeCache = new FakeAppCache();
        _service = new ProductStateCacheService(_fakeCache);
    }

    [Fact]
    public void GetProductStates_ShouldReturnCachedStates()
    {
        // Arrange
        var expectedStates = new Dictionary<int, string>
        {
            { 1, "Active" },
            { 0, "Inactive" }
        };

        // Act
        var result = _service.GetProductStates();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStates, result);
    }

    [Fact]
    public void GetProductStatus_ShouldReturnCorrectStatus()
    {
        // Arrange
        var states = new Dictionary<int, string>
        {
            { 1, "Active" },
            { 0, "Inactive" }
        };

        // Populate the cache with the states
        _fakeCache.GetOrAdd("productStates", () => states, TimeSpan.FromMinutes(5));

        // Act & Assert
        Assert.Equal("Active", _service.GetProductStatus(1));
        Assert.Equal("Inactive", _service.GetProductStatus(0));
        Assert.Equal("Unknown", _service.GetProductStatus(99));
    }
}
public class FakeAppCache : IAppCache
{
    private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

    public ICacheProvider CacheProvider => throw new NotImplementedException();

    public CacheDefaults DefaultCachePolicy => throw new NotImplementedException();

    public void Add<T>(string key, T item, MemoryCacheEntryOptions policy)
    {
        throw new NotImplementedException();
    }

    public T Get<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync<T>(string key)
    {
        throw new NotImplementedException();
    }

    public T GetOrAdd<T>(string key, Func<T> addItemFactory, TimeSpan? expiration = null)
    {
        if (!_cache.TryGetValue(key, out var value))
        {
            value = addItemFactory();
            _cache[key] = value;
        }
        return (T)value;
    }

    public T GetOrAdd<T>(string key, Func<ICacheEntry, T> addItemFactory)
    {
        throw new NotImplementedException();
    }

    public T GetOrAdd<T>(string key, Func<ICacheEntry, T> addItemFactory, MemoryCacheEntryOptions policy)
    {
        if (!_cache.TryGetValue(key, out var value))
        {
            var cacheEntry = new FakeCacheEntry(); // Mock or simple implementation of ICacheEntry
            value = addItemFactory(cacheEntry);
            _cache[key] = value;
        }
        return (T)value;
    }
    

    public Task<T> GetOrAddAsync<T>(string key, Func<ICacheEntry, Task<T>> addItemFactory)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrAddAsync<T>(string key, Func<ICacheEntry, Task<T>> addItemFactory, MemoryCacheEntryOptions policy)
    {
        throw new NotImplementedException();
    }

    public void Remove(string key)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        throw new NotImplementedException();
    }
}
public class FakeCacheEntry : ICacheEntry
{
    public object Key { get; set; }
    public object Value { get; set; }
    public DateTimeOffset? AbsoluteExpiration { get; set; }
    public DateTimeOffset? AbsoluteExpirationRelativeToNow { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }
    public bool HasValue => Value != null;

    public IList<IChangeToken> ExpirationTokens => throw new NotImplementedException();

    public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks => throw new NotImplementedException();

    public CacheItemPriority Priority { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public long? Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    TimeSpan? ICacheEntry.AbsoluteExpirationRelativeToNow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Dispose() { }
}