using MauiAppTempoAgora.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using MauiAppTempoAgora.Service;
using MauiAppTempoAgora.Data;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        private readonly DataService _dataService;
        public MainPage()
        {
            InitializeComponent();
            // Configurar DbContextOptions para o TempoDbContext
            var optionsBuilder = new DbContextOptionsBuilder<TempoDbContext>();
            optionsBuilder.UseSqlite("Data Source=previsaoTempo.db"); // Certifique-se de que o caminho do banco de dados esteja correto

            // Instanciar o DataService com o DbContext configurado
            _dataService = new DataService(new TempoDbContext(optionsBuilder.Options));
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            try
            {
                string cidade = entryCidade.Text; // Obtendo o valor da cidade do campo de entrada
                if (!string.IsNullOrEmpty(cidade))
                {
                    var previsao = await _dataService.GetPrevisaoDoTempoAsync(cidade);
                    if (previsao != null)
                    {
                        lbl_previsao.Text = $"Previsão para {cidade} salva no banco de dados!";
                    }
                    else
                    {
                        lbl_previsao.Text = "Não foi possível obter a previsão.";
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void OnBuscarPrevisaoPorDataClicked(object sender, EventArgs e)
        {
            DateTime dataSelecionada = datePicker.Date;

            var previsao = await _dataService.GetPrevisaoPorDataAsync(dataSelecionada); // Adicione um método para buscar previsões por data, se necessário

            //var previsao = await _dataService.Previsoes
                //.Where(p => p.DataPrevisao.Date == dataSelecionada.Date)
                //.FirstOrDefaultAsync();

            if (previsao != null)
            {
                lbl_resultadoBusca.Text = $"Previsão de {previsao.Title} em {dataSelecionada.ToShortDateString()}:\n" +
                                          $"Temperatura: {previsao.Temperature}\n" +
                                          $"Humidade: {previsao.Humidity}\n" +
                                          $"Descrição: {previsao.WeatherDescription}\n";
            }
            else
            {
                lbl_resultadoBusca.Text = "Nenhuma previsão encontrada para essa data.";
            }
        }
    }


}