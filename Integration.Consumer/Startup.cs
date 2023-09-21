using Integration.Consumer.ResponseHandler;
using Integration.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zenoti.MessageBroker.Kafka.Extensions;
using Zenoti.MessageBroker.Kafka.Models;
using System;

namespace Integration.Consumer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            RegisterCodatConsumerServices(services);
        }

        /// <summary>
        /// Register Services related to Codat
        /// </summary>
        /// <param name="services"></param>
        private void RegisterCodatConsumerServices(IServiceCollection services)
        {
            services.AddHttpClient<CodatResponseHandler>();
            services.AddSingleton(Configuration);
            // Get Codat Consumer values from appsettings-codat.json
            var codatConsumerConfiguration = Configuration
                                    .GetSection("Codat:ConsumerConfiguration")
                                    .Get<KafkaConsumerConfigModel>();
            
            // Bind model and response handler to the service extension in Kafka.
            //TODO: Determine where we should get BootstrapServers from.
            services.AddKafkaConsumer<string, CodatEvent, CodatResponseHandler>(consumerConfig =>
            {
                consumerConfig.BootstrapServers = codatConsumerConfiguration.BootstrapServers;
                consumerConfig.GroupId = codatConsumerConfiguration.GroupId;
                // Throws an error if topic is not present. Kafka does not recommend creating a topic
                consumerConfig.Topic = codatConsumerConfiguration.Topic;
                consumerConfig.TimeOut = codatConsumerConfiguration.TimeOut;
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                                   name: "default",
                                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
