using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiAppDAW.Behaviors;

public class ImageUrlBehavior : Behavior<Image>
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(8)
    };

    public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create(
        nameof(ImageUrl),
        typeof(string),
        typeof(ImageUrlBehavior),
        default(string),
        propertyChanged: OnImageUrlChanged);

    public string? ImageUrl
    {
        get => (string?)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    private Image? _associatedImage;
    private CancellationTokenSource? _cts;

    protected override void OnAttachedTo(Image bindable)
    {
        base.OnAttachedTo(bindable);
        _associatedImage = bindable;
    }

    protected override void OnDetachingFrom(Image bindable)
    {
        base.OnDetachingFrom(bindable);
        _cts?.Cancel();
        _associatedImage = null;
    }

    private static void OnImageUrlChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ImageUrlBehavior behavior)
        {
            behavior.UpdateImageSourceAsync(newValue as string);
        }
    }

    private async void UpdateImageSourceAsync(string? url)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
        var ct = _cts.Token;

        try
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                SetPlaceholder();
                return;
            }

            // Try HEAD first
            using var request = new HttpRequestMessage(HttpMethod.Head, url);
            using var response = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                SetImage(url);
                return;
            }

            // Some servers do not support HEAD; try GET with Range header to avoid full download
            request.Method = HttpMethod.Get;
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(0, 0);
            using var response2 = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);
            if (response2.IsSuccessStatusCode)
            {
                SetImage(url);
                return;
            }

            SetPlaceholder();
        }
        catch
        {
            SetPlaceholder();
        }
    }

    private void SetImage(string url)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_associatedImage != null)
                _associatedImage.Source = ImageSource.FromUri(new Uri(url));
        });
    }

    private void SetPlaceholder()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_associatedImage != null)
                _associatedImage.Source = ImageSource.FromUri(new Uri("https://via.placeholder.com/70"));
        });
    }
}
