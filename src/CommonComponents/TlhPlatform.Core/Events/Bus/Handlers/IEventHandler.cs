using TlhPlatform.Core.Reflection;

namespace TlhPlatform.Core.Event
{
    /// <summary>
    /// Undirect base interface for all event handlers.
    /// Implement <see cref="IEventHandler{TEventData}"/> instead of this one.
    /// </summary>
    [IgnoreDependency]
    public interface IEventHandler
    {
        
    }
}