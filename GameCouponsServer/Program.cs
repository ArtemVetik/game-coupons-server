﻿var handler = new GameCouponsServer.Handler();
var response = await handler.FunctionHandler(null, default);
Console.WriteLine(response);