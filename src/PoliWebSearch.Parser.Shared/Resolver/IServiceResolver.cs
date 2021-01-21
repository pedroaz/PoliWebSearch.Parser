
using Autofac;

namespace PoliWebSearch.Parser.Shared.Resolver
{
    public interface IServiceResolver
    {
        void Initialize(IContainer container);

        T ResolveService<T>();
    }
}
