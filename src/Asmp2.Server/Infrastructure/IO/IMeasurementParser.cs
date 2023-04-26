namespace Asmp2.Server.Core.Parsers;

public interface IMeasurementParser
{
    Measurement Parse(List<string> lines);
}
