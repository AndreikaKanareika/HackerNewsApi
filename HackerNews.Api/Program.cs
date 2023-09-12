using HackerNews.Api.Helpers;
using HackerNews.Api.Services;

namespace HackerNews.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;

            // Add services to the container.
            services.AddOptions<CacheOptions>().Bind(configuration.GetSection("CacheSettings"));

            var baseUrl = configuration.GetValue<string>("HackerNewsApiSettings:BaseUrl");
            var retryCount = configuration.GetValue<int?>("HackerNewsApiSettings:RetryCount");

            services.AddMemoryCache();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHackerNewsApiClient(baseUrl, retryCount ?? 0);
            services.AddTransient<IHackerNewsApiService, CachedHackerNewsApiService>();
            services.AddTransient<IHackerNewsService, HackerNewsService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}