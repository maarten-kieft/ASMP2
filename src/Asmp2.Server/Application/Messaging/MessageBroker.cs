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
        if (!subscribers.ContainsKey(typeof(T)))
        {
            subscribers.Add(typeof(T), new List<Action<Message>>());
        }

        subscribers[typeof(T)].Add((Message message) =>
        {
            action((T)message);
        });
    }
}
