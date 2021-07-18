using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OlegChibikov.SympliInterview.SeoChecker.Contracts;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data;
using OlegChibikov.SympliInterview.SeoChecker.Contracts.Data.Settings;
using OlegChibikov.SympliInterview.SeoChecker.Core;
using OlegChibikov.SympliInterview.SeoChecker.Core.Bing;
using OlegChibikov.SympliInterview.SeoChecker.Core.Google;

namespace OlegChibikov.SympliInterview.SeoChecker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            void RegisterBusinessLogicObjects()
            {
                void RegisterSearchEngineResultsRetriever(SearchEngine searchEngine)
                {
                    services.AddSingleton<ISearchEngineResultsRetriever>(
                        serviceProvider => new SearchEngineResultsRetriever(
                            serviceProvider.GetRequiredService<IHttpClientFactory>(),
                            serviceProvider.GetRequiredService<IOptionsMonitor<SearchEngineRetrieverSettings>>(),
                            serviceProvider.GetRequiredService<Func<SearchEngine, ISearchEngineResultsParser>>(),
                            serviceProvider.GetRequiredService<Func<SearchEngine, IQueryProvider>>(),
                            searchEngine));
                }

                services.AddSingleton<Func<SearchEngine, ISearchEngineResultsParser>>(
                    serviceProvider => searchEngine =>
                    {
                        return searchEngine switch
                        {
                            SearchEngine.Google => serviceProvider.GetRequiredService<GoogleSearchResultsParser>(),
                            SearchEngine.Bing => serviceProvider.GetRequiredService<BingSearchResultsParser>(),
                            _ => throw new ArgumentOutOfRangeException(nameof(searchEngine))
                        };
                    });

                services.AddSingleton<Func<SearchEngine, IQueryProvider>>(
                    serviceProvider => searchEngine =>
                    {
                        return searchEngine switch
                        {
                            SearchEngine.Google => serviceProvider.GetRequiredService<GoogleQueryProvider>(),
                            SearchEngine.Bing => serviceProvider.GetRequiredService<BingQueryProvider>(),
                            _ => throw new ArgumentOutOfRangeException(nameof(searchEngine))
                        };
                    });

                services.AddSingleton<IReferenceMatcher, ReferenceMatcher>();

                RegisterSearchEngineResultsRetriever(SearchEngine.Bing);
                RegisterSearchEngineResultsRetriever(SearchEngine.Google);

                // Parsers and uriPartProviders are registered not as interfaces. They will be retrieved using the factory
                services.AddSingleton<GoogleSearchResultsParser>();
                services.AddSingleton<BingSearchResultsParser>();

                services.AddSingleton<GoogleQueryProvider>();
                services.AddSingleton<BingQueryProvider>();
            }

            void RegisterSettings()
            {
                services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
                services.Configure<SearchEngineRetrieverSettings>(Configuration.GetSection(nameof(SearchEngineRetrieverSettings)));
            }

            void RegisterHttpClients()
            {
                // Distinct HttpClient should be registered for each distinct web address
                services.AddHttpClient(
                    SearchEngine.Google.GetName(),
                    c =>
                    {
                        c.BaseAddress = new Uri("https://www.google.com/");
                    });
                services.AddHttpClient(
                    SearchEngine.Bing.GetName(),
                    c =>
                    {
                        c.BaseAddress = new Uri("https://bing.com/");
                        c.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                        {
                            NoCache = true
                        };

                        c.DefaultRequestHeaders.TryAddWithoutValidation(
                            "User-Agent",
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    });
            }

            _ = services ?? throw new ArgumentNullException(nameof(services));

            services.AddControllers();
            services.AddCors(
                options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            var settings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
                            builder.WithOrigins(settings.WebAppHost).AllowAnyMethod().AllowCredentials().AllowAnyHeader();
                        });
                });

            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SeoChecker.Api", Version = "v1" });
                });

            RegisterHttpClients();
            RegisterSettings();
            RegisterBusinessLogicObjects();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _ = env ?? throw new ArgumentNullException(nameof(env));
            _ = app ?? throw new ArgumentNullException(nameof(app));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SeoChecker.Api v1"));
            }

            app.UseCors();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}