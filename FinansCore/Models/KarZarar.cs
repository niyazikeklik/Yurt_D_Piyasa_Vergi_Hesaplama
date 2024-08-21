namespace YurtDisiKazancHesapla.Models;

using FinansCore.Models;

/******************************************************************************

                            Online C# Compiler.
                Code, Compile, Run and Debug C# program online.
Write your code in this editor and press "Run" button to execute it.

*******************************************************************************/

using System;

public class KarZarar : BaseEntity
{
    public Hisse Hisse { get; set; }
    public DateTime Tarih { get; set; }
    public int Year => Tarih.Year;
    public int Month => Tarih.Month;
    public string HisseKodu => Hisse.Sembol;
    public decimal VergiIcınGelir { get; set; }
    public decimal AsılGelirTL { get; set; }
    public decimal AsılMaliyetTL { get; set; }
    public decimal GelirKazancFarkiTL => AsılGelirTL - AsılMaliyetTL;
    public decimal DolarSabitKalsaKazancTL { get; set; }
    public decimal AsılGelirUSD { get; set; }
    public decimal AsılMaliyetUSD { get; set; }
    public decimal GelirKazancFarkiUSD => AsılGelirUSD - AsılMaliyetUSD;
    public decimal AlisDovizKuru { get; set; }
    public decimal SatisDovizKuru { get; set; }
    public decimal KaraKurEtkisiOran => SatisDovizKuru  / AlisDovizKuru;
    public decimal UfeIndirimi { get; set; }
    public decimal AsilUfe { get; set; }
}

