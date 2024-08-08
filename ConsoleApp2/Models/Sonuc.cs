namespace ConsoleApp2.Models;

/******************************************************************************

                            Online C# Compiler.
                Code, Compile, Run and Debug C# program online.
Write your code in this editor and press "Run" button to execute it.

*******************************************************************************/

using System;

public class Sonuc
{
    public Hisse Hisse { get; set; }
    public string HisseSembol => Hisse.Sembol;
    public DateTime Tarih { get; set; }
    public decimal KarZararTutar { get; set; }
    public decimal GerceklesenKarZararUsd { get; set; }
    public decimal DovizSabitKalsaydiKarZararTl { get; set; }
    public decimal Yıl { get; set; }
    public decimal DovizKuruFarki { get; set; }
    public decimal UfeFarki { get; set; }
    public decimal DoivzKuruOrani { get; set; }
    public decimal AlisSatisArasindakiGunFarki { get; set; }
    public decimal AlisDovizKuru { get; set; }
    public decimal SatisDovizKuru { get; set; }
}

