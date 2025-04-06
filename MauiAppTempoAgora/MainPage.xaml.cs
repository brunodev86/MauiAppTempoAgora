using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Net;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    HttpResponseMessage response = await DataService.GetHttpResponse(txt_cidade.Text);
                    if (response.IsSuccessStatusCode)
                    {
                        Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);
                        if (t != null)
                        {
                            string dados_previsao = $"Latitude: {t.lat} \n" +
                                                    $"Longitude: {t.lon} \n" +
                                                    $"Nascer do Sol: {t.sunrise} \n" +
                                                    $"Por do Sol: {t.sunset} \n" +
                                                    $"Temp Máxima: {t.temp_max}°C \n" +
                                                    $"Temp Miníma: {t.temp_min}°C \n" +
                                                    $"Descrição do Clima: {t.description} \n" +
                                                    $"Velocidade do Vento: {t.speed} m/s \n" +
                                                    $"Visibilidade: {t.visibility} m \n";

                            lbl_res.Text = dados_previsao;
                        }
                        else
                        {
                            lbl_res.Text = "Sem dados de previsão.";
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        lbl_res.Text = "Cidade não encontrada.";
                    }
                    else
                    {
                        lbl_res.Text = "Erro ao obter dados de previsão.";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                await DisplayAlert("Erro", "Sem conexão com a internet.", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "Ok");
            }
        }
    }
}
