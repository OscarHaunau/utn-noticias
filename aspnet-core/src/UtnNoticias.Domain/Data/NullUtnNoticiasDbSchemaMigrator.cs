using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace UtnNoticias.Data;

/* This is used if database provider does't define
 * IUtnNoticiasDbSchemaMigrator implementation.
 */
public class NullUtnNoticiasDbSchemaMigrator : IUtnNoticiasDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
