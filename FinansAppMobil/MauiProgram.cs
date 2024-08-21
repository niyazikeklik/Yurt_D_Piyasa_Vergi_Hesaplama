using DTO;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinansAppMobil;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FinansDb.db");

            options.UseSqlite($"Data Source={dbPath}", options =>
            {
                options.MigrationsAssembly("MauiApp1");
            });
        });

        var db = new AppDbContext();
        if (!db.Database.CanConnect())
        {
            db.Database.Migrate();
        }

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
