using FinansCore.Models;

namespace YurtDisiKazancHesapla.Models.VergiDilimleri;

public class Kademeler : BaseEntity
{
    public decimal UstSinir { get; set; }
    public decimal Oran { get; set; }
    public decimal SabitVergi { get; set; }

}
