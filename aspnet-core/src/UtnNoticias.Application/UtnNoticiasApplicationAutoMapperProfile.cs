using AutoMapper;
using UtnNoticias.Alerts;
using UtnNoticias.News;
using UtnNoticias.ReadingLists;
using UtnNoticias.Themes;
using UtnNoticias.User;
using Volo.Abp.Identity;

namespace UtnNoticias;

public class UtnNoticiasApplicationAutoMapperProfile : Profile
{
	public UtnNoticiasApplicationAutoMapperProfile()
	{
		CreateMap<Theme, ThemeDto>();
		CreateMap<IdentityUser, UserDto>();
		CreateMap<NewsDto, ArticleDto>().ReverseMap();
		CreateMap<ReadingList, ReadingListDto>();
		CreateMap<ReadingListItem, ReadingListItemDto>();
		CreateMap<NewsAlert, NewsAlertDto>();
		CreateMap<NewsNotification, NewsNotificationDto>();
	}
}

