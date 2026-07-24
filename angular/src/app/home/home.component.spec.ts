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
});
