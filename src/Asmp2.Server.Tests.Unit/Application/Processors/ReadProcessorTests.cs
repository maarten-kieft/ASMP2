using Asmp2.Server.Application.IO;
using Asmp2.Server.Application.Messaging;
using Asmp2.Server.Application.Processors;
using Asmp2.Server.Application.Repositories;
using Moq;

namespace Asmp2.Server.Tests.Unit.Processors;

public class ReadProcessorTests
{
    private readonly Mock<ISerialReader> _serialReaderMock = new();
    private readonly Mock<IMessageBroker> _messageBrokerMock = new();
    private readonly Mock<IMeasurementRepository> _measurementRepository = new();
    private readonly Reader _sut;

    public ReadProcessorTests()
    {
        _sut = new Reader(
            _serialReaderMock.Object,
            _messageBrokerMock.Object,
            _measurementRepository.Object
        );
    }

    [Fact]
    public void Constructor_WhenNullProvidedForSerialReader_ThenArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new Reader(null!, _messageBrokerMock.Object, _measurementRepository.Object));

        exception.Message.Should().Contain("serialReader");
    }

    [Fact]
    public void Constructor_WhenNullProvidedForMessageBroker_ThenArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new Reader(_serialReaderMock.Object, null!, _measurementRepository.Object));

        exception.Message.Should().Contain("messageBroker");
    }

    [Fact]
    public void Constructor_WhenNullProvidedForMeasurementRepository_ThenArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new Reader(_serialReaderMock.Object, _messageBrokerMock.Object, null!));

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
