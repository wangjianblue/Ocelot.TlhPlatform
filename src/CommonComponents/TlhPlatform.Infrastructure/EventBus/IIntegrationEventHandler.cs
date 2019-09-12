namespace TlhPlatform.Infrastructure.EventBus
{
    public interface IIntegrationEventHandler<T> where T : IntegrationEvent
    {
    }
}