﻿using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Twitter
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // setup type conversion for object id
            TypeConversion.Default.Register<string, ObjectId>(from => ObjectId.Parse(from));
            TypeConversion.Default.Register<ObjectId, string>(from => from.ToString());

            // setup the repositories
            services.AddSingleton<IMongoClient>(new MongoClient("mongodb://127.0.0.1:27017"));
            services.AddSingleton<IMongoDatabase>(s => s.GetRequiredService<IMongoClient>().GetDatabase("PagingDemo"));
            services.AddSingleton<IMongoCollection<Message>>(s => s.GetRequiredService<IMongoDatabase>().GetCollection<Message>("messages"));
            services.AddSingleton<IMongoCollection<User>>(s => s.GetRequiredService<IMongoDatabase>().GetCollection<User>("users"));
            services.AddSingleton<MessageRepository>();
            services.AddSingleton<UserRepository>();

            // Add GraphQL Services
            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .BindClrType<ObjectId, StringType>()
                    .Create(),
                    new QueryExecutionOptions
          {
              TracingPreference = TracingPreference.Always
          });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseGraphQL();
        }
    }
}
