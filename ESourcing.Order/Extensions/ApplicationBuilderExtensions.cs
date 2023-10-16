﻿using ESourcing.Order.Consumers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ESourcing.Order.Extensions
{
    public static class ApplicationBuilderExtensions //ugulama start veya stop oldugunda comsume işleminin baslayıp durması icin middleware yazdık, startup da configure altında cagıracagız.
    {
        public static EventBusOrderCreateConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app) 
        {
            Listener = app.ApplicationServices.GetService<EventBusOrderCreateConsumer>();

            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}