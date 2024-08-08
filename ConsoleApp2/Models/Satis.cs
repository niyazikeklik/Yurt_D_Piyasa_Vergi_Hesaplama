namespace ConsoleApp2.Models;

/******************************************************************************

                            Online C# Compiler.
                Code, Compile, Run and Debug C# program online.
Write your code in this editor and press "Run" button to execute it.

*******************************************************************************/

using System;

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

