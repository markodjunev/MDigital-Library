using MDigitalLibrary.Catalog.Data;
using MDigitalLibrary.Catalog.Data.Seeders;
using MDigitalLibrary.Infrastructure;
using MDigitalLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebService<CatalogDbContext>(builder.Configuration)
                .AddTransient<IDataSeeder, LibraryDataSeeder>()
                .AddTransient<IDataSeeder, AuthorDataSeeder>();

var app = builder.Build();

app.UseWebService(app.Environment)
    .Initialize()
    .Run();
