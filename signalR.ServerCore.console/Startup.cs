using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using signalR.ServerCore.Console.Hubs;
using Microsoft.AspNetCore.SignalR;
using signalR.ServerCore.Clients;
using System.Threading.Tasks;

namespace signalR.ServerCore.console
{
    public class AppSettings
    {
        public string BackplaneUrl { get; set; }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;       

            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            _backplaneClient = new BackplaneClient(appSettings.BackplaneUrl);
        }

        public IConfiguration Configuration { get; }
        private BackplaneClient _backplaneClient { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>{
            builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                //.WithOrigins("http://localhost:4200")
                .AllowAnyOrigin()
                .AllowCredentials()
                ;
            }));
            services.AddSignalR();            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            /* Swashbuckle         */            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My API", Version = "v1" });
            });            

            //services.AddSingleton<INotifyHub, NotifyHub>();
            services.AddSingleton<Hub<ITypedHubClient>, NotifyHub>();
            services.AddSingleton<SimpleHub>();

            services.AddSingleton<BackplaneClient>(_backplaneClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            /* NSwag 
            app.UseSwagger();
            app.UseSwaggerUi3();
            */

            /* Swashbuckle */
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
 
            app.Use((context, next) =>
            {
                //string[] incomingOriginValue;
                if (context.Request.Headers.TryGetValue("origin", out var incomingOriginValue))
                {
                    var origin = incomingOriginValue.ToArray()[0];
                    if (origin.StartsWith("http://localhost:") || origin.StartsWith("http://127.0.0.1:"))
                    {
                        context.Response.Headers.Add("Access-Control-Allow-Origin", new string[] { origin });
                        context.Response.Headers.Add("Access-Control-Allow-Credentials", new string[] { "true" });
                        context.Response.Headers.Add("Access-Control-Allow-Methods", new string[] { "HEAD,GET,PUT,POST,DELETE,OPTIONS" });
                        context.Response.Headers.Add("Access-Control-Allow-Headers", new string[] { "Content-Type,TestHeader,another-header" });
                        context.Response.Headers.Add("Access-Control-Expose-Headers", new string[] { "MyCustomHeader,TestHeader,another-header" });
                    }
                }

                return next();
            });

            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotifyHub>("/notify");
                routes.MapHub<SimpleHub>("/simplehub");
            });

            app.UseHttpsRedirection();
            app.UseMvc();

            RegisterInstanceInBackplane();
        }

        private void RegisterInstanceInBackplane()
        {
            // TODO: 
            // read url on startup
            // read response and log possible error when backplane not available
            var task = Task.Run(async () => await _backplaneClient.AddInstance("localhost:5000")); 
        }
    }
}
