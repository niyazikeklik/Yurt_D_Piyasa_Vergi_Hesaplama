using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YurtDisiKazancHesapla.Models.VergiDilimleri;
public class VergiDilim
{
    public int Year { get; set; }
    public int Month { get; set; }
    public List<Kademeler> Kademeler { get; set; }
}
