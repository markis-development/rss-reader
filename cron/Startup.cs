using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Chroniton;
using Chroniton.Jobs;
using Chroniton.Schedules;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace app
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            
            //var connection = @"Server=db:3306;Database=db;Uid=root;Pwd=dbroot;";
            
            //services.AddDbContext<BloggingContext>(options => options.UseSqlServer(connection));
            //services.AddDbContext<BloggingContext>(options => options.UseMySQL(Configuration.GetConnectionString("SqlConnection")));
            services.AddSingleton<ISingularity, Singularity>(serviceProvider => Singularity.Instance);
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            // {
            //      var context = serviceScope.ServiceProvider.GetService<BloggingContext>();
            //      context.Database.Migrate();
            // }
            var singularity = Singularity.Instance;
            singularity.Start();
        }


    }
}
