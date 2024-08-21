using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinansCore.Models.ViewModel;
public class SatisViewModel
{
    [Display(Name = "Last Name"), DataType(DataType.DateTime)]
    public DateTime Tarih { get; set; }
    [Display(Name = "Last Name"), DataType(DataType.Currency)]
    public decimal Fiyat { get; set; }
    [Display(Name = "Last Name"), DataType(DataType.Currency)]
    public decimal Adet { get; set; }
}
