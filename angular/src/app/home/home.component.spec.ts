import { CommonModule } from '@angular/common';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { AuthService } from '@abp/ng.core';
import { HomeComponent } from './home.component';
import { TpBackendClientService } from './tp-backend-client.service';

describe('HomeComponent', () => {
  let fixture: ComponentFixture<HomeComponent>;
  let isAuthenticated = false;
  const mockAuthService = jasmine.createSpyObj('AuthService', ['navigateToLogin']);
  const mockTpBackendClient = jasmine.createSpyObj<TpBackendClientService>('TpBackendClientService', [
    'searchNews',
    'getReadingLists',
    'createReadingList',
    'updateReadingList',
    'deleteReadingList',
    'addItemToReadingList',
    'createNewsAlert',
  ]);

  beforeEach(
    waitForAsync(() => {
      Object.defineProperty(mockAuthService, 'isAuthenticated', {
        configurable: true,
        get: () => isAuthenticated,
      });

      TestBed.configureTestingModule({
        declarations: [HomeComponent],
        imports: [CommonModule, FormsModule],
        providers: [
          {
            provide: AuthService,
            useValue: mockAuthService,
          },
          {
            provide: TpBackendClientService,
            useValue: mockTpBackendClient,
          },
        ],
      }).compileComponents();
    }),
  );

  beforeEach(() => {
    isAuthenticated = false;
    mockAuthService.navigateToLogin.calls.reset();
    mockTpBackendClient.searchNews.calls.reset();
    mockTpBackendClient.getReadingLists.calls.reset();
    mockTpBackendClient.createReadingList.calls.reset();
    mockTpBackendClient.updateReadingList.calls.reset();
    mockTpBackendClient.deleteReadingList.calls.reset();
    mockTpBackendClient.addItemToReadingList.calls.reset();
    mockTpBackendClient.createNewsAlert.calls.reset();
    fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
  });

  it('should be initiated', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should navigate to login', () => {
    fixture.componentInstance.login();

    expect(mockAuthService.navigateToLogin).toHaveBeenCalled();
  });

  it('should call backend client when searching news', async () => {
    mockTpBackendClient.searchNews.and.resolveTo([
      {
        title: 'Titulo',
        url: 'https://example.test/news',
      },
    ]);

    fixture.componentInstance.searchQuery = 'tecnologia';
    await fixture.componentInstance.searchNews();

    expect(mockTpBackendClient.searchNews).toHaveBeenCalledOnceWith('tecnologia');
    expect(fixture.componentInstance.newsResults.length).toBe(1);
  });

  it('should not call backend client with an empty query', async () => {
    fixture.componentInstance.searchQuery = '   ';
    await fixture.componentInstance.searchNews();

    expect(mockTpBackendClient.searchNews).not.toHaveBeenCalled();
    expect(fixture.componentInstance.errorMessage).toContain('Ingresa un texto');
  });

  it('should create a reading list for authenticated users', async () => {
    isAuthenticated = true;
    mockTpBackendClient.createReadingList.and.resolveTo({
      id: 'list-1',
      name: 'Noticias para leer',
      ownerId: 'user-1',
      items: [],
    });

    fixture.componentInstance.newReadingListName = ' Noticias para leer ';
    await fixture.componentInstance.createReadingList();

    expect(mockTpBackendClient.createReadingList).toHaveBeenCalledOnceWith('Noticias para leer');
    expect(fixture.componentInstance.readingLists[0].id).toBe('list-1');
    expect(fixture.componentInstance.newsPendingToAdd).toBeNull();
  });

  it('should update a reading list name', async () => {
    isAuthenticated = true;
    fixture.componentInstance.readingLists = [
      {
        id: 'list-1',
        name: 'Anterior',
        ownerId: 'user-1',
        items: [],
      },
    ];
    fixture.componentInstance.startEditingReadingList(fixture.componentInstance.readingLists[0]);
    fixture.componentInstance.editingReadingListName = 'Actualizada';
    mockTpBackendClient.updateReadingList.and.resolveTo({
      id: 'list-1',
      name: 'Actualizada',
      ownerId: 'user-1',
      items: [],
    });

    await fixture.componentInstance.updateReadingList(fixture.componentInstance.readingLists[0]);

    expect(mockTpBackendClient.updateReadingList).toHaveBeenCalledOnceWith('list-1', 'Actualizada');
    expect(fixture.componentInstance.readingLists[0].name).toBe('Actualizada');
  });

  it('should delete a reading list', async () => {
    isAuthenticated = true;
    fixture.componentInstance.readingLists = [
      {
        id: 'list-1',
        name: 'Para borrar',
        ownerId: 'user-1',
        items: [],
      },
    ];
    mockTpBackendClient.deleteReadingList.and.resolveTo();

    await fixture.componentInstance.deleteReadingList(fixture.componentInstance.readingLists[0]);

    expect(mockTpBackendClient.deleteReadingList).toHaveBeenCalledOnceWith('list-1');
    expect(fixture.componentInstance.readingLists.length).toBe(0);
  });

  it('should open a modal to choose a reading list for a searched news item', () => {
    isAuthenticated = true;
    const news = {
      title: 'Titulo',
      url: 'https://example.test/news',
      author: 'Autor',
    };
    fixture.componentInstance.readingLists = [
      {
        id: 'list-1',
        name: 'Favoritas',
        ownerId: 'user-1',
        items: [],
      },
    ];

    fixture.componentInstance.openAddToListModal(news);

    expect(fixture.componentInstance.newsPendingToAdd).toEqual(news);
    expect(fixture.componentInstance.selectedModalReadingListId).toBe('list-1');
  });

  it('should add a searched news item to the list selected in the modal', async () => {
    isAuthenticated = true;
    const news = {
      title: 'Titulo',
      url: 'https://example.test/news',
      author: 'Autor',
    };
    fixture.componentInstance.readingLists = [
      {
        id: 'list-1',
        name: 'Favoritas',
        ownerId: 'user-1',
        items: [],
      },
    ];
    fixture.componentInstance.openAddToListModal(news);
    fixture.componentInstance.selectedModalReadingListId = 'list-1';
    mockTpBackendClient.addItemToReadingList.and.resolveTo({
      id: 'list-1',
      name: 'Favoritas',
      ownerId: 'user-1',
      items: [
        {
          id: 'item-1',
          title: 'Titulo',
          url: 'https://example.test/news',
        },
      ],
    });

    await fixture.componentInstance.confirmAddNewsToReadingList();

    expect(mockTpBackendClient.addItemToReadingList).toHaveBeenCalledOnceWith(
      'list-1',
      jasmine.objectContaining({
        title: 'Titulo',
        url: 'https://example.test/news',
        author: 'Autor',
      }),
    );
    expect(fixture.componentInstance.readingLists[0].items?.length).toBe(1);
    expect(fixture.componentInstance.newsPendingToAdd).toBeNull();
  });

  it('should not add a searched news item without choosing a reading list', async () => {
    isAuthenticated = true;
    const news = {
      title: 'Titulo',
      url: 'https://example.test/news',
    };
    fixture.componentInstance.readingLists = [
      {
        id: 'list-1',
        name: 'Favoritas',
        ownerId: 'user-1',
        items: [],
      },
    ];

    expect(fixture.componentInstance.canAddNewsToReadingList(news)).toBeTrue();
    fixture.componentInstance.openAddToListModal(news);
    fixture.componentInstance.selectedModalReadingListId = '';

    await fixture.componentInstance.confirmAddNewsToReadingList();

    expect(mockTpBackendClient.addItemToReadingList).not.toHaveBeenCalled();
    expect(fixture.componentInstance.readingListErrorMessage).toContain('Elegí una lista destino');
  });

  it('should copy current search text to the alert form', () => {
    fixture.componentInstance.searchQuery = ' bitcoin ';

    fixture.componentInstance.useCurrentSearchForAlert();

    expect(fixture.componentInstance.alertSearchText).toBe('bitcoin');
  });

  it('should create a news alert for authenticated users', async () => {
    isAuthenticated = true;
    mockTpBackendClient.createNewsAlert.and.resolveTo({
      id: 'alert-1',
      searchText: 'bitcoin',
      isActive: true,
      notifications: [],
    });

    fixture.componentInstance.alertSearchText = ' bitcoin ';
    await fixture.componentInstance.createNewsAlert();

    expect(mockTpBackendClient.createNewsAlert).toHaveBeenCalledOnceWith('bitcoin');
    expect(fixture.componentInstance.createdAlerts[0].id).toBe('alert-1');
    expect(fixture.componentInstance.alertSearchText).toBe('');
  });

  it('should not create a news alert with empty search text', async () => {
    isAuthenticated = true;
    fixture.componentInstance.alertSearchText = '   ';

    await fixture.componentInstance.createNewsAlert();

    expect(mockTpBackendClient.createNewsAlert).not.toHaveBeenCalled();
    expect(fixture.componentInstance.alertErrorMessage).toContain('Ingresa un texto');
  });
});
