using CommentAPI.Entities;
using CommentAPI.Extensions;
using CommentAPI.Models;
using CommentAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace CommentAPI
{
    public class Startup
    {
        // public IConfigurationRoot Configuration { get; }
        public static IConfigurationRoot Configuration;     // to use it in services

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            //// If want to manipulate JSON serializer settings, do it here (optional)
            //.AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;
            //    }
            //});

            // Add application services.
            // use compiler directives
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=CommentInfoDB;Trusted_Connection=True;";
            services.AddDbContext<CommentInfoContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<ICommentInfoRepository, CommentInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            CommentInfoContext commentInfoContext)
        {
            // The logger is a built-in service in ASP.NET Core, so don't need to add it to the container in ConfigureServices
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            // Add third-party logger https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-(csproj---vs2017)
            // Add NLog to ASP.NET Core
            loggerFactory.AddNLog();
            // Add NLog.Web
            app.AddNLogWeb();

            commentInfoContext.EnsureSeedDataForContext();

            // Without this line, 404 message only appears in DeveloperTool-Console
            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Comment, CommentWithoutSubDto>();
                cfg.CreateMap<Comment, CommentDto>();
                cfg.CreateMap<SubComment, SubCommentDto>();
                cfg.CreateMap<SubCommentForCreationDto, SubComment>();
                cfg.CreateMap<SubCommentForUpdateDto, SubComment>();
            });

            app.UseMvc();
        }
    }
}
