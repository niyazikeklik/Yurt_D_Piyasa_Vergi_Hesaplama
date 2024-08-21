using FinansCore.Models;

namespace YurtDisiKazancHesapla.Models;

/******************************************************************************

                            Online C# Compiler.
                Code, Compile, Run and Debug C# program online.
Write your code in this editor and press "Run" button to execute it.

*******************************************************************************/

public class Hisse : BaseEntity
{
    public string Sembol { get; set; }
    public ICollection<Alis> Alislar { get; set; } = new List<Alis>();
    public ICollection<Satis> Satislar { get; set; } = new List<Satis>();
}

