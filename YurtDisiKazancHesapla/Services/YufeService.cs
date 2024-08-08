using HtmlAgilityPack;

using System.Globalization;

namespace YurtDisiKazancHesapla.Services;
public static class YufeService
{
    private static string? _yufeHtml = null;
    private static string YufeHtml
    {
        get
        {
            if (_yufeHtml == null)
            {
                _yufeHtml = GetYufeHtml().Result;
            }
            return _yufeHtml;
        }
    }
    private static async Task<string> GetYufeHtml()
    {
        using (var client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://www.hakedis.org/endeksler/yi-ufe-yurtici-uretici-fiyat-endeksi");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }
    }
    public static decimal GetValueForYearAndMonth(int year, int month)
    {
        if (month == 1)
        {
            month = 12;
            year--;
        }
        else
        {
            month--;
        }

        var htmlContent = YufeHtml;

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);

        string AyAdi = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

        // Tabloyu bul
        var table = htmlDocument.DocumentNode.SelectSingleNode("//table");
        if (table == null)
        {
            throw new Exception("Table not found in the HTML content.");
        }

        // Tablo satırlarını seç
        var rows = table.SelectNodes(".//tr");
        if (rows == null || rows.Count < 2)
        {
            throw new Exception("Table rows are missing or insufficient.");
        }

        // Başlık satırını al
        var headerCells = rows[0].SelectNodes(".//th");
        if (headerCells == null)
        {
            throw new Exception("Table headers are missing.");
        }

        // Yıl ve ay sütunlarını bul
        int yilRowIndex = -1;
        int ayColumnIndex = -1;

        //for all rows
        for (int i = 0; i < headerCells.Count; i++)
        {
            var cell = headerCells[i];
            if (cell.InnerText.ToUpper().Contains(AyAdi.ToString().ToUpper()))
            {
                ayColumnIndex = i;
            }
        }

        if (ayColumnIndex == -1)
        {
            throw new Exception("Column for the month is missing.");
        }

        for (int i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            if (row.InnerText.Contains(year.ToString()))
            {
                yilRowIndex = i;
            }
        }

        if (yilRowIndex == -1)
        {
            throw new Exception("Row for the year is missing.");
        }



        string result = rows[yilRowIndex].SelectNodes(".//td")[ayColumnIndex].InnerText;

        decimal.TryParse(result, out decimal ufe);

        return ufe;
    }
}
