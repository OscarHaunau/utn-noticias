using System.Threading.Tasks;

namespace UtnNoticias.Data;

public interface IUtnNoticiasDbSchemaMigrator
{
    Task MigrateAsync();
}
