using System.ComponentModel.DataAnnotations;

namespace MauiAppTempoAgora.Models
{
    public class Tempo
    {
        [Key]
        public int Id { get; set; } // Id para uso no SQLite
        public string? Title { get; set; }
        public string? Temperature { get; set; }
        public string? Wind { get; set; }
        public string? Humidity { get; set; }
        public string? Visibility { get; set; }
        public string? Sunrise { get; set; }
        public string? Sunset { get; set; }
        public string? Weather { get; set; }
        public string? WeatherDescription { get; set; }
        public DateTime DataPrevisao { get; set; } // Data da pesquisa
    }
}
