import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { TranslationService } from '../../services/translation.service';
import { LanguageService } from '../../services/language.service';
import { Subscription } from 'rxjs';

/**
 * Componente de Login
 * Maneja la autenticación de usuarios con soporte multiidioma
 * La sesión se guarda en sessionStorage y expira al cerrar el navegador
 */
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm!: FormGroup;
  isLoading = false;
  errorMessage: string | null = null;
  returnUrl: string = '/';
  showPassword = false;

  // Subscripción para el cambio de idioma
  private languageSubscription?: Subscription;

  // Traducciones dinámicas
  translations = {
    title: '',
    subtitle: '',
    username: '',
    password: '',
    usernamePlaceholder: '',
    passwordPlaceholder: '',
    submit: '',
    loading: '',
    forgotPassword: '',
    showPassword: '',
    hidePassword: '',
    footer: '',
    // Validaciones
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
    public translate: TranslationService,
    private languageService: LanguageService
  ) {
    // Log del idioma inicial
    console.log('🌍 Login iniciado con idioma:', this.languageService.getCurrentLanguage());
  }

  ngOnInit(): void {
    // Si ya está autenticado, redirigir
    if (this.authService.isAuthenticated()) {
      console.log('✅ Usuario ya autenticado, redirigiendo...');
      this.router.navigate(['/']);
      return;
    }

    // Crear el formulario con validaciones
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });

    // Obtener la URL de retorno
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    console.log('📍 URL de retorno configurada:', this.returnUrl);

    // Cargar traducciones iniciales
    this.loadTranslations();

    // Suscribirse a cambios de idioma
    this.languageSubscription = this.translate.translations$.subscribe(() => {
      console.log('🔄 Idioma cambiado, recargando traducciones...');
      this.loadTranslations();
    });
  }

  ngOnDestroy(): void {
    // Cancelar suscripción al destruir el componente
    this.languageSubscription?.unsubscribe();
    console.log('🧹 LoginComponent destruido, suscripciones canceladas');
  }

  /**
   * Cargar todas las traducciones del componente
   */
  private loadTranslations(): void {
    this.translations = {
      title: this.translate.t('login.title'),
      subtitle: this.translate.t('login.subtitle'),
      username: this.translate.t('login.username'),
      password: this.translate.t('login.password'),
      usernamePlaceholder: this.translate.t('login.usernamePlaceholder'),
      passwordPlaceholder: this.translate.t('login.passwordPlaceholder'),
      submit: this.translate.t('login.submit'),
      loading: this.translate.t('login.loading'),
      forgotPassword: this.translate.t('login.forgotPassword'),
      showPassword: this.translate.t('login.showPassword'),
      hidePassword: this.translate.t('login.hidePassword'),
      footer: this.translate.t('login.footer'),
      // Validaciones
      usernameRequired: this.translate.t('validation.usernameRequired'),
      usernameMinLength: this.translate.t('validation.usernameMinLength'),
      passwordRequired: this.translate.t('validation.passwordRequired'),
      passwordMinLength: this.translate.t('validation.passwordMinLength')
    };
  }

  /**
   * Obtener controles del formulario para validaciones en el template
   */
  get f() {
    return this.loginForm.controls;
  }

  /**
   * Manejar el envío del formulario de login
   */
  onSubmit(): void {
    // Limpiar mensaje de error previo
    this.errorMessage = null;

    // Validar que el formulario sea válido
    if (this.loginForm.invalid) {
      console.warn('⚠️ Formulario inválido');
      this.markFormGroupTouched(this.loginForm);
      return;
    }

    // Iniciar proceso de autenticación
    this.isLoading = true;
    const credentials = {
      username: this.loginForm.value.username.trim(), // Quitar espacios
      password: this.loginForm.value.password
    };

    console.log('🔐 Intentando login para usuario:', credentials.username);
    console.log('🌍 Idioma actual:', this.languageService.getCurrentLanguage());

    this.authService.login(credentials).subscribe({
      next: (response) => {
        console.log('✅ Respuesta del servidor:', response);

        if (response.succeeded && response.data) {
          console.log('✅ Login exitoso para:', response.data.user.userName);
          console.log('👤 Usuario autenticado:', response.data.user);
          console.log('🎭 Roles:', response.data.user.roles);

          // Verificar que el idioma se mantiene después del login
          const currentLang = this.languageService.getCurrentLanguage();
          console.log('🌍 Idioma después del login:', currentLang);

          // Redirigir a la página solicitada
          this.router.navigate([this.returnUrl]);
        } else {
          // Credenciales inválidas
          const errorMsg = response.message || this.translate.t('login.errorGeneral');
          console.error('❌ Login fallido:', errorMsg);
          this.errorMessage = errorMsg;
          this.isLoading = false;

          // Limpiar el campo de contraseña por seguridad
          this.loginForm.patchValue({ password: '' });
        }
      },
      error: (error) => {
        console.error('❌ Error de conexión:', error);
        this.errorMessage = this.translate.t('login.errorGeneral');
        this.isLoading = false;

        // Limpiar el campo de contraseña por seguridad
        this.loginForm.patchValue({ password: '' });
      }
    });
  }

  /**
   * Alternar la visibilidad de la contraseña
   */
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
    console.log('👁️ Visibilidad de contraseña:', this.showPassword ? 'visible' : 'oculta');
  }

  /**
   * Marcar todos los campos del formulario como tocados
   * para mostrar los mensajes de validación
   */
  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  /**
   * Resetear el formulario (opcional, para uso futuro)
   */
  resetForm(): void {
    this.loginForm.reset();
    this.errorMessage = null;
    this.isLoading = false;
    console.log('🔄 Formulario reseteado');
  }

  /**
   * Verificar si un campo tiene errores y ha sido tocado
   */
  hasError(fieldName: string, errorType: string): boolean {
    const field = this.loginForm.get(fieldName);
    return !!(field?.hasError(errorType) && field?.touched);
  }
}
