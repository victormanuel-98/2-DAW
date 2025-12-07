using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using MauiAppDAW.Models;

namespace MauiAppDAW.Services;

public class SwapiService
{
    private readonly HttpClient _httpClient;
    private readonly string? _imageBaseUrl;
    private readonly string? _databankBaseUrl;

    // simple translation map for common terms
    private static readonly Dictionary<string,string> _translations = new(StringComparer.OrdinalIgnoreCase)
    {
        {"blond","rubio"},
        {"blonde","rubio"},
        {"brown","marron"},
        {"black","negro"},
        {"auburn","castano"},
        {"fair","claro"},
        {"n/a","n/a"},
        {"none","ninguno"},
        {"unknown","desconocido"}
    };

    // Expanded-universe fallback data
    private static readonly List<Character> _expandedCharacters = new()
    {
        new Character
        {
            Name = "Darth Nihilus",
            Height = "unknown",
            Mass = "unknown",
            HairColor = "none",
            SkinColor = "pale",
            EyeColor = "black",
            BirthYear = "unknown",
            Gender = "male",
            Url = "https://swapi.dev/api/people/9999/" // fake url for id parsing
        },
        new Character
        {
            Name = "Grand Admiral Thrawn",
            Height = "188",
            Mass = "unknown",
            HairColor = "blue",
            SkinColor = "pale",
            EyeColor = "red",
            BirthYear = "unknown",
            Gender = "male",
            Url = "https://swapi.dev/api/people/9998/"
        }
    };

    private static readonly List<Starship> _expandedStarships = new()
    {
        new Starship { Name = "Ebon Hawk", Model = "Freighter", Manufacturer = "Sith-era shipyards" },
        new Starship { Name = "TIE Defender (Expanded)", Model = "Defender", Manufacturer = "Sienar Fleet Systems" }
    };

    private static readonly List<Planet> _expandedPlanets = new()
    {
        new Planet { Name = "Ravager", Climate = "arid", Terrain = "wasteland" },
        new Planet { Name = "Korriban", Climate = "dry", Terrain = "desert" }
    };

    public SwapiService(HttpClient httpClient, string? imageBaseUrl = null, string? databankBaseUrl = null)
    {
        _httpClient = httpClient;
        _imageBaseUrl = imageBaseUrl?.TrimEnd('/');
        _databankBaseUrl = databankBaseUrl?.TrimEnd('/');
    }

