using Microsoft.Extensions.DependencyInjection;

public class LazyResolver<T>(IServiceProvider serviceProvider) : Lazy<T>(() => serviceProvider.GetRequiredService<T>())
{
}
