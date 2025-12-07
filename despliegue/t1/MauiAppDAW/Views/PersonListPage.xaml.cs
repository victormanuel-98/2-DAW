using MauiAppDAW.Models;

namespace MauiAppDAW.Views;

public partial class PersonListPage : ContentPage
{
    public PersonListPage(List<Character> characters)
    {
        InitializeComponent();
        cvCharacters.ItemsSource = characters;
    }
}
