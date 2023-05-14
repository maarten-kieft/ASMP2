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
    private DateTimeOffset timestampStart = new DateTimeOffset(new DateTime(DateTime.Today.Year, 1, 1));
    private Period period = Period.Year;
    private string? selectedMeterId;
    private string formatTimestampPattern = "MMMM";
    private List<Statistic> statistics = new List<Statistic>();

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
        statistics = await Http.GetFromJsonAsync<List<Statistic>>(uri) ?? new List<Statistic>();
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

         timestampStart = ((Statistic)args.Data).TimestampStart;

        return LoadData();
    }
}
