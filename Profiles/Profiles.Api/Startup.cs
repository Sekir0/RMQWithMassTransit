using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Profile.Domain;
using Profile.MongoDb;
using Profiles.Api.Helpers;

namespace Profiles.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        private const string DefaultDbName = "profiles";
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Profiles.Api", Version = "v1"
                });
                
                options.IncludeXmlComments(XmlPathProvider.XmlPath);
            });
            
            services.AddScoped(_ =>
            {
                var connectionString = Configuration["MONGO_CONNECTION"];
                var mongoUrl = MongoUrl.Create(connectionString);
                var client = new MongoClient(mongoUrl);
                return new DbContext(client.GetDatabase(mongoUrl.DatabaseName ?? DefaultDbName));
            });

            services.AddScoped<IProfileStorage, ProfileStorage>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IRelationsService, RelationsStorage>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profiles.Api v1");
                    c.DisplayRequestDuration();
                });
                
                var profileStorage = provider.GetRequiredService<IProfileStorage>();
                var relationService = provider.GetRequiredService<IRelationsService>();
                //SeedAsync(profileStorage, relationService).GetAwaiter().GetResult();
            }

            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // private async Task SeedAsync(IProfileStorage profileStorage, IRelationsService relationsService)
        // {
        //     var alice = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66af87");
        //     if (await profileStorage.FindByIdAsync(alice) == null)
        //     {
        //         await profileStorage.InsertAsync(new Profile.Domain.Profile(alice, "Alice", "Alice", "Female", null, null));
        //         for (int i = 0; i < 100000; i++)
        //         {
        //             await profileStorage.InsertAsync(new Profile.Domain.Profile(Guid.NewGuid(), "Follower", $"{i}", "Female", null, null));
        //         }
        //     }
        //
        //     var (_, totalCountFollowers) = await relationsService.SearchFollowersAsync(0, 1, alice);
        //     
        //     if (totalCountFollowers == 0)
        //     {
        //         var (profiles, _) = await profileStorage.SearchProfilesAsync(1, 100000);
        //
        //         foreach (var profile in profiles)
        //         {
        //             // TODO test create news
        //             await relationsService.SendRequestAsync(profile, alice);
        //             
        //             // TODO test performance of news
        //             //await relationsService.SendRequestAsync(alice, profile);
        //         }
        //     }
        //     
        //     // just wait couple of minutes)) 
        // }
    }
}