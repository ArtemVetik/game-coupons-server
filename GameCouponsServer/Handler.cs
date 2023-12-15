using Yandex.Cloud.Functions;

namespace GameCouponsServer
{
    public class Handler : YcFunction<object, Task<string>>
    {
        public async Task<string> FunctionHandler(object request, Context context)
        {
            return await LoginRequest.CreateRequest();
        }
    }
}
