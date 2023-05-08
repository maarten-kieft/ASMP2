using Asmp2.Shared.Model;
using Radzen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Asmp2.Client.Shared;

public partial class Overview
{
    private DateTimeOffset timestampStart = new DateTimeOffset(new DateTime(DateTime.Today.Year, 1, 1));
    private Period period = Period.Year;
    private string formatTimestampPattern = "MMMM";
    private List<Statistic> statistics = new List<Statistic>();

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        var uri = $"/statistic/{period}/{timestampStart.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
        statistics = await Http.GetFromJsonAsync<List<Statistic>>(uri) ?? new List<Statistic>();
    }

    private Task HandleSeriesClick(SeriesClickEventArgs args)
    {
        if (period == Period.Year)
        {
            period = Period.Month;
        }

        if(period == Period.Month)
        {
            period = Period.Day;
        }

         timestampStart = ((Statistic)args.Data).TimestampStart;

        return LoadData();
    }
}
