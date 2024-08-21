using YurtDisiKazancHesapla.Models;

namespace YurtDisiKazancHesapla.Business;
public class KazancHesapla
{
    private const decimal ufeLimit = 1.10m;

    private static decimal UfeHesapla(decimal alis, decimal satis)
    {
        return (satis / alis);
    }

    private static decimal MaliyetHesapla(decimal kur, decimal adet, decimal fiyat)
    {
        return fiyat * kur * adet;
    }

    private static KarZarar HesaplaKarZarar(Hisse hisse, Satis satis, Alis alis, decimal dusulecekAdet, decimal ufe = 1)
    {
        decimal karZararTutar;
        karZararTutar = MaliyetHesapla(satis.Kur, dusulecekAdet, satis.Fiyat) - MaliyetHesapla(alis.Kur, dusulecekAdet, alis.Fiyat) * ufe;

        return new KarZarar
        {
            Hisse = hisse,
            Tarih = satis.Tarih,
            VergiIcınGelir = karZararTutar,
            UfeIndirimi = ufe,
            AsılGelirTL = MaliyetHesapla(satis.Kur, dusulecekAdet, satis.Fiyat),
            AsılMaliyetTL = MaliyetHesapla(alis.Kur, dusulecekAdet, alis.Fiyat),
            DolarSabitKalsaKazancTL = MaliyetHesapla(alis.Kur, dusulecekAdet, satis.Fiyat) - MaliyetHesapla(alis.Kur, dusulecekAdet, alis.Fiyat),
            AsılGelirUSD = satis.Fiyat * dusulecekAdet,
            AsılMaliyetUSD = alis.Fiyat * dusulecekAdet,
            AlisDovizKuru = alis.Kur,
            SatisDovizKuru = satis.Kur,
            AsilUfe = satis.Ufe / alis.Ufe
        };
    }

    private static void ProcessSatis(Hisse hisse, Satis satis, List<KarZarar> karZararlar)
    {
        while (satis.Adet > 0)
        {
            var alis = hisse.Alislar.FirstOrDefault(x => x.Adet > 0 && x.Tarih <= satis.Tarih);
            if (alis == null)
            {
                throw new Exception("Alışı olmayan satış bulundu");
            }

            var ufe = UfeHesapla(alis.Ufe, satis.Ufe);
            ufe = ufe >= ufeLimit ? ufe : 1;
            var dusulecekAdet = Math.Min(satis.Adet, alis.Adet);
            var karZarar = HesaplaKarZarar(hisse, satis, alis, dusulecekAdet, ufe);

            karZararlar.Add(karZarar);
            satis.Adet -= dusulecekAdet;
            alis.Adet -= dusulecekAdet;
        }
    }

    

    public static List<KarZarar> GelirHesapla(Hisse hisse)
    {
        hisse.Alislar = hisse.Alislar.OrderBy(x => x.Tarih).ToList();
        hisse.Satislar = hisse.Satislar.OrderBy(x => x.Tarih).ToList();

        List<KarZarar> karZararlar = new List<KarZarar>();
        foreach (var satis in hisse.Satislar)
        {
            ProcessSatis(hisse, satis, karZararlar);
        }

        var groupedKarZararlar = karZararlar
            .GroupBy(x => new { x.Tarih, x.HisseKodu, x.Hisse
            })
            .Select(x => new KarZarar
            {
                Hisse = x.Key.Hisse,
                Tarih = x.Key.Tarih,
                AlisDovizKuru = x.Average(y => y.AlisDovizKuru),
                SatisDovizKuru = x.Average(y => y.SatisDovizKuru),
                AsılGelirTL = x.Sum(y => y.AsılGelirTL),
                AsılMaliyetTL = x.Sum(y => y.AsılMaliyetTL),
                DolarSabitKalsaKazancTL = x.Sum(y => y.DolarSabitKalsaKazancTL),
                AsılGelirUSD = x.Sum(y => y.AsılGelirUSD),
                AsılMaliyetUSD = x.Sum(y => y.AsılMaliyetUSD),
                UfeIndirimi = x.Average(y => y.UfeIndirimi),
                AsilUfe = x.Average(y => y.AsilUfe),
                VergiIcınGelir = x.Sum(y => y.VergiIcınGelir),
                
            }).ToList();

        return groupedKarZararlar;
    }
    public static void VergiHesapla(List<KarZarar> sonuclar)
    {
        var Gelirler = sonuclar.GroupBy(x => x.Year).Select(x =>  new
        {
            Yil = x.Key,
            Gelir = x.Sum(x => x.VergiIcınGelir)
        }).ToList();
        //2024 takvim yılı(G.V.K.madde:103) Gelir Vergisi Tarifesi
        //110.000 TL'ye kadar 15%
        //230.000 TL'nin 110.000 TL'si için 16.500 TL, fazlası 20%
        //580.000 TL'nin 230.000 TL'si için 40.500 TL(ücret gelirlerinde 870.000 TL'nin 230.000 TL'si için 40.500 TL), fazlası 27%
        //3.000.000 TL'nin 580.000 TL'si için 135.000 TL, (ücret gelirlerinde 3.000.000 TL'nin 870.000 TL'si için 213.300 TL), fazlası 35%
        //3.000.000 TL'den fazlasının 3.000.000 TL'si için 982.000 TL, (ücret gelirlerinde 3.000.000 TL'den fazlasının 3.000.000 TL'si için 958.800 TL), fazlası 40%

        List<decimal> Vergiler = new List<decimal>();

        foreach (var gelir in Gelirler)
        {
            if (gelir.Gelir <= 110000)
            {
                Vergiler.Add(gelir.Gelir * 0.15m);
            }
            else if (gelir.Gelir <= 230000)
            {
                Vergiler.Add(16500 + (gelir.Gelir - 110000) * 0.20m);
            }
            else if (gelir.Gelir <= 580000)
            {
                Vergiler.Add(40500 + (gelir.Gelir - 230000) * 0.27m);
            }
            else if (gelir.Gelir <= 3000000)
            {
                Vergiler.Add(135000 + (gelir.Gelir - 580000) * 0.35m);
            }
            else
            {
                Vergiler.Add(982000 + (gelir.Gelir - 3000000) * 0.40m);
            }
            Console.WriteLine("Gelir: " + gelir.Gelir + " Vergi: " + Vergiler.Last() + " Yıl: " + gelir.Yil);
        }
    }
}
