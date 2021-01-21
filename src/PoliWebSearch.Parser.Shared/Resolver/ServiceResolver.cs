using Autofac;

namespace PoliWebSearch.Parser.Shared.Resolver
{
    public class ServiceResolver : IServiceResolver
    {
        private IContainer container;

        public void Initialize(IContainer container)
        {
            this.container = container;
        }

        public T ResolveService<T>()
        {
            var obj = container.Resolve<T>();
            return obj;
        }
    }
}
