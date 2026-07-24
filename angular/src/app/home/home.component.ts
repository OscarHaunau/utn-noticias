import { AuthService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import {
  AddReadingListItemDto,
  NewsAlertDto,
  NewsDto,
  NewsNotificationDto,
  ReadingListDto,
  TpBackendClientService,
} from './tp-backend-client.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  searchQuery = 'tecnologia';
  newsResults: NewsDto[] = [];
  isSearching = false;
  statusMessage = '';
  errorMessage = '';
  readingLists: ReadingListDto[] = [];
  newsPendingToAdd: NewsDto | null = null;
  selectedModalReadingListId = '';
  newReadingListName = '';
  editingReadingListId = '';
  editingReadingListName = '';
  isLoadingReadingLists = false;
  isCreatingReadingList = false;
  updatingReadingListId = '';
  deletingReadingListId = '';
  addingNewsUrl = '';
  readingListStatusMessage = '';
  readingListErrorMessage = '';
  alertSearchText = '';
  createdAlerts: NewsAlertDto[] = [];
  isCreatingAlert = false;
  alertStatusMessage = '';
  alertErrorMessage = '';
  notifications: NewsNotificationDto[] = [];
  isLoadingNotifications = false;
  isRunningAlerts = false;
  notificationsStatusMessage = '';
  notificationsErrorMessage = '';

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService, private tpBackendClient: TpBackendClientService) {}

  ngOnInit(): void {
    if (this.hasLoggedIn) {
      void this.loadReadingLists();
      void this.loadNotifications();
    }
  }

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
      this.closeAddToListModal();
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

  trackReadingList(index: number, item: ReadingListDto): string {
    return item.id || `${item.name}-${index}`;
  }

  trackReadingListItem(index: number, item: { id?: string; url?: string; title?: string }): string {
    return item.id || item.url || `${item.title}-${index}`;
  }

  trackAlert(index: number, item: NewsAlertDto): string {
    return item.id || `${item.searchText}-${index}`;
  }

  trackNotification(index: number, item: NewsNotificationDto): string {
    return item.id || item.url || `${item.title}-${index}`;
  }

  async loadReadingLists(): Promise<void> {
    if (!this.ensureAuthenticatedForReadingLists()) {
      return;
    }

    this.isLoadingReadingLists = true;
    this.clearReadingListMessages();

    try {
      this.readingLists = this.normalizeReadingLists(await this.tpBackendClient.getReadingLists());
      this.keepValidModalSelection();
    } catch (error) {
      this.setReadingListError(this.getErrorMessage(error));
    } finally {
      this.isLoadingReadingLists = false;
    }
  }

  async createReadingList(): Promise<void> {
    const name = this.newReadingListName.trim();

    if (!name) {
      this.setReadingListError('Ingresa un nombre para la lista.');
      return;
    }

    if (!this.ensureAuthenticatedForReadingLists()) {
      return;
    }

    this.isCreatingReadingList = true;
    this.clearReadingListMessages();

    try {
      const created = this.normalizeReadingList(await this.tpBackendClient.createReadingList(name));
      this.upsertReadingList(created);
      this.newReadingListName = '';
      this.readingListStatusMessage = `Lista "${created.name}" creada.`;
    } catch (error) {
      this.setReadingListError(this.getErrorMessage(error));
    } finally {
      this.isCreatingReadingList = false;
    }
  }

  startEditingReadingList(list: ReadingListDto): void {
    this.editingReadingListId = list.id;
    this.editingReadingListName = list.name;
    this.clearReadingListMessages();
  }

  cancelEditingReadingList(): void {
    this.editingReadingListId = '';
    this.editingReadingListName = '';
  }

  async updateReadingList(list: ReadingListDto): Promise<void> {
    const name = this.editingReadingListName.trim();

    if (!name) {
      this.setReadingListError('Ingresa un nombre para actualizar la lista.');
      return;
    }

    if (!this.ensureAuthenticatedForReadingLists()) {
      return;
    }

    this.updatingReadingListId = list.id;
    this.clearReadingListMessages();

    try {
      const updated = this.normalizeReadingList(await this.tpBackendClient.updateReadingList(list.id, name));
      this.upsertReadingList(updated);
      this.cancelEditingReadingList();
      this.readingListStatusMessage = `Lista "${updated.name}" actualizada.`;
    } catch (error) {
      this.setReadingListError(this.getErrorMessage(error));
    } finally {
      this.updatingReadingListId = '';
    }
  }

  async deleteReadingList(list: ReadingListDto): Promise<void> {
    if (!this.ensureAuthenticatedForReadingLists()) {
      return;
    }

    this.deletingReadingListId = list.id;
    this.clearReadingListMessages();

    try {
      await this.tpBackendClient.deleteReadingList(list.id);
      this.readingLists = this.readingLists.filter(item => item.id !== list.id);
      this.keepValidModalSelection();
      this.readingListStatusMessage = `Lista "${list.name}" eliminada.`;
    } catch (error) {
      this.setReadingListError(this.getErrorMessage(error));
    } finally {
      this.deletingReadingListId = '';
    }
  }

  canAddNewsToReadingList(news: NewsDto): boolean {
    return Boolean(
      this.hasLoggedIn &&
        this.readingLists.length &&
        news.title?.trim() &&
        news.url?.trim() &&
        !this.addingNewsUrl,
    );
  }

  openAddToListModal(news: NewsDto): void {
    if (!this.ensureAuthenticatedForReadingLists()) {
      return;
    }

    if (!news.title?.trim() || !news.url?.trim()) {
      this.setReadingListError('La noticia necesita titulo y url para guardarse.');
      return;
    }

    this.clearReadingListMessages();
    this.newsPendingToAdd = news;
    this.selectedModalReadingListId = this.readingLists[0]?.id || '';
  }

  closeAddToListModal(): void {
    this.newsPendingToAdd = null;
    this.selectedModalReadingListId = '';
  }

  async confirmAddNewsToReadingList(): Promise<void> {
    if (!this.newsPendingToAdd) {
      return;
    }

    if (!this.selectedModalReadingListId) {
      this.setReadingListError('Elegí una lista destino para guardar la noticia.');
      return;
    }

    this.addingNewsUrl = this.newsPendingToAdd.url || '';
    this.clearReadingListMessages();

    try {
      const updated = this.normalizeReadingList(
        await this.tpBackendClient.addItemToReadingList(
          this.selectedModalReadingListId,
          this.toReadingListItemInput(this.newsPendingToAdd),
        ),
      );
      this.upsertReadingList(updated);
      this.readingListStatusMessage = `Noticia agregada a "${updated.name}".`;
      this.closeAddToListModal();
    } catch (error) {
      this.setReadingListError(this.getErrorMessage(error));
    } finally {
      this.addingNewsUrl = '';
    }
  }

  useCurrentSearchForAlert(): void {
    this.alertSearchText = this.searchQuery.trim();
    this.clearAlertMessages();
  }

  async createNewsAlert(): Promise<void> {
    const searchText = this.alertSearchText.trim();

    if (!searchText) {
      this.setAlertError('Ingresa un texto para crear la alerta.');
      return;
    }

    if (!this.ensureAuthenticatedForAlerts()) {
      return;
    }

    this.isCreatingAlert = true;
    this.clearAlertMessages();

    try {
      const created = await this.tpBackendClient.createNewsAlert(searchText);
      this.createdAlerts = [this.normalizeNewsAlert(created), ...this.createdAlerts];
      this.alertSearchText = '';
      this.alertStatusMessage = `Alerta "${created.searchText}" creada.`;
    } catch (error) {
      this.setAlertError(this.getErrorMessage(error));
    } finally {
      this.isCreatingAlert = false;
    }
  }

  async loadNotifications(): Promise<void> {
    if (!this.ensureAuthenticatedForNotifications()) {
      return;
    }

    this.isLoadingNotifications = true;
    this.clearNotificationsMessages();

    try {
      await this.refreshNotifications();
      this.notificationsStatusMessage = `Notificaciones persistidas: ${this.notifications.length}.`;
    } catch (error) {
      this.setNotificationsError(this.getErrorMessage(error));
    } finally {
      this.isLoadingNotifications = false;
    }
  }

  async runAlerts(): Promise<void> {
    if (!this.ensureAuthenticatedForNotifications()) {
      return;
    }

    this.isRunningAlerts = true;
    this.clearNotificationsMessages();

    try {
      const savedCount = await this.tpBackendClient.runAlerts();
      await this.refreshNotifications();
      this.notificationsStatusMessage = `Ejecucion finalizada: ${savedCount} notificacion(es) nueva(s) persistida(s).`;
    } catch (error) {
      this.setNotificationsError(this.getErrorMessage(error));
    } finally {
      this.isRunningAlerts = false;
    }
  }

  private setError(message: string): void {
    this.errorMessage = message;
    this.statusMessage = '';
  }

  private setReadingListError(message: string): void {
    this.readingListErrorMessage = message;
    this.readingListStatusMessage = '';
  }

  private clearReadingListMessages(): void {
    this.readingListErrorMessage = '';
    this.readingListStatusMessage = '';
  }

  private setAlertError(message: string): void {
    this.alertErrorMessage = message;
    this.alertStatusMessage = '';
  }

  private clearAlertMessages(): void {
    this.alertErrorMessage = '';
    this.alertStatusMessage = '';
  }

  private setNotificationsError(message: string): void {
    this.notificationsErrorMessage = message;
    this.notificationsStatusMessage = '';
  }

  private clearNotificationsMessages(): void {
    this.notificationsErrorMessage = '';
    this.notificationsStatusMessage = '';
  }

  private ensureAuthenticatedForReadingLists(): boolean {
    if (this.hasLoggedIn) {
      return true;
    }

    this.setReadingListError('Inicia sesion para trabajar con listas de lectura.');
    return false;
  }

  private ensureAuthenticatedForAlerts(): boolean {
    if (this.hasLoggedIn) {
      return true;
    }

    this.setAlertError('Inicia sesion para crear alertas.');
    return false;
  }

  private ensureAuthenticatedForNotifications(): boolean {
    if (this.hasLoggedIn) {
      return true;
    }

    this.setNotificationsError('Inicia sesion para consultar o ejecutar alertas.');
    return false;
  }

  private normalizeReadingLists(lists: ReadingListDto[]): ReadingListDto[] {
    return lists.map(list => this.normalizeReadingList(list));
  }

  private normalizeReadingList(list: ReadingListDto): ReadingListDto {
    return {
      ...list,
      items: list.items || [],
    };
  }

  private upsertReadingList(list: ReadingListDto): void {
    const index = this.readingLists.findIndex(item => item.id === list.id);

    if (index === -1) {
      this.readingLists = [...this.readingLists, list].sort((left, right) =>
        left.name.localeCompare(right.name),
      );
      return;
    }

    this.readingLists = this.readingLists.map(item => (item.id === list.id ? list : item));
  }

  private keepValidModalSelection(): void {
    if (
      this.selectedModalReadingListId &&
      !this.readingLists.some(list => list.id === this.selectedModalReadingListId)
    ) {
      this.selectedModalReadingListId = this.readingLists[0]?.id || '';
    }
  }

  private normalizeNewsAlert(alert: NewsAlertDto): NewsAlertDto {
    return {
      ...alert,
      notifications: alert.notifications || [],
    };
  }

  private async refreshNotifications(): Promise<void> {
    this.notifications = await this.tpBackendClient.getMyNotifications();
  }

  private toReadingListItemInput(news: NewsDto): AddReadingListItemDto {
    return {
      title: news.title?.trim() || '',
      url: news.url?.trim() || '',
      author: news.author || null,
      description: news.description || null,
      urlToImage: news.urlToImage || null,
      publishedAt: news.publishedAt || null,
      content: news.content || null,
    };
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

    if (err.status === 403) {
      return 'Tu usuario no tiene permisos para esta operacion.';
    }

    return err.error?.error?.message || err.error?.message || err.message || 'Ocurrio un error inesperado.';
  }
}
