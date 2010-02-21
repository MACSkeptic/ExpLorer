namespace MACSkeptic.ExpLorer.Tests.Proxies.Interfaces
{
    public interface IConnections
    {
        string Database { get; set; }
        string Amqp { get; set; }
    }
}