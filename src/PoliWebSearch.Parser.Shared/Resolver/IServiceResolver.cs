using Autofac;

namespace PoliWebSearch.Parser.Shared.Resolver
{
    /// <summary>
    /// Service resolve whcih can be used the get instnace of interfaces
    /// </summary>
    public interface IServiceResolver
    {
        /// <summary>
        /// Initialize the service resolver with a contianer
        /// </summary>
        /// <param name="container"></param>
        void Initialize(IContainer container);

        /// <summary>
        /// Get a instance of certain service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ResolveService<T>();
    }
}
