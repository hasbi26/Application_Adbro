using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using UserInterestsAPIService.Services;
using Microsoft.Azure.Cosmos;

namespace UserInterestsAPIService
{
    internal sealed class UserInterestsAPIService : StatelessService
    {
        public UserInterestsAPIService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        var builder = WebApplication.CreateBuilder();

                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);

                        // Add services to the container.
                        builder.Services.AddControllers();
                        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                        builder.Services.AddEndpointsApiExplorer();
                        builder.Services.AddSwaggerGen();

                        // Add Cosmos DB service to the container.
                        builder.Services.AddSingleton<I_IntrstsCosmosService>(options =>
                        {
                            string cosmosUrl = builder.Configuration.GetSection("AzureCosmosDbSettings")
                                .GetValue<string>("URL");
                            string cosmosKey = builder.Configuration.GetSection("AzureCosmosDbSettings")
                                .GetValue<string>("PrimaryKey");
                            string cosmosDbName = builder.Configuration.GetSection("AzureCosmosDbSettings")
                                .GetValue<string>("DatabaseName");
                            string cosmosContainerName = builder.Configuration.GetSection("AzureCosmosDbSettings")
                                .GetValue<string>("ContainerName");

                            var cosmosClient = new CosmosClient(cosmosUrl, cosmosKey);
                            return new IntrstsCosmosService(cosmosClient, cosmosDbName, cosmosContainerName);
                        });

                        builder.WebHost
                            .UseKestrel()
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                            .UseUrls(url);

                        var app = builder.Build();

                        // Configure the HTTP request pipeline.
                        if (app.Environment.IsDevelopment())
                        {
                            app.UseSwagger();
                            app.UseSwaggerUI();
                        }

                        app.UseAuthorization();

                        app.MapControllers();

                        return app;
                    }))
            };
        }
    }
}




//using System;
//using System.Collections.Generic;
//using System.Fabric;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
//using Microsoft.ServiceFabric.Services.Communication.Runtime;
//using Microsoft.ServiceFabric.Services.Runtime;
//using Microsoft.ServiceFabric.Data;
//using UserInterestsAPIService.Services;
//using Microsoft.Azure.Cosmos;

//namespace UserInterestsAPIService
//{
//    internal sealed class UserInterestsAPIService : StatelessService
//    {
//        public UserInterestsAPIService(StatelessServiceContext context)
//            : base(context)
//        { }

//        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
//        {
//            return new ServiceInstanceListener[]
//            {
//                new ServiceInstanceListener(serviceContext =>
//                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
//                    {
//                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

//                        var builder = WebApplication.CreateBuilder();

//                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);

//                        builder.WebHost
//                                    .UseKestrel()
//                                    .UseContentRoot(Directory.GetCurrentDirectory())
//                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
//                                    .UseUrls(url);

//                        // Add services to the container.

//                        builder.Services.AddControllers();
//                        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//                        builder.Services.AddEndpointsApiExplorer();
//                        builder.Services.AddSwaggerGen();

//                        var app = builder.Build();

//                        // Configure the HTTP request pipeline.
//                        if (app.Environment.IsDevelopment())
//                        {
//                        app.UseSwagger();
//                        app.UseSwaggerUI();
//                        }

//                        app.UseAuthorization();

//                        app.MapControllers();


//                        return app;


//                    }))
//            };
//        }
//    }
//}








//builder.Services.AddSingleton<I_IntrstsCosmosService>(options =>
//{
//    string url = builder.Configuration.GetSection("AzureCosmosDbSettings")
//    .GetValue<string>("URL");
//    string primaryKey = builder.Configuration.GetSection("AzureCosmosDbSettings")
//    .GetValue<string>("PrimaryKey");
//    string dbName = builder.Configuration.GetSection("AzureCosmosDbSettings")
//    .GetValue<string>("DatabaseName");
//    string containerName = builder.Configuration.GetSection("AzureCosmosDbSettings")
//    .GetValue<string>("ContainerName");
//    var cosmosClient = new CosmosClient(
//        url,
//        primaryKey
//    );
//    return new IntrstsCosmosService(cosmosClient, dbName, containerName);
//});