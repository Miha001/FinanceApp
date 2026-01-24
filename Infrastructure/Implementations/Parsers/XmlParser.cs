using Finances.Domain.Db.Entities;
using System.Globalization;
using System.Xml.Linq;

namespace Finances.DAL.Parsers;
public class XmlParser
{
    private const string ValuteElement = "Valute";
    private const string NameElement = "Name";
    private const string RateElement = "VunitRate";

    // Используем GetCultureInfo, оно кэширует экземпляр внутри .NET
    private static readonly CultureInfo RuCulture = CultureInfo.GetCultureInfo("ru-RU");

    public static List<Currency> Parse(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        var currencies = new List<Currency>();

        foreach(var valute in doc.Descendants(ValuteElement))
        {
            var name = valute.Element(NameElement)?.Value;
            var vunitRateString = valute.Element(RateElement)?.Value;

            if (
                name is null || vunitRateString is null)
            {
                continue;
            }

            if(decimal.TryParse(vunitRateString,
                NumberStyles.Currency,
                RuCulture,
                out decimal rate))
            {
                currencies.Add(new Currency
                {
                    Name = name,
                    Rate = rate
                });
            }
        }

        return currencies;
    }
}