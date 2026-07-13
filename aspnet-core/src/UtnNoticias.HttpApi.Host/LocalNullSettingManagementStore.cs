using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace UtnNoticias;

// Solo para ver Swagger local sin SQL Server.
// Libro: p.106 usa dependencias falsas en pruebas; aca usamos una dependencia vacia para desarrollo local.
public class LocalNullSettingManagementStore : ISettingManagementStore
{
    public Task<string?> GetOrNullAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult<string?>(null);
    }

    public Task<List<SettingValue>> GetListAsync(string providerName, string providerKey)
    {
        return Task.FromResult(new List<SettingValue>());
    }

    public Task<List<SettingValue>> GetListAsync(string[] names, string providerName, string providerKey)
    {
        return Task.FromResult(new List<SettingValue>());
    }

    public Task SetAsync(string name, string value, string providerName, string providerKey)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string name, string providerName, string providerKey)
    {
        return Task.CompletedTask;
    }
}
