﻿using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,
                                            Action<TContext, IServiceProvider> seeder,
                                            int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope()) //To get logger and other services from IServices
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>(); // get DB context from entity framework
                
                var configuration = services.GetRequiredService<IConfiguration>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    logger.LogCritical("*************************");
                    logger.LogCritical(configuration.GetConnectionString("OrderingConnectionString"));
                    logger.LogCritical(context.Database.GetConnectionString());
                    logger.LogCritical("*************************");

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                    }
                }
                
            }

            return host;

        }

        private static void InvokeSeeder<TContext>(Action<TContext,
                                                   IServiceProvider> seeder,
                                                   TContext context,
                                                   IServiceProvider services)
                                                   where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }

    }
}

