using MauiAppDAW.Models;
using MauiAppDAW.Services;

namespace MauiAppDAW.Views;

public partial class HistoryPage : TabbedPage
{
    private readonly SearchHistoryService _historyService;

    public HistoryPage(SearchHistoryService historyService)
    {
        InitializeComponent();
        _historyService = historyService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAll();
    }

    private async Task LoadAll()
    {
        var chars = await _historyService.GetSavedCharactersAsync();
        var ships = await _historyService.GetSavedStarshipsAsync();
        var planets = await _historyService.GetSavedPlanetsAsync();

        // Ensure DisplayImageUrl set for each
        foreach (var c in chars) c.DisplayImageUrl ??= c.ExternalImageUrl ?? c.ImageUrl;
        foreach (var s in ships) s.DisplayImageUrl ??= s.ExternalImageUrl;
        foreach (var p in planets) p.DisplayImageUrl ??= p.ExternalImageUrl;

        cvCharacters.ItemsSource = chars;
        cvStarships.ItemsSource = ships;
        cvPlanets.ItemsSource = planets;
    }
}
