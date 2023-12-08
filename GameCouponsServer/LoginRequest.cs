using Newtonsoft.Json;
using System.Text;

namespace GameCouponsServer
{
    public static class LoginRequest
    {
        public static async Task<string> CreateRequest()
        {
            string id = Environment.GetEnvironmentVariable("id");
            string password = Environment.GetEnvironmentVariable("password");

            var postData = new Login()
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

            return responseString;
        }
    }
}
