using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YurtDisiKazancHesapla.Models.Service;
public class HıstoryModel
{
    public partial class DovizCom
    {
        [JsonPropertyName("error")]
        public bool error { get; set; }

        [JsonPropertyName("data")]
        public List<Datum>? Data { get; set; }
    }

    public partial class Datum
    {
        [JsonPropertyName("update_date")]
        public long update_date { get; set; }

        [JsonPropertyName("open")]
        public decimal open { get; set; }

        [JsonPropertyName("highest")]
        public decimal highest { get; set; }

        [JsonPropertyName("lowest")]
        public decimal lowest { get; set; }

        [JsonPropertyName("close")]
        public decimal close { get; set; }

        [JsonPropertyName("close_try")]
        public decimal close_try { get; set; }

        [JsonPropertyName("volume")]
        public long volume { get; set; }

        [JsonPropertyName("selling")]
        public decimal selling { get; set; }
        public DateTime DateTime => DateTimeOffset.FromUnixTimeSeconds(update_date).DateTime;
        public decimal AltinSertikasiOlmasiGerekenFiyat { get; set;}
        public decimal AltinSertikasiFarki => close - AltinSertikasiOlmasiGerekenFiyat;
        public decimal AltinSertikasiFarkiYuzde => AltinSertikasiFarki / AltinSertikasiOlmasiGerekenFiyat * 100;
    }
}
