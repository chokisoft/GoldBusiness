import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { TranslationService } from '../../services/translation.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-test-connection',
  templateUrl: './test-connection.component.html',
  styleUrls: ['./test-connection.component.css']
})
export class TestConnectionComponent implements OnInit, OnDestroy {
  apiResponse: any = null;
  isLoading = false;
  error: string | null = null;

  private languageSubscription?: Subscription;

  // Traducciones
  translations = {
    title: '',
    configTitle: '',
    connectionTitle: '',
    testButton: '',
    testing: '',
    successTitle: '',
    errorLabel: ''
  };

  constructor(
    private apiService: ApiService,
    public translate: TranslationService
  ) { }

  ngOnInit(): void {
    this.loadTranslations();

    // Suscribirse a cambios de idioma
    this.languageSubscription = this.translate.translations$.subscribe(() => {
      this.loadTranslations();
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private loadTranslations(): void {
    this.translations = {
      title: this.translate.t('test.title'),
      configTitle: this.translate.t('test.configTitle'),
      connectionTitle: this.translate.t('test.connectionTitle'),
      testButton: this.translate.t('test.testButton'),
      testing: this.translate.t('test.testing'),
      successTitle: this.translate.t('test.successTitle'),
      errorLabel: this.translate.t('test.errorLabel')
    };
  }

  /**
   * Probar conexión con la API
   */
  testConnection(): void {
    this.isLoading = true;
    this.error = null;
    this.apiResponse = null;

    this.apiService.get<any>('info').subscribe({
      next: (response) => {
        console.log('✅ Conexión exitosa:', response);
        this.apiResponse = response;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('❌ Error de conexión:', error);
        this.error = error.message;
        this.isLoading = false;
      }
    });
  }
}
