using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2;

using HtmlAgilityPack;
/******************************************************************************

                            Online C# Compiler.
                Code, Compile, Run and Debug C# program online.
Write your code in this editor and press "Run" button to execute it.

*******************************************************************************/

using System;
using System.Globalization;
using System.Xml.Linq;

public class Hisse
{
    public string Sembol { get; set; }
    public List<Alis> Alislar { get; set; } = new List<Alis>();
    public List<Satis> Satislar { get; set; } = new List<Satis>();
}

public class Alis
{
    public DateTime Tarih { get; set; }
    public decimal Fiyat { get; set; }
    public int Adet { get; set; }
    public decimal ToplamTutar => Fiyat * Adet;
    public decimal Kur => HelloWorld.GetDovizKuru(Tarih).Result;
    public decimal Ufe => HelloWorld.GetValueForYearAndMonth(Tarih.Year, Tarih.Month);
    public decimal FiyatTL => Fiyat * Kur;
    public decimal ToplamTutarTL => Fiyat * Adet * Kur;
}

public class Satis
{
    public DateTime Tarih { get; set; }
    public decimal Fiyat { get; set; }
    public int Adet { get; set; }
    public decimal Kur => HelloWorld.GetDovizKuru(Tarih).Result;
    public decimal Ufe => HelloWorld.GetValueForYearAndMonth(Tarih.Year, Tarih.Month);
    public decimal ToplamTutar => Fiyat * Adet;
    public decimal FiyatTL => Fiyat * Kur;
    public decimal ToplamTutarTL => Fiyat * Adet * Kur;

}

public class KarZarar
{
    public Hisse Hisse { get; set; }
    public DateTime Tarih { get; set; }
    public decimal KarZararTutar { get; set; }

}

