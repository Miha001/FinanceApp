using Finances.Domain.Db.Entities;
using Finances.Domain.Models.Dto;

namespace Finances.Application.Extensions;

public static class CurrencyMappingExtensions
{
    public static CourseDto ToDto(this Currency currency)
    {
        if (currency is null)
        {
            throw new ArgumentNullException(nameof(currency));
        }

        return new CourseDto(currency.Id, currency.Name, currency.Rate);
    }

    public static List<CourseDto> ToDto(this IEnumerable<Currency> currencies)
    {
        if (currencies is null)
        {
            return new List<CourseDto>();
        }

        return currencies.Select(c => c.ToDto()).ToList();
    }
}