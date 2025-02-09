using System;
using Fpl.Client.Abstractions;
using Fpl.Client.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fpl.Client.Infra
{
    public static class IFplApiClientServiceCollectionExtensions
    {
        public static IServiceCollection AddFplApiClient(this IServiceCollection services, IConfiguration config)
        {
            AddFplApiClient(services);
            services.Configure<FplApiClientOptions>(config);
            return services;
        }
        
        public static IServiceCollection AddFplApiClient(this IServiceCollection services, Action<FplApiClientOptions> configurator)
        {
            AddFplApiClient(services);
            services.Configure<FplApiClientOptions>(configurator);
            return services;
        }

        private static void AddFplApiClient(IServiceCollection services)
        {
            services.AddHttpClient<IEntryClient, EntryClient>();
            services.AddHttpClient<IEntryHistoryClient, EntryHistoryClient>();
            services.AddHttpClient<IFixtureClient, FixtureClient>();
            services.AddHttpClient<IGameweekClient, GameweekClient>();
            services.AddHttpClient<ILeagueClient, LeagueClient>();
            services.AddHttpClient<IPlayerClient, PlayerClient>();
            services.AddHttpClient<ITeamsClient, TeamsClient>();
            services.AddHttpClient<ITransfersClient, TransfersClient>();
            services.ConfigureOptions<FplClientOptionsConfigurator>();
            services.AddSingleton<Authenticator>();
            services.AddSingleton<CookieFetcher>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<CookieCache>();
            services.AddSingleton<FplHttpHandler>();
        }
    }
}