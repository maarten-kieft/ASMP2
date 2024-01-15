namespace Asmp2.Server.Application.Messaging;

public abstract class Message
{
    public object Sender { get; }

    public Message(object sender)
    {
        Sender = sender;
    }
}
