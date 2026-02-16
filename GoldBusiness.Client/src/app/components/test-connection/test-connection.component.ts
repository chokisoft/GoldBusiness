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
    private translationService: TranslationService // ← CAMBIO 1: Renombrado
  ) { }

  ngOnInit(): void {
    this.loadTranslations();

    // Suscribirse a cambios de idioma
    this.languageSubscription = this.translationService.translations$.subscribe(() => {
      this.loadTranslations();
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private loadTranslations(): void {
    this.translations = {
      title: this.translationService.translate('test.title'), // ← CAMBIO 2: .t() → .translate()
      configTitle: this.translationService.translate('test.configTitle'),
      connectionTitle: this.translationService.translate('test.connectionTitle'),
      testButton: this.translationService.translate('test.testButton'),
      testing: this.translationService.translate('test.testing'),
      successTitle: this.translationService.translate('test.successTitle'),
      errorLabel: this.translationService.translate('test.errorLabel')
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
