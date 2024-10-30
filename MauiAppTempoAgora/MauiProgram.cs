using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MauiAppTempoAgora.Data; // Supondo que criaremos uma pasta 'Data' para o contexto

namespace MauiAppTempoAgora
{
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configuração do banco de dados SQLite
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PrevisaoTempo.db3");
            builder.Services.AddDbContext<TempoDbContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
