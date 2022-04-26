using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTech.Weather.DAL;
using MyTech.Weather.Model;

namespace MyTech.Weather.Entities
{
    public class Request
    {
        public bool continuToRun = true;

        private Dictionary<string, WeatherCallResult> WeatherTable = new Dictionary<string, WeatherCallResult>();

        APICityRequset aPICity = new APICityRequset();
        public void Load()
        {
            WeatherTable = aPICity.Load();
        }
        public async Task<Dictionary<string, WeatherCallResult>> Add(string nameCity)
        {
            var oneCity = await aPICity.GetCityData(nameCity);

            if (!WeatherTable.ContainsKey(oneCity.location.name))
            {
                WeatherTable.Add(oneCity.location.name, oneCity);
            }
            else
            {
                Update(oneCity);
            }

            return WeatherTable;
        }
        private void Update(WeatherCallResult oneCity)
        {
            WeatherTable[oneCity.location.name] = oneCity;
        }
        public void Save()
        {

            aPICity.Save(WeatherTable);
        }
        public void Start_Auto_Request(string cityName, int numRefreshe)
        {

            Task.Factory.StartNew(async () =>
            {
                continuToRun = true;
                while (continuToRun)
                {
                    WeatherCallResult weatherCallResult;
                    weatherCallResult = await aPICity.GetCityData(cityName);

                    aPICity.Save(weatherCallResult);

                    System.Threading.Thread.Sleep(numRefreshe * 1000);
                }
            });


        }
        public void Stop_Auto_Request()
        {
            continuToRun = false;
        }
        public async Task UpdateList()
        {
            var listFromFile = aPICity.Load();
            foreach (var OneFile in listFromFile)
            {
                var result = await aPICity.GetCityData(OneFile.Key);

                if (OneFile.Value.current.temp_c != result.current.temp_c)
                {
                    Update(result);
                }
            }
            aPICity.Save(WeatherTable);

        }
    }
}
