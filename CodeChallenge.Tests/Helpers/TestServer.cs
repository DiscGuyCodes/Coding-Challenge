using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.Tests.Integration.Helpers
{
    public class TestServer : IDisposable, IAsyncDisposable
    {
        private WebApplicationFactory<Program> applicationFactory;

        public TestServer()
        {
            applicationFactory = new WebApplicationFactory<Program>();
        }

        public HttpClient NewClient()
        {
            return applicationFactory.CreateClient();
        }

        public void AddEmployee(Employee employee)
        {
            using (var scope = applicationFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
                context.Employees.Add(employee);
                context.SaveChanges();
            }
        }

        public void AddCompensation(Compensation compensation)
        {
            using (var scope = applicationFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CompensationContext>();
                context.Compensations.Add(compensation);
                context.SaveChanges();
            }
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)applicationFactory).DisposeAsync();
        }

        public void Dispose()
        {
            ((IDisposable)applicationFactory).Dispose();
        }
    }
}
