using Asmp2.Server.Core.Parsers;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Asmp2.Server.Application.Parsers;

public class MeasurementParser : IMeasurementParser
{
    private const string linePattern = @"^([^\(]*)(\([^\)]*\))*\(([^\)]*)\)$";

    public Measurement Parse(List<string> lines)
    {
        var measurement = new Measurement(
            Meter: ParseMeterDetails(lines),
            Timestamp: DateTime.Now,
            PowerUsage: ParsePowerUsage(lines),
            PowerSupply: ParsePowerSupply(lines),
            GasUsage: ParseGasUsage(lines)
        );

        return measurement;
    }

    private PowerReading ParsePowerUsage(List<string> lines)
    {
        var totalLow = ParseLine("1-0:1.8.1", lines);
        var totalRegular = ParseLine("1-0:1.8.2", lines);
        var current = ParseLine("1-0:1.7.0", lines);

        return new PowerReading(
            current,
            totalLow,
            totalRegular
        );
    }

    private PowerReading ParsePowerSupply(List<string> lines)
    {
        var totalLow = ParseLine("1-0:2.8.1", lines);
        var totalRegular = ParseLine("1-0:2.8.2", lines);
        var current = ParseLine("1-0:2.7.0", lines);

        return new PowerReading(
            current,
            totalLow,
            totalRegular
        );
    }

    private GasReading ParseGasUsage(List<string> lines)
    {
        var total = ParseLine("0-1:24.2.1", lines);

        return new GasReading(
            false,
            total
       );
    }

    private Meter ParseMeterDetails(List<string> lines)
    {
        var meterIdKeyPattern = "0-0:96.1.1";
        var line = lines.First(l => l.Contains(meterIdKeyPattern));
        var match = Regex.Match(line, linePattern);

        if (!match.Success)
        {
            throw new ParseMeasurementException($"Parsing the following line {line} failed");
        }

        var key = match.Groups[1].Value;
        var stringValue = match.Groups[3].Value;

        return new Meter(stringValue);
    }

    private decimal ParseLine(string keyPattern, List<string> lines)
    {
        var line = lines.First(l => l.Contains(keyPattern));
        var match = Regex.Match(line, linePattern);

        if (!match.Success)
        {
            throw new ParseMeasurementException($"Parsing the following line {line} failed");
        }

        var key = match.Groups[1].Value;
        var stringValue = match.Groups[3].Value;

        if (key != keyPattern)
        {
            throw new ParseMeasurementException($"The line '{line}' contains keyPattern '{keyPattern}' but the key is '{key}'");
        }

        return ParseLineValue(stringValue);
    }

    private static decimal ParseLineValue(string stringValue)
    {
        return decimal.Parse(
            stringValue
                .Replace("*kWh", string.Empty)
                .Replace("*kW", string.Empty)
                .Replace("*m3", string.Empty)
            ,
            CultureInfo.InvariantCulture
        );
    }
}
