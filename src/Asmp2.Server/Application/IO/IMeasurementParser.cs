namespace Asmp2.Server.Application.IO;

public interface IMeasurementParser
{
    Measurement Parse(List<string> lines);
}
