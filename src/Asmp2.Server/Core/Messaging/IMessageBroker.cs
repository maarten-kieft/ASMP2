namespace Asmp2.Server.Core.Messaging;

public interface IMessageBroker
{
    public void Publish(Message message);

    public void Subscribe<T>(Action<T> action) where T : Message;
}
