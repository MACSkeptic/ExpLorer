namespace MACSkeptic.ExpLorer.Tests.Proxies.Interfaces
{
    public interface IConfiguration
    {
        IInfrastructure Infrastructure { get; }
        string Answer { get; }
        Configuration Surrogate { get; }
    }
}