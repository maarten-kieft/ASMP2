using Asmp2.Client.Model;
using Asmp2.Shared.Model;
using Radzen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Asmp2.Client.Shared;

public partial class Overview
{
    private string? selectedMeterId;
    private DateTimeOffset timestampStart = new DateTimeOffset(new DateTime(DateTime.Today.Year, 1, 1));
    private Period period = Period.Year;
    private string formatTimestampPattern = "MMMM";
    private List<StatisticDataPoint> usageData = new List<StatisticDataPoint>();
    private List<StatisticDataPoint> supplyData = new List<StatisticDataPoint>();

    protected override async Task OnInitializedAsync()
    {
        var meters = await Http.GetFromJsonAsync<List<Meter>>("/meter/all");
        selectedMeterId = meters!.First(m => !m.Id.Contains("fake")).Id;

        await LoadData();
    }

    private async Task LoadData()
    {
        if(selectedMeterId == null)
        {
            return;
        }

        var uri = $"/statistic/{selectedMeterId}/{period}/{timestampStart.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
        var statistics = await Http.GetFromJsonAsync<List<Statistic>>(uri) ?? new List<Statistic>();
        
        usageData = statistics.Select(s =>
                new StatisticDataPoint
                {
                    Timestamp = s.TimestampStart.LocalDateTime,
                    Value = s.PowerUsage.Total
                }
               ).ToList();
        
        supplyData = statistics.Select(s =>
                new StatisticDataPoint
                {
                    Timestamp = s.TimestampStart.LocalDateTime,
                    Value = -s.PowerSupply.Total
                }
               ).ToList();

    }

    private Task HandleSeriesClick(SeriesClickEventArgs args)
    {
        if (period == Period.Month)
        {
            period = Period.Day;
            formatTimestampPattern = "HH:mm";
        }

        if (period == Period.Year)
        {
            period = Period.Month;
            formatTimestampPattern = "dd-MM";
        }

         timestampStart = ((StatisticDataPoint)args.Data).Timestamp;

        return LoadData();
    }
}
