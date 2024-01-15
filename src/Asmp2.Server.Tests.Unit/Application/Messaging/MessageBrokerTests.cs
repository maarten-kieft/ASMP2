using Asmp2.Server.Application.Messaging;

namespace Asmp2.Server.Tests.Unit.Application.Messaging;

public class MessageBrokerTests
{
    private readonly MessageBroker _sut = new();

    [Fact]
    public void Subscribe_WhenSubscribed_ThenShouldReceiveMessage()
    {
        TestMessage1 Input = new(this);
        TestMessage1 output = null!;

        _sut.Subscribe<TestMessage1>(m => output = m);
        _sut.Publish(Input);

        output.Should().Be(Input);
    }

    [Fact]
    public void Subscribe_WhenSubscribedTwice_ThenShouldReceiveMessageTwice()
    {
        TestMessage1 Input = new(this);
        TestMessage1 output1 = null!;
        TestMessage1 output2 = null!;

        _sut.Subscribe<TestMessage1>(m => output1 = m);
        _sut.Subscribe<TestMessage1>(m => output2 = m);
        _sut.Publish(Input);

        output1.Should().Be(Input);
        output2.Should().Be(Input);
    }

    [Fact]
    public void Subscribe_WhenSubscribedToDifferentMessageType_ThenShouldNotReiveMessage()
    {
        TestMessage2 Input = new(this);
        TestMessage1 output1 = null!;

        _sut.Subscribe<TestMessage1>(m => output1 = m);
        _sut.Publish(Input);

        output1.Should().BeNull();
    }

    class TestMessage1 : Message
    {
        public TestMessage1(object sender) : base(sender)
        {
        }
    }

    class TestMessage2 : Message
    {
        public TestMessage2(object sender) : base(sender)
        {
        }
    }
}
