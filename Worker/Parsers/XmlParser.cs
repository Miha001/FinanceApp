using Domain.Db.Entities;
using System.Globalization;
using System.Transactions;
using System.Xml.Linq;

namespace Worker.Parsers;
public class XmlParser
{
    public static IEnumerable<Currency> Parse(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        var currencies = new List<Currency>();

        foreach(var valute in doc.Descendants("Valute"))
        {
           // var charCode = valute.Element("CharCode")?.Value;
            var name = valute.Element("Name")?.Value;
            var vunitRateString = valute.Element("VunitRate")?.Value;

            if (//charCode is null ||
                name is null || vunitRateString is null)
            {
                continue;
            }

            if(float.TryParse(vunitRateString,
                NumberStyles.Currency,
                new CultureInfo("ru-RU"),
                out float rate))
            {
                currencies.Add(new Currency
                {
                    //charCode,
                    Name = name,
                    Rate = rate
                });
            }
        }

        return currencies;
    }
}