    private static string Translate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input ?? string.Empty;
        // try simple translation per word
        var parts = input.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
        for (int i=0;i<parts.Length;i++)
        {
            var p = parts[i].Trim();
            if (_translations.TryGetValue(p, out var t)) parts[i] = t;
        }
        return string.Join(' ', parts);
    }

    // Modify GetCharacterAsync to translate some fields
    public async Task<Character?> GetCharacterAsync(string name, CancellationToken cancellationToken = default)
    {
        var character = await GetCharacterInternalAsync(name, cancellationToken);
        if (character != null)
        {
            // translate hair/skin/eye colors
            character.HairColor = Translate(character.HairColor);
            character.SkinColor = Translate(character.SkinColor);
            character.EyeColor = Translate(character.EyeColor);
        }
        return character;
    }

    private async Task<Character?> GetCharacterInternalAsync(string name, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<SwapiResponse<Character>>($"people/?search={Uri.EscapeDataString(name)}", cancellationToken);
            var result = response?.Results?.FirstOrDefault();

            if (result != null)
            {
                ApplyImageUrl(result, "characters");
                result.DisplayImageUrl = await ChooseBestImageAsync(result, cancellationToken);
                System.Diagnostics.Debug.WriteLine($"Character image for {result.Name}: {result.DisplayImageUrl}");
                return result;
            }

            // Fallback to expanded-universe list (case-insensitive contains)
            var fallback = _expandedCharacters.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (fallback != null)
            {
                ApplyImageUrl(fallback, "characters");
                fallback.DisplayImageUrl = await ChooseBestImageAsync(fallback, cancellationToken);
                System.Diagnostics.Debug.WriteLine($"Fallback character image for {fallback.Name}: {fallback.DisplayImageUrl}");
            }
            return fallback;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            // On error, try fallback
            var fallback = _expandedCharacters.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (fallback != null)
            {
                ApplyImageUrl(fallback, "characters");
                fallback.DisplayImageUrl = await ChooseBestImageAsync(fallback, cancellationToken);
                System.Diagnostics.Debug.WriteLine($"Fallback character image (on error) for {fallback.Name}: {fallback.DisplayImageUrl}");
            }
            return fallback;
        }
    }

    // Enrich starship: resolve pilot -> owner and owner's homeworld
    public async Task<List<Starship>?> GetStarshipsAsync(string name, CancellationToken cancellationToken = default)
    {
        var results = await GetStarshipsInternalAsync(name, cancellationToken);
        if (results != null)
        {
            foreach (var s in results)
            {
                // if pilots available, resolve first pilot as Owner and its homeworld as World
                if (s.Pilots != null && s.Pilots.Count > 0)
                {
                    try
                    {
                        var pilotUrl = s.Pilots[0];
                        var pilot = await FetchResourceAsync<Character>(pilotUrl, cancellationToken);
                        s.Owner = pilot?.Name;
                        if (pilot != null && !string.IsNullOrWhiteSpace(pilot.Url))
                        {
                            var homeworldUrl = pilotUrl.Replace("people/","people/"); // placeholder
                            if (!string.IsNullOrWhiteSpace(pilot.Url))
                            {
                                var hw = await FetchResourceAsync<Planet>(pilot.Url.Contains("/people/") ? pilot.Url.Replace("people/","planets/") : pilot.Url, cancellationToken);
                                s.World = hw?.Name;
                            }
                        }
                    }
                    catch
                    {
                        // ignore resolution errors
                    }
                }
            }
        }
        return results;
    }

    private async Task<List<Starship>?> GetStarshipsInternalAsync(string name, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<SwapiResponse<Starship>>($"starships/?search={Uri.EscapeDataString(name)}", cancellationToken);
            var results = response?.Results;
            if (results != null && results.Count > 0)
            {
                foreach (var r in results)
                {
                    ApplyImageUrl(r, "starships");
                    r.DisplayImageUrl = await ChooseBestImageAsync(r, cancellationToken);
                    System.Diagnostics.Debug.WriteLine($"Starship image for {r.Name}: {r.DisplayImageUrl}");
                }
                return results;
            }

            // Fallback: return expanded starships matching name
            var fallback = _expandedStarships.Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (fallback.Count > 0)
            {
                foreach (var f in fallback)
                {
                    ApplyImageUrl(f, "starships");
                    f.DisplayImageUrl = await ChooseBestImageAsync(f, cancellationToken);
                    System.Diagnostics.Debug.WriteLine($"Fallback starship image for {f.Name}: {f.DisplayImageUrl}");
                }
                return fallback;
            }

            return null;
        }
        catch
        {
            var fallback = _expandedStarships.Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (fallback.Count > 0)
            {
                foreach (var f in fallback)
                {
                    ApplyImageUrl(f, "starships");
                    f.DisplayImageUrl = await ChooseBestImageAsync(f, cancellationToken);
                    System.Diagnostics.Debug.WriteLine($"Fallback starship image (on error) for {f.Name}: {f.DisplayImageUrl}");
                }
                return fallback;
            }
            return null;
        }
    }

    // Enrich planets: fetch residents and set FamousResidents
    public async Task<List<Planet>?> GetPlanetsAsync(string name, CancellationToken cancellationToken = default)
    {
        var results = await GetPlanetsInternalAsync(name, cancellationToken);
        if (results != null)
        {
            foreach (var p in results)
            {
                if (p.Residents != null && p.Residents.Count > 0)
                {
                    var names = new List<string>();
                    foreach (var url in p.Residents.Take(5))
                    {
                        try
                        {
                            var res = await FetchResourceAsync<Character>(url, cancellationToken);
                            if (res != null) names.Add(res.Name);
                        }
                        catch { }
                    }
                    p.FamousResidents = names;
                }
            }
        }
        return results;
    }

    private async Task<List<Planet>?> GetPlanetsInternalAsync(string name, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<SwapiResponse<Planet>>($"planets/?search={Uri.EscapeDataString(name)}", cancellationToken);
            var results = response?.Results;
            if (results != null && results.Count > 0)
            {
                foreach (var r in results)
                {
                    ApplyImageUrl(r, "planets");
                    r.DisplayImageUrl = await ChooseBestImageAsync(r, cancellationToken);
                    System.Diagnostics.Debug.WriteLine($"Planet image for {r.Name}: {r.DisplayImageUrl}");
                }
                return results;
            }

            var fallback = _expandedPlanets.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (fallback.Count > 0)
            {
                foreach (var f in fallback)
                {
                    ApplyImageUrl(f, "planets");
                    f.DisplayImageUrl = await ChooseBestImageAsync(f, cancellationToken);
                    System.Diagnostics.Debug.WriteLine($"Fallback planet image for {f.Name}: {f.DisplayImageUrl}");
                }
                return fallback;
            }

            return null;
        }
        catch
        {
            var fallback = _expandedPlanets.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (fallback.Count > 0)
            {
                foreach (var f in fallback)
                {
                    ApplyImageUrl(f, "planets");
                    f.DisplayImageUrl = await ChooseBestImageAsync(f, cancellationToken);
                    System.Diagnostics.Debug.WriteLine($"Fallback planet image (on error) for {f.Name}: {f.DisplayImageUrl}");
                }
                return fallback;
            }
            return null;
        }
    }

    private async Task<T?> FetchResourceAsync<T>(string url, CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            // Ensure absolute URL
            var u = url.StartsWith("http") ? url : new Uri(_httpClient.BaseAddress!, url).ToString();
            var resp = await _httpClient.GetFromJsonAsync<T>(u, cancellationToken);
            return resp;
        }
        catch
        {
            return null;
        }
    }

    private async Task<string?> ChooseBestImageAsync(object item, CancellationToken cancellationToken)
    {
        // Prefer databank images, then ExternalImageUrl (swudb), then ImageUrl (visualguide)
        if (!string.IsNullOrWhiteSpace(_databankBaseUrl))
        {
            var databankCandidates = BuildDatabankCandidates(item);
            foreach (var cand in databankCandidates)
            {
                if (await UrlExistsAsync(cand, cancellationToken))
                    return cand;
            }
        }

        string? external = item switch
        {
            Character c => c.ExternalImageUrl,
            Starship s => s.ExternalImageUrl,
            Planet p => p.ExternalImageUrl,
            _ => null
        };

        string? visual = item switch
        {
            Character c => c.ImageUrl,
            Starship _ => null,
            Planet _ => null,
            _ => null
        };

        if (!string.IsNullOrWhiteSpace(external))
        {
            if (await UrlExistsAsync(external, cancellationToken))
                return external;
        }

        if (!string.IsNullOrWhiteSpace(visual))
        {
            if (await UrlExistsAsync(visual, cancellationToken))
                return visual;
        }

        return null;
    }

    private IEnumerable<string> BuildDatabankCandidates(object item)
    {
        var name = item switch
        {
            Character c => c.Name,
            Starship s => s.Name,
            Planet p => p.Name,
            _ => string.Empty
        } ?? string.Empty;

        var slug = Slugify(name);
        var list = new List<string>();

        // Common candidate patterns for the databank site
        if (!string.IsNullOrWhiteSpace(_databankBaseUrl))
        {
            list.Add($"{_databankBaseUrl}/images/{slug}.jpg");
            list.Add($"{_databankBaseUrl}/images/{slug}.png");
            list.Add($"{_databankBaseUrl}/{slug}.jpg");
            list.Add($"{_databankBaseUrl}/{slug}.png");
            list.Add($"{_databankBaseUrl}/characters/{slug}.jpg");
            list.Add($"{_databankBaseUrl}/characters/{slug}.png");
            list.Add($"{_databankBaseUrl}/starships/{slug}.jpg");
            list.Add($"{_databankBaseUrl}/starships/{slug}.png");
            list.Add($"{_databankBaseUrl}/planets/{slug}.jpg");
            list.Add($"{_databankBaseUrl}/planets/{slug}.png");
        }

        return list.Distinct();
    }

    private async Task<bool> UrlExistsAsync(string url, CancellationToken cancellationToken)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"UrlExistsAsync: checking {url}");
            using var req = new HttpRequestMessage(HttpMethod.Head, url);
            using var resp = await _httpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine($"UrlExistsAsync: HEAD {url} -> {(int)resp.StatusCode} {resp.ReasonPhrase}");
            if (resp.IsSuccessStatusCode) return true;

            // some servers don't support HEAD
            req.Method = HttpMethod.Get;
            req.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(0, 0);
            using var resp2 = await _httpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine($"UrlExistsAsync: GET {url} -> {(int)resp2.StatusCode} {resp2.ReasonPhrase}");
            return resp2.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UrlExistsAsync: error checking {url} -> {ex.GetType().Name}: {ex.Message}");
            return false;
        }
    }

    private void ApplyImageUrl(object item, string type)
    {
        // set ExternalImageUrl from swudb-style imageBaseUrl
        if (!string.IsNullOrWhiteSpace(_imageBaseUrl))
        {
            string name = item switch
            {
                Character c => c.Name,
                Starship s => s.Name,
                Planet p => p.Name,
                _ => null
            } ?? string.Empty;

            var slug = Slugify(name);
            var imageUrl = $"{_imageBaseUrl}/{type}/{slug}.jpg";

            switch (item)
            {
                case Character c:
                    c.ExternalImageUrl = imageUrl;
                    break;
                case Starship s:
                    s.ExternalImageUrl = imageUrl;
                    break;
                case Planet p:
                    p.ExternalImageUrl = imageUrl;
                    break;
            }
        }

        // Also set ExternalImageUrl from databank if available (kept in same property)
        if (!string.IsNullOrWhiteSpace(_databankBaseUrl))
        {
            string name = item switch
            {
                Character c => c.Name,
                Starship s => s.Name,
                Planet p => p.Name,
                _ => null
            } ?? string.Empty;

            var slug = Slugify(name);
            var imageUrlDb = $"{_databankBaseUrl}/images/{slug}.jpg";

            switch (item)
            {
                case Character c:
                    c.ExternalImageUrl ??= imageUrlDb;
                    break;
                case Starship s:
                    s.ExternalImageUrl ??= imageUrlDb;
                    break;
                case Planet p:
                    p.ExternalImageUrl ??= imageUrlDb;
                    break;
            }
        }
    }

    private static string Slugify(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Normalize and remove diacritics
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var ch in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        }
        var cleaned = sb.ToString().Normalize(NormalizationForm.FormC);

        // To lower, replace spaces with hyphens, remove invalid chars
        cleaned = cleaned.ToLowerInvariant();
        cleaned = Regex.Replace(cleaned, "\\s+", "-");
        cleaned = Regex.Replace(cleaned, "[^a-z0-9-_]", string.Empty);
        cleaned = Regex.Replace(cleaned, "-+", "-");
        return cleaned.Trim('-');
    }

    private class SwapiResponse<T>
    {
        public List<T>? Results { get; set; }
    }
}
