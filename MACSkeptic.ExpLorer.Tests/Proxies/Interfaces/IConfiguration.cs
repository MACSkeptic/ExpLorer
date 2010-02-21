namespace MACSkeptic.ExpLorer.Tests.Proxies.Interfaces
{
    public interface IConfiguration
    {
        IInfrastructure Infrastructure { get; set; }
        string Answer { get; set; }
    }
}