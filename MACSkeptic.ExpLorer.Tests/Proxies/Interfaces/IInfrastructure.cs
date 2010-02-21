namespace MACSkeptic.ExpLorer.Tests.Proxies.Interfaces
{
    public interface IInfrastructure
    {
        IConnections Connections { get; set; }
        IEmail Email { get; set; }
    }
}