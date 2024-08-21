using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YurtDisiKazancHesapla.Models;

namespace FinansCore.Services;

public enum IslemTuru
{
    Alis,
    Satis
}


public class Islem
{
    public DateTime Tarih { get; set; }
    public decimal Fiyat { get; set; }
    public decimal Adet { get; set; }
    public string Sembol { get; set; }
    public IslemTuru IslemTuru { get; set; }

}
public class Converter
{
    public List<Islem> ConvertToIslems(List<Hisse> islems)
    {
        List<Islem> islemList = new List<Islem>();
        foreach (var hisse in islems)
        {
            foreach (var alis in hisse.Alislar)
            {
                islemList.Add(new Islem
                {
                    Tarih = alis.Tarih,
                    Fiyat = alis.Fiyat,
                    Adet = alis.Adet,
                    Sembol = hisse.Sembol,
                    IslemTuru = IslemTuru.Alis
                });
            }
            foreach (var satis in hisse.Satislar)
            {
                islemList.Add(new Islem
                {
                    Tarih = satis.Tarih,
                    Fiyat = satis.Fiyat,
                    Adet = satis.Adet,
                    Sembol = hisse.Sembol,
                    IslemTuru = IslemTuru.Satis
                });
            }
        }
        return islemList.OrderBy(x=> x.Sembol).ThenBy(x => x.Tarih).ToList();
    }
    public List<Hisse> ConvertToHisses(List<Islem> islems)
    {
        List<Hisse> hisseList = new List<Hisse>();
        foreach (var islem in islems)
        {
            var hisse = hisseList.FirstOrDefault(x => x.Sembol == islem.Sembol);
            if (hisse == null)
            {
                hisse = new Hisse { Sembol = islem.Sembol };
                hisseList.Add(hisse);
            }
            if (islem.IslemTuru == IslemTuru.Alis)
            {
                hisse.Alislar.Add(new Alis
                {
                    Tarih = islem.Tarih,
                    Fiyat = islem.Fiyat,
                    Adet = islem.Adet
                });
            }
            else
            {
                hisse.Satislar.Add(new Satis
                {
                    Tarih = islem.Tarih,
                    Fiyat = islem.Fiyat,
                    Adet = islem.Adet
                });
            }
        }
        return hisseList;
    }
}
