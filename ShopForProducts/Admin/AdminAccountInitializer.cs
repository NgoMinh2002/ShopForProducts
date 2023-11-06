
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
namespace ShopForProducts.Admin
{
    public class AdminAccountInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AdminAccountInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var adminAccountService = scope.ServiceProvider.GetRequiredService<AdminServices>();
                adminAccountService.InitializeAdminAccount();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}