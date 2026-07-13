using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UtnNoticias.Permissions;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace UtnNoticias.ReadingLists;

[Authorize(UtnNoticiasPermissions.ReadingLists.Default)]
public class ReadingListAppService : UtnNoticiasAppService, IReadingListAppService
{
	private readonly IRepository<ReadingList, Guid> _readingListRepository;

	public ReadingListAppService(IRepository<ReadingList, Guid> readingListRepository)
	{
		_readingListRepository = readingListRepository;
	}

	public async Task<ICollection<ReadingListDto>> GetListAsync()
	{
		var ownerId = GetCurrentUserId();
		var queryable = await _readingListRepository.WithDetailsAsync(x => x.Items);
		var query = queryable
			.Where(x => x.OwnerId == ownerId)
			.OrderBy(x => x.Name);

		var lists = await AsyncExecuter.ToListAsync(query);
		return ObjectMapper.Map<List<ReadingList>, ICollection<ReadingListDto>>(lists);
	}

	public async Task<ReadingListDto> GetAsync(Guid id)
	{
		var list = await GetOwnedReadingListAsync(id, includeItems: true);
		return ObjectMapper.Map<ReadingList, ReadingListDto>(list);
	}

	[Authorize(UtnNoticiasPermissions.ReadingLists.Create)]
	public async Task<ReadingListDto> CreateAsync(CreateReadingListDto input)
	{
		var ownerId = GetCurrentUserId();
		var list = new ReadingList(GuidGenerator.Create(), ownerId, input.Name);
		list = await _readingListRepository.InsertAsync(list, autoSave: true);
		return ObjectMapper.Map<ReadingList, ReadingListDto>(list);
	}

	[Authorize(UtnNoticiasPermissions.ReadingLists.Update)]
	public async Task<ReadingListDto> UpdateAsync(Guid id, UpdateReadingListDto input)
	{
		var list = await GetOwnedReadingListAsync(id, includeItems: true);
		list.SetName(input.Name);
		list = await _readingListRepository.UpdateAsync(list, autoSave: true);
		return ObjectMapper.Map<ReadingList, ReadingListDto>(list);
	}

	[Authorize(UtnNoticiasPermissions.ReadingLists.Delete)]
	public async Task DeleteAsync(Guid id)
	{
		var list = await GetOwnedReadingListAsync(id, includeItems: false);
		await _readingListRepository.DeleteAsync(list, autoSave: true);
	}

	[Authorize(UtnNoticiasPermissions.ReadingLists.AddItem)]
	public async Task<ReadingListDto> AddItemAsync(Guid id, AddReadingListItemDto input)
	{
		var list = await GetOwnedReadingListAsync(id, includeItems: true);
		list.AddItem(
			input.Title,
			input.Url,
			input.Author,
			input.Description,
			input.UrlToImage,
			input.PublishedAt,
			input.Content
		);

		list = await _readingListRepository.UpdateAsync(list, autoSave: true);
		return ObjectMapper.Map<ReadingList, ReadingListDto>(list);
	}

	private async Task<ReadingList> GetOwnedReadingListAsync(Guid id, bool includeItems)
	{
		var ownerId = GetCurrentUserId();
		var queryable = includeItems
			? await _readingListRepository.WithDetailsAsync(x => x.Items)
			: await _readingListRepository.GetQueryableAsync();

		var query = queryable.Where(x => x.Id == id && x.OwnerId == ownerId);
		var list = await AsyncExecuter.FirstOrDefaultAsync(query);
		if (list is null)
		{
			throw new EntityNotFoundException(typeof(ReadingList), id);
		}

		return list;
	}

	private Guid GetCurrentUserId()
	{
		var userId = CurrentUser.Id;
		if (!userId.HasValue)
		{
			throw new AbpAuthorizationException("Current user is not authenticated.");
		}

		return userId.Value;
	}
}
