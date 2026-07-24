import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

export interface NewsDto {
  author?: string | null;
  title?: string | null;
  description?: string | null;
  url?: string | null;
  urlToImage?: string | null;
  publishedAt?: string | null;
  content?: string | null;
}

export interface ReadingListItemDto {
  id: string;
  title: string;
  url: string;
  author?: string | null;
  description?: string | null;
  urlToImage?: string | null;
  publishedAt?: string | null;
  content?: string | null;
  addedAt?: string | null;
}

export interface ReadingListDto {
  id: string;
  name: string;
  ownerId?: string;
  items?: ReadingListItemDto[];
}

export interface AddReadingListItemDto {
  title: string;
  url: string;
  author?: string | null;
  description?: string | null;
  urlToImage?: string | null;
  publishedAt?: string | null;
  content?: string | null;
}

export interface NewsNotificationDto {
  id: string;
  alertId: string;
  ownerId?: string;
  title: string;
  url: string;
  createdAt: string;
  isRead: boolean;
}

export interface NewsAlertDto {
  id: string;
  ownerId?: string;
  searchText: string;
  isActive: boolean;
  lastRunTime?: string | null;
  notifications?: NewsNotificationDto[];
}

@Injectable({
  providedIn: 'root',
})
export class TpBackendClientService {
  private readonly readingListUrl = '/api/app/reading-list';
  private readonly alertUrl = '/api/app/alert';

  constructor(private rest: RestService) {}

  searchNews(query: string): Promise<NewsDto[]> {
    return firstValueFrom(
      this.rest.request<unknown, NewsDto[]>(
        {
          method: 'POST',
          url: '/api/app/utn-noticias/search',
          params: { query },
        },
        { apiName: 'default' },
      ),
    );
  }

  getReadingLists(): Promise<ReadingListDto[]> {
    return firstValueFrom(
      this.rest.request<unknown, ReadingListDto[]>(
        {
          method: 'GET',
          url: this.readingListUrl,
        },
        { apiName: 'default' },
      ),
    );
  }

  createReadingList(name: string): Promise<ReadingListDto> {
    return firstValueFrom(
      this.rest.request<{ name: string }, ReadingListDto>(
        {
          method: 'POST',
          url: this.readingListUrl,
          body: { name },
        },
        { apiName: 'default' },
      ),
    );
  }

  updateReadingList(id: string, name: string): Promise<ReadingListDto> {
    return firstValueFrom(
      this.rest.request<{ name: string }, ReadingListDto>(
        {
          method: 'PUT',
          url: `${this.readingListUrl}/${id}`,
          body: { name },
        },
        { apiName: 'default' },
      ),
    );
  }

  deleteReadingList(id: string): Promise<void> {
    return firstValueFrom(
      this.rest.request<unknown, void>(
        {
          method: 'DELETE',
          url: `${this.readingListUrl}/${id}`,
        },
        { apiName: 'default' },
      ),
    );
  }

  addItemToReadingList(id: string, item: AddReadingListItemDto): Promise<ReadingListDto> {
    return firstValueFrom(
      this.rest.request<AddReadingListItemDto, ReadingListDto>(
        {
          method: 'POST',
          url: `${this.readingListUrl}/${id}/item`,
          body: item,
        },
        { apiName: 'default' },
      ),
    );
  }

  createNewsAlert(searchText: string): Promise<NewsAlertDto> {
    return firstValueFrom(
      this.rest.request<{ searchText: string }, NewsAlertDto>(
        {
          method: 'POST',
          url: this.alertUrl,
          body: { searchText },
        },
        { apiName: 'default' },
      ),
    );
  }

  getMyNotifications(): Promise<NewsNotificationDto[]> {
    return firstValueFrom(
      this.rest.request<unknown, NewsNotificationDto[]>(
        {
          method: 'GET',
          url: `${this.alertUrl}/my-notifications`,
        },
        { apiName: 'default' },
      ),
    );
  }

  runAlerts(): Promise<number> {
    return firstValueFrom(
      this.rest.request<unknown, number>(
        {
          method: 'POST',
          url: `${this.alertUrl}/run-alerts`,
        },
        { apiName: 'default' },
      ),
    );
  }
}
