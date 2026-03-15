import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { TranslationService } from '../../services/translation.service';
import { LanguageService } from '../../services/language.service';
import { Subscription } from 'rxjs';
import { environment } from '../../../environments/environment.development';

interface LoginTranslations {
  title: string;
  subtitle: string;
  formTitle: string;
  formSubtitle: string;
  username: string;
  password: string;
  usernamePlaceholder: string;
  passwordPlaceholder: string;
  submit: string;
  loading: string;
  forgotPassword: string;
  googleSignIn: string;
  showPassword: string;
  hidePassword: string;
  footer: string;
  usernameRequired: string;
  usernameMinLength: string;
  passwordRequired: string;
  passwordMinLength: string;
}

const GOOGLE_ERROR_KEYS: Record<string, string> = {
  'google_user_not_found': 'login.errorGoogleUserNotFound',
  'google_provider_not_allowed': 'login.errorGoogleProviderNotAllowed',
  'google_user_inactive': 'login.errorGoogleUserInactive',
  'google_email_not_found': 'login.errorGoogleEmailNotFound',
  'google_token_failed': 'login.errorGoogleTokenFailed',
  'google_remote_failure': 'login.errorGoogleRemoteFailure',
  'google_internal_error': 'login.errorGoogleInternalError',
  'google_user_creation_failed': 'login.errorGoogleUserCreationFailed'
} as const;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm!: FormGroup;
  isLoading = false;
  errorMessage: string | null = null;
  returnUrl: string = '/dashboard';
  showPassword: boolean = false; // Inicializado explícitamente
  readonly googleAuthEnabled = environment.googleAuthEnabled;

  private languageSubscription?: Subscription;

  translations: LoginTranslations = this.getEmptyTranslations();

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly translationService: TranslationService,
    private readonly languageService: LanguageService,
    private readonly cdr: ChangeDetectorRef // ← Añadido para forzar detección
  ) {
    console.log('🌍 Login iniciado con idioma:', this.languageService.getCurrentLanguage());
  }

  get f() {
    return this.loginForm.controls;
  }

  ngOnInit(): void {
    this.initializeForm();
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    console.log('📍 URL de retorno configurada:', this.returnUrl);

    this.loadTranslations();
    this.subscribeToLanguageChanges();

    if (this.handleGoogleCallbackResult()) {
      return;
    }

    if (this.authService.isAuthenticated()) {
      console.log('✅ Usuario ya autenticado, redirigiendo al dashboard...');
      this.router.navigate(['/dashboard']);
    }
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    console.log('🧹 LoginComponent destruido, suscripciones canceladas');
  }

  private initializeForm(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  private loadTranslations(): void {
    this.translations = {
      title: this.translationService.translate('login.title'),
      subtitle: this.translationService.translate('login.subtitle'),
      formTitle: this.translationService.translate('login.formTitle'),
      formSubtitle: this.translationService.translate('login.formSubtitle'),
      username: this.translationService.translate('login.username'),
      password: this.translationService.translate('login.password'),
      usernamePlaceholder: this.translationService.translate('login.usernamePlaceholder'),
      passwordPlaceholder: this.translationService.translate('login.passwordPlaceholder'),
      submit: this.translationService.translate('login.submit'),
      loading: this.translationService.translate('login.loading'),
      forgotPassword: this.translationService.translate('login.forgotPassword'),
      googleSignIn: this.translationService.translate('login.googleSignIn'),
      showPassword: this.translationService.translate('login.showPassword'),
      hidePassword: this.translationService.translate('login.hidePassword'),
      footer: this.translationService.translate('login.footer'),
      usernameRequired: this.translationService.translate('validation.usernameRequired'),
      usernameMinLength: this.translationService.translate('validation.usernameMinLength'),
      passwordRequired: this.translationService.translate('validation.passwordRequired'),
      passwordMinLength: this.translationService.translate('validation.passwordMinLength')
    };
  }

  private subscribeToLanguageChanges(): void {
    this.languageSubscription = this.translationService.translations$.subscribe(() => {
      console.log('🔄 Idioma cambiado, recargando traducciones...');
      this.loadTranslations();
    });
  }

  private getEmptyTranslations(): LoginTranslations {
    return {
      title: '',
      subtitle: '',
      formTitle: '',
      formSubtitle: '',
      username: '',
      password: '',
      usernamePlaceholder: '',
      passwordPlaceholder: '',
      submit: '',
      loading: '',
      forgotPassword: '',
      googleSignIn: '',
      showPassword: '',
      hidePassword: '',
      footer: '',
      usernameRequired: '',
      usernameMinLength: '',
      passwordRequired: '',
      passwordMinLength: ''
    };
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    const { username, password } = this.loginForm.value;

    this.authService.login({ username, password }).subscribe({
      next: () => {
        console.log('✅ Login exitoso, redirigiendo a:', this.returnUrl);
        this.router.navigate([this.returnUrl]);
      },
      error: (err) => {
        console.error('❌ Error en login:', err);
        this.errorMessage = this.translationService.translate('login.errorGeneral');
        this.isLoading = false;
      }
    });
  }

  /**
   * Toggle de visibilidad de contraseña con detección forzada
   */
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
    console.log('👁️ Toggle password visibility:', this.showPassword);
    this.cdr.detectChanges(); // Forzar detección de cambios
  }

  loginWithGoogle(): void {
    this.authService.startGoogleLogin(this.returnUrl);
  }

  private handleGoogleCallbackResult(): boolean {
    const hash = this.getHashParams();

    if (!hash) {
      return false;
    }

    const params = new URLSearchParams(hash);
    const error = params.get('error');

    if (error) {
      this.handleGoogleError(error);
      this.cleanUrlHash();
      return true;
    }

    return this.processGoogleTokens(params);
  }

  private getHashParams(): string | null {
    const hash = window.location.hash;
    return hash?.startsWith('#') ? hash.substring(1) : null;
  }

  private handleGoogleError(error: string): void {
    const translationKey = GOOGLE_ERROR_KEYS[error];

    if (translationKey) {
      this.errorMessage = this.translationService.translate(translationKey);
    } else {
      this.errorMessage = this.translationService.translate('login.errorGoogleGeneric');
    }

    console.error('❌ Error en Google OAuth:', error, '→', this.errorMessage);
  }

  private processGoogleTokens(params: URLSearchParams): boolean {
    const token = params.get('token') ?? '';
    const refreshToken = params.get('refreshToken') ?? '';
    const expiresAt = params.get('expiresAt') ?? '';

    if (!token || !refreshToken || !expiresAt) {
      return false;
    }

    const callbackReturnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    const completed = this.authService.completeExternalLogin({ token, refreshToken, expiresAt });

    this.cleanUrlHash();

    if (completed) {
      console.log('✅ Login con Google exitoso, redirigiendo a:', callbackReturnUrl);
      this.router.navigate([callbackReturnUrl]);
      return true;
    }

    this.errorMessage = this.translationService.translate('login.errorGoogleCompleteLogin');
    return true;
  }

  private cleanUrlHash(): void {
    window.history.replaceState({}, document.title, window.location.pathname + window.location.search);
  }
}
