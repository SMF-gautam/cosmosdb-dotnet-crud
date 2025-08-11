
using CosmosDB.IService;
using CosmosDB.Service;
using Microsoft.Azure.Cosmos;

namespace CosmosDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Access configuration  
            var configuration = builder.Configuration;
            var cosmosSection = configuration.GetSection("CosmosDb");
            string account = cosmosSection["Account"]!;
            string key = cosmosSection["Key"]!;
            string dbName = cosmosSection["DatabaseName"]!;
            string contName = cosmosSection["ContainerName"]!;

            // Register CosmosClient and CosmosDbService  
            builder.Services.AddSingleton(s => new CosmosClient(account, key));
            builder.Services.AddSingleton<ICosmosDbService>(s =>
            {
                var client = s.GetRequiredService<CosmosClient>();
                var logger = s.GetRequiredService<ILogger<CosmosDbService>>();
                return new CosmosDbService(client, dbName, contName, logger);
            });

            builder.Services.AddControllers();

            // Add Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
