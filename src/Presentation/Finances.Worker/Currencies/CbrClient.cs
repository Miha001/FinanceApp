using Finances.Application.Abstractions.Shared;
using Finances.DAL.Parsers;
using Finances.Domain.Db.Entities;
using Finances.Worker.Abstractions;
using System.Text;

namespace Finances.DAL.Implementations.Shared;

public class CbrClient : ICbrClient
{
    private static readonly Encoding _windows1251 = Encoding.GetEncoding("windows-1251");
    private readonly HttpClient _httpClient;

    public CbrClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Currency>> GetCurrencies(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync(string.Empty, ct); // URL пустой, т.к. BaseAddress задан в Program.cs
        response.EnsureSuccessStatusCode();

        if (response.RequestMessage?.RequestUri?.ToString().Contains("/Error/") == true)
        {
            throw new InvalidOperationException($"Invalid CBR URL. Redirected to: {response.RequestMessage.RequestUri}");
        }

        if (response.Content.Headers.ContentType?.MediaType == "text/html")
        {
            var htmlContent = await response.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException($"Received HTML instead of XML. Check your URL configuration. Content preview: {htmlContent[..100]}");
        }

        var bytes = await response.Content.ReadAsByteArrayAsync(ct);
        var xmlContent = _windows1251.GetString(bytes);

        return XmlParser.Parse(xmlContent);
    }
}
