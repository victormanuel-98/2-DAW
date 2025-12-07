using MauiAppDAW.Models;

namespace MauiAppDAW.Views;

public partial class PersonDetailPage : ContentPage
{
    public PersonDetailPage(Character character)
    {
        InitializeComponent();
        BindingContext = character;
    }
}
