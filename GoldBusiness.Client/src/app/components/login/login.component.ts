import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { TranslationService } from '../../services/translation.service';
import { LanguageService } from '../../services/language.service';
import { Subscription } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm!: FormGroup;
  isLoading = false;
  errorMessage: string | null = null;
  returnUrl: string = '/dashboard'; // ← CAMBIADO de '/' a '/dashboard'
  showPassword = false;
  googleAuthEnabled = environment.googleAuthEnabled;

  private languageSubscription?: Subscription;

  // Traducciones dinámicas
  translations = {
    title: '',
    subtitle: '',
    formTitle: '',      // << nuevo
    formSubtitle: '',   // << nuevo
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

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private translationService: TranslationService,
    private languageService: LanguageService
  ) {
    console.log('🌍 Login iniciado con idioma:', this.languageService.getCurrentLanguage());
  }

  /**
   * Helper para acceder a los controles del formulario fácilmente en el template
   */
  get f() {
    return this.loginForm.controls;
  }

  ngOnInit(): void {
    if (this.handleGoogleCallbackResult()) {
      return;
    }

    // ✅ NUEVO: Si ya está autenticado, redirigir
    if (this.authService.isAuthenticated()) {
      console.log('✅ Usuario ya autenticado, redirigiendo al dashboard...');
      this.router.navigate(['/dashboard']);
      return;
    }

    // Crear el formulario con validaciones
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });

    // Obtener la URL de retorno (o dashboard por defecto)
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    console.log('📍 URL de retorno configurada:', this.returnUrl);

    // Cargar traducciones iniciales
    this.loadTranslations();

    // Suscribirse a cambios de idioma
    this.languageSubscription = this.translationService.translations$.subscribe(() => {
      console.log('🔄 Idioma cambiado, recargando traducciones...');
      this.loadTranslations();
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    console.log('🧹 LoginComponent destruido, suscripciones canceladas');
  }

  /**
   * Cargar todas las traducciones del componente
   */
  private loadTranslations(): void {
    this.translations = {
      title: this.translationService.translate('login.title'),
      subtitle: this.translationService.translate('login.subtitle'),
      formTitle: this.translationService.translate('login.formTitle'),       // << nuevo
      formSubtitle: this.translationService.translate('login.formSubtitle'), // << nuevo
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

  /**
   * Manejar el envío del formulario de login
   */
  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    const { username, password } = this.loginForm.value;

    // Login con redirección al dashboard
    this.authService.login({ username, password }).subscribe({
      next: () => {
        console.log('✅ Login exitoso, redirigiendo a:', this.returnUrl);
        this.router.navigate([this.returnUrl]); // Esto ahora redirige a /dashboard por defecto
      },
      error: (err) => {
        console.error('❌ Error en login:', err);
        this.errorMessage = this.translationService.translate('login.errorGeneral');
        this.isLoading = false;
      }
    });
  }

  /**
   * Alternar visibilidad de la contraseña
   */
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  loginWithGoogle(): void {
    this.authService.startGoogleLogin(this.returnUrl);
  }

  private handleGoogleCallbackResult(): boolean {
    const hash = window.location.hash?.startsWith('#')
      ? window.location.hash.substring(1)
      : '';

    if (!hash) {
      return false;
    }

    const params = new URLSearchParams(hash);
    const error = params.get('error');

    if (error) {
      this.errorMessage = `Error de autenticación Google: ${error}`;
      window.history.replaceState({}, document.title, window.location.pathname + window.location.search);
      return true;
    }

    const token = params.get('token') ?? '';
    const refreshToken = params.get('refreshToken') ?? '';
    const expiresAt = params.get('expiresAt') ?? '';

    if (!token || !refreshToken || !expiresAt) {
      return false;
    }

    const callbackReturnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    const completed = this.authService.completeExternalLogin({ token, refreshToken, expiresAt });

    window.history.replaceState({}, document.title, window.location.pathname + window.location.search);

    if (completed) {
      this.router.navigate([callbackReturnUrl]);
      return true;
    }

    this.errorMessage = 'No se pudo completar el inicio de sesión con Google.';
    return true;
  }
}
