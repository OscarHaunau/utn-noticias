using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace UtnNoticias.ReadingLists;

public interface IReadingListAppService : IApplicationService
{
	Task<ICollection<ReadingListDto>> GetListAsync();
	Task<ReadingListDto> GetAsync(Guid id);
	Task<ReadingListDto> CreateAsync(CreateReadingListDto input);
	Task<ReadingListDto> UpdateAsync(Guid id, UpdateReadingListDto input);
	Task DeleteAsync(Guid id);
	Task<ReadingListDto> AddItemAsync(Guid id, AddReadingListItemDto input);
}

