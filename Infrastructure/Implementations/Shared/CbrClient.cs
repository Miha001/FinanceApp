using Finances.Application.Abstractions.Shared;
using Finances.DAL.Parsers;
using Finances.Domain.Db.Entities;
using System.Text;

namespace Finances.DAL.Implementations.Shared;

public class CbrClient : ICbrClient
{

    private readonly HttpClient _httpClient;

    public CbrClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Currency>> GetCurrencies(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync(string.Empty, ct); // URL пустой, т.к. BaseAddress задан в Program.cs
        response.EnsureSuccessStatusCode();

        var bytes = await response.Content.ReadAsByteArrayAsync(ct);
        var xmlContent = Encoding.GetEncoding("windows-1251").GetString(bytes);

        return XmlParser.Parse(xmlContent);
    }
}
