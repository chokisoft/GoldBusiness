import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-system-configuration-form',
  templateUrl: './system-configuration-form.component.html',
  styleUrls: ['./system-configuration-form.component.css']
})
export class SystemConfigurationFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  configId: number | null = null;
  loading = false;
  error: string | null = null;
  cuentas: CuentaDTO[] = [];
  loadingCuentas = true;

  // 🖼️ Logo
  selectedLogoFile: File | null = null;
  logoPreviewUrl: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private systemConfigurationService: SystemConfigurationService,
    private cuentaService: CuentaService,
    private router: Router,
    private route: ActivatedRoute,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      codigoSistema: ['', [Validators.required, Validators.maxLength(50)]],
      licencia: ['', [Validators.required, Validators.maxLength(100)]],
      nombreNegocio: ['', [Validators.required, Validators.maxLength(256)]],
      direccion: ['', Validators.maxLength(512)],
      municipio: ['', Validators.maxLength(128)],
      provincia: ['', Validators.maxLength(128)],
      codPostal: ['', Validators.maxLength(20)],
      imagen: ['', Validators.maxLength(500)],
      web: ['', Validators.maxLength(256)],
      email: ['', [Validators.email, Validators.maxLength(256)]],
      telefono: ['', Validators.maxLength(20)],
      cuentaPagarId: [null, Validators.required],
      cuentaCobrarId: [null, Validators.required],
      caducidad: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    this.loadCuentas();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.configId = +id;
      this.loadConfiguration();
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 SysConfigForm: Idioma cambiado, recargando datos...');
        this.loadCuentas();
        if (this.isEditMode) this.loadConfiguration();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private setupFormSubscriptions(): void {
    const upperFields = ['nombreNegocio', 'direccion', 'municipio', 'provincia'];
    upperFields.forEach(field => {
      this.form.get(field)?.valueChanges.subscribe(value => {
        if (value && typeof value === 'string' && value !== value.toUpperCase()) {
          this.form.get(field)?.setValue(value.toUpperCase(), { emitEvent: false });
        }
      });
    });
  }

  loadCuentas(): void {
    this.loadingCuentas = true;
    this.cuentaService.getAll().subscribe({
      next: (data) => {
        this.cuentas = data.filter(c => !c.cancelado);
        this.loadingCuentas = false;
      },
      error: (err: any) => {
        console.error('Error al cargar cuentas:', err);
        this.loadingCuentas = false;
      }
    });
  }

  loadConfiguration(): void {
    if (!this.configId) return;
    this.loading = true;
    this.systemConfigurationService.getById(this.configId).subscribe({
      next: (data) => {
        this.form.patchValue(data);
        if (this.isEditMode) this.form.get('codigoSistema')?.disable();

        // Cargar preview del logo existente desde la API
        if (data.imagen) {
          this.logoPreviewUrl = this.systemConfigurationService.getLogoUrl(data.imagen);
        }
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Error al cargar configuración';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  // ═══════════════════════════════════════════════════════════
  // 🖼️ LOGO
  // ═══════════════════════════════════════════════════════════

  onLogoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];

    // ✅ Validar por extensión (más confiable)
    const allowedExtensions = ['.png', '.jpg', '.jpeg', '.gif', '.webp'];
    const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
      this.error = 'Formato no válido. Use PNG, JPG, GIF o WEBP.';
      input.value = '';
      return;
    }

    if (file.size > 2 * 1024 * 1024) {
      this.error = 'El archivo no puede superar 2MB.';
      input.value = '';
      return;
    }

    this.error = null;
    this.selectedLogoFile = file;

    const reader = new FileReader();
    reader.onload = (e) => {
      this.logoPreviewUrl = e.target?.result as string;
    };
    reader.readAsDataURL(file);
  }

  removeLogo(): void {
    this.selectedLogoFile = null;
    this.logoPreviewUrl = null;
    this.form.get('imagen')?.setValue('');
  }

  onLogoError(event: Event): void {
    console.warn('❌ Error al cargar preview del logo');
    this.logoPreviewUrl = null;
  }

  // ═══════════════════════════════════════════════════════════
  // 💾 SUBMIT
  // ═══════════════════════════════════════════════════════════

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.error = null;

    if (this.selectedLogoFile) {
      const codigoSistema = this.form.getRawValue().codigoSistema;
      this.systemConfigurationService.uploadLogo(codigoSistema, this.selectedLogoFile).subscribe({
        next: (result) => {
          this.form.get('imagen')?.setValue(result.fileName);
          this.submitForm();
        },
        error: (err: any) => {
          this.error = 'Error al subir el logo';
          this.loading = false;
          console.error('Error al subir logo:', err);
        }
      });
    } else {
      this.submitForm();
    }
  }

  private submitForm(): void {
    const dto: SystemConfigurationDTO = this.form.getRawValue();

    if (this.isEditMode) {
      this.systemConfigurationService.update(this.configId!, dto).subscribe({
        next: () => this.router.navigate(['/configuracion']),
        error: (err: any) => {
          this.error = 'Error al guardar configuración';
          this.loading = false;
          console.error('Error:', err);
        }
      });
    } else {
      this.systemConfigurationService.create(dto).subscribe({
        next: () => this.router.navigate(['/configuracion']),
        error: (err: any) => {
          this.error = 'Error al guardar configuración';
          this.loading = false;
          console.error('Error:', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/configuracion']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (control?.hasError('required')) return 'Este campo es requerido';
    if (control?.hasError('maxlength')) {
      return `Máximo ${control.errors?.['maxlength'].requiredLength} caracteres`;
    }
    if (control?.hasError('email')) return 'Email no válido';
    return '';
  }
}
