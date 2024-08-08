using YurtDisiKazancHesapla.Models;

namespace YurtDisiKazancHesapla.Business;
public class KazancHesapla
{
    public static List<Sonuc> CalculateTax(Hisse hisse)
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
                ReelKarZararTutarTL = isPartial ? karZararTutar * (alis.Adet / satis.Adet) : karZararTutar,
                GerceklesenKarZararUsd = satis.Adet * satis.Fiyat - alis.Adet * alis.Fiyat,
                DovizSabitKalsaydiKarZararTl = MaliyetHesapla(alis.Kur, satis.Adet, satis.Fiyat) - MaliyetHesapla(alis.Kur, alis.Adet, alis.Fiyat),
                DovizKuruOrani = (satis.Kur - alis.Kur) / alis.Kur * 100,
                UfeFarki = ufe,
                DovizKuruFarki = satis.Kur - alis.Kur,
                AlisDovizKuru = alis.Kur,
                SatisDovizKuru = satis.Kur,
                AlisSatisArasindakiGunFarki = (satis.Tarih - alis.Tarih).Days
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
            .GroupBy(x => new { x.Tarih, x.Hisse, x.AlisSatisArasindakiGunFarki })
            .Select(x => new Sonuc
            {
                Hisse = x.Key.Hisse,
                Tarih = x.Key.Tarih,
                KarZararTutar = x.Sum(y => y.ReelKarZararTutarTL),
                DovizSabitKalsaydiKarZararTl = x.Sum(y => y.DovizSabitKalsaydiKarZararTl),
                GerceklesenKarZararUsd = x.Sum(y => y.GerceklesenKarZararUsd),
                Yıl = x.Key.Tarih.Year,
                UfeFarki = x.Sum(y => y.UfeFarki),
                AlisDovizKuru = x.Average(y => y.AlisDovizKuru),
                SatisDovizKuru = x.Average(y => y.SatisDovizKuru),
                DovizKuruFarki = x.Sum(y => y.DovizKuruFarki),
                DoivzKuruOrani = x.Average(y => y.DovizKuruOrani),
                AlisSatisArasindakiGunFarki = x.Key.AlisSatisArasindakiGunFarki
            }).ToList();

        return groupedKarZararlar;
    }
}
