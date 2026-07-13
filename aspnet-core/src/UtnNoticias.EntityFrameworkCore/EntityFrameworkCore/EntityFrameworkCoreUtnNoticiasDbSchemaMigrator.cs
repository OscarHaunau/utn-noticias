using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UtnNoticias.Data;
using Volo.Abp.DependencyInjection;

namespace UtnNoticias.EntityFrameworkCore;

public class EntityFrameworkCoreUtnNoticiasDbSchemaMigrator
    : IUtnNoticiasDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreUtnNoticiasDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the UtnNoticiasDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<UtnNoticiasDbContext>()
            .Database
            .MigrateAsync();
    }
}
