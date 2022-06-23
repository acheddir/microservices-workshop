using MassTransit;

namespace OrderMgmt.API.Extensions.Services.EventBus;

public static class ProducersExtensions
{
    public static void RegisterProducers(this IRabbitMqBusFactoryConfigurator cfg)
    {
        // cfg.Message<ISendReportRequest>(e => e.SetEntityName("report-requests")); // name of the primary exchange
        // cfg.Publish<ISendReportRequest>(e => e.ExchangeType = ExchangeType.Fanout); // primary exchange type
    }
}