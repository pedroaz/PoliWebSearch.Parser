using Autofac;

namespace PoliWebSearch.Parser.Infra.Resolver
{
    /// <summary>
    /// Implementation of IServiceResolver
    /// </summary>
    public class ServiceResolver : IServiceResolver
    {
        private IContainer container;

        // <inheritdoc/>
        public void Initialize(IContainer container)
        {
            this.container = container;
        }

        // <inheritdoc/>
        public T ResolveService<T>()
        {
            var obj = container.Resolve<T>();
            return obj;
        }
    }
}
