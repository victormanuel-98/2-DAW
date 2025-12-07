using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using MauiAppDAW.Services;

namespace MauiAppDAW.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var username = txtUsername.Text?.Trim();
        var password = txtPassword.Text?.Trim();

        if (username == "User123" && password == "1234")
        {
            SearchChoicePage choicePage;

            if (App.ServiceProvider != null)
            {
                choicePage = App.ServiceProvider.GetService<SearchChoicePage>() ?? new SearchChoicePage();
            }
            else
            {
                choicePage = new SearchChoicePage();
            }

            await Navigation.PushAsync(choicePage);
        }
        else
        {
            lblMessage.Text = "Usuario o contraseña incorrectos.";
        }
    }

    private MainPage CreateFallbackMainPage()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://swapi.dev/api/") };
        var swapi = new SwapiService(httpClient, "https://swudb.com");
        var history = new SearchHistoryService();
        return new MainPage(swapi, history);
    }
}
