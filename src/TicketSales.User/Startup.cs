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

namespace TicketSales.User
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
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<GetAllConcertsCommandHandler>();
            services.AddSingleton<GetTicketByUserIdCommandHandler>();
            services.AddScoped<BuyTicketCommandHandler>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetAllConcertsCommandHandler>();
                x.AddConsumer<GetTicketByUserIdCommandHandler>();
                x.AddConsumer<BuyTicketCommandHandler>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host("localhost", "zoran", h => { h.Username("guest"); h.Password("guest"); });

                    cfg.ReceiveEndpoint(host, "user", e =>
                    {
                        e.PrefetchCount = 16;

                        e.ConfigureConsumer<GetAllConcertsCommandHandler>(provider);
                        e.ConfigureConsumer<GetTicketByUserIdCommandHandler>(provider);
                        e.ConfigureConsumer<BuyTicketCommandHandler>(provider);

                        EndpointConvention.Map<GetAllConcertsCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                        EndpointConvention.Map<GetTicketByUserIdCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                        EndpointConvention.Map<BuyTicketCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                    });
                }));
            });

            services.AddHostedService<BusService>();
            services.AddDbContext<DataContext>();
            services.AddTransient<IDataContext, DataContext>();
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetAllConcertsCommand>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetTicketByUserIdCommand>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<BuyTicketCommand>());

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ConcertMapper());
                mc.AddProfile(new TicketMapper());
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
