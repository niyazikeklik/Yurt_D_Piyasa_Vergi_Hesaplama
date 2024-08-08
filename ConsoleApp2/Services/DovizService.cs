using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp2.Services;
public static class DovizService
{
    public static async Task<decimal> GetDovizKuru(DateTime tarih)
    {
        tarih = tarih.AddDays(-1);

        if (tarih.DayOfWeek == DayOfWeek.Saturday)
        {
            tarih = tarih.AddDays(-1);
        }
        else if (tarih.DayOfWeek == DayOfWeek.Sunday)
        {
            tarih = tarih.AddDays(-2);
        }
        string yyyyMM = tarih.ToString("yyyyMM");
        string yyyyMMdd = tarih.ToString("ddMMyyyy");
        // URL
        var url = "https://www.tcmb.gov.tr/kurlar/" + yyyyMM + "/" + yyyyMMdd + ".xml";

        // HttpClient ile veriyi çek
        using var client = new HttpClient();
        var response = await client.GetStringAsync(url);

        // XML veriyi parse et
        var xmlDoc = XDocument.Parse(response);

        // USD bilgilerini al
        var usdElement = xmlDoc?.Root?
            .Element("Currency");

        if (usdElement != null && usdElement.Attribute("Kod")?.Value == "USD")
        {
            var forexBuying = usdElement.Element("ForexBuying")?.Value.ToString().Replace(".", ",");

            decimal alisKuru = 0;
            decimal.TryParse(forexBuying, out alisKuru);

            return alisKuru;
        }
        else
        {
            throw new Exception("USD bilgisi bulunamadı");
        }
    }
}
