namespace Asmp2.Server.Application.Messaging;

public class MeasurementMessage : Message
{
    public Measurement Measurement { get; set; }

    public MeasurementMessage(Measurement measurement, object sender) : base(sender)
    {
        Measurement = measurement;
    }
}
