// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using TradeHelper.Data;

var context = new TradeContext();

context.Database.MigrateAsync();