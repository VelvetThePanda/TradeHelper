// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TradeHelper.Data;
using TradeHelper.Data.MediatR;

var services = new ServiceCollection()
    .AddDbContext<TradeContext>()
    .AddMediatR(typeof(TradeContext))
    .BuildServiceProvider();
    
var mediator = services.GetService<IMediator>();

await mediator.Send(new UserExists.Request(default));