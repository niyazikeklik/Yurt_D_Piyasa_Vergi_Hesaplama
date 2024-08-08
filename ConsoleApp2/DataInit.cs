using YurtDisiKazancHesapla.Business;
using YurtDisiKazancHesapla.Models;

namespace YurtDisiKazancHesapla;

public static class DataInit
{
    public static void Run()
    {

        List<Hisse> Hissler = new List<Hisse>();

        // DIS 
        Hissler.Add(new()
        {
            Sembol = "DIS",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2023, 8, 29), Fiyat = 83.73m, Adet = 4 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2024, 2, 5), Fiyat = 96.62m, Adet = 4 }
            }
        });

        // Pypal
        Hissler.Add(new()
        {
            Sembol = "Pypal",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2023, 9, 25), Fiyat = 57.89m, Adet = 7 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2023, 12, 13), Fiyat = 61.40m, Adet = 7 }
            }
        });

        // BW
        Hissler.Add(new()
        {
            Sembol = "BW",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2023, 11, 10), Fiyat = 1.03m, Adet = 100 },
                new Alis { Tarih = new DateTime(2024, 3, 15), Fiyat = 1, Adet = 1000 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2023, 11, 15), Fiyat = 1.66m, Adet = 100 },
                new Satis { Tarih = new DateTime(2024, 3, 20), Fiyat = 1.06m, Adet = 1000 }
            }
        });

        // ANTX
        Hissler.Add(new()
        {
            Sembol = "ANTX",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2024, 2, 12), Fiyat = 5.22m, Adet = 200 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2024, 2, 13), Fiyat = 4.72m, Adet = 200 }
            }
        });

        // SSRM
        Hissler.Add(new()
        {
            Sembol = "SSRM",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2024, 2, 28), Fiyat = 4.30m, Adet = 220 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2024, 4, 4), Fiyat = 4.69m, Adet = 220 }
            }
        });

        // TMF
        Hissler.Add(new()
        {
            Sembol = "TMF",
            Alislar = new List<Alis>
            {
                new Alis { Tarih = new DateTime(2024, 5, 6), Fiyat = 47.30m, Adet = 106 }
            },
            Satislar = new List<Satis>
            {
                new Satis { Tarih = new DateTime(2024, 6, 4), Fiyat = 50.71m, Adet = 106 }
            }
        });

        List<Sonuc> sonuclar = new List<Sonuc>();
        foreach (var hisse in Hissler)
        {
            var sonuc = KazancHesapla.CalculateTax(hisse);
            sonuclar.AddRange(sonuc);
        }

        Console.Read();
    }
}

