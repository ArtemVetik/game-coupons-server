using Newtonsoft.Json;

namespace GameCouponsServer
{
    public static class CityListRequest
    {
        public static async Task<string> CreateRequest()
        {
            var url = Environment.GetEnvironmentVariable("city_data_url");

            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var cityList = new List<City>();

            var rows = content.Split('\n');
            foreach (var row in rows)
            {
                var columns = row.Split(';');
                
                if (columns.Length == 3)
                {
                    cityList.Add(new City
                    {
                        Name = columns[0],
                        Longitude = float.Parse(columns[1]),
                        Latitude = float.Parse(columns[2]),
                    });
                }
            }

            var cityListObject = new
            {
                Data = cityList,
            };

            return JsonConvert.SerializeObject(cityListObject);
        }
    }
}
