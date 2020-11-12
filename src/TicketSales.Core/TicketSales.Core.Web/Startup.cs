using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using TicketSales.Core.Application;
using TicketSales.Core.Database;
using TicketSales.Core.Database.Interfaces;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Mapper;

namespace TicketSales.Core.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SaveConcertCommandHandler>();
            services.AddScoped<GetAllConcertsCommandHandler>();
            services.AddScoped<GetTicketByUserIdCommandHandler>();
            services.AddScoped<BuyTicketCommandHandler>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SaveConcertCommandHandler>();
                x.AddConsumer<GetAllConcertsCommandHandler>();
                x.AddConsumer<GetTicketByUserIdCommandHandler>();
                x.AddConsumer<BuyTicketCommandHandler>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host("localhost", "zoran", h => { h.Username("guest"); h.Password("guest"); });

                    cfg.ReceiveEndpoint(host, "core", e =>
                    {
                        e.PrefetchCount = 16;

                        e.ConfigureConsumer<GetAllConcertsCommandHandler>(provider);
                        e.ConfigureConsumer<SaveConcertCommandHandler>(provider);
                        e.ConfigureConsumer<GetTicketByUserIdCommandHandler>(provider);
                        e.ConfigureConsumer<BuyTicketCommandHandler>(provider);

                        EndpointConvention.Map<SaveConcertCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                        EndpointConvention.Map<BuyTicketCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                        EndpointConvention.Map<GetAllConcertsCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                        EndpointConvention.Map<GetTicketByUserIdCommand>(new Uri("rabbitmq://localhost/zoran/core"));
                    });
                }));
            });

            services.AddHostedService<BusService>();
            services.AddDbContext<DataContext>();
            services.AddTransient<IDataContext, DataContext>();
            services.AddCors();
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetAllConcertsCommand>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetTicketByUserIdCommand>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<SaveConcertCommand>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<BuyTicketCommand>());

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ConcertMapper());
                mc.AddProfile(new TicketMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
