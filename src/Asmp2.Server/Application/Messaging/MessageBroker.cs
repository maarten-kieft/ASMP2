using Asmp2.Server.Core.Messaging;

namespace Asmp2.Server.Application.Messaging;

public class MessageBroker : IMessageBroker
{
    private readonly Dictionary<Type, List<Action<Message>>> subscribers = new();

    public void Publish(Message message)
    {
        var messageType = message.GetType();

        if (!subscribers.ContainsKey(messageType))
        {
            return;
        }

        foreach (var subscriber in subscribers[messageType])
        {
            subscriber.Invoke(message);
        }
    }

    public void Subscribe<T>(Action<T> action) where T : Message
    {
        Subscribe(typeof(T), (Action<Message>)action);
    }

    public void Subscribe(Type messageType, Action<Message> action)
    {
        if(messageType.IsAssignableFrom(typeof(Message)))
        {
            throw new InvalidOperationException("Cannot subscribe to non message types");
        }

        if (!subscribers.ContainsKey(messageType))
        {
            subscribers.Add(messageType, new List<Action<Message>>());
        }

        subscribers[messageType].Add((Message message) =>
        {
            action(message);
        });
    }
}
