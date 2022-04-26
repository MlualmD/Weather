using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTech.Weather.Model;
using System.Text.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyTech.Weather.DAL
{
    public class APICityRequset
    {
        readonly string OneCity = @"C:\Users\מולועלם דפרשה\Desktop\MyProjectAtZiont\ProjectZiont\Tech.MyProject.Weather\MyTech.Weather.UI\bin\Debug\OneCity.txt";
        readonly string History = @"C:\Users\מולועלם דפרשה\Desktop\MyProjectAtZiont\ProjectZiont\Tech.MyProject.Weather\MyTech.Weather.UI\bin\Debug\History.txt";
        public async Task<WeatherCallResult> GetCityData(string nameCity)
        {

            WeatherCallResult weather;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.weatherapi.com/");

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage pespones = await client.GetAsync($@"v1/current.json?key=12bac74e2e2743d6961184414221304&q={nameCity}& aqi=no");

                string lines = await pespones.Content.ReadAsStringAsync();

                weather = JsonSerializer.Deserialize<WeatherCallResult>(lines);

                return weather;

            }
        }
        public void Save(Dictionary<string, WeatherCallResult> dic)
        {
            string dataDiC = JsonSerializer.Serialize(dic);

            File.WriteAllText(History, dataDiC);
        }
        public void Save(WeatherCallResult dic)
        {
            string dataDiC = JsonSerializer.Serialize(dic);

            File.WriteAllText(OneCity, dataDiC);
        }
        public WeatherCallResult LoadOneCity()
        {
            string res = File.ReadAllText(OneCity);
            var dic = JsonSerializer.Deserialize<WeatherCallResult>(res);
            return dic;
        }
        public Dictionary<string, WeatherCallResult> Load()
        {
            string res = File.ReadAllText(History);
            var dic = JsonSerializer.Deserialize<Dictionary<string, WeatherCallResult>>(res);
            return dic;
        }
    }
}
