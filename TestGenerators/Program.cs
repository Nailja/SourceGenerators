using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestGenerators;
using TestGenerators.Contract;
using TestGenerators.Generated;

var builder = new HostBuilder();

string connection = "Data Source = D:\\SourceGenerators\\TestGenerators\\DataBase\\meteoData.sqlite";

MeteoDataRepository repo = new(connection);

builder.ConfigureServices((context, services) =>
{
    services.AddScoped<ITestRepositoryOperations>(x => new TestRepositoryOperations(repo));
});

IHost host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var testRepo = services.GetRequiredService<ITestRepositoryOperations>();
    testRepo.GetAllOpertation();
    testRepo.TestGetElementOperation(5);
    testRepo.InsertOperation();
    testRepo.UpdateOperation(4393);
    testRepo.DeleteOperation(4393);
}

await host.RunAsync();