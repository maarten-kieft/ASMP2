using Asmp2.Server.Application.IO;
using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Core.Messaging;
using Asmp2.Server.Core.Processors;
using Moq;
using Xunit;

namespace Asmp2.Server.Tests.Unit.Processors;

public class ReadProcessorTests
{
    private readonly Mock<ISerialReader> _serialReaderMock = new();
    private readonly Mock<IMessageBroker> _messageBrokerMock = new();
    private readonly Mock<IMeasurementRepository> _measurementRepository = new();
    private readonly ReadProcessor _sut;

    public ReadProcessorTests()
    {
        _sut = new ReadProcessor(
            _serialReaderMock.Object,
            _messageBrokerMock.Object,
            _measurementRepository.Object
        );
    }

    [Fact]
    public void Constructor_WhenNullProvidedForSerialReader_ThenArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ReadProcessor(null!, _messageBrokerMock.Object, _measurementRepository.Object));

        exception.Message.Should().Contain("serialReader");
    }

    [Fact]
    public void Constructor_WhenNullProvidedForMessageBroker_ThenArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ReadProcessor(_serialReaderMock.Object, null!, _measurementRepository.Object));

        exception.Message.Should().Contain("messageBroker");
    }

    [Fact]
    public void Constructor_WhenNullProvidedForMeasurementRepository_ThenArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ReadProcessor(_serialReaderMock.Object, _messageBrokerMock.Object, null!));

        exception.Message.Should().Contain("measurementRepository");
    }

    [Fact]
    public async Task RunAsync_WhenCalled_ThenSubscribe()
    {
        await _sut.RunAsync(CancellationToken.None);

        _serialReaderMock.Verify(s => s.Subscribe(It.IsAny<Func<Measurement, Task>>()), Times.Once);
    }

    [Fact]
    public async Task RunAsync_WhenCalled_ThenCallSerialReaderRunAsync()
    {
        await _sut.RunAsync(CancellationToken.None);

        _serialReaderMock.Verify(s => s.RunAsync(It.Is<CancellationToken>(t => t == CancellationToken.None)), Times.Once);
    }
}
