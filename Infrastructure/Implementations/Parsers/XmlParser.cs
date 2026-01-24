using Finances.Domain.Db.Entities;
using System.Globalization;
using System.Xml.Linq;

namespace Finances.DAL.Parsers;
public class XmlParser
{
    public static List<Currency> Parse(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        var currencies = new List<Currency>();

        foreach(var valute in doc.Descendants("Valute"))
        {
            var name = valute.Element("Name")?.Value;
            var vunitRateString = valute.Element("VunitRate")?.Value;

            if (
                name is null || vunitRateString is null)
            {
                continue;
            }

            if(decimal.TryParse(vunitRateString,
                NumberStyles.Currency,
                new CultureInfo("ru-RU"),
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