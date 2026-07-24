import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { NewsDto, TpBackendClientService } from './tp-backend-client.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  searchQuery = 'tecnologia';
  newsResults: NewsDto[] = [];
  isSearching = false;
  statusMessage = '';
  errorMessage = '';

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService, private tpBackendClient: TpBackendClientService) {}

  login() {
    this.authService.navigateToLogin();
  }

  async searchNews(): Promise<void> {
    const query = this.searchQuery.trim();

    if (!query) {
      this.setError('Ingresa un texto para buscar.');
      return;
    }

    this.isSearching = true;
    this.errorMessage = '';
    this.statusMessage = '';

    try {
      this.newsResults = await this.tpBackendClient.searchNews(query);
      this.statusMessage = `Busqueda finalizada: ${this.newsResults.length} resultado(s).`;
    } catch (error) {
      this.setError(this.getErrorMessage(error));
    } finally {
      this.isSearching = false;
    }
  }

  trackNews(index: number, item: NewsDto): string {
    return item.url || `${item.title}-${index}`;
  }

  private setError(message: string): void {
    this.errorMessage = message;
    this.statusMessage = '';
  }

  private getErrorMessage(error: unknown): string {
    const err = error as {
      status?: number;
      error?: { error?: { message?: string }; message?: string };
      message?: string;
    };

    if (err.status === 401) {
      return 'No estas autenticado. Inicia sesion y proba nuevamente.';
    }

    return err.error?.error?.message || err.error?.message || err.message || 'Ocurrio un error inesperado.';
  }
}
