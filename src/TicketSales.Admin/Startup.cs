using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TicketSales.Core.Application;
using TicketSales.Core.Database;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Mapper;

namespace TicketSales.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<SaveConcertCommandHandler>();
            services.AddScoped<GetAllConcertsCommandHandler>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SaveConcertCommandHandler>();
                x.AddConsumer<GetAllConcertsCommandHandler>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host("localhost", "zoran", h => { h.Username("guest"); h.Password("guest"); });

                    cfg.ReceiveEndpoint(host, "admin", e =>
                    {
                        e.PrefetchCount = 16;

                        e.ConfigureConsumer<SaveConcertCommandHandler>(provider);
                        e.ConfigureConsumer<GetAllConcertsCommandHandler>(provider);

                        EndpointConvention.Map<GetAllConcertsCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                        EndpointConvention.Map<SaveConcertCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                    });
                }));
            });

            services.AddHostedService<BusService>();
            services.AddDbContext<DataContext>();
            services.AddTransient<IDataContext, DataContext>();
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<SaveConcertCommand>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetAllConcertsCommand>());

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ConcertMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
