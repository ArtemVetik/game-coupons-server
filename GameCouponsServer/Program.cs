var handler = new GameCouponsServer.Handler();
var response = await handler.FunctionHandler(new GameCouponsServer.Request(), default);
Console.WriteLine(response);