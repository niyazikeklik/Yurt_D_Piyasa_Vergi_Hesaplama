namespace YurtDisiKazancHesapla.Models;

using YurtDisiKazancHesapla.Services;

/******************************************************************************

                            Online C# Compiler.
                Code, Compile, Run and Debug C# program online.
Write your code in this editor and press "Run" button to execute it.

*******************************************************************************/

using System;

public class Alis
{
    public DateTime Tarih { get; set; }
    public decimal Fiyat { get; set; }
    public decimal Adet { get; set; }
    public decimal ToplamTutar => Fiyat * Adet;
    public decimal Kur => DovizService.GetDovizKuru(Tarih).Result;
    public decimal Ufe => YufeService.GetValueForYearAndMonth(Tarih.Year, Tarih.Month);
    public decimal FiyatTL => Fiyat * Kur;
    public decimal ToplamTutarTL => Fiyat * Adet * Kur;
}

