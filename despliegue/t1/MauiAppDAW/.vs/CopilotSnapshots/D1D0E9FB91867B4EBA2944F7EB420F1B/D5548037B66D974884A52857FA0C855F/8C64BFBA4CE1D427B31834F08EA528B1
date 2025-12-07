using MauiAppDAW.Models;
using MauiAppDAW.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppDAW.Views;

public partial class MainPage : ContentPage
{
    private readonly SwapiService _swapiService;
    private readonly SearchHistoryService _historyService;

    public MainPage(SwapiService swapiService, SearchHistoryService historyService)
    {
        InitializeComponent();
        _swapiService = swapiService;
        _historyService = historyService;
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        var name = txtName.Text?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "Escribe un nombre", "OK");
            return;
        }

        lblResult.Text = $"Buscando: {name}...";

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25));

        try
        {
            var character = await _swapiService.GetCharacterAsync(name, cts.Token);

            if (character != null)
            {
                lblResult.Text = $"Nombre: {character.Name}\n" +
                                 $"Altura: {character.Height}\n" +
                                 $"Masa: {character.Mass}\n" +
                                 $"Color de pelo: {character.HairColor}\n" +
                                 $"Color de piel: {character.SkinColor}\n" +
                                 $"Color de ojos: {character.EyeColor}\n" +
                                 $"Año de nacimiento: {character.BirthYear}\n" +
                                 $"Género: {character.Gender}";

                // save to history
                await _historyService.SaveCharactersAsync(new[] { character });
            }
            else
            {
                lblResult.Text = "No se encontró ningún personaje.";
            }
        }
        catch (OperationCanceledException)
        {
            lblResult.Text = "Error: Tiempo de espera superado (timeout). Comprueba la conexión del emulador o aumenta el timeout.";
        }
        catch (HttpRequestException hre)
        {
            lblResult.Text = $"Error de conexión: {hre.Message}";
        }
        catch (Exception ex)
        {
            lblResult.Text = $"Error: {ex.Message}";
        }
    }

    private async void OnSavedClicked(object sender, EventArgs e)
    {
        var saved = await _historyService.GetSavedCharactersAsync();
        var page = new PersonListPage(saved);
        await Navigation.PushAsync(page);
    }
}
