using Moq;
using TodoApp.Interfaces.IServices;

namespace TodoApp.UnitTests.Helper;

public static class CacheMockHelper
{
    public static void SetupPassThrough<T>(Mock<ICachedQueryService> cachedQueryServiceMock)
    {
        cachedQueryServiceMock
            .Setup(x => x.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<T?>>>(),
                It.IsAny<TimeSpan?>()))
            .Returns<string, Func<Task<T?>>, TimeSpan?>((key, fetch, expiry) => fetch());
    }
}