using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Tests.Helpers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using ToDo.Server.Services.Interfaces;
    using ToDo.Server;
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IToDoService> MockService { get; private set; } = new Mock<IToDoService>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing service registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IToDoService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Register the mock service
                services.AddSingleton(MockService.Object);
            });
        }
    }

}
