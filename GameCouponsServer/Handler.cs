using Newtonsoft.Json;
using System.Text;
using Yandex.Cloud.Functions;

namespace GameCouponsServer
{
    public class Response
    {
        public string access { get; set; }
        public string refresh { get; set; }
    }

    public class Request
    {
        public int id { get; set; }
        public string password { get; set; }
    }

    public class Handler : YcFunction<object, Task<Response>>
    {
        public async Task<Response> FunctionHandler(object request, Context context)
        {
            string id = Environment.GetEnvironmentVariable("id");
            string password = Environment.GetEnvironmentVariable("password");

            var postData = new Request()
            {
                id = int.Parse(id),
                password = password,
            };

            using var client = new HttpClient();
            using var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
            using var response = await client.PostAsync($"https://stage.game-coupon.com/api/v1/login", content);

            var responseString = await response.Content.ReadAsStringAsync();

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException exception)
            {
                throw new HttpRequestException(responseString, exception);
            }

            return JsonConvert.DeserializeObject<Response>(responseString);
        }
    }
}
