namespace YurtDisiKazancHesapla.Models;

using FinansCore.Models;
using FinansCore.Models.DataAnnotations;

/******************************************************************************

                            Online C# Compiler.
                Code, Compile, Run and Debug C# program online.
Write your code in this editor and press "Run" button to execute it.

*******************************************************************************/

using System;

using YurtDisiKazancHesapla.Services;

public class Satis : BaseEntity
{
    [ShowDynamicFormAnnotation]
    public DateTime Tarih { get; set; } = DateTime.Now;
    [ShowDynamicFormAnnotation]
    public decimal Fiyat { get; set; }
    [ShowDynamicFormAnnotation]
    public decimal Adet { get; set; }
    public decimal Kur => DovizService.GetDovizKuru(Tarih);
    public decimal Ufe => YufeService.GetValueForYearAndMonth(Tarih.Year, Tarih.Month);
    public decimal ToplamTutar => Fiyat * Adet;
    public decimal FiyatTL => Fiyat * Kur;
    public decimal ToplamTutarTL => Fiyat * Adet * Kur;
}

