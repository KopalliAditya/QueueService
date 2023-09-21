using Integration.Models;
using Integration.Producer.Services;
using Integration.Producer.Services.IServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zenoti.MessageBroker.Kafka.Extensions;
using Zenoti.MessageBroker.Kafka.Models;

namespace Integration.Producer
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
            services.AddControllers();

            RegisterCodatKafkaProducerServices(services);
        }

        private void RegisterCodatKafkaProducerServices(IServiceCollection services)
        {
            // Add singleton in the service layer
            services.AddKafkaServiceProducer();

            // Get Codat Producer Confiuration from appsettings-codat.json
            var codatProducerConfiguration = Configuration
                                     .GetSection("Codat:ProducerConfiguration")
                                     .Get<KafkaProducerConfigModel>();

            //TODO: Determine where we should get BootstrapServers from.
            services.AddKafkaProducer<string, CodatEvent>(producerConfig =>
            {
                producerConfig.BootstrapServers = codatProducerConfiguration.BootstrapServers;
                producerConfig.Topic = codatProducerConfiguration.Topic;
            });

            services.AddScoped<ICodatService, CodatService>();
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
                endpoints.MapControllers();
            });
        }
    }
}
