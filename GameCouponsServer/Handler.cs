using Yandex.Cloud.Functions;

namespace GameCouponsServer
{
    public class Handler : YcFunction<Request, Task<string>>
    {
        public async Task<string> FunctionHandler(Request request, Context context)
        {
            switch (request.method)
            {
                case "LOGIN":
                    return await LoginRequest.CreateRequest();
                case "CITY_LIST":
                    return await CityListRequest.CreateRequest(request);
                default:
                    throw new InvalidOperationException($"Method {request.method} not found.");
            }
        }
    }
}
