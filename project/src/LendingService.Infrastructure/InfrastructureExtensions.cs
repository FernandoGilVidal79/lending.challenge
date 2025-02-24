using LendingService.Core.Ports;
using LendingService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LendingService.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static void AddPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("LendingServiceInMemoryDb"));


            serviceCollection.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}
