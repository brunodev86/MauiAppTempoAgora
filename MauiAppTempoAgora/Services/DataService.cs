﻿using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public static class DataService
    {
        public static async Task<HttpResponseMessage> GetHttpResponse(string cidade)
        {
            string chave = "e3ea185b30a47e6eb59b2e9a4744c0d9";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}";
            using (HttpClient client = new HttpClient())
            {
                return await client.GetAsync(url);
            }
        }

        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;
            string chave = "e3ea185b30a47e6eb59b2e9a4744c0d9";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime sunrise = epoch.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = epoch.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    };
                }
            }

            return t;
        }
    }
}
