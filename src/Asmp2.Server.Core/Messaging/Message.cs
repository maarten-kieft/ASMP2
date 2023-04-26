namespace Asmp2.Server.Core.Messaging;

public abstract class Message
{
    public object Sender { get; }

    public Message(object sender)
    {
        Sender = sender;
    }
}
