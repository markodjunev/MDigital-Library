using MDigitalLibrary.Identity.Data;
using MDigitalLibrary.Identity.Infrastructure;
using MDigitalLibrary.Infrastructure;
using MDigitalLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebService<IdentityDbContext>(builder.Configuration)
                .AddUserStorage()
                .AddTransient<IDataSeeder, IdentityDataSeeder>();

var app = builder.Build();

app.UseWebService(app.Environment)
    .Initialize()
    .Run();
