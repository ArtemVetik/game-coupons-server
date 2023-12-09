using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace GameCouponsServer
{
    public static class CityListRequest
    {
        private const string LongitudeHeader = "longitude";
        private const string LatitudeHeader = "latitude";

        public static async Task<string> CreateRequest(Request request)
        {
            var url = Environment.GetEnvironmentVariable("city_data_url");

            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var csvStream = await response.Content.ReadAsStreamAsync();

            var cityList = new List<City>();

            var parser = new TextFieldParser(csvStream);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(new string[] { ";" });

            var headRow = parser.ReadFields();
            FindIndexes(headRow, request.body, out int longitudeColumn, out int latitudeColumn, out int nameColumn);

            while (!parser.EndOfData)
            {
                string[] columns = parser.ReadFields();
                cityList.Add(new City
                {
                    Name = columns[nameColumn],
                    Longitude = float.Parse(columns[longitudeColumn]),
                    Latitude = float.Parse(columns[latitudeColumn]),
                });
            }

            var cityListObject = new
            {
                Data = cityList,
            };

            return JsonConvert.SerializeObject(cityListObject);
        }

        private static void FindIndexes(string[] headers, string language, out int longitudeColumn, out int latitudeColumn, out int nameColumn)
        {
            longitudeColumn = -1;
            latitudeColumn = -1;
            nameColumn = -1;

            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == LongitudeHeader)
                    longitudeColumn = i;
                else if (headers[i] == LatitudeHeader)
                    latitudeColumn = i;
                else if (headers[i] == language)
                    nameColumn = i;
            }

            if (longitudeColumn < 0 || latitudeColumn < 0 || nameColumn < 0)
                throw new InvalidOperationException("Can't find column indexes");
        }
    }
}
