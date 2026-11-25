using FlotixPlatform.Api.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace FlotixPlatform.Api.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}