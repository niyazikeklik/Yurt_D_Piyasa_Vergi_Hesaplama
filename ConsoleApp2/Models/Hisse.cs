namespace ConsoleApp2.Models;

/******************************************************************************

                            Online C# Compiler.
                Code, Compile, Run and Debug C# program online.
Write your code in this editor and press "Run" button to execute it.

*******************************************************************************/

public class Hisse
{
    public string Sembol { get; set; }
    public List<Alis> Alislar { get; set; } = new List<Alis>();
    public List<Satis> Satislar { get; set; } = new List<Satis>();
}

