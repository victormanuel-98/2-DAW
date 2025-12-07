using System;
using Microsoft.Maui.Controls;
using MauiAppDAW.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppDAW.Views;

public partial class PlanetsPage : ContentPage
{
    private readonly SwapiService? _swapiService;
    private readonly SearchHistoryService? _historyService;

    public PlanetsPage(SwapiService? swapiService, SearchHistoryService? historyService)
    {
        InitializeComponent();
        _swapiService = swapiService ?? App.ServiceProvider?.GetService<SwapiService>();
        _historyService = historyService ?? App.ServiceProvider?.GetService<SearchHistoryService>();
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        var name = txtSearch.Text?.Trim();
        if (string.IsNullOrEmpty(name))
            return;

        var results = _swapiService != null ? await _swapiService.GetPlanetsAsync(name) : null;
        cvResults.ItemsSource = results ?? new System.Collections.Generic.List<Models.Planet>();

        if (results != null && results.Count > 0 && _historyService != null)
        {
            await _historyService.SavePlanetsAsync(results);
        }
    }

    private async void OnSavedClicked(object sender, EventArgs e)
    {
        if (_historyService == null)
            return;

        var saved = await _historyService.GetSavedPlanetsAsync();
        cvResults.ItemsSource = saved;
    }
}
