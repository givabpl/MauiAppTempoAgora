using Microsoft.Data.Sqlite;
using MauiAppTempoAgora.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace MauiAppTempoAgora.Service
{
    public class DatabaseService
    {
        private readonly string _dbPath;

        public DatabaseService()
        {
            _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tempo.db");

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS PrevisaoTempo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Cidade TEXT,
                Data TEXT,
                Humidity TEXT,
                Temperature TEXT,
                Wind TEXT,
                Weather TEXT,
                WeatherDescription TEXT,
                Sunrise TEXT,
                Sunset TEXT
            )";
            tableCmd.ExecuteNonQuery();
        }

        public async Task SalvarPrevisao(Tempo previsao)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
            INSERT INTO PrevisaoTempo (Cidade, Data, Humidity, Temperature, Wind, Weather, WeatherDescription, Sunrise, Sunset)
            VALUES (@cidade, @data, @humidity, @temperature, @wind, @weather, @weatherDescription, @sunrise, @sunset)";

            insertCmd.Parameters.AddWithValue("@cidade", previsao.Title);
            insertCmd.Parameters.AddWithValue("@data", DateTime.Now.ToString("yyyy-MM-dd"));
            insertCmd.Parameters.AddWithValue("@humidity", previsao.Humidity);
            insertCmd.Parameters.AddWithValue("@temperature", previsao.Temperature);
            insertCmd.Parameters.AddWithValue("@wind", previsao.Wind);
            insertCmd.Parameters.AddWithValue("@weather", previsao.Weather);
            insertCmd.Parameters.AddWithValue("@weatherDescription", previsao.WeatherDescription);
            insertCmd.Parameters.AddWithValue("@sunrise", previsao.Sunrise);
            insertCmd.Parameters.AddWithValue("@sunset", previsao.Sunset);

            await insertCmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Tempo>> ObterPrevisoes(string cidade, string data)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = @"
            SELECT * FROM PrevisaoTempo WHERE Cidade = @cidade AND Data = @data";

            selectCmd.Parameters.AddWithValue("@cidade", cidade);
            selectCmd.Parameters.AddWithValue("@data", data);

            var previsoes = new List<Tempo>();

            using var reader = await selectCmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                previsoes.Add(new Tempo
                {
                    Title = reader.GetString(1),
                    Humidity = reader.GetString(2),
                    Temperature = reader.GetString(3),
                    Wind = reader.GetString(4),
                    Weather = reader.GetString(5),
                    WeatherDescription = reader.GetString(6),
                    Sunrise = reader.GetString(7),
                    Sunset = reader.GetString(8),
                });
            }

            return previsoes;
        }
    }
}
