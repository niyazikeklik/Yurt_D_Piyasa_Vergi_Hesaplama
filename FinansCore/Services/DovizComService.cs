
using Newtonsoft.Json;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using static YurtDisiKazancHesapla.Models.Service.HıstoryModel;

namespace YurtDisiKazancHesapla.Services;
public class DovizComService
{
    HttpClient _httpClient;

    public DovizComService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("authorization", "Bearer c998cac32a1ec945c7642a2244334be11dcd23fa04e04f03ec3e67ad6da966e8");
        _httpClient.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
        _httpClient.BaseAddress = new Uri("https://www.doviz.com/api/v11/assets/");
    }

    public enum Emtia {
        USD,
        ONS,
        ALTINS1,
    }

    public enum Aralik
    {
        Tumu = 0,
        BirHafta = 1,
        BirAy = 2,
        UcAy = 3,
        AltıAy = 4,
        BirYıl = 5,
        OnYil = 6
    }

    private Tuple<string, string> AralikHesapla(Aralik aralik)
    {
        DateTime baslangicTarihi = new DateTime(1970, 1, 1);
        switch (aralik)
        {
            case Aralik.BirHafta:
                return new Tuple<string, string>("0", (DateTime.Now - baslangicTarihi).TotalSeconds.ToString());
            case Aralik.BirAy:
                return new Tuple<string, string>((DateTime.Now.AddMonths(-1) - baslangicTarihi).TotalSeconds.ToString(), (DateTime.Now - baslangicTarihi).TotalSeconds.ToString());
            case Aralik.UcAy:
                return new Tuple<string, string>((DateTime.Now.AddMonths(-3) - baslangicTarihi).TotalSeconds.ToString(), (DateTime.Now - baslangicTarihi).TotalSeconds.ToString());
            case Aralik.AltıAy:
                return new Tuple<string, string>((DateTime.Now.AddMonths(-6) - baslangicTarihi).TotalSeconds.ToString(), (DateTime.Now - baslangicTarihi).TotalSeconds.ToString());
            case Aralik.BirYıl:
                return new Tuple<string, string>((DateTime.Now.AddYears(-1) - baslangicTarihi).TotalSeconds.ToString(), (DateTime.Now - baslangicTarihi).TotalSeconds.ToString());
            case Aralik.OnYil:
                return new Tuple<string, string>((DateTime.Now.AddYears(-10) - baslangicTarihi).TotalSeconds.ToString(), (DateTime.Now - baslangicTarihi).TotalSeconds.ToString());
            default:
                return new Tuple<string, string>("0", (DateTime.Now - baslangicTarihi).TotalSeconds.ToString());
        }
    }

    private async Task<DovizCom> GetHistoryAsync(Emtia endpoint, Aralik aralik)
    {
        var araliklar = AralikHesapla(aralik);
        var response = await _httpClient.GetAsync(endpoint.ToString() + $"/archive?start={araliklar.Item1}&end={araliklar.Item2}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<DovizCom>(content) ?? new DovizCom() { error = true };
    }


    public async Task<DovizCom> GetDovizHistory()
    {
        return await GetHistoryAsync(Emtia.USD, Aralik.Tumu);
    }

    public async Task<DovizCom> GetOnsHistory()
    {
        return await GetHistoryAsync(Emtia.ONS, Aralik.Tumu);
    }

    public async Task<DovizCom> GetAltinS1History()
    {
        return await GetHistoryAsync(Emtia.ALTINS1, Aralik.Tumu);
    }
}
