using MauiAppTempoAgora.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using MauiAppTempoAgora.Data;
using Microsoft.EntityFrameworkCore;

namespace MauiAppTempoAgora.Service
{
    public class DataService
    {
        private readonly TempoDbContext _dbContext;

        public DataService(TempoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tempo?> GetPrevisaoDoTempoAsync(string cidade)
        {
            string appId = "6135072afe7f6cec1537d5cb08a5a1a2";
            string url = $"http://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={appId}";

            Tempo? tempo = null;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    tempo = new Tempo
                    {
                        Humidity = rascunho["main"]["humidity"].ToString(),
                        Temperature = rascunho["main"]["temp"].ToString(),
                        Title = rascunho["name"].ToString(),
                        Visibility = rascunho["visibility"].ToString(),
                        Wind = rascunho["wind"]["speed"].ToString(),
                        Sunrise = sunrise.ToString(),
                        Sunset = sunset.ToString(),
                        Weather = rascunho["weather"][0]["main"].ToString(),
                        WeatherDescription = rascunho["weather"][0]["description"].ToString(),
                        DataPrevisao = DateTime.Now // Data atual
                    };

                    // Salvar no banco de dados
                    _dbContext.Previsoes.Add(tempo);
                    await _dbContext.SaveChangesAsync();
                }
            }

            return tempo;
        }

        // Novo método para buscar previsões por data
        public async Task<Tempo?> GetPrevisaoPorDataAsync(DateTime data)
        {
            return await _dbContext.Previsoes
                .FirstOrDefaultAsync(p => p.DataPrevisao.Date == data.Date);
        }
    }
}