public static class HelloWorld
{
    private static string? _yufeHtml = null;
    public static string YufeHtml
    {
        get
        {
            if (_yufeHtml == null)
            {
                _yufeHtml = GetYufeHtml().Result;
            }
            return _yufeHtml;
        }
    }
    private static async Task<string> GetYufeHtml()
    {
        using (var client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response =  await client.GetAsync("https://www.hakedis.org/endeksler/yi-ufe-yurtici-uretici-fiyat-endeksi");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }

        }
    }

    public static decimal GetValueForYearAndMonth(int year, int month)
    {
        if(month == 1)
        {
            month = 12;
            year--;
        }
        else
        {
            month--;
        }

        var htmlContent = HelloWorld.YufeHtml;

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);

        string AyAdi = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

        // Tabloyu bul
        var table = htmlDocument.DocumentNode.SelectSingleNode("//table");
        if (table == null)
        {
            throw new Exception("Table not found in the HTML content.");
        }

        // Tablo satırlarını seç
        var rows = table.SelectNodes(".//tr");
        if (rows == null || rows.Count < 2)
        {
            throw new Exception("Table rows are missing or insufficient.");
        }

        // Başlık satırını al
        var headerCells = rows[0].SelectNodes(".//th");
        if (headerCells == null)
        {
            throw new Exception("Table headers are missing.");
        }

        // Yıl ve ay sütunlarını bul
        int yilRowIndex = -1;
        int ayColumnIndex = -1;

        //for all rows
        for (int i = 0; i < headerCells.Count; i++)
        {
            var cell = headerCells[i];
            if (cell.InnerText.ToUpper().Contains(AyAdi.ToString().ToUpper()))
            {
                ayColumnIndex = i;
            }
        }

        if (ayColumnIndex == -1)
        {
            throw new Exception("Column for the month is missing.");
        }

        for (int i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            if (row.InnerText.Contains(year.ToString()))
            {
                yilRowIndex = i;
            }
        }

        if (yilRowIndex == -1)
        {
            throw new Exception("Row for the year is missing.");
        }



        string result = rows[yilRowIndex].SelectNodes(".//td")[ayColumnIndex].InnerText;

        decimal.TryParse(result, out decimal ufe);

        return ufe;
    }
    public static async Task<decimal> GetDovizKuru(DateTime tarih)
    {
        tarih = tarih.AddDays(-1);
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
        var usdElement = xmlDoc.Root
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
    private static void CalculateTax(Hisse hisse)
    {
        const decimal ufeLimit = 10;

        static decimal UfeHesapla(decimal alis, decimal satis)
        {
            return (satis - alis) / alis * 100;
        }

        static decimal MaliyetHesapla(decimal kur, decimal adet, decimal fiyat)
        {
            return fiyat * kur * adet;
        }

        static KarZarar HesaplaKarZarar(Hisse hisse, Satis satis, Alis alis, decimal ufe, bool isPartial)
        {
            decimal karZararTutar;

            if (ufe >= ufeLimit)
            {
                karZararTutar = MaliyetHesapla(satis.Kur, alis.Adet, satis.Fiyat) -
                                MaliyetHesapla(alis.Kur, alis.Adet, alis.Fiyat) * (satis.Ufe / alis.Ufe);
            }
            else
            {
                karZararTutar = MaliyetHesapla(satis.Kur, alis.Adet, satis.Fiyat) -
                                MaliyetHesapla(alis.Kur, alis.Adet, alis.Fiyat);
            }

            return new KarZarar
            {
                Hisse = hisse,
                Tarih = satis.Tarih,
                KarZararTutar = isPartial ? karZararTutar * (alis.Adet / satis.Adet) : karZararTutar
            };
        }

        hisse.Alislar = hisse.Alislar.OrderBy(x => x.Tarih).ToList();
        hisse.Satislar = hisse.Satislar.OrderBy(x => x.Tarih).ToList();

        List<KarZarar> karZararlar = new List<KarZarar>();
        foreach (var satis in hisse.Satislar)
        {
            while (satis.Adet > 0)
            {
                var alis = hisse.Alislar.FirstOrDefault(x => x.Adet > 0 && x.Tarih <= satis.Tarih);
                if (alis == null) throw new Exception("Alışı olmayan satış bulundu");

                var ufe = UfeHesapla(alis.Ufe, satis.Ufe);

                if (satis.Adet > alis.Adet)
                {
                    karZararlar.Add(HesaplaKarZarar(hisse, satis, alis, ufe, true));
                    satis.Adet -= alis.Adet;
                    alis.Adet = 0;
                }
                else
                {
                    karZararlar.Add(HesaplaKarZarar(hisse, satis, alis, ufe, false));
                    alis.Adet -= satis.Adet;
                    satis.Adet = 0;
                }
            }
        }

        var groupedKarZararlar = karZararlar
            .GroupBy(x => new { x.Tarih, x.Hisse })
            .Select(x => new KarZarar
            {
                Hisse = x.Key.Hisse,
                Tarih = x.Key.Tarih,
                KarZararTutar = x.Sum(y => y.KarZararTutar)
            });

        // Grouped KarZararlar'ı bir yere eklemeyi unutmayın
    }

    //private static void CalculateTax(Hisse hisse)
    //{

    //    const decimal ufeLimit = 10;

    //    static decimal UfeHesapla(decimal alis, decimal satis)
    //    {
    //        return (satis - alis) / alis * 100;
    //    }
    //    static decimal MaliyetHesapla(decimal kur, decimal adet, decimal fiyat)
    //    {
    //        return fiyat * kur * adet;
    //    }

    //    hisse.Alislar = hisse.Alislar.OrderBy(x => x.Tarih).ToList();
    //    hisse.Satislar = hisse.Satislar.OrderBy(x => x.Tarih).ToList();

    //    List<KarZarar> karZararlar = new List<KarZarar>();
    //    foreach (var satis in hisse.Satislar)
    //    {
    //        while (satis.Adet > 0)
    //        {
    //            var alis = hisse.Alislar.FirstOrDefault(x => x.Adet > 0 && x.Tarih <= satis.Tarih);//küçük eşittiri düşün.
    //            if (alis == null) throw new Exception("Alışı olmayan satış bulundu");

    //            KarZarar karZarar;
    //            var ufe = UfeHesapla(alis.Ufe, satis.Ufe);

    //            if (satis.Adet > alis.Adet)
    //            {
    //                if (ufe >= ufeLimit)
    //                {
    //                    karZarar = new KarZarar
    //                    {
    //                        Hisse = hisse,
    //                        Tarih = satis.Tarih,
    //                        KarZararTutar =
    //                        MaliyetHesapla(satis.Kur, alis.Adet, satis.Fiyat) -
    //                        MaliyetHesapla(alis.Kur, alis.Adet, alis.Fiyat) * (satis.Ufe / alis.Ufe)
    //                    };
    //                }
    //                else
    //                {
    //                    karZarar = new KarZarar
    //                    {
    //                        Hisse = hisse,
    //                        Tarih = satis.Tarih,
    //                        KarZararTutar = MaliyetHesapla(satis.Kur, alis.Adet, satis.Fiyat) -
    //                        MaliyetHesapla(alis.Kur, alis.Adet, alis.Fiyat)
    //                    };
    //                }

    //                satis.Adet -= alis.Adet;
    //                alis.Adet = 0;
    //            }
    //            else
    //            {
    //                if (ufe >= ufeLimit)
    //                {
    //                    karZarar = new KarZarar
    //                    {
    //                        Hisse = hisse,
    //                        Tarih = satis.Tarih,
    //                        KarZararTutar =
    //                        MaliyetHesapla(satis.Kur, satis.Adet, satis.Fiyat) -
    //                        MaliyetHesapla(alis.Kur, satis.Adet, alis.Fiyat) * (satis.Ufe / alis.Ufe)
    //                    };
    //                }
    //                else
    //                {
    //                    karZarar = new KarZarar
    //                    {
    //                        Hisse = hisse,
    //                        Tarih = satis.Tarih,
    //                        KarZararTutar = MaliyetHesapla(satis.Kur, satis.Adet, satis.Fiyat) -
    //                        MaliyetHesapla(alis.Kur, satis.Adet, alis.Fiyat)
    //                    };
    //                }

    //                alis.Adet -= satis.Adet;
    //                satis.Adet = 0;
    //            }

    //            karZararlar.Add(karZarar);
    //        }





    //    }
    //    //group by tarih, hisse, sum karzarar

    //    var groupedKarZararlar = karZararlar.GroupBy(x => new { x.Tarih, x.Hisse }).Select(x => new KarZarar
    //    {
    //        Hisse = x.Key.Hisse,
    //        Tarih = x.Key.Tarih,
    //        KarZararTutar = x.Sum(y => y.KarZararTutar)
    //    });

    //    return;


    //}
    public static void Init()

    {
        var hisse1 = new Hisse
        {
            Sembol = "META",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2023, 6, 23), Fiyat = 288.73m, Adet = 10 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2023, 9, 1), Fiyat = 296.38m, Adet = 10 }
            }
        };

        var hisse2 = new Hisse
        {
            Sembol = "TSLA",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2023, 9, 1), Fiyat = 266.15m, Adet = 5},
                new Alis { Tarih = new DateTime(2023, 10, 10), Fiyat = 222.58m, Adet = 10 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2023, 12, 29), Fiyat = 257.22m, Adet = 7 }
            }
        };

        var hisse3 = new Hisse
        {
            Sembol = "AAPL",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2023, 6, 1), Fiyat = 150m, Adet = 10},
                new Alis { Tarih = new DateTime(2023, 6, 1), Fiyat = 140m, Adet = 5 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2023, 9, 1), Fiyat = 160m, Adet = 15 }
            }
        };

        // Hesaplamalar
        CalculateTax(hisse1);
        CalculateTax(hisse2);
        CalculateTax(hisse3);
    }
